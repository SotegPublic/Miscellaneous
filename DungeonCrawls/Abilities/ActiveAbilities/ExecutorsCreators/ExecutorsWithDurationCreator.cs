using Engine;
using System;
using System.Collections.Generic;
using BattleSystem;
using Units;
using UnitsControlSystem;
using UnityEngine;

namespace Abilities
{
    public class ExecutorsWithDurationCreator<T> : ExecutorsCreator where T: class, ITimerExecutor
    {
        protected Dictionary<Unit, T> _executors = new Dictionary<Unit, T>();

        public Dictionary<Unit, T> Executors => _executors;

        public ExecutorsWithDurationCreator(Unit unit, List<Unit> targets, Vector3 pont, AbilityModel model, BattleController battleController,
                          Action<int, Unit, bool> executeCallback, MoveUnitsController moveUnitsController, AbilityExecutorFactory abilityExecutorFactory,
                          AbilitiesController abilitiesController, StaminaParametersController staminaParametersController) :
                          base(unit, targets, pont, model, battleController, executeCallback, moveUnitsController, abilityExecutorFactory, abilitiesController,
                               staminaParametersController)
        {
        }

        protected override void CreateAndExecuteAbilityExecutors()
        {
            base.CreateAndExecuteAbilityExecutors();
            for (int i = 0; i < _targets.Count; i++)
            {
                T executor = _abilitiesController.FindTimerExecutorByTarget<T>(_abilityModel.AbilitiID, _abilityModel.AbilityType, _targets[i]);

                if (executor is null)
                {
                    switch (_abilityModel.AbilityType)
                    {
                        case AbilityTypes.Buff:
                        case AbilityTypes.Debuff:
                        
                            executor = _abilityExecutorFactory.CreateExecutor<T>(_owner, _targets[i], _targets[i].transform.position, _abilityModel);
                            break;
                        case AbilityTypes.HoT:
                        case AbilityTypes.DoT:

                            executor = _abilityExecutorFactory.CreateExecutor<T>(_owner, _targets[i], _targets[i].transform.position, _abilityModel);
                            break;
                        default:
                            break;
                    }
                }

                executor.OnAbilityTimerEnd += RemoveEndedExecutor;
                _executors.Add(_targets[i], executor);
                executor.UseAbility();
            }
        }

        protected void RemoveEndedExecutor(Unit unit)
        {
            _executors[unit].OnAbilityTimerEnd -= RemoveEndedExecutor;
            _executors.Remove(unit);

            if (_executors.Count == 0)
            {
                Clear();
            }
        }
    }
}