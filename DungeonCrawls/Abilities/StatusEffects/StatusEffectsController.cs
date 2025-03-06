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
    public class StatusEffectsController: IController, ICleanable
    {
        private StatusBaseParametersKeeper _statusBaseParametersKeeper;
        private StatusEffectsExecutorsFactory _statusEffectsExecutorsFactory;
        private AppliedEffectsUIController _appliedEffectsUIController;
        private Dictionary<int, StatusEffectProtoModel> _statusProtoModels = new Dictionary<int, StatusEffectProtoModel>();
        private Dictionary<Unit, UnitStatusEffectsModel> _unitStatusEffects = new Dictionary<Unit, UnitStatusEffectsModel>();

        public Dictionary<Unit, UnitStatusEffectsModel> UnitStatusEffects => _unitStatusEffects;
        public StatusBaseParametersKeeper StatusBaseParametersKeeper => _statusBaseParametersKeeper;

        public StatusEffectsController(GlobalConfigLoader globalConfigLoader, StatusBaseParametersKeeper statusBaseParametersKeeper, AppliedEffectsUIController appliedEffectsUIController)
        {
            _statusBaseParametersKeeper = statusBaseParametersKeeper;
            _appliedEffectsUIController = appliedEffectsUIController;
            var statusEffectConfigurators = globalConfigLoader.StatusEffectsComposite.StatusEffectConfigurators;

            for (int i = 0; i < statusEffectConfigurators.Count; i++)
            {
                if (_statusProtoModels.ContainsKey(statusEffectConfigurators[i].StatusEffectID)) continue;

                _statusProtoModels.Add(statusEffectConfigurators[i].StatusEffectID, new StatusEffectProtoModel(statusEffectConfigurators[i]));
            }
        }

        public void Init(BattleController battleController)
        {
            _statusEffectsExecutorsFactory = new StatusEffectsExecutorsFactory(battleController, _appliedEffectsUIController, _statusBaseParametersKeeper);
        }

        public void CreateStatusEffect(Unit target, int statusEffectID)
        {
            var model = _statusProtoModels[statusEffectID];

            if(CheckTargetAndExecutor(target, model.StatusType))
            {
                _unitStatusEffects[target].StatusEffects[model.StatusType].ProlongateStatus();
                if(model.StatusEffectValue > _unitStatusEffects[target].StatusEffects[model.StatusType].StatusEffectValue)
                {
                    _unitStatusEffects[target].StatusEffects[model.StatusType].UpgradeStatus(model.StatusComboValue, model.StatusComboRadius, model.StatusEffectValue);
                }
            } else
            {
                var statusEffectExecutor = _statusEffectsExecutorsFactory.CreateExecutor(target, model);
                _unitStatusEffects[target].AddStatusEffect(statusEffectExecutor);
                statusEffectExecutor.ActivateExecutor();
            }
        }

        private void ClearUnitStatusEffectModelWhenUnitDead(Vector3 position, Unit unit)
        {
            unit.OnUnitDeath -= ClearUnitStatusEffectModelWhenUnitDead;
            ClearUnitStatuses(unit);
        }

        private bool CheckTargetAndExecutor(Unit target, StatusTypes statusType)
        {
            if(!_unitStatusEffects.ContainsKey(target))
            {
                _unitStatusEffects.Add(target, new UnitStatusEffectsModel());
                target.OnUnitDeath += ClearUnitStatusEffectModelWhenUnitDead;
                return false;
            }
            else
            {
                if (_unitStatusEffects[target].StatusEffects.ContainsKey(statusType))
                {
                    return true;
                }
                return false;
            }
        }

        public void AOEAroundTargetCombo(Unit target, StatusTypes statusType, int fractionID)
        {
            switch (statusType)
            {
                case StatusTypes.TornSoul:
                    _unitStatusEffects[target].TornSoulStatusEffectExecutor.ActivateTornSoulComboEffect(target, fractionID);
                    break;
                case StatusTypes.WeakToMagic:
                    _unitStatusEffects[target].WeakToMagicStatusEffectExecutor.ActivateWeakToMagicComboEffect(target, fractionID);
                    break;
                default:
                    break;
            }
        }

        public void ActivateTornSoulEffect(Unit attacker, Unit target, float damage)
        {
            _unitStatusEffects[target].TornSoulStatusEffectExecutor.ActivateTornSoulStatusEffect(attacker, damage);
        }

        private void ClearUnitStatuses(Unit target)
        {
            _unitStatusEffects[target].Clear();
            _unitStatusEffects.Remove(target);
        }

        public void CleanUp()
        {
            _statusProtoModels.Clear();

            foreach(var statusEffect in _unitStatusEffects)
            {
                statusEffect.Value.Clear();
            }
            _unitStatusEffects.Clear();
        }
    }
}