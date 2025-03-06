using Engine.Timer;

namespace PassiveAbilities
{
    public interface ITickableStackablePassiveAbilityExecutor : IStackablePassiveAbilityExecutor
    {
        public Timer TickTimer { get; }
        public int TicksCount { get; }
        public float TimeBetweenTicks { get; }
        public float Impact { get; }
    }
}