using Engine.Timer;
using System;
using Units;

namespace Abilities
{
    public interface ITimerExecutor: IExecutor
    {
        public Timer AbilityTimer { get; }
        public void ReactivateAbility();
        public Action<Unit> OnAbilityTimerEnd { get; set; }
    }
}