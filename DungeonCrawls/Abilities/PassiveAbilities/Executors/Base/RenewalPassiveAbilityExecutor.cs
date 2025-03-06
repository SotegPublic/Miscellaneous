using Abilities;
using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using System;
using Units;

namespace PassiveAbilities
{
    public abstract class RenewalPassiveAbilityExecutor : PassiveAbilityExecutorWithTimer<IRenewalPassiveAbilityExecutor>, IRenewalPassiveAbilityExecutor
    {
        protected RenewalPassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController) :
            base(unit, target, model, battleController, statusEffectsController, appliedEffectsUIController)
        {
        }

        public override void ActivateExecutor()
        {
            _appliedEffectsUIController.ActivateEffectMarker(_target, _displayedEffectModel);
            _passiveAbilityTimer.SetNewTimerDuration(_passiveAbilityProtoModel.ImpactDuration);
            TimersList.AddTimer(_passiveAbilityTimer);
            base.ActivateExecutor();
        }

        public virtual void ReactivateExecutor()
        {
            _passiveAbilityTimer.CancelTimer();
            _passiveAbilityTimer = new Timer(ClearExecutor);
            _passiveAbilityTimer.SetNewTimerDuration(_passiveAbilityProtoModel.ImpactDuration);
            TimersList.AddTimer(_passiveAbilityTimer);
        }

        public override void ClearExecutor()
        {
            OnPassiveAbilityExecutorEnded?.Invoke(_target, this);
            base.ClearExecutor();
        }
    }
}