using AppliedEffectsSystem;
using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public class TauntPassiveAbilityExecutor: RenewalPassiveAbilityExecutor
    {
        public TauntPassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController) :
            base(unit, target, model, battleController, statusEffectsController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            Taunt();
        }

        private void Taunt()
        {
            if(!_target.IsDead)
            {
                _battleController.ManualAgroIncrease(_owner, _target, 999);
            }
        }

        private void Detaunt()
        {
            if (!_target.IsDead)
            {
                _battleController.ManualAgroIncrease(_owner, _target, -999);
            }
        }

        public override void ClearExecutor()
        {
            Detaunt();
            base.ClearExecutor();
        }
    }
}