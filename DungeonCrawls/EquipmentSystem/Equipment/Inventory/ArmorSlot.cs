using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Equipment
{
    [Serializable]
    public class ArmorSlot : EquipmentSlot<ArmorModel>
    {
        [SerializeField] private List<ArmorBindingModel> _armorBindingModels;
        [SerializeField] private ArmorSlotTypes _slotType;

        public override void EquipItem(ArmorModel newItemModel)
        {
            if (newItemModel.ArmorSlotTypeID != (int)_slotType)
            {
                Debug.Log("Данный предмет не подходит к этому слоту");
                return;
            }
            _itemModel = newItemModel;
            ExpandSkinnedMesh();
            ChangeUnitArmor(true);
            _unit.RebindAnimation();
        }

        public override ArmorModel UnequipItem()
        {
            ArmorModel previousModel = null;
            if (!(_itemModel is null))
            {
                var transformArray = new Transform[_itemModel.SkinnedMeshRenderer.bones.Length];
                _itemModel.SkinnedMeshRenderer.rootBone = null;
                _itemModel.SkinnedMeshRenderer.bones = transformArray;
                ChangeUnitArmor(false);
            }

            previousModel = _itemModel;
            _itemModel = null;

            return previousModel;
        }

        private void ExpandSkinnedMesh()
        {
            _itemModel.ItemObject.transform.position = _holderTransform.transform.position;
            _itemModel.ItemObject.transform.SetParent(_holderTransform);

            var bindingModel = SearchBindingModel(_itemModel.ArmorBindingsID);

            if (bindingModel is null)
            {
                throw new Exception($"Нет подходящего набора костей для развертки");
            }

            _itemModel.SkinnedMeshRenderer.rootBone = bindingModel.RootBone;
            _itemModel.SkinnedMeshRenderer.bones = bindingModel.BonesForBinding;
        }

        private ArmorBindingModel SearchBindingModel(int armorBindingsID)
        {
            ArmorBindingModel armorBindingModel = null;
            for (int i = 0; i < _armorBindingModels.Count; i++)
            {
                if (_armorBindingModels[i].ArmorBindingID == armorBindingsID)
                {
                    armorBindingModel = _armorBindingModels[i];
                }
            }
            return armorBindingModel;
        }

        private void ChangeUnitArmor(bool isEquipting)
        {
            var unitDefenseList = _unit.UnitParameters.UnitDefenseList;
            var itemDefenseList = _itemModel.ArmorDefenseModel.DefenseParametersList;

            for (int i = 0; i < unitDefenseList.Count; i++)
            {
                for (int j = 0; j < itemDefenseList.Count; j++)
                {
                    if (unitDefenseList[i].DamageType == itemDefenseList[j].DefenceType)
                    {
                        float newValue;
                        if (isEquipting)
                        {
                            newValue = unitDefenseList[i].Value + itemDefenseList[j].Value;
                        }
                        else
                        {
                            newValue = unitDefenseList[i].Value - itemDefenseList[j].Value;
                        }
                        unitDefenseList[i].SetValue(newValue);
                    }
                }
            }

            if (isEquipting)
            {
                var value = _unit.UnitParameters.UnitDefense.DefenseParameter.Value + _itemModel.ArmorDefenseModel.Defense.Value;
                _unit.UnitParameters.UnitDefense.DefenseParameter.SetValue(value);
            } else
            {
                var value = _unit.UnitParameters.UnitDefense.DefenseParameter.Value - _itemModel.ArmorDefenseModel.Defense.Value;
                _unit.UnitParameters.UnitDefense.DefenseParameter.SetValue(value);
            }
        }

        public override void ChangeMaterial(AssetReferenceMaterial materialReference)
        {
            base.ChangeMaterial(materialReference);
        }

        protected override void SetNewMaterial(AsyncOperationHandle<Material> newMaterialHandle)
        {
            base.SetNewMaterial(newMaterialHandle);
            _itemModel.SkinnedMeshRenderer.material = _itemModel.LoadableElementsModel.Material;
        }
    }
}