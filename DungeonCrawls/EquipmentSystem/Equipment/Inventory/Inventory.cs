using System;
using System.Collections.Generic;
using Units;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class Inventory
    {
        [Header("ArmorSlots")]
        [SerializeField] private ArmorSlot _glovesSlot;
        [SerializeField] private ArmorSlot _shouldersSlot;
        [SerializeField] private ArmorSlot _helmetSlot;
        [SerializeField] private ArmorSlot _bootsSlot;
        [SerializeField] private ArmorSlot _bodyarmorSlot;

        [Header("Weapon Slots")]
        [SerializeField] private WeaponSlotController _weaponSlotController;

        public Action OnAnyArmorSlotChange;
        public ArmorSlot GlovesSlot => _glovesSlot;
        public ArmorSlot ShouldersSlot => _shouldersSlot;
        public ArmorSlot HelmetSlot => _helmetSlot;
        public ArmorSlot BootsSlot => _bootsSlot;
        public ArmorSlot BodyarmorSlot => _bodyarmorSlot;
        public WeaponSlotController WeaponSlotController => _weaponSlotController;
        public List<ArmorSlot> ArmorSlots { get; private set; }
        public List<WeaponSlot> WeaponMainSlots { get; private set; }
        public List<BackSlot> WeaponBackSlots { get; private set; }

        public ArmorModel EquipArmor(ArmorModel armorModel)
        {
            ArmorModel oldArmor = null;
            switch (armorModel.ArmorSlotType.Value)
            {
                case ArmorSlotTypes.BodyArmor:
                    oldArmor = _bodyarmorSlot.UnequipItem();
                    _bodyarmorSlot.EquipItem(armorModel);
                    break;
                case ArmorSlotTypes.Gloves:
                    oldArmor = _glovesSlot.UnequipItem();
                    _glovesSlot.EquipItem(armorModel);
                    break;
                case ArmorSlotTypes.Boots:
                    oldArmor = _bootsSlot.UnequipItem();
                    _bootsSlot.EquipItem(armorModel);
                    break;
                case ArmorSlotTypes.Helmets:
                    oldArmor = _helmetSlot.UnequipItem();
                    _helmetSlot.EquipItem(armorModel);
                    break;
                case ArmorSlotTypes.Shoulders:
                    oldArmor = _shouldersSlot.UnequipItem();
                    _shouldersSlot.EquipItem(armorModel);
                    break;
                case ArmorSlotTypes.None:
                default:
                    break;
            }

            OnAnyArmorSlotChange?.Invoke();
            return oldArmor;
        }
        
        public void SetUnitParameters(Unit unit)
        {
            _glovesSlot.SetUnitParameters(unit);
            _shouldersSlot.SetUnitParameters(unit);
            _helmetSlot.SetUnitParameters(unit);
            _bootsSlot.SetUnitParameters(unit);
            _bodyarmorSlot.SetUnitParameters(unit);
            _weaponSlotController.LeftHandSlot.SetUnitParameters(unit);
            _weaponSlotController.RightHandSlot.SetUnitParameters(unit);
            _weaponSlotController.TwoHandSlot.SetUnitParameters(unit);

            ArmorSlots = new List<ArmorSlot>();
            AddSlotToList(ArmorSlots, _glovesSlot);
            AddSlotToList(ArmorSlots, _shouldersSlot);
            AddSlotToList(ArmorSlots, _helmetSlot);
            AddSlotToList(ArmorSlots, _bootsSlot);
            AddSlotToList(ArmorSlots, _bodyarmorSlot);

            WeaponMainSlots = new List<WeaponSlot>();
            AddSlotToList(WeaponMainSlots, _weaponSlotController.LeftHandSlot);
            AddSlotToList(WeaponMainSlots, _weaponSlotController.RightHandSlot);
            AddSlotToList(WeaponMainSlots, _weaponSlotController.TwoHandSlot);

            WeaponBackSlots = new List<BackSlot>();
            AddSlotToList(WeaponBackSlots, _weaponSlotController.LeftBackSlot);
            AddSlotToList(WeaponBackSlots, _weaponSlotController.RightBackSlot);
            AddSlotToList(WeaponBackSlots, _weaponSlotController.TwoHandBackSlot);
        }

        public void Release()
        {
            if (_bodyarmorSlot.ItemModel != null)
            {
                _bodyarmorSlot.ItemModel.LoadableElementsModel.Release();
            }
            if (_bootsSlot.ItemModel != null)
            {
                _bootsSlot.ItemModel.LoadableElementsModel.Release();
            }
            if (_glovesSlot.ItemModel != null)
            {
                _glovesSlot.ItemModel.LoadableElementsModel.Release();
            }
            if (_helmetSlot.ItemModel != null)
            {
                _helmetSlot.ItemModel.LoadableElementsModel.Release();
            }
            if (_shouldersSlot.ItemModel != null)
            {
                _shouldersSlot.ItemModel.LoadableElementsModel.Release();
            }
            _weaponSlotController.Release();
        }

        private void AddSlotToList<T1>(List<T1> list, T1 slot) where T1 : class
        {
            if(slot != null)
            {
                list.Add(slot);
            }
        }
    }
}