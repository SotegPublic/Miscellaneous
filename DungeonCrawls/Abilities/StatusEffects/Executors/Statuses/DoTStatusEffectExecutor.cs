using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;
using UnityEngine;

namespace PassiveAbilities
{
    public class DoTStatusEffectExecutor: TickableStatusEffectExecutor
    {
        public DoTStatusEffectExecutor(Unit target, StatusExecutorModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(target, model, battleController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            _ticksCount -= 1;

            _battleController.AbilitiesActionController.DealDamage(_target, _statusExecutorModel.StatusEffectValue, true, _statusExecutorModel.StatusEffectName);

            if (_ticksCount > 0)
            {
                _tickTimer = new Timer(Execute);
                _tickTimer.SetNewTimerDuration(_statusExecutorModel.StatusEffectFrequency);
                TimersList.AddTimer(_tickTimer);
            }
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}