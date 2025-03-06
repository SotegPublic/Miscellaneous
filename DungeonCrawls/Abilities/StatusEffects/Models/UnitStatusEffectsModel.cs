using System;
using System.Collections.Generic;

namespace PassiveAbilities
{
    public class UnitStatusEffectsModel
    {
        private Dictionary<StatusTypes, IStatusEffectExecutor> _statusEffects = new Dictionary<StatusTypes, IStatusEffectExecutor>();
        private TornSoulStatusEffectExecutor _tornSoulStatusEffectExecutor;
        private WeakToMagicStatusEffectExecutor _weakToMagicStatusEffectExecutor;

        public Dictionary<StatusTypes, IStatusEffectExecutor> StatusEffects => _statusEffects;
        public TornSoulStatusEffectExecutor TornSoulStatusEffectExecutor => _tornSoulStatusEffectExecutor;
        public WeakToMagicStatusEffectExecutor WeakToMagicStatusEffectExecutor => _weakToMagicStatusEffectExecutor;

        public void AddStatusEffect(IStatusEffectExecutor statusEffect)
        {
            _statusEffects.Add(statusEffect.StatusType, statusEffect);
            statusEffect.OnStatusEnded += RemoveStatusEffect;
            switch(statusEffect.StatusType)
            {
                case StatusTypes.TornSoul:
                    _tornSoulStatusEffectExecutor = (TornSoulStatusEffectExecutor)statusEffect;
                    break;
                case StatusTypes.WeakToMagic:
                    _weakToMagicStatusEffectExecutor = (WeakToMagicStatusEffectExecutor) statusEffect;
                    break;
                default:
                    break;
            }
        }

        public void RemoveStatusEffect(StatusTypes statusType)
        {
            _statusEffects[statusType].OnStatusEnded -= RemoveStatusEffect;
            _statusEffects.Remove(statusType);

            switch (statusType)
            {
                case StatusTypes.TornSoul:
                    _tornSoulStatusEffectExecutor = null;
                    break;
                case StatusTypes.WeakToMagic:
                    _weakToMagicStatusEffectExecutor = null;
                    break;
                default:
                    break;
            }
        }

        public void Clear()
        {
            foreach (var status in _statusEffects)
            {
                status.Value.OnStatusEnded -= RemoveStatusEffect;
                status.Value.Clear();
            }
            _statusEffects.Clear();
            _tornSoulStatusEffectExecutor = null;
            _weakToMagicStatusEffectExecutor = null;
        }
    }
}