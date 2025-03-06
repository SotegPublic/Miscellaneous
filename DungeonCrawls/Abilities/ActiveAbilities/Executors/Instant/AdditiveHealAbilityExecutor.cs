using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AdditiveHealAbilityExecutor : AbilityExecutor
    {
        public AdditiveHealAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController) :
            base(unit, target, point, model, battleController)
        {
        }

        protected override void Execute()
        {
            var heal = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _abilityModel.Impact;

            _battleController.AbilitiesActionController.HealUnit(_owner, _target, heal);
            Clear();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}