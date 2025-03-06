using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public class MultiplicativeDamageAbilityExecutor: AbilityExecutor
    {
        public MultiplicativeDamageAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController) :
            base(unit, target, point, model, battleController)
        {
        }

        protected override void Execute()
        {
            var damage = _owner.UnitParameters.UnitAttack.AttackDamage.Value * _abilityModel.Impact;

            _battleController.AbilitiesActionController.DealDamage(_owner, _target, damage, false);
            Clear();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}