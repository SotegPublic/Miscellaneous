namespace PassiveAbilities
{
    public class StatusEffectProtoModel
    {
        public int StatusEffectID { get; private set; }
        public StatusTypes StatusType { get; private set; }
        public float StatusComboValue { get; private set; }
        public float StatusComboRadius { get; private set; }
        public float StatusEffectValue { get; private set; }

        public StatusEffectProtoModel(StatusEffectConfigurator statusEffectConfigurator)
        {
            StatusEffectID = statusEffectConfigurator.StatusEffectID;
            StatusType = statusEffectConfigurator.StatusType;
            StatusComboValue = statusEffectConfigurator.StatusComboValue;
            StatusComboRadius = statusEffectConfigurator.StatusComboRadius;
            StatusEffectValue = statusEffectConfigurator.StatusEffectValue;
        }
    }
}