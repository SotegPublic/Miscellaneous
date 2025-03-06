using Units;

namespace Abilities
{
    public interface IExecutor
    {
        public int AbilityID { get; }
        public Unit Owner { get; }
        public Unit Target { get; }
        public void UseAbility();
    }
}