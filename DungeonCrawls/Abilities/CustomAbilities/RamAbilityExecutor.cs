using BattleSystem;
using DG.Tweening;
using Engine;
using Engine.Timer;
using InGameants;
using Units;
using UnityEngine;

namespace Abilities
{
    public class RamAbilityExecutor : AbilityExecutor
    {
        private float _moveSpeed;
        private float _animationSpeed;
        private Timer _executeTimer;
        private ChargeArrowView _chargeEffect;
        private RamAbilityConfigurator _ramAbilityConfigurator;
        private TargetsSeeker _targetsSeeker;
        private DigitalGlobalParameters _digitalGlobalParameters;
        private Vector3 _targetPoint;

        public RamAbilityExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model,
            BattleController battleController, RamAbilityConfigurator ramAbilityConfigurator, Vector3 targetPoint) :
                base(unit, target, point, model, battleController)
        {
            _ramAbilityConfigurator = ramAbilityConfigurator;
            _targetsSeeker = new TargetsSeeker();
            _digitalGlobalParameters = new DigitalGlobalParameters();
            _targetPoint = targetPoint;
        }

        protected override void Execute()
        {
            _executeTimer = new Timer(StartRam);
            _executeTimer.SetNewTimerDuration(5);
            TimersList.AddTimer(_executeTimer);

            _moveSpeed = _owner.NavMeshAgent.speed;
            _animationSpeed = _owner.Animator.speed;
            _owner.SetIsControlledByAbility(true);
            _owner.NavMeshAgent.avoidancePriority -= 20;
            _owner.SetTarget(null);

            var chargeEffectGameObject = Object.Instantiate(_ramAbilityConfigurator.ChargeArrow, _owner.transform);
            _chargeEffect = chargeEffectGameObject.GetComponent<ChargeArrowView>();
            _chargeEffect.ActivateEffect(_owner.transform, 1, 5);
        }

        private void StartRam()
        {
            _executeTimer = null;
            _chargeEffect.DeactivateEffect();

            Debug.Log("RamExecut");

            //зыпуск овнера
            if (_owner.IsDead) return;
            var runPoint = _targetPoint + _owner.transform.forward * _ramAbilityConfigurator.RunDistance;
            _owner.NavMeshAgent.ResetPath();
            _owner.NavMeshAgent.SetDestination(runPoint);
            _owner.NavMeshAgent.speed = _ramAbilityConfigurator.RunSpeed;
            _owner.Animator.speed *= _ramAbilityConfigurator.AnimationSpeedMultiplier; 

            LocalSequenceTimer();
        }

        private void LocalSequenceTimer()
        {

            var deltaTime = 0.5f;
            var sequence = DOTween.Sequence();
            sequence.SetId($"{_owner.gameObject.GetInstanceID()}InRam");
            sequence.AppendInterval(deltaTime);
            sequence.OnComplete(() =>
            {
                CheckCollisions();
            });
        }
        private void CheckCollisions()
        {
            var wallsLayerMask = LayerMask.GetMask("Building");
            var enemyhits = _targetsSeeker.FindAllEnemyTargetsInRadius(_owner.transform.position, _ramAbilityConfigurator.CollisionRadius, _owner.FractionID);
            var navMesh = _owner.NavMeshAgent;

            if (enemyhits.Count > 0)
            {
                foreach (var enemy in enemyhits)
                {
                    _battleController.AbilitiesActionController.DealDamage(_owner, enemy, _ramAbilityConfigurator.CollisionDamage, false);
                    _battleController.UnitBattleStatesController.StunUnit(enemy, _digitalGlobalParameters.StunResistTime);
                }

                Clear();
                return;
            }

            var WallHits = Physics.OverlapSphere(_owner.transform.position, _ramAbilityConfigurator.CollisionRadius * 2, wallsLayerMask);
            
            if (WallHits.Length > 0)
            {
                _battleController.AbilitiesActionController.DealDamage(_owner, _owner, _ramAbilityConfigurator.SelfDamage, false);

                _owner.NavMeshAgent.ResetPath();
                _owner.NavMeshAgent.speed = _moveSpeed;
                _owner.Animator.speed = _animationSpeed;

                _battleController.UnitBattleStatesController.StunUnit(_owner, _digitalGlobalParameters.StunResistTime);

                Clear();
                return;
            }

            if ((navMesh.pathEndPosition - navMesh.transform.position).magnitude <= 3.1f)
            {
                Clear();
                return;
            }

            LocalSequenceTimer();
        }

        public override void Clear()
        {
            DOTween.Kill($"{_owner.gameObject.GetInstanceID()}InRam");

            _owner.NavMeshAgent.ResetPath();
            _owner.NavMeshAgent.speed = _moveSpeed;
            _owner.Animator.speed = _animationSpeed;
            _owner.NavMeshAgent.avoidancePriority += 20;
            _owner.SetIsControlledByAbility(false);
            _owner.SetTarget(_target);
            _owner.OnBattleEnter.Invoke(_owner, _target);

            base.Clear();
        }
    }
}
