using Engine.Timer;

namespace Abilities
{
    public interface ITickableExecutor: ITimerExecutor
    {
        public Timer TickTimer { get; }
        public int TicksCount { get; }
        public float TimeBetweenTicks { get; }
        public float Impact { get; }
    }
}