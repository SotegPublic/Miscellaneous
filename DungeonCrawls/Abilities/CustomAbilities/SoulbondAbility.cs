using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;
using UnityEngine;

namespace Abilities
{
    public class SoulbondAbility: TickableAbilityExecutorWithTimer
    {
        private SoulbondAbilityConfigurator _soulbondAbilityConfigurator;
        private float _lastTickImpact;

        public SoulbondAbility(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController, SoulbondAbilityConfigurator soulbondAbilityConfigurator):
            base(unit, target, point, model, battleController, appliedEffectsUIController)
        {
            _tickTimer = new Timer(Execute);
            _soulbondAbilityConfigurator = soulbondAbilityConfigurator;

        }

        public override void UseAbility()
        {
            _abilityTimer.SetNewTimerDuration(_soulbondAbilityConfigurator.Duration);
            TimersList.AddTimer(_abilityTimer);

            _ticksCount = _soulbondAbilityConfigurator.TicksCount;
            _timeBetweenTicks = _soulbondAbilityConfigurator.Duration / _soulbondAbilityConfigurator.TicksCount;
            _impact = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _soulbondAbilityConfigurator.TickDamage;
            _lastTickImpact = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _soulbondAbilityConfigurator.LastTickDamage;

            _battleController.UnitBattleStatesController.TormentTarget(_target);
            _battleController.UnitBattleStatesController.TormentTarget(_owner);
            base.UseAbility();
        }

        protected override void Execute()
        {
            Debug.Log("Soulbond Start!");

            _ticksCount--;

            if(_ticksCount == 0)
            {
                _battleController.AbilitiesActionController.DealDamage(_owner, _target, _lastTickImpact, true);
            }
            else
            {
                _battleController.AbilitiesActionController.DealDamage(_owner, _target, _impact, true);
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
            _battleController.UnitBattleStatesController.RemoveTorment(_target);
            _battleController.UnitBattleStatesController.RemoveTorment(_owner);
            base.Clear();
        }
    }
}