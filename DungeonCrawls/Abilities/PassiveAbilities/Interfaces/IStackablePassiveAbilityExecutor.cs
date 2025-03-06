namespace PassiveAbilities
{
    public interface IStackablePassiveAbilityExecutor : IPassiveAbilityTimerExecutor<IStackablePassiveAbilityExecutor>
    {
        public int StackCount { get; }
        public void StackAbility();
    }
}