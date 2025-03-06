using BattleSystem;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    public class AdditiveHealPassiveAbilityExecutor : PassiveAbilityExecutor
    {
        public AdditiveHealPassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController) : base(unit, target, model, battleController, statusEffectsController)
        {
        }

        protected override void Execute()
        {
            var heal = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _passiveAbilityProtoModel.Impact;

            _battleController.AbilitiesActionController.HealUnit(_owner, _target, heal);

            ClearExecutor();
        }

        public override void ClearExecutor()
        {
            base.ClearExecutor();
        }
    }
}