namespace PassiveAbilities
{
    public interface IStatusEffect
    {
        public float StatusEffectValue { get; }
        public float StatusComboValue { get; }
        public StatusTypes StatusType { get; }
    }
}