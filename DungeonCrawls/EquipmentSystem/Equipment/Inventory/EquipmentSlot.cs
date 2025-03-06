using System;
using Units;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Equipment
{
    [Serializable]
    public abstract class EquipmentSlot<T> where T: ItemModel
    {
        protected T _itemModel;
        protected Unit _unit;
        [SerializeField] protected Transform _holderTransform;

        public T ItemModel => _itemModel;

        public virtual void ChangeMaterial(AssetReferenceMaterial materialReference)
        {
            Addressables.LoadAssetAsync<Material>(materialReference).Completed += SetNewMaterial;
        }

        public abstract void EquipItem(T itemModel);
        public abstract T UnequipItem();
        public void SetUnitParameters(Unit unit)
        {
            _unit = unit;
        }
        protected virtual void SetNewMaterial(AsyncOperationHandle<Material> newMaterialHandle)
        {
            var oldMaterialHandle = _itemModel.LoadableElementsModel.MaterialHandle;

            _itemModel.LoadableElementsModel.MaterialHandle = newMaterialHandle;
            _itemModel.LoadableElementsModel.Material = newMaterialHandle.Result;
            Addressables.Release(oldMaterialHandle);
            newMaterialHandle.Completed -= SetNewMaterial;
        }
    }
}