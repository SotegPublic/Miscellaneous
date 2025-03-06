using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public class LightningBoltAbilityExecutor : AbilityExecutor
    {
        private LightningBoltAbilityConfigurator _lightningBoltAbilityConfigurator;
        public LightningBoltAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            LightningBoltAbilityConfigurator lightningBoltAbilityConfigurator) : base(unit, target, point, model, battleController)
        {
            _lightningBoltAbilityConfigurator = lightningBoltAbilityConfigurator;
        }

        protected override void Execute()
        {
            Debug.Log("Lightning Start!");

            var damage = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _lightningBoltAbilityConfigurator.Damage;

            _battleController.AbilitiesActionController.DealDamage(_owner, _target, damage, false);
            _battleController.UnitBattleStatesController.ConfuseTarget(_target);
            Clear();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}