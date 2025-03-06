using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;

namespace PassiveAbilities
{
    public abstract class TickableStatusEffectExecutor: StatusEffectExecutor
    {
        protected int _ticksCount;
        protected Timer _tickTimer;

        public Timer TickTimer => _tickTimer;
        public int TicksCount => _ticksCount;

        protected TickableStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController): base(target, model, battleController, appliedEffectsUIController)
        {
            _tickTimer = new Timer(Execute);
        }

        public override void ProlongateStatus()
        {
            base.ProlongateStatus();
            _ticksCount = (int)(_statusTimer.TimerDuration.Value / _statusExecutorModel.StatusEffectFrequency);
        }

        public override void ActivateExecutor()
        {
            _ticksCount = (int)(_statusExecutorModel.StatusEffectDuration/ _statusExecutorModel.StatusEffectFrequency);
            base.ActivateExecutor();
        }

        public override void Clear()
        {
            if (_tickTimer != null)
            {
                _tickTimer.CancelTimer();
                _tickTimer = null;
            }
            _ticksCount = 0;
            base.Clear();
        }
    }
}