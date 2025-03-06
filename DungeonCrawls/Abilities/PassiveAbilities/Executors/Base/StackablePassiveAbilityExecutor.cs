using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;

namespace PassiveAbilities
{
    public abstract class StackablePassiveAbilityExecutor : PassiveAbilityExecutorWithTimer<IStackablePassiveAbilityExecutor>, IStackablePassiveAbilityExecutor
    {
        protected int _stackCount;

        public int StackCount => _stackCount;
        protected StackablePassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController) :
            base(unit, target, model, battleController, statusEffectsController, appliedEffectsUIController)
        {
        }

        public virtual void StackAbility()
        {
            if(_stackCount < _passiveAbilityProtoModel.MaxStackCount)
            {
                _stackCount++;
                _displayedEffectModel.Stacks.SetValue(_stackCount);
            }
            _passiveAbilityTimer.IncreaseTimerDuration(_passiveAbilityProtoModel.ImpactDuration * 0.5f);
        }

        public override void ActivateExecutor()
        {
            _appliedEffectsUIController.ActivateEffectMarker(_target, _displayedEffectModel);
            base.ActivateExecutor();
        }

        public override void ClearExecutor()
        {
            OnPassiveAbilityExecutorEnded?.Invoke(_target, this);
            base.ClearExecutor();
        }
    }
}