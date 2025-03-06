using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using System;
using Units;

namespace PassiveAbilities
{
    public abstract class PassiveAbilityExecutorWithTimer<T>: PassiveAbilityExecutor, IPassiveAbilityTimerExecutor<T> where T : IPassiveAbilityTimerExecutor<T>
    {
        protected AppliedEffectsUIController _appliedEffectsUIController;
        protected Timer _passiveAbilityTimer;
        protected DisplayedEffectModelForUI _displayedEffectModel;
        public Timer PassiveAbilityTimer => _passiveAbilityTimer;
        public Action<Unit, T> OnPassiveAbilityExecutorEnded { get; set; }

        protected PassiveAbilityExecutorWithTimer(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController) : base(unit, target, model, battleController, statusEffectsController)
        {
            _passiveAbilityTimer = new Timer(ClearExecutor);
            _appliedEffectsUIController = appliedEffectsUIController;
            _displayedEffectModel = new DisplayedEffectModelForUI(_passiveAbilityProtoModel.PassiveAbilityName, _passiveAbilityProtoModel.PassiveAbilityDescription,
                _passiveAbilityProtoModel.PassiveAbilityEffectIcon, _owner);
        }

        public override void ActivateExecutor()
        {
            base.ActivateExecutor();
        }

        public override void ClearExecutor()
        {
            if (_passiveAbilityTimer != null)
            {
                _passiveAbilityTimer.CancelTimer();
                _passiveAbilityTimer = null;
            }
            _appliedEffectsUIController.DeactivateEffectMarker(_target, _displayedEffectModel);
            base.ClearExecutor();
        }
    }
}