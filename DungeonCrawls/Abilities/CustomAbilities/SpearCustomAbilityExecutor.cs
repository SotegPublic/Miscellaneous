using BattleSystem;
using DG.Tweening;
using Engine;
using System.Collections.Generic;
using Units;
using UnityEngine;
using UnityEngine.UI;

namespace Abilities
{
    public class SpearCustomAbilityExecutor : AbilityExecutor
    {        
        private float _moveSpeed;
        private float _runDistance;
        private float _runSpeed;
        private float _collisionRadius;
        private float _abilityDamage;
        private List<Unit> _hitedEnemies;

        public SpearCustomAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model,
                    BattleController battleController, SpearCustomAbilityConfigurator spearCastomAbilityConfigurator) :
                        base(unit, target, point, model, battleController)
        {
            _runDistance = spearCastomAbilityConfigurator.RunDistance;
            _runSpeed = spearCastomAbilityConfigurator.RunSpeed;
            _collisionRadius = spearCastomAbilityConfigurator.CollisionRadius;
            _abilityDamage = spearCastomAbilityConfigurator.AbilityDamage;
            _hitedEnemies = new List<Unit>();
        }
        
        protected override void Execute()
        {
            //зыпуск овнера
            _owner.transform.LookAt(_castPoint);

            Debug.Log(_castPoint);

            var runPoint = _owner.transform.position + _owner.transform.forward * _runDistance;
            _owner.SetIsControlledByAbility(true);
            _owner.NavMeshAgent.avoidancePriority -= 20;
            _owner.SetTarget(null);
            _owner.NavMeshAgent.ResetPath();
            _owner.NavMeshAgent.SetDestination(runPoint);
            _moveSpeed = _owner.NavMeshAgent.speed;
            _owner.NavMeshAgent.speed = _runSpeed;

            var sequenceTimer = 10f;
            var unitsLayerMask = LayerMask.GetMask("Unit");
            var wallsLayerMask = LayerMask.GetMask("Building");
            var targetsSeeker = new TargetsSeeker();

            LocalSequenceTimer();

            void LocalSequenceTimer()
            {
                var deltaTime = 0.1f;
                var sequence = DOTween.Sequence();
                sequence.SetId($"{_owner.UnitName} InCharge");
                sequence.AppendInterval(deltaTime);
                sequence.OnComplete(() =>
                {
                    sequenceTimer -= deltaTime;
                    CheckCollisions(sequenceTimer);                    
                });
            }

            void CheckCollisions(float sequenceTimer)
            {
                var enemyhits = targetsSeeker.FindAllEnemyTargetsInRadius(_owner.transform.position, _collisionRadius, _owner.FractionID);

                foreach (var enemy in enemyhits)
                {
                    //защита от х2
                    if (_hitedEnemies.Contains(enemy))
                    {
                        continue;
                    }

                    _hitedEnemies.Add(enemy);

                    //нанесение урона                    
                    _battleController.AbilitiesActionController.DealDamage(_owner, enemy, _abilityDamage, false);                    
                }

                var navMesh = _owner.NavMeshAgent;
                if (sequenceTimer < 0 || (navMesh.pathEndPosition - navMesh.transform.position).magnitude == 0)
                {
                    Clear();
                    return;
                }

                var WallHits = Physics.OverlapSphere(_owner.transform.position, _collisionRadius, wallsLayerMask);
                if (WallHits.Length > 0)
                {
                    Clear();
                    return;
                }

                LocalSequenceTimer();
            }
        }

        public override void Clear()
        {
            Debug.Log("ClearSpearExecutor");

            DOTween.Kill($"{_owner.UnitName} InCharge");

            _owner.SetIsControlledByAbility(false);
            _owner.NavMeshAgent.speed = _moveSpeed;
            _owner.NavMeshAgent.avoidancePriority += 20;
            _owner.SetTarget(null);
            _owner.NavMeshAgent.ResetPath();
            _owner.NavMeshAgent.SetDestination(_owner.transform.position);

            _hitedEnemies.Clear();

            base.Clear();
        }
    }
}
