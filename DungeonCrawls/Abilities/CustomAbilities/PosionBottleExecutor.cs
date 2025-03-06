using AppliedEffectsSystem;
using BattleSystem;
using Engine;
using Engine.Timer;
using System;
using System.Collections.Generic;
using Units;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Abilities
{
    public class PosionBottleExecutor: AoETickableAbilityExecutorWithTimer
    {
        private TargetsSeeker _targetsSeeker;
        private List<Vector3> _bottleExplosionPoints = new List<Vector3>();
        private PosionBottleAbilityConfigurator _posionBottleAbilityConfigurator;
        private List<GameObject> _voidZones = new List<GameObject>();

        public PosionBottleExecutor(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController, PosionBottleAbilityConfigurator posionBottleAbilityConfigurator): base(unit, target, point, model, battleController, appliedEffectsUIController)
        {
            _targetsSeeker = new TargetsSeeker();
            _posionBottleAbilityConfigurator = posionBottleAbilityConfigurator;
        }

        public override void UseAbility()
        {
            Debug.Log("Potion Start!");

            _abilityTimer.SetNewTimerDuration(_posionBottleAbilityConfigurator.Duration);
            TimersList.AddTimer(_abilityTimer);

            for (int i = 0; i < _posionBottleAbilityConfigurator.BottleCount; i++)
            {
                var bottleExplosionPoint = GetExplosionPoint();
                _bottleExplosionPoints.Add(bottleExplosionPoint);

                var voidZone = GameObject.Instantiate(_posionBottleAbilityConfigurator.VoidZonePref, bottleExplosionPoint, Quaternion.identity);
                _voidZones.Add(voidZone);
                var voidZoneEffect = voidZone.GetComponent<ZoneHealView>();
                voidZoneEffect.ActivateEffect(voidZone.transform, 2, _posionBottleAbilityConfigurator.Duration);
            }

            _ticksCount = _posionBottleAbilityConfigurator.TicksCount;
            _timeBetweenTicks = _posionBottleAbilityConfigurator.Duration / _posionBottleAbilityConfigurator.TicksCount;
            _impact = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _posionBottleAbilityConfigurator.TickDamage;

            base.UseAbility();
        }

        private Vector3 GetExplosionPoint()
        {
            var tile = _targetsSeeker.FindRandomFloorTileInRadius(_owner.transform.position, _posionBottleAbilityConfigurator.ThrowRadius);
            var explosionPoint = new Vector3(tile.x + Random.Range(-0.49f, 0.5f), tile.y + 0.5f, tile.z + Random.Range(-0.49f, 0.5f));

            for (int i = 0; i < _bottleExplosionPoints.Count; i++)
            {
                if (Vector3.Distance(_bottleExplosionPoints[i], explosionPoint) < (_posionBottleAbilityConfigurator.DamageRadius * 0.5f))
                {
                    explosionPoint.x = explosionPoint.x + Random.Range(-_posionBottleAbilityConfigurator.DamageRadius, _posionBottleAbilityConfigurator.DamageRadius * 0.5f);
                    explosionPoint.z = explosionPoint.z + Random.Range(-_posionBottleAbilityConfigurator.DamageRadius, _posionBottleAbilityConfigurator.DamageRadius * 0.5f);
                }
            }

            return explosionPoint;
        }

        protected override void Execute()
        {
            _ticksCount--;

            for (int i = 0; i < _bottleExplosionPoints.Count; i++)
            {
                var targets = _targetsSeeker.FindAllEnemyTargetsInRadius(_bottleExplosionPoints[i], _posionBottleAbilityConfigurator.DamageRadius, _owner.FractionID);

                for (int j = 0; j < targets.Count; j++)
                {
                    _battleController.AbilitiesActionController.DealDamage(_owner, targets[j], _impact, true);
                }
            }

            if (_ticksCount > 0)
            {
                _tickTimer = new Timer(Execute);
                _tickTimer.SetNewTimerDuration(_timeBetweenTicks);
                TimersList.AddTimer(_tickTimer);
            }
        }

        public override void Clear()
        {
            foreach(var zone in _voidZones)
            {
                GameObject.Destroy(zone);
            }

            base.Clear();
        }
    }
}