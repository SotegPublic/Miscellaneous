using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Equipment
{
    [Serializable]
    public class WeaponSlot: EquipmentSlot<WeaponModel>
    {
        [SerializeField] private Transform _accessoryHolderTransform;
        [SerializeField] private WeaponGripTypes _weaponGripSlot;
        [SerializeField] private WeaponSlotTypes _weaponSlotType;

        public WeaponSlot SecondSlot;

        public override void EquipItem(WeaponModel newItemModel)
        {
            _itemModel = newItemModel;
            if (_itemModel != null)
            {
                SetWeaponPrefab(_itemModel.ItemObject);
                _itemModel.ItemObject.SetActive(true);

                if (_itemModel.Accessory != null)
                {
                    _itemModel.Accessory.transform.SetParent(_accessoryHolderTransform);
                    _itemModel.Accessory.transform.localPosition = Vector3.zero;
                    _itemModel.Accessory.transform.localRotation = Quaternion.identity;
                }

                _unit.UnitParameters.UnitDefense.ShieldBlokChance.SetValue(_unit.UnitParameters.UnitDefense.ShieldBlokChance.Value + newItemModel.ShieldModel.BlockChance.Value);
            }
        }

        public override WeaponModel UnequipItem()
        {
            if (!(_itemModel is null))
            {
                _unit.UnitParameters.UnitDefense.ShieldBlokChance.SetValue(_unit.UnitParameters.UnitDefense.ShieldBlokChance.Value - _itemModel.ShieldModel.BlockChance.Value);
            }
            var previousModel = _itemModel;
            _itemModel = null;

            return previousModel;
        }

        public override void ChangeMaterial(AssetReferenceMaterial materialReference)
        {
            base.ChangeMaterial(materialReference);
        }

        protected override void SetNewMaterial(AsyncOperationHandle<Material> newMaterialHandle)
        {
            base.SetNewMaterial(newMaterialHandle);
            _itemModel.WeaponRenderer.material = _itemModel.LoadableElementsModel.Material;
            _itemModel.BackSlotRenderer.material = _itemModel.LoadableElementsModel.Material;
        }

        public void SetWeaponPrefab(GameObject prefab)
        {
            if (prefab != null)
            {
                prefab.transform.SetParent(_holderTransform);
                prefab.transform.localPosition = Vector3.zero;
                prefab.transform.localRotation = Quaternion.identity;
                prefab.SetActive(false);
            }
        }
    }
}