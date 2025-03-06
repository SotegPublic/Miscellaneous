using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Equipment
{
    [Serializable]
    public class BackSlot: EquipmentSlot<WeaponModel>
    {
        [SerializeField] private Transform _accessoryHolderTransform;
        [SerializeField] private WeaponSlotTypes _weaponSlotType;

        public WeaponSlotTypes WeaponSlotType => _weaponSlotType;

        public override void EquipItem(WeaponModel newItemModel)
        {
            _itemModel = newItemModel;
            SetBackPrefab(_itemModel.BackSlotPrefab);

            if (_itemModel != null)
            {
                if (_itemModel.Accessory != null)
                {
                    _itemModel.Accessory.transform.position = _accessoryHolderTransform.position;
                    _itemModel.Accessory.transform.SetParent(_accessoryHolderTransform);
                    _itemModel.Accessory.transform.localPosition = Vector3.zero;
                    _itemModel.Accessory.transform.localRotation = Quaternion.identity;
                    _itemModel.Accessory.SetActive(false);
                }

                _itemModel.BackSlotPrefab.SetActive(true);
            }
        }

        public override WeaponModel UnequipItem()
        {
            if (!(_itemModel is null))
            {
                _itemModel.ItemObject.SetActive(true);
                if (_itemModel.Accessory != null)
                {
                    _itemModel.Accessory.SetActive(true);
                }
            }
            var previousModel = _itemModel;

            _itemModel = null;

            return previousModel;
        }

        public void SetBackPrefab(GameObject prefab)
        {
            if (prefab != null)
            {
                prefab.transform.SetParent(_holderTransform);
                prefab.transform.localPosition = Vector3.zero;
                prefab.transform.localRotation = Quaternion.identity;
                prefab.SetActive(false);
            }
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
    }
}