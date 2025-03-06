using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;

namespace PassiveAbilities
{
    public class RenewalHoTPassiveAbilityExecutor : TickableRenewalPassiveAbilityExecutor
    {
        public RenewalHoTPassiveAbilityExecutor(Unit unit, Unit target, PassiveAbilityProtoModel model, BattleController battleController,
            StatusEffectsController statusEffectsController, AppliedEffectsUIController appliedEffectsUIController)
            : base(unit, target, model, battleController, statusEffectsController, appliedEffectsUIController)
        {
        }

        protected override void Execute()
        {
            _ticksCount -= 1;

            _battleController.AbilitiesActionController.HealUnit(_owner, _target, _impact);

            if (_ticksCount > 0)
            {
                _tickTimer = new Timer(Execute);
                _tickTimer.SetNewTimerDuration(_timeBetweenTicks);
                TimersList.AddTimer(_tickTimer);
            }
        }

        public override void ClearExecutor()
        {
            base.ClearExecutor();
        }
    }
}