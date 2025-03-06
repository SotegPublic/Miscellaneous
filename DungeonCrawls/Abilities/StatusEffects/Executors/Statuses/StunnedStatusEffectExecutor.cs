using AppliedEffectsSystem;
using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public class StunnedStatusEffectExecutor : StatusEffectExecutor
    {
        public StunnedStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            ApplyStunning();
        }

        private void ApplyStunning()
        {
            _battleController.UnitBattleStatesController.AbilityUnitStun(_target, true);
        }

        private void DeclineStunning()
        {
            _battleController.UnitBattleStatesController.AbilityUnitStun(_target, false);
        }

        public override void Clear()
        {
            DeclineStunning();
            base.Clear();
        }
    }
}