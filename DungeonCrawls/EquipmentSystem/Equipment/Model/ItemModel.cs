using UnityEngine;

namespace Equipment
{
    public class ItemModel
    {
        protected int _itemID;
        protected string _name;
        protected ItemPowerParameter _itemPower;
        protected ItemGradeParameter _grade;
        protected GameObject _itemObject;
        protected Sprite _icon;
        protected Material _material;
        protected ItemWeightParameter _itemWeight;
        protected LoadableElementsModel _loadableElementsModel;
       
        public bool IsDroped;

        public int ItemID => _itemID;
        public string Name => _name;
        public ItemPowerParameter ItemPower => _itemPower;
        public GameObject ItemObject => _itemObject;
        public Sprite Icon => _icon;
        public Material Material => _material;
        public GameObject LootDropObject => _loadableElementsModel.LootItemObject;
        public LootDropItem LootDropItem => _loadableElementsModel.LootDropItem;
        public ItemWeightParameter ItemWeight => _itemWeight;
        public LoadableElementsModel LoadableElementsModel => _loadableElementsModel;
        public ItemGradeParameter Grade => _grade;

        protected ItemModel(IItemConfigurator configurator, LoadableElementsModel loadableElements)
        {
            _itemID = configurator.ItemID;
            _name = configurator.Name;
            _itemObject = loadableElements.ItemObject;
            _icon = loadableElements.Sprite;
            _material = loadableElements.Material;
            _loadableElementsModel = loadableElements;
            _grade = new ItemGradeParameter(configurator.Grade);
        }
    }
}