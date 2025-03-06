using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(ArmorConfigurator), menuName = "Equipment/ArmorConfig", order = 0)]
    public class ArmorConfigurator : ItemConfigurator, IArmorConfigurator
    {
        [Space]
        [Header("Armor properties")]
        [SerializeField] private ArmorSlotTypes _armorSlotType;
        [SerializeField] private ArmorBindingsTypes _armorBindingsType;
        [SerializeField] private ArmorTypes _armorType;
        [SerializeField] private ArmorMaterialTypes _armorMaterial;

        public ArmorBindingsTypes ArmorBindings => _armorBindingsType;
        public ArmorSlotTypes ArmorSlotType => _armorSlotType;
        public ArmorTypes ArmorType => _armorType;
        public ArmorMaterialTypes ArmorMaterial => _armorMaterial;
    }
}