using AppliedEffectsSystem;
using Engine;
using Pause;
using SelectionSystem;
using System.Collections.Generic;
using BattleSystem;
using Units;
using UnitsControlSystem;
using Utils;

namespace Abilities
{
    public class AbilitiesController : IController, ICleanable, IPauseHandler
    {
        private Dictionary<int, AbilityModel> _abilityProtoModels = new Dictionary<int, AbilityModel>();
        private SelectAbilityTargetController _selectAbilityTargetController;
        private AbilityExecutorFactory _abilityExecutorFactory;
        private BattleController _battleController;
        private bool _isPaused;
        private MoveUnitsController _moveUnitsController;
        private StaminaParametersController _staminaParametersController;

        private Dictionary<int, List<InstantExecutorsCreator>> _instantExecutorsCreators = new Dictionary<int, List<InstantExecutorsCreator>>();
        private Dictionary<int, List<TimerExecutorsCreator>> _timerExecutorsCreators = new Dictionary<int, List<TimerExecutorsCreator>>();
        private Dictionary<int, List<TickableExecutorsCreator>> _tickableExecutorsCreators = new Dictionary<int, List<TickableExecutorsCreator>>();
        private Dictionary<Unit, ExecutorsCreator> _executorsCreatorsOnPause = new Dictionary<Unit, ExecutorsCreator>();

        public Dictionary<int, List<InstantExecutorsCreator>> InstantExecutorsCreators => _instantExecutorsCreators;
        public Dictionary<int, List<TimerExecutorsCreator>> TimerExecutorsCreators => _timerExecutorsCreators;
        public Dictionary<int, List<TickableExecutorsCreator>> TickableExecutorsCreators => _tickableExecutorsCreators;  

        public AbilitiesController(GlobalConfigLoader globalConfig, SelectAbilityTargetController selectAbilityTargetController,
            BattleController battleController, MoveUnitsController moveUnitsController, StaminaParametersController staminaParametersController,
            AppliedEffectsUIController appliedEffectsUIController)
        {
            _selectAbilityTargetController = selectAbilityTargetController;
            _selectAbilityTargetController.OnUnitClick += CreateAbilityExecutorsCreator;
            _moveUnitsController = moveUnitsController;
            _battleController = battleController;
            _staminaParametersController = staminaParametersController;

            _abilityExecutorFactory = new AbilityExecutorFactory(battleController, appliedEffectsUIController);

            var abilitiesConfigurators = globalConfig.AbilitiesComposite.AbilityConfigurators;

            for (int i = 0; i < abilitiesConfigurators.Count; i++)
            {
                if (_abilityProtoModels.ContainsKey(abilitiesConfigurators[i].AbilityID)) continue;

                _abilityProtoModels.Add(abilitiesConfigurators[i].AbilityID, new AbilityModel(abilitiesConfigurators[i]));
            }
        }

        public void CreateAbilityExecutorsCreator(Unit unit, IAbilityDataForExecutor abilitiData)
        {
            if (_abilityProtoModels.TryGetValue(abilitiData.AbilityID, out AbilityModel model))
            {

                switch (model.AbilityType)
                {
                    case AbilityTypes.TargetDamage:
                    case AbilityTypes.TargetHeal:
                    case AbilityTypes.MultiplicativeTargetDamage:
                    case AbilityTypes.AoEDamage:
                    case AbilityTypes.AoEHeal:

                        CheckKey(abilitiData.AbilityID, _instantExecutorsCreators);

                        var instantExecutorsCreator = new InstantExecutorsCreator(unit, abilitiData.TargetList, abilitiData.CastPoint, model,
                            _battleController, abilitiData.ExecuteCallback, _moveUnitsController, _abilityExecutorFactory, this, _staminaParametersController);
                        RunCreators(abilitiData.AbilityID, instantExecutorsCreator, _instantExecutorsCreators);

                        break;
                    case AbilityTypes.Buff:
                    case AbilityTypes.Debuff:

                        CheckKey(abilitiData.AbilityID, _timerExecutorsCreators);

                        var timerExecutorsCreator = new TimerExecutorsCreator(unit, abilitiData.TargetList, abilitiData.CastPoint, model,
                            _battleController, abilitiData.ExecuteCallback, _moveUnitsController, _abilityExecutorFactory, this, _staminaParametersController);
                        RunCreators(abilitiData.AbilityID, timerExecutorsCreator, _timerExecutorsCreators);

                        break;
                    case AbilityTypes.HoT:
                    case AbilityTypes.DoT:

                        CheckKey(abilitiData.AbilityID, _tickableExecutorsCreators);

                        var tickableExecutorsCreator = new TickableExecutorsCreator(unit, abilitiData.TargetList, abilitiData.CastPoint, model,
                            _battleController, abilitiData.ExecuteCallback, _moveUnitsController, _abilityExecutorFactory, this, _staminaParametersController);
                        RunCreators(abilitiData.AbilityID, tickableExecutorsCreator, _tickableExecutorsCreators);

                        break;
                    case AbilityTypes.StanceWithCost:
                    case AbilityTypes.ConsumingStance:
                        break;
                    case AbilityTypes.None:
                    default:
                        break;
                }
            }
        }

        private void RunCreators<T>(int AbilityID, T executorsCreator, Dictionary<int, List<T>> executorsCreators) where T : ExecutorsCreator
        {
            if (_isPaused)
            {
                SetCreatorsOnPause(executorsCreator);
            }
            else
            {
                executorsCreators[AbilityID].Add(executorsCreator);
                executorsCreator.RunCreator();
            }
        }

        private void SetCreatorsOnPause<T>(T executorsCreator) where T : ExecutorsCreator
        {
            if (_executorsCreatorsOnPause.ContainsKey(executorsCreator.Owner))
            {
                _executorsCreatorsOnPause[executorsCreator.Owner].Clear();
                _executorsCreatorsOnPause.Remove(executorsCreator.Owner);
            }

            _executorsCreatorsOnPause.Add(executorsCreator.Owner, executorsCreator);
        }

        private void CheckKey<T>(int abilitiID, Dictionary<int, List<T>> executorsCreators)
        {
            if (!executorsCreators.ContainsKey(abilitiID))
            {
                executorsCreators.Add(abilitiID, new List<T>());
            }
        }

        private void RunPausedCreators()
        {
            foreach (var unit in _executorsCreatorsOnPause)
            {
                if (unit.Value.IsCanceled) continue;

                if (unit.Value is TickableExecutorsCreator tickableExecutorCreator)
                {
                    _tickableExecutorsCreators[tickableExecutorCreator.AbilityModel.AbilitiID].Add(tickableExecutorCreator);
                }
                else if (unit.Value is TimerExecutorsCreator timerExecutorsCreator) // else if для исключения дублирования ITickableExecutor в словаре ITimerExecutor
                {
                    _timerExecutorsCreators[timerExecutorsCreator.AbilityModel.AbilitiID].Add(timerExecutorsCreator);
                }
                else if (unit.Value is InstantExecutorsCreator instantExecutorCreator)
                {
                    _instantExecutorsCreators[instantExecutorCreator.AbilityModel.AbilitiID].Add(instantExecutorCreator);
                }
                unit.Value.RunCreator();
            }

            _executorsCreatorsOnPause.Clear();
        }

        public void SetPause(bool isPaused)
        {
            _isPaused = isPaused;

            if (!isPaused)
            {
                RunPausedCreators();
            }
        }

        public T FindTimerExecutorByTarget<T>(int abilitiID, AbilityTypes abilityType, Unit unit) where T: class, ITimerExecutor
        {
            switch (abilityType)
            {
                case AbilityTypes.Buff:
                case AbilityTypes.Debuff:
                    if (_timerExecutorsCreators.ContainsKey(abilitiID))
                    {
                        var creators = _timerExecutorsCreators[abilitiID];

                        for (int i = 0; i < creators.Count; i++)
                        {
                            if (creators[i].Executors.ContainsKey(unit))
                            {
                                return (T)creators[i].Executors[unit];
                            }
                        }
                    }
                    break;
                case AbilityTypes.HoT:
                case AbilityTypes.DoT:
                    if (_tickableExecutorsCreators.ContainsKey(abilitiID))
                    {
                        var creators = _tickableExecutorsCreators[abilitiID];

                        for (int i = 0; i < creators.Count; i++)
                        {
                            if (creators[i].Executors.ContainsKey(unit))
                            {
                                return (T)creators[i].Executors[unit];
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return null;
        }

        public void CleanUp()
        {
            _selectAbilityTargetController.OnUnitClick -= CreateAbilityExecutorsCreator;
        }

    }
}