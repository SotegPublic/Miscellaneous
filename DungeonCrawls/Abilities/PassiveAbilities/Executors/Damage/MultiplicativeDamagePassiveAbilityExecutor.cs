using BattleSystem;
using Units;

namespace PassiveAbilities
{
    public class MultiplicativeDamagePassiveAbilityExecutor : PassiveAbilityExecutor
    {
        public MultiplicativeDamagePassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController) : base(unit, target, model, battleController, statusEffectsController)
        {
        }

        protected override void Execute()
        {
            var damage = _owner.UnitParameters.UnitAttack.AttackDamage.Value * _passiveAbilityProtoModel.Impact;

            _battleController.AbilitiesActionController.DealDamage(_owner, _target, damage, false);

            if (_passiveAbilityProtoModel.StatusEffectID != PassiveAbilityProtoModel.NO_STATUS_EFFECT_ID)
            {
                _statusEffectsController.CreateStatusEffect(_target, _passiveAbilityProtoModel.StatusEffectID);
            }
            ClearExecutor();
        }

        public override void ClearExecutor()
        {
            base.ClearExecutor();
        }
    }

}