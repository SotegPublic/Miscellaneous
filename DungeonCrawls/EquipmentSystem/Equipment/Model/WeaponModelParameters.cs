

using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public class WeaponModelParameters
    {
        private IWeaponConfigurator _configurator;
        private WeaponsSettingsConfigurator _weaponsSettings;
        private ShieldsSettingsConfigurator _shieldsSettings;

        private float _attack;
        private float _weight;
        private float _speed;
        private float _damage;
        private float _damagePerSecond;
        private float _blockChance;
        private float _itemPower;
        private List<DamageTypeModifier> _weaponDamageTypeModifiers = new List<DamageTypeModifier>();
        private List<DamageParameter> _weaponDamageParameters = new List<DamageParameter>();

        public float Attack => _attack;
        public float Weight => _weight;
        public float Speed => _speed;
        public float Damage => _damage;
        public float DamagePerSecond => _damagePerSecond;
        public float BlockChance => _blockChance;
        public float ItemPower => _itemPower;
        public List<DamageTypeModifier> WeaponDamageTypeModifiers => _weaponDamageTypeModifiers;
        public List<DamageParameter> WeaponDamageParameters => _weaponDamageParameters;

        public WeaponModelParameters(IWeaponConfigurator configurator, WeaponsSettingsConfigurator weaponsSettings,
            ShieldsSettingsConfigurator shieldsSettings)
        {
            _configurator = configurator;
            _weaponsSettings = weaponsSettings;
            _shieldsSettings = shieldsSettings;

            Init();
        }

        public WeaponModelParameters()
        {

        }

        private void Init()
        {
            GetBaseParameters();
            if(!_configurator.IsShield)
            {
                GetDamageTypesParameters();
                GetDPS();
            }
            GetItemPower();
        }

        private void GetItemPower()
        {
            if (_configurator.IsShield)
            {
                var gradeModifier = _shieldsSettings.ItemGradePowerModifiers.Find(x => x.Grade == _configurator.Grade).MainModifier;

                _itemPower = _blockChance * (100 / _weight) * gradeModifier;
            } 
            else
            {
                var gradeModifier = _weaponsSettings.WeaponGradePowerModifiers.Find(x => x.Grade == _configurator.Grade).MainModifier;

                _itemPower = _damagePerSecond * 10 * gradeModifier;
            }
        }

        private void GetDamageTypesParameters()
        {
            var gradeBonusMapping = _weaponsSettings.WeaponBonusDamageMappings
                .Find(x => x.WeaponType == _configurator.WeaponType).GradeBonusMappings
                .Find(y => y.Grade == _configurator.Grade);

            var cuttingDamageModifier = 0f;
            var piercingDamageModifier = 0f;
            var chippingDamageModifier = 0f;
            var crushingDamageModifier = 0f;

            if (gradeBonusMapping != null)
            {
                cuttingDamageModifier = gradeBonusMapping.CuttingDamage;
                piercingDamageModifier = gradeBonusMapping.PiercingDamage;
                chippingDamageModifier = gradeBonusMapping.ChippingDamage;
                crushingDamageModifier = gradeBonusMapping.CrushingDamage;
            }

            var cutting = new DamageTypeModifier(cuttingDamageModifier, DamageTypes.Cutting);
            _weaponDamageTypeModifiers.Add(cutting);
            var piercing = new DamageTypeModifier(piercingDamageModifier, DamageTypes.Piercing);
            _weaponDamageTypeModifiers.Add(piercing);
            var chipping = new DamageTypeModifier(chippingDamageModifier, DamageTypes.Chipping);
            _weaponDamageTypeModifiers.Add(chipping);
            var crushing = new DamageTypeModifier(crushingDamageModifier, DamageTypes.Crushing);
            _weaponDamageTypeModifiers.Add(crushing);

            GetBonusDamage(cuttingDamageModifier, piercingDamageModifier, chippingDamageModifier, crushingDamageModifier);
        }

        private void GetBonusDamage(float cuttingDamageModifier, float piercingDamageModifier,
            float chippingDamageModifier, float crushingDamageModifier)
        {
            float cuttingModifier = GetModifier(DamageTypes.Cutting);
            var piercingModifier = GetModifier(DamageTypes.Piercing);
            var chippingModifier = GetModifier(DamageTypes.Chipping);
            var crushingModifier = GetModifier(DamageTypes.Crushing);

            CalculateAndAddDamageParameter(cuttingDamageModifier, cuttingModifier, DamageTypes.Cutting, "Режущий урон");
            CalculateAndAddDamageParameter(piercingDamageModifier, piercingModifier, DamageTypes.Piercing, "Колющий урон");
            CalculateAndAddDamageParameter(chippingDamageModifier, chippingModifier, DamageTypes.Chipping, "Рубящий урон");
            CalculateAndAddDamageParameter(crushingDamageModifier, crushingModifier, DamageTypes.Crushing, "Дробящий урон");
        }

        private void GetDPS()
        {
            _damagePerSecond = _damage * _speed;
        }

        private void CalculateAndAddDamageParameter(float cuttingDamageModifier, float cuttingModifier,
            DamageTypes damageType, string parameterName)
        {
            var damage = cuttingDamageModifier * cuttingModifier * _weight;
            _weaponDamageParameters.Add(new DamageParameter(damage, damageType, parameterName));
            _damage += damage;
        }

        private float GetModifier(DamageTypes damageType)
        {
            return _weaponsSettings.WeaponTypeModifiers.Find(x => x.DamageType == damageType).Modifier;
        }

        private void GetBaseParameters()
        {
            if(_configurator.IsShield)
            {
                var baseParameters = _shieldsSettings.GradeParametersMappings
                    .Find(x => x.Grade == _configurator.Grade);

                _blockChance = Random.Range(baseParameters.MinValue, baseParameters.MinValue);
                _weight = Random.Range(baseParameters.MinWeight, baseParameters.MaxWeight);
            } 
            else
            {
              //  Debug.Log($"Item grade {_configurator.Grade} slot {_configurator.WeaponType}");
                var baseParameters = _weaponsSettings.WeaponParametersMappings
                    .Find(x => x.WeaponType == _configurator.WeaponType)
                    .GradeParametersMappings.Find(y => y.Grade == _configurator.Grade);

                _attack = Random.Range(baseParameters.MinValue, baseParameters.MaxValue);
                _damage += _attack;
                _weight = Random.Range(baseParameters.MinWeight, baseParameters.MaxWeight);
                _speed = _weaponsSettings.BaseAttackSpeed / _weight;
            }
        }


    }
}