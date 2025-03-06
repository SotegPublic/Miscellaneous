using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;

namespace PassiveAbilities
{
    public class MarkedStatusEffectExecutor : StatusEffectExecutor
    {
        public MarkedStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}