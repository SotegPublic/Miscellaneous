namespace PassiveAbilities
{
    public class WeaponStatusEffectModel
    {
        private int _statusEffectID;
        private StatusTypes _statusType;

        public int StatusEffectID => _statusEffectID;
        public StatusTypes StatusType => _statusType;

        public WeaponStatusEffectModel()
        {
            _statusEffectID = PassiveAbilityProtoModel.NO_STATUS_EFFECT_ID;
            _statusType = StatusTypes.None;
        }

        public WeaponStatusEffectModel(StatusEffectConfigurator passiveAbilityStatusEffect)
        {
            _statusEffectID = passiveAbilityStatusEffect.StatusEffectID;
            _statusType = passiveAbilityStatusEffect.StatusType;
        }
    }
}