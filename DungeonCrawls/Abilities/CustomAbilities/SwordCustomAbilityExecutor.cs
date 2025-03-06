using BattleSystem;
using Units;
using UnityEngine;

namespace Abilities
{
    public class SwordCustomAbilityExecutor : AbilityExecutor
    {
        private float _abilityDamage;
        private float _damageMultiplier;
        private float _additivePercentage;
        private float _abilityRange;

        public SwordCustomAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, 
                    BattleController battleController, SwordCustomAbilityConfigurator swordCastomAbilityConfigurator) :
                        base(unit, target, point, model, battleController)
        {
            _abilityDamage = swordCastomAbilityConfigurator.Damage;
            _damageMultiplier = swordCastomAbilityConfigurator.DamageMultiplier;
            _additivePercentage = swordCastomAbilityConfigurator.AdditivePercentage;
            _abilityRange = swordCastomAbilityConfigurator.AbilityRange;
        }

        protected override void Execute()
        {
            if ((_target.transform.position - _owner.transform.position).magnitude > _abilityRange)
            {
                Debug.Log("Not In Sword Ability Range");
                return;
            }

            var _targetMaxHP = _target.UnitParameters.UnitCharacteristics.Health.MaxValue;
            var _targetCurrentHP = _target.UnitParameters.UnitCharacteristics.Health.Value;

            var damage = _abilityDamage * _damageMultiplier + ((_targetMaxHP - _targetCurrentHP) * (_additivePercentage * 0.01f));

            _battleController.AbilitiesActionController.DealDamage(_owner, _target, damage, false);
            Clear();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}