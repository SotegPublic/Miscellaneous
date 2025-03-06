using Units;

namespace PassiveAbilities
{
    public interface IPassiveAbilityExecutor
    {
        public int PassiveAbilityID { get; }
        public Unit Owner { get; }
        public Unit Target { get; }
        public void ActivateExecutor();
        public void ClearExecutor();
    }

}