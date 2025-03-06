using AppliedEffectsSystem;
using BattleSystem;
using Engine;
using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Utils;
using static UnityEngine.GraphicsBuffer;

namespace PassiveAbilities
{
    public class PassiveAbilitiesController: IController, ICleanable
    {
        private Dictionary<int, PassiveAbilityProtoModel> _passiveAbilityProtoModels = new Dictionary<int, PassiveAbilityProtoModel>();
        private BattleController _battleController;
        private PassiveAbilityAnimationsController _passiveAbilityAnimationsController;
        private PassiveAbilityExecutorFactory _passiveAbilityExecutorFactory;
        private TargetsSeeker _targetsSeeker;

        //ToDo
        //Переделать словари с листами, на словари с классами, внутри которых еще один словарь - для более быстрого поиска
        private Dictionary<Unit, List<IRenewalPassiveAbilityExecutor>> _renewalPassiveExecutors = new Dictionary<Unit, List<IRenewalPassiveAbilityExecutor>>();
        private Dictionary<Unit, List<IStackablePassiveAbilityExecutor>> _stackablePassiveExecutors = new Dictionary<Unit, List<IStackablePassiveAbilityExecutor>>();

        public Action<AudioClip> OnExecutorActivated;

        public PassiveAbilitiesController(StatusEffectsController statusEffectsController, GlobalConfigLoader globalConfig, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController, PassiveAbilityAnimationsController passiveAbilityAnimationsController)
        {
            _battleController = battleController;
            _passiveAbilityAnimationsController = passiveAbilityAnimationsController;
            _targetsSeeker = new TargetsSeeker();

            _battleController.DamageDealController.OnAbilityUse += CreateAbilityExecutor;
            _passiveAbilityExecutorFactory = new PassiveAbilityExecutorFactory(battleController, statusEffectsController, appliedEffectsUIController);

            var passiveAbilitiesConfigurators = globalConfig.PassiveAbilitiesConfiguratorsList.PassiveAbilityConfigurators;

            for (int i = 0; i < passiveAbilitiesConfigurators.Count; i++)
            {
                if (_passiveAbilityProtoModels.ContainsKey(passiveAbilitiesConfigurators[i].PassiveAbilityID)) continue;

                _passiveAbilityProtoModels.Add(passiveAbilitiesConfigurators[i].PassiveAbilityID, new PassiveAbilityProtoModel(passiveAbilitiesConfigurators[i]));
            }
        }

        public void CreateAbilityExecutor(Unit caster, Unit targetActivator, int abilityId)
        {
            if (targetActivator == null) // только для дебага
            {
                Debug.LogError("TARGET NULL");
            }

             if (_passiveAbilityProtoModels.TryGetValue(abilityId, out PassiveAbilityProtoModel model))
            {
                List<Unit> targets = new List<Unit>();
                IPassiveAbilityExecutor executor = default;

                switch (model.PassiveAbilityType)
                {
                    case PassiveAbilityTypes.TargetDamage:
                        executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, targetActivator, model);
                        executor.ActivateExecutor();

                        _passiveAbilityAnimationsController.ActivateVisualEffect(caster, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.MultiplicativeTargetDamage:
                        executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, targetActivator, model);
                        executor.ActivateExecutor();

                        _passiveAbilityAnimationsController.ActivateVisualEffect(caster, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.RenewalTargetDebuff:
                    case PassiveAbilityTypes.Provoke:

                        if(SearchExistingExecutor(caster, targetActivator, model.PassiveAbilityID, _renewalPassiveExecutors, out var renewalExecutor))
                        {
                            renewalExecutor.ReactivateExecutor();
                        } else
                        {
                            renewalExecutor = _passiveAbilityExecutorFactory.CreateExecutor<IRenewalPassiveAbilityExecutor>(caster, targetActivator, model);

                            SearchKeyAndAddExecutor(targetActivator, renewalExecutor, _renewalPassiveExecutors);
                            renewalExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                            renewalExecutor.ActivateExecutor();
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.RandomRenewalBuffAroundCaster:

                        if(_targetsSeeker.FindRandomAllyTargetInRadius(caster.transform.position, model.ImpactRadius, caster.FractionID, out var unitForBuffNearCaster))
                        {
                            if (SearchExistingExecutor(caster, unitForBuffNearCaster, model.PassiveAbilityID, _renewalPassiveExecutors, out var renewalRandomBuffExecutor))
                            {
                                renewalRandomBuffExecutor.ReactivateExecutor();
                            }
                            else
                            {
                                renewalRandomBuffExecutor = _passiveAbilityExecutorFactory.CreateExecutor<IRenewalPassiveAbilityExecutor>(caster, unitForBuffNearCaster, model);

                                SearchKeyAndAddExecutor(unitForBuffNearCaster, renewalRandomBuffExecutor, _renewalPassiveExecutors);
                                renewalRandomBuffExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                                renewalRandomBuffExecutor.ActivateExecutor();
                            }
                            _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        }
                        break;

                    case PassiveAbilityTypes.RandomRenewalBuffAroundTarget:

                        if(_targetsSeeker.FindRandomAllyTargetInRadius(targetActivator.transform.position, model.ImpactRadius, caster.FractionID, out var unitForBuffNearTarget))
                        {

                            if (SearchExistingExecutor(caster, unitForBuffNearTarget, model.PassiveAbilityID, _renewalPassiveExecutors, out var renewalRandomBuffExecutor))
                            {
                                renewalRandomBuffExecutor.ReactivateExecutor();
                            }
                            else
                            {
                                renewalRandomBuffExecutor = _passiveAbilityExecutorFactory.CreateExecutor<IRenewalPassiveAbilityExecutor>(caster, unitForBuffNearTarget, model);

                                SearchKeyAndAddExecutor(unitForBuffNearTarget, renewalRandomBuffExecutor, _renewalPassiveExecutors);
                                renewalRandomBuffExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                                renewalRandomBuffExecutor.ActivateExecutor();
                            }
                            _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        }
                        break;

                    case PassiveAbilityTypes.AOEProvoke:

                        targets = _targetsSeeker.FindAllEnemyTargetsInRadius(caster.transform.position, model.ImpactRadius, caster.FractionID);

                        for(int i = 0; i < targets.Count; i++)
                        {
                            if (SearchExistingExecutor(caster, targets[i], model.PassiveAbilityID, _renewalPassiveExecutors, out var aoeRenewalExecutor))
                            {
                                aoeRenewalExecutor.ReactivateExecutor();
                            }
                            else
                            {
                                aoeRenewalExecutor = _passiveAbilityExecutorFactory.CreateExecutor<IRenewalPassiveAbilityExecutor>(caster, targetActivator, model);

                                SearchKeyAndAddExecutor(targets[i], aoeRenewalExecutor, _renewalPassiveExecutors);
                                aoeRenewalExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                                aoeRenewalExecutor.ActivateExecutor();
                            }
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.AoEDamageAroundCaster:

                        targets = _targetsSeeker.FindAllEnemyTargetsInRadius(caster.transform.position, model.ImpactRadius, caster.FractionID);
                        foreach (var target in targets)
                        {
                            executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, target, model);
                            executor.ActivateExecutor();
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(caster, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.AoEDamageAroundTarget:

                        targets = _targetsSeeker.FindAllEnemyTargetsInRadius(targetActivator.transform.position, model.ImpactRadius, caster.FractionID);
                        foreach (var target in targets)
                        {
                            executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, target, model);
                            executor.ActivateExecutor();
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        break;


                    case PassiveAbilityTypes.AoEHealAroundTarget:

                        targets = _targetsSeeker.FindAllAllyTargetsInRadius(targetActivator.transform.position, model.ImpactRadius, caster.FractionID);
                        foreach (var target in targets)
                        {
                            executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, target, model);
                            executor.ActivateExecutor();
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.AoEHealAroundCaster:

                        targets = _targetsSeeker.FindAllAllyTargetsInRadius(caster.transform.position, model.ImpactRadius, caster.FractionID);
                        foreach (var target in targets)
                        {
                            executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, target, model);
                            executor.ActivateExecutor();
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(caster, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.RandomTargetHealAroundCaster:

                        if(_targetsSeeker.FindRandomAllyTargetInRadius(caster.transform.position, model.ImpactRadius, caster.FractionID, out var unitForHealNearCaster))
                        {
                            executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, unitForHealNearCaster, model);
                            executor.ActivateExecutor();
                            _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        }
                        break;

                    case PassiveAbilityTypes.RandomTargetHealAroundTarget:

                        if (_targetsSeeker.FindRandomAllyTargetInRadius(caster.transform.position, model.ImpactRadius, caster.FractionID, out var unitForHealNearTarget))
                        {
                            executor = _passiveAbilityExecutorFactory.CreateExecutor<IPassiveAbilityExecutor>(caster, unitForHealNearTarget, model);
                            executor.ActivateExecutor();
                            _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        }
                        break;

                    case PassiveAbilityTypes.RenewalTargetDoT:

                        if (SearchExistingExecutor(caster, targetActivator, model.PassiveAbilityID, _renewalPassiveExecutors, out var dotExecutor))
                        {
                            dotExecutor.ReactivateExecutor();
                        }
                        else
                        {
                            dotExecutor = _passiveAbilityExecutorFactory.CreateExecutor<ITickableRenewalPassiveAbilityExecutor>(caster, targetActivator, model);

                            SearchKeyAndAddExecutor(targetActivator, dotExecutor, _renewalPassiveExecutors);
                            dotExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                            dotExecutor.ActivateExecutor();
                        }

                        _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        break;

                    case PassiveAbilityTypes.RenewalHoTAroundCaster:

                        if(_targetsSeeker.FindRandomAllyTargetInRadius(targetActivator.transform.position, model.ImpactRadius, caster.FractionID, out var unitForHoTNearCaster))
                        {

                            if (SearchExistingExecutor(caster, unitForHoTNearCaster, model.PassiveAbilityID, _renewalPassiveExecutors, out var renewalRandomHotExecutor))
                            {
                                renewalRandomHotExecutor.ReactivateExecutor();
                            }
                            else
                            {
                                renewalRandomHotExecutor = _passiveAbilityExecutorFactory.CreateExecutor<ITickableRenewalPassiveAbilityExecutor>(caster, unitForHoTNearCaster, model);

                                SearchKeyAndAddExecutor(unitForHoTNearCaster, renewalRandomHotExecutor, _renewalPassiveExecutors);
                                renewalRandomHotExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                                renewalRandomHotExecutor.ActivateExecutor();
                            }
                            _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        }
                        break;

                    case PassiveAbilityTypes.RenewalHoTAroundTarget:

                        if(_targetsSeeker.FindRandomAllyTargetInRadius(targetActivator.transform.position, model.ImpactRadius, caster.FractionID, out var unitForHoTNearTarget))
                        {
                            if (SearchExistingExecutor(caster, unitForHoTNearTarget, model.PassiveAbilityID, _renewalPassiveExecutors, out var renewalRandomHotExecutor))
                            {
                                renewalRandomHotExecutor.ReactivateExecutor();
                            }
                            else
                            {
                                renewalRandomHotExecutor = _passiveAbilityExecutorFactory.CreateExecutor<ITickableRenewalPassiveAbilityExecutor>(caster, unitForHoTNearTarget, model);

                                SearchKeyAndAddExecutor(unitForHoTNearTarget, renewalRandomHotExecutor, _renewalPassiveExecutors);
                                renewalRandomHotExecutor.OnPassiveAbilityExecutorEnded += ClearOnExecutorEnd;
                                renewalRandomHotExecutor.ActivateExecutor();
                            }
                            _passiveAbilityAnimationsController.ActivateVisualEffect(targetActivator, model.AnimationEffectID);
                        }
                        break;

                    case PassiveAbilityTypes.StackableRandomBuffAroundCaster:
                        break;
                    case PassiveAbilityTypes.StackableRandomBuffAroundTarget:
                        break;
                    case PassiveAbilityTypes.StackableDebuff:
                        break;
                    case PassiveAbilityTypes.StackableTargetDoT:
                        break;
                    case PassiveAbilityTypes.None:
                    default:
                        break;
                }

                OnExecutorActivated?.Invoke(model.PassiveAbilityAudioClip);
            }
        }

        private void SearchKeyAndAddExecutor<T>(Unit target, T executor,
            Dictionary<Unit, List<T>> collectionForSearch) where T : IPassiveAbilityExecutor
        {
            if(collectionForSearch.ContainsKey(target))
            {
                collectionForSearch[target].Add(executor);
            } else
            {
                collectionForSearch.Add(target, new List<T> {executor});
                target.OnUnitDeath += ClearOnUnitDeath;
            }
        }

        private bool SearchExistingExecutor<T>(Unit caster, Unit targetActivator, int passiveAbilityID, 
            Dictionary<Unit, List<T>> collectionForSearch, out T executor) where T: IPassiveAbilityExecutor
        {
            executor = default;

            if(collectionForSearch.ContainsKey(targetActivator))
            {
                executor = collectionForSearch[targetActivator].Find(searchableExecutor => searchableExecutor.PassiveAbilityID == passiveAbilityID &&
                    searchableExecutor.Owner == caster);

                if(executor != null)
                {
                    return true;
                } 
                else
                {
                    return false;
                }
            }

            return false;
        }


        private void ClearOnExecutorEnd(Unit unit, IRenewalPassiveAbilityExecutor executor)
        {
            if (!unit.IsDead) // если юнит мертв, все экзекуторы итак будут очищены
            {
                _renewalPassiveExecutors[unit].Remove(executor);
            }
            executor.OnPassiveAbilityExecutorEnded -= ClearOnExecutorEnd;
        }

        private void ClearOnExecutorEnd(Unit unit, IStackablePassiveAbilityExecutor executor)
        {
            if (!unit.IsDead) // если юнит мертв, все экзекуторы итак будут очищены
            {
                _stackablePassiveExecutors[unit].Remove(executor);
            }
            executor.OnPassiveAbilityExecutorEnded -= ClearOnExecutorEnd;
        }

        private void ClearOnUnitDeath(Vector3 position, Unit unit)
        {
            if (_renewalPassiveExecutors.ContainsKey(unit))
            {
                for (int i = 0; i < _renewalPassiveExecutors[unit].Count; i++)
                {
                    _renewalPassiveExecutors[unit][i].ClearExecutor();
                }
                _renewalPassiveExecutors[unit].Clear();
                _renewalPassiveExecutors.Remove(unit);
            }

            if(_stackablePassiveExecutors.ContainsKey(unit))
            {
                for (int i = 0; i < _stackablePassiveExecutors[unit].Count; i++)
                {
                    _stackablePassiveExecutors[unit][i].ClearExecutor();
                }
                _stackablePassiveExecutors[unit].Clear();
                _stackablePassiveExecutors.Remove(unit);
            }

            unit.OnUnitDeath -= ClearOnUnitDeath;
        }

        public void CleanUp()
        {
            _battleController.DamageDealController.OnAbilityUse -= CreateAbilityExecutor;

            foreach (var protomodel in _passiveAbilityProtoModels)
            {
                protomodel.Value.Clear();
            }
            _passiveAbilityProtoModels.Clear();

            foreach(var executor in _renewalPassiveExecutors)
            {
                executor.Value.Clear();
            }
            _renewalPassiveExecutors.Clear();

            foreach (var executor in _stackablePassiveExecutors)
            {
                executor.Value.Clear();
            }
            _stackablePassiveExecutors.Clear();
        }
    }
}