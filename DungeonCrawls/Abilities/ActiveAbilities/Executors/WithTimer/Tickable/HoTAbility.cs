using AppliedEffectsSystem;
using BattleSystem;
using Engine.Timer;
using Units;
using UnityEngine;

namespace Abilities
{
    public class HoTAbility : TickableAbilityExecutorWithTimer
    {
        public HoTAbility(Unit unit, Unit target, Vector3 point, AbilityModel model, BattleController battleController,
            AppliedEffectsUIController appliedEffectsUIController) : base(unit, target, point, model, battleController, appliedEffectsUIController)
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

        public override void Clear()
        {
            base.Clear();
        }
    }
}