using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using InGameants;
using Units;
using UnityEngine;

namespace Abilities
{
    public abstract class AoETickableAbilityExecutorWithTimer : AbilityExecutorWithTimer, ITickableExecutor
    {
        protected int _ticksCount;
        protected float _timeBetweenTicks;
        protected float _impact;
        protected Timer _tickTimer;

        public Timer TickTimer => _tickTimer;
        public int TicksCount => _ticksCount;
        public float TimeBetweenTicks => _timeBetweenTicks;
        public float Impact => _impact;

        protected AoETickableAbilityExecutorWithTimer(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(unit, target, point, model, battleController, appliedEffectsUIController)
        {
            _tickTimer = new Timer(Execute);
        }

        public override void UseAbility()
        {
            // Сейчас не используется, так как все параметры идут из кастомных конфигураторов
            //_ticksCount = _abilityModel.ImpactCounts;
            //_timeBetweenTicks = _abilityModel.ImpactDuration / _abilityModel.ImpactCounts;
            //_impact = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _abilityModel.Impact;

            base.UseAbility();
        }

        public override void Clear()
        {
            if (_tickTimer != null)
            {
                _tickTimer.CancelTimer();
                _tickTimer = null;
            }
            _ticksCount = 0;
            _timeBetweenTicks = 0;
            _impact = 0;
            base.Clear();
        }

        public override void ReactivateAbility()
        {
            base.ReactivateAbility();
            _tickTimer.CancelTimer();

            //_ticksCount = _abilityModel.ImpactCounts;
            Execute();
        }
    }
}