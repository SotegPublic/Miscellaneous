using Engine.Timer;
using System;
using Units;

namespace PassiveAbilities
{
    public interface IPassiveAbilityTimerExecutor<T>: IPassiveAbilityExecutor where T : IPassiveAbilityExecutor
    {
        public Timer PassiveAbilityTimer { get; }
        public Action<Unit, T> OnPassiveAbilityExecutorEnded { get; set; }
    }
}