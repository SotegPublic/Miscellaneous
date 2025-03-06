using System.Collections.Generic;

namespace Equipment
{
    public class ArmorDefenseModel
    {
        private ArmorParameter _armor;
        private DefenseParameter _defense;
        private ItemTypeDefenseParameter _chippingDefence;
        private ItemTypeDefenseParameter _crushingDefence;
        private ItemTypeDefenseParameter _piercingDefence;
        private ItemTypeDefenseParameter _cuttingDefence;
        private List<ItemTypeDefenseParameter> _defenseParametersList = new List<ItemTypeDefenseParameter>();

        public ArmorParameter Armor => _armor;
        public DefenseParameter Defense => _defense;
        public ItemTypeDefenseParameter ChippingDefence => _chippingDefence;
        public ItemTypeDefenseParameter CrushingDefence => _crushingDefence;
        public ItemTypeDefenseParameter PiercingDefence => _piercingDefence;
        public ItemTypeDefenseParameter CuttingDefence => _cuttingDefence;
        public List<ItemTypeDefenseParameter> DefenseParametersList => _defenseParametersList;

        public ArmorDefenseModel(ArmorModelParameters armorModelParameters)
        {
            _armor = new ArmorParameter(armorModelParameters.Armor);
            _defense = new DefenseParameter(armorModelParameters.Defense);

            _chippingDefence = new ItemTypeDefenseParameter(GetDefence(armorModelParameters.ArmorDefenseTypeModifiers, DamageTypes.Chipping),
                DamageTypes.Chipping, "от рубящего");
            _defenseParametersList.Add(_chippingDefence);

            _crushingDefence = new ItemTypeDefenseParameter(GetDefence(armorModelParameters.ArmorDefenseTypeModifiers, DamageTypes.Crushing),
                DamageTypes.Crushing, "от дробящего");
            _defenseParametersList.Add(_crushingDefence);

            _piercingDefence = new ItemTypeDefenseParameter(GetDefence(armorModelParameters.ArmorDefenseTypeModifiers, DamageTypes.Piercing),
                DamageTypes.Piercing, "от колющего");
            _defenseParametersList.Add(_piercingDefence);

            _cuttingDefence = new ItemTypeDefenseParameter(GetDefence(armorModelParameters.ArmorDefenseTypeModifiers, DamageTypes.Cutting),
                DamageTypes.Cutting, "от режущего");
            _defenseParametersList.Add(_cuttingDefence);
        }

        private float GetDefence(List<DamageTypeModifier> armorDefenseTypeModifiers, DamageTypes damageType)
        {
            var modifier = armorDefenseTypeModifiers.Find(x => x.DamageType == damageType).Modifier;
            return modifier;
        }
    }
}