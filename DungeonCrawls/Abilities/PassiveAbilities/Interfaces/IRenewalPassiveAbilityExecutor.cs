namespace PassiveAbilities
{
    public interface IRenewalPassiveAbilityExecutor : IPassiveAbilityTimerExecutor<IRenewalPassiveAbilityExecutor>
    {
        public void ReactivateExecutor();
    }
}