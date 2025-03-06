using AppliedEffectsSystem;
using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public abstract class ChangableParametersStatusEffectExecutor: StatusEffectExecutor
    {
        protected float _valueBeforeChange;
        protected ChangableParametersStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            ApplyStatusEffect();
        }

        public override void UpgradeStatus(float newComboValue, float newComboRadius, float newEffectValue)
        {
            DeclineStatusEffect();
            base.UpgradeStatus(newComboValue, newComboRadius, newEffectValue);
            ApplyStatusEffect();
        }

        protected abstract void ApplyStatusEffect();

        protected abstract void DeclineStatusEffect();

        public override void Clear()
        {
            DeclineStatusEffect();
            base.Clear();
        }
    }
}