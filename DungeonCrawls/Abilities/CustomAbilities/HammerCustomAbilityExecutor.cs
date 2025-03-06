using BattleSystem;
using Engine;
using Units;
using UnityEngine;

namespace Abilities
{
    public class HammerCustomAbilityExecutor : AbilityExecutor
    {        
        private float _abilityDamage;
        private float _abilityRadius;
        private float _abilityRange;

        public HammerCustomAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model,
                   BattleController battleController, HammerCustomAbilityConfigurator hammerCastomAbilityConfigurator) :
                       base(unit, target, point, model, battleController)
        {
            _abilityDamage = hammerCastomAbilityConfigurator.AbilityDamage;
            _abilityRadius = hammerCastomAbilityConfigurator.AbilityRadius;
            _abilityRange = hammerCastomAbilityConfigurator.AbilityRange;          
        }

        protected override void Execute()
        {
            if ((_castPoint - _owner.transform.position).magnitude > _abilityRange)
            {
                Debug.Log("Not In Hammer Ability Range");
                return;
            }

            var targetsSeeker = new TargetsSeeker();

            var affectedEnemies = targetsSeeker.FindAllEnemyTargetsInRadius(_castPoint, _abilityRadius, _owner.FractionID);
            if(affectedEnemies.Count != 0)
            {
                foreach(var enemy in affectedEnemies)
                {
                    // дамаг
                    _battleController.AbilitiesActionController.DealDamage(_owner, enemy, _abilityDamage, false);
                    _battleController.UnitBattleStatesController.ConfuseTarget(enemy);
                }

                Clear();
            }
            else
            {
                Clear();
            }
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}