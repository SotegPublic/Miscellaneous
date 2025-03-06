using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using System;
using Units;

namespace PassiveAbilities
{
    public abstract class StatusEffectExecutor: IStatusEffectExecutor
    {
        protected Unit _target;
        protected StatusExecutorModel _statusExecutorModel;
        protected BattleController _battleController;
        protected AppliedEffectsUIController _appliedEffectsUIController;
        protected DisplayedEffectModelForUI _displayedEffectModel;
        protected Timer _statusTimer;

        public Action<StatusTypes> OnStatusEnded { get; set; }
        public Unit Target => _target;
        public Timer PassiveAbilityTimer => _statusTimer;
        public float StatusEffectValue => _statusExecutorModel.StatusEffectValue;
        public float StatusComboValue => _statusExecutorModel.StatusComboValue;
        public StatusTypes StatusType => _statusExecutorModel.StatusType;

        protected StatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController, AppliedEffectsUIController appliedEffectsUIController)
        {
            _target = target;
            _statusExecutorModel = model;
            _battleController = battleController;
            _appliedEffectsUIController = appliedEffectsUIController;
            _statusTimer = new Timer(Clear);
            _displayedEffectModel = new DisplayedEffectModelForUI(_statusExecutorModel.StatusEffectName, _statusExecutorModel.StatusEffectDescription,
                _statusExecutorModel.StatusEffectIcon, null);
        }

        public virtual void ProlongateStatus()
        {
            _statusTimer.IncreaseTimerDuration(_statusExecutorModel.StatusEffectProlongationTime);
        }

        public virtual void UpgradeStatus(float newComboValue, float newComboRadius, float newEffectValue)
        {
            _statusExecutorModel.UpgrageModel(newComboValue, newComboRadius, newEffectValue);
        }

        public virtual void ActivateExecutor()
        {
            _appliedEffectsUIController.ActivateEffectMarker(_target, _displayedEffectModel);
            _statusTimer.SetNewTimerDuration(_statusExecutorModel.StatusEffectDuration);
            TimersList.AddTimer(_statusTimer);

            Execute();
        }

        public virtual void Clear()
        {
            if (_statusTimer != null)
            {
                _statusTimer.CancelTimer();
                _statusTimer = null;
            }
            _appliedEffectsUIController.DeactivateEffectMarker(_target, _displayedEffectModel);
            OnStatusEnded?.Invoke(StatusType);
        }

        protected abstract void Execute();
    }
}