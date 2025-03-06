using System;

namespace PassiveAbilities
{
    public interface IStatusEffectExecutor: IStatusEffect
    {
        public Action<StatusTypes> OnStatusEnded { get; set; }
        public void ProlongateStatus();
        public void UpgradeStatus(float newComboValue, float newComboRadius, float newEffectValue);
        public void ActivateExecutor();
        public void Clear();
    }
}