using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    public class ArmorModelParameters
    {
        private IArmorConfigurator _configurator;
        private ArmorSettingsConfigurator _armorSettings;
        private float _defenseBonus;

        private float _armor;
        private float _weight;
        private float _defense;
        private float _itemPower;
        private List<DamageTypeModifier> _armorDefenseTypeModifiers = new List<DamageTypeModifier>();

        public float Armor => _armor;
        public float Weight => _weight;
        public float Defense => _defense;
        public float ItemPower => _itemPower;
        public List<DamageTypeModifier> ArmorDefenseTypeModifiers => _armorDefenseTypeModifiers;

        public ArmorModelParameters(IArmorConfigurator configurator, ArmorSettingsConfigurator armorSettings)
        {
            _configurator = configurator;
            _armorSettings = armorSettings;

            InitArmor();
        }

        private void InitArmor()
        {
            GetArmorBaseParameters();
            GetDefenseTypeModifiers();
            GetItemPower();
        }

        private void GetItemPower()
        {
            if (_configurator.ArmorSlotType == ArmorSlotTypes.BodyArmor)
            {
                var powerModifier = _armorSettings.ItemGradePowerModifiers.Find(x => x.Grade == _configurator.Grade).MainModifier;
                _itemPower = (((_defense * 0.01f) * _defenseBonus * ((100 - _weight) * 0.01f)) * (100 / _weight) * 100) * powerModifier;
            } 
            else
            {
                var powerModifier = _armorSettings.ItemGradePowerModifiers.Find(x => x.Grade == _configurator.Grade).OtherModifier;
                _itemPower = (((_armor * 0.01f) * 1000) - _weight) * powerModifier;
            }
        }

        private void GetDefenseTypeModifiers()
        {
//            Debug.Log($"Item {_configurator.ArmorMaterial} grade {_configurator.Grade} slot {_configurator.ArmorSlotType}");
            var slotDefenseParametersMapping = _armorSettings.ArmorDefenseTypeMappings
                .Find(x => x.ArmorMaterialType == _configurator.ArmorMaterial)
                .GradeDefenseParametersMappings.Find(y => y.Grade == _configurator.Grade)
                .SlotDefenseParametersMappings.Find(z => z.ArmorSlot == _configurator.ArmorSlotType);
            var cuttingDefense = 0f;
            var piercingDefense = 0f;
            var chippingDefense = 0f;
            var crushingDefense = 0f;

            if (slotDefenseParametersMapping != null)
            {
                cuttingDefense = slotDefenseParametersMapping.CuttingDamage;
                piercingDefense = slotDefenseParametersMapping.PiercingDamage;
                chippingDefense = slotDefenseParametersMapping.ChippingDamage;
                crushingDefense = slotDefenseParametersMapping.CrushingDamage;
            }

            var cutting = new DamageTypeModifier(cuttingDefense, DamageTypes.Cutting);
            _armorDefenseTypeModifiers.Add(cutting);
            var piercing = new DamageTypeModifier(piercingDefense, DamageTypes.Piercing);
            _armorDefenseTypeModifiers.Add(piercing);
            var chipping = new DamageTypeModifier(chippingDefense, DamageTypes.Chipping);
            _armorDefenseTypeModifiers.Add(chipping);
            var crushing = new DamageTypeModifier(crushingDefense, DamageTypes.Crushing);
            _armorDefenseTypeModifiers.Add(crushing);

            GetDefense(cuttingDefense, piercingDefense, chippingDefense, crushingDefense);
        }

        private void GetDefense(float cuttingDefense, float piercingDefense, float chippingDefense, float crushingDefense)
        {
            float cuttingDefenseModifier = GetDefenseModifier(DamageTypes.Cutting);
            var piercingDefenseModifier = GetDefenseModifier(DamageTypes.Piercing);
            var chippingDefenseModifier = GetDefenseModifier(DamageTypes.Chipping);
            var crushingDefenseModifier = GetDefenseModifier(DamageTypes.Crushing);

            _defense = _armor + (chippingDefense * chippingDefenseModifier)
                + (crushingDefense * crushingDefenseModifier)
                + (cuttingDefense * cuttingDefenseModifier)
                + (piercingDefense * piercingDefenseModifier);

            _defenseBonus = cuttingDefense + piercingDefense + chippingDefense + crushingDefense;
        }

        private float GetDefenseModifier(DamageTypes damageType)
        {
            return _armorSettings.ItemDamageTypeModifiers.Find(x => x.DamageType == damageType).Modifier;
        }

        private void GetArmorBaseParameters()
        {
     //       Debug.Log($"Item grade {_configurator.Grade} slot {_configurator.ArmorSlotType} {_configurator.ArmorMaterial}");
            var baseParameters = _armorSettings.ArmorParametersMappings
                .Find(x => x.ArmorMaterialType == _configurator.ArmorMaterial)
                .TypeDefenseParametersMappings.Find(y => y.SlotType == _configurator.ArmorSlotType)
                .GradeDefenseParametersMappings.Find(z => z.Grade == _configurator.Grade);

            //Debug.Log(_configurator.ArmorMaterial.ToString() + " " + _configurator.ArmorSlotType.ToString() + " " + _configurator.Grade.ToString());
            _armor = Random.Range(baseParameters.MinValue, baseParameters.MinValue);
            _weight = Random.Range(baseParameters.MinWeight, baseParameters.MaxWeight);
        }
    }
}