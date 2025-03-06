using AppliedEffectsSystem;
using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public class BlindedStatusEffectExecutor : StatusEffectExecutor
    {
        public BlindedStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            ApplyBlinding();
        }

        private void ApplyBlinding()
        {
            _battleController.UnitBattleStatesController.AbilityUnitBlind(_target, true);
        }

        private void DeclineBlinding()
        {
            _battleController.UnitBattleStatesController.AbilityUnitBlind(_target, false);
        }

        public override void Clear()
        {
            DeclineBlinding();
            base.Clear();
        }
    }
}