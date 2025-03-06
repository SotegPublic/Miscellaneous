using UnityEngine;

namespace Equipment
{
    public class ArmorModel: ItemModel
    {
        protected ArmorDefenseModel _armorDefenseModel;
        private ArmorSlotParameter _armorSlotType;
        private ArmorBindingsTypes _armorBindingsType;
        private ArmorTypeParameter _armorType;
        private ArmorMaterialParameter _armorMaterial;
        private SkinnedMeshRenderer _skinnedMeshRenderer;

        public int ArmorBindingsID => (int)_armorBindingsType;
        public int ArmorSlotTypeID => (int)_armorSlotType.Value;
        public ArmorSlotParameter ArmorSlotType => _armorSlotType;
        public ArmorTypeParameter ArmorType => _armorType;
        public ArmorMaterialParameter ArmorMaterial => _armorMaterial;
        public SkinnedMeshRenderer SkinnedMeshRenderer => _skinnedMeshRenderer;
        public ArmorDefenseModel ArmorDefenseModel => _armorDefenseModel;

        public ArmorModel (IArmorConfigurator configurator, LoadableElementsModel loadableElements, 
            ArmorModelParameters armorModelParameters) : base(configurator, loadableElements)
        {
            _armorSlotType = new ArmorSlotParameter(configurator.ArmorSlotType);
            _armorBindingsType = configurator.ArmorBindings;
            _armorType = new ArmorTypeParameter(configurator.ArmorType);
            _armorMaterial = new ArmorMaterialParameter(configurator.ArmorMaterial);
            _skinnedMeshRenderer = _itemObject.GetComponent<SkinnedMeshRenderer>();
            _skinnedMeshRenderer.material = _material;
            _armorDefenseModel = new ArmorDefenseModel(armorModelParameters);
            _itemPower = new ItemPowerParameter(armorModelParameters.ItemPower);
            _itemWeight = new ItemWeightParameter(armorModelParameters.Weight);
            base.LootDropItem.ArmorModel = this;
        }
    }
}