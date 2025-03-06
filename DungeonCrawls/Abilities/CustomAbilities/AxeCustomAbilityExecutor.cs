using BattleSystem;
using DG.Tweening;
using Engine;
using InGameants;
using System.Collections.Generic;
using System.Linq;
using Units;
using UnityEngine;

namespace Abilities
{
    public class AxeCustomAbilityExecutor : AbilityExecutor
    {
        private Transform _targetParent;
        private float _moveSpeed;
        private float _runDistance;
        private float _runSpeed;
        private float _collisionRadius;
        private float _wallDamageMultiplier;
        private float _abilityDamage;
        private float _confuseDuration;
        private float _abilityRange;
        private List<Unit> _hitedEnemies;
        private DigitalGlobalParameters _digitalGlobalParameters;
        public AxeCustomAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, 
                    BattleController battleController, AxeCustomAbilityConfigurator axeCastomAbilityConfigurator) :
                        base(unit, target, point, model, battleController)
        {
            _runDistance = axeCastomAbilityConfigurator.RunDistance;
            _runSpeed = axeCastomAbilityConfigurator.RunSpeed;
            _collisionRadius = axeCastomAbilityConfigurator.CollisionRadius;
            _wallDamageMultiplier = axeCastomAbilityConfigurator.WallDamageMultiplier;
            _abilityDamage = axeCastomAbilityConfigurator.AbilityDamage;
            _confuseDuration = axeCastomAbilityConfigurator.ConfuseDuration;
            _abilityRange = axeCastomAbilityConfigurator.AbilityRange;
            _hitedEnemies = new List<Unit>();
            _digitalGlobalParameters = new DigitalGlobalParameters();
        }

        protected override void Execute()
        {
            //if ((_target.transform.position - _owner.transform.position).magnitude > _abilityRange)
            //{
            //    Debug.Log("Not In Axe Ability Range");
            //    return;
            //}

            Debug.Log("AxeExecut");
            //захват таргета
            
            _targetParent = _target.transform.parent;
            _target.SetIsControlledByAbility(true);
            _target.Immobilise(true);
            _target.SetTarget(null);
            _target.transform.SetParent(_owner.transform);
            _target.transform.position = _owner.transform.position;
            _target.transform.localPosition = Vector3.zero;
            var carriedLocalPosition = new Vector3(0.5f, 1, -0.5f);
            _target.transform.localPosition = carriedLocalPosition;
            _target.transform.localRotation = Quaternion.Euler(90, 0, 0);
            _target.NavMeshAgent.enabled = false;

            //зыпуск овнера
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
                sequence.SetId($"{_owner.UnitName}InCharge");
                sequence.AppendInterval(deltaTime);
                sequence.OnComplete(() =>
                {
                    sequenceTimer -= deltaTime;
                    //Debug.Log($"{sequenceTimer}");

                    CheckCollisions();
                });
            }

            void CheckCollisions()
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

                    Debug.Log($"Нанес урон {enemy}");
                    _battleController.AbilitiesActionController.DealDamage(_owner, enemy, _abilityDamage, false);
                    _battleController.UnitBattleStatesController.ConfuseTarget(enemy);
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
                    //нанесение х3 урона цели
                    Debug.Log($"Долбанул апстену {_target}");

                    _battleController.AbilitiesActionController.DealDamage(_owner, _target, _abilityDamage * _wallDamageMultiplier, false);
                    _battleController.UnitBattleStatesController.StunUnit(_target, _digitalGlobalParameters.StunResistTime,3.5f);                    

                    Clear();
                    return;
                }

                LocalSequenceTimer();
            }
        }

        public override void Clear()
        {
            Debug.Log("ClearAxeExecutor");
            Debug.Log($"moveSpeed = {_moveSpeed}");
            Debug.Log($"parent = {_targetParent}");

            DOTween.Kill($"{_owner.UnitName}InCharge");
            _target.transform.localRotation = Quaternion.identity;
            _target.transform.localPosition = Vector3.zero;
            _target.transform.position = _owner.transform.position - (2 * Vector3.forward);
            _target.transform.parent = _targetParent;
            _target.NavMeshAgent.enabled = true;
            _target.SetIsControlledByAbility(false);
            _target.Immobilise(false);

            _owner.SetIsControlledByAbility(false);
            _owner.NavMeshAgent.speed = _moveSpeed;
            _owner.NavMeshAgent.avoidancePriority += 20;
            _owner.NavMeshAgent.ResetPath();
            //_owner.NavMeshAgent.SetDestination(_owner.transform.position);

            _hitedEnemies.Clear();

            base.Clear();
        }
    }

}
