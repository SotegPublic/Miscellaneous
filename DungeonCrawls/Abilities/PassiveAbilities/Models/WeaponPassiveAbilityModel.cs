using UnityEngine;

namespace PassiveAbilities
{
    public class WeaponPassiveAbilityModel
    {
        private int _abilityID;
        private string _abilityName;
        private string _abilityDiscription;
        private Sprite _abilityIcon;
        private WeaponStatusEffectModel _weaponStatusEffectModel;

        public int AbilityID => _abilityID;
        public string AbilityName => _abilityName;
        public string AbilityDiscription => _abilityDiscription;
        public Sprite AbilityIcon => _abilityIcon;
        public int StatusEffectID => _weaponStatusEffectModel.StatusEffectID;
        public StatusTypes StatusType => _weaponStatusEffectModel.StatusType;

        public WeaponPassiveAbilityModel(PassiveAbilityConfigurator passiveAbilityConfigurator)
        {
            _abilityID = passiveAbilityConfigurator.PassiveAbilityID;
            _abilityName = passiveAbilityConfigurator.PassiveAbilityName;
            _abilityDiscription = passiveAbilityConfigurator.PassiveAbilityDiscription;
            _abilityIcon = passiveAbilityConfigurator.PassiveAbilityIcon;
            if (passiveAbilityConfigurator.PassiveAbilityStatusEffect != null)
            {
                _weaponStatusEffectModel = new WeaponStatusEffectModel(passiveAbilityConfigurator.PassiveAbilityStatusEffect);
            } 
            else
            {
                _weaponStatusEffectModel = new WeaponStatusEffectModel();
            }
        }
    }
}