using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;

namespace PassiveAbilities
{
    public abstract class TickableStackablePassiveAbilityExecutor : StackablePassiveAbilityExecutor, ITickableStackablePassiveAbilityExecutor
    {
        protected int _ticksCount;
        protected float _timeBetweenTicks;
        protected float _impact;
        protected Timer _tickTimer;

        public Timer TickTimer => _tickTimer;
        public int TicksCount => _ticksCount;
        public float TimeBetweenTicks => _timeBetweenTicks;
        public float Impact => _impact;

        protected TickableStackablePassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController)
            : base(unit, target, model, battleController, statusEffectsController, appliedEffectsUIController)
        {
            _tickTimer = new Timer(Execute);
        }

        public override void StackAbility()
        {
            base.StackAbility();

            _ticksCount = (int)(_passiveAbilityTimer.TimerDuration.Value / _timeBetweenTicks);
            if (_stackCount < _passiveAbilityProtoModel.MaxStackCount)
            {
                _impact = _impact + _impact;
            }
        }

        public override void ActivateExecutor()
        {
            _ticksCount = _passiveAbilityProtoModel.ImpactCounts;
            _timeBetweenTicks = _passiveAbilityProtoModel.ImpactDuration / _passiveAbilityProtoModel.ImpactCounts;
            _impact = _owner.UnitParameters.UnitAttack.AttackDamage.Value + _passiveAbilityProtoModel.Impact;

            base.ActivateExecutor();
        }

        public override void ClearExecutor()
        {
            if (_tickTimer != null)
            {
                _tickTimer.CancelTimer();
                _tickTimer = null;
            }
            _ticksCount = 0;
            _timeBetweenTicks = 0;
            _impact = 0;
            base.ClearExecutor();
        }
    }
}