using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class WeaponSlotController
    {
        [SerializeField] private WeaponSlot _rightHandSlot;
        [SerializeField] private WeaponSlot _leftHandSlot;
        [SerializeField] private WeaponSlot _twoHandSlot;
        [SerializeField] private BackSlot _leftBackSlot;
        [SerializeField] private BackSlot _rightBackSlot;
        [SerializeField] private BackSlot _twoHandBackSlot;

        private WeaponSlot _activeSlot;

        public Action<float> OnNeedRecalculateStats;
        public Action OnAnyWeaponSlotChange;
        public bool _isMainSlotActive = true;

        public WeaponSlot RightHandSlot => _rightHandSlot;
        public WeaponSlot LeftHandSlot => _leftHandSlot;
        public WeaponSlot TwoHandSlot => _twoHandSlot;
        public BackSlot LeftBackSlot => _leftBackSlot;
        public BackSlot RightBackSlot => _rightBackSlot;
        public BackSlot TwoHandBackSlot => _twoHandBackSlot;
        public WeaponSlot ActiveSlot => _activeSlot;

        public List<WeaponModel> EquipWeapon (WeaponModel newWeaponModel)
        {
            List<WeaponModel> previousWeapons = new List<WeaponModel>();

            if(newWeaponModel is null)
            {
                OnNeedRecalculateStats?.Invoke(0);
                OnAnyWeaponSlotChange?.Invoke();
                return previousWeapons;
            }

            switch (newWeaponModel.WeaponGripTypeID)
            {
                case 1:
                    _rightBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    previousWeapons.Add(_twoHandSlot.UnequipItem());
                    previousWeapons.Add(_rightHandSlot.UnequipItem());
                    _rightHandSlot.EquipItem(newWeaponModel);
                    if(_leftHandSlot.ItemModel==null)
                    {
                        _activeSlot = _rightHandSlot;
                    } else
                    {
                        if (_leftHandSlot.ItemModel.WeaponType.Value != WeaponTypes.BattleStaff && _leftHandSlot.ItemModel.WeaponType.Value != WeaponTypes.HealStaff)
                        {
                            _activeSlot = _rightHandSlot;
                        } else
                        {
                            _activeSlot = _leftHandSlot;
                        }
                    }
                    break;
                case 2:
                    _twoHandBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    previousWeapons.Add(_leftHandSlot.UnequipItem());
                    previousWeapons.Add(_rightHandSlot.UnequipItem());
                    previousWeapons.Add(_twoHandSlot.UnequipItem());
                    _twoHandSlot.EquipItem(newWeaponModel);
                    _activeSlot = _twoHandSlot;
                    break;
                case 3:
                    _leftBackSlot.SetBackPrefab(newWeaponModel.BackSlotPrefab);
                    previousWeapons.Add(_twoHandSlot.UnequipItem());
                    previousWeapons.Add(_leftHandSlot.UnequipItem());
                    _leftHandSlot.EquipItem(newWeaponModel);
                    if(newWeaponModel.WeaponType.Value == WeaponTypes.BattleStaff || newWeaponModel.WeaponType.Value == WeaponTypes.HealStaff)
                    {
                        _activeSlot = _leftHandSlot;
                    } else
                    {
                        _activeSlot = _rightHandSlot;
                    }
                    break;
                default:
                    break;
            }

            if(newWeaponModel.IsShield == false)
            {
                OnNeedRecalculateStats?.Invoke(newWeaponModel.WeaponDamageModel.TotalDamage.Value);
            }

            OnAnyWeaponSlotChange?.Invoke();
            return previousWeapons;
        }

        public List<WeaponModel> EquipWeaponOnBack(WeaponModel newWeaponModel) // ToDo - переделать логику эквипа двуручника в слот для двуручника за спиной
        {
            List<WeaponModel> previousWeapons = new List<WeaponModel>();

            if (newWeaponModel is null)
            {
                OnAnyWeaponSlotChange?.Invoke();
                return previousWeapons;
            }

            switch (newWeaponModel.WeaponGripTypeID)
            {
                case 1:
                    _rightHandSlot.SetWeaponPrefab(newWeaponModel.ItemObject);
                    previousWeapons.Add(_rightBackSlot.UnequipItem());
                    previousWeapons.Add(_twoHandBackSlot.UnequipItem());
                    _rightBackSlot.EquipItem(newWeaponModel);
                    break;
                case 2:
                    _twoHandSlot.SetWeaponPrefab(newWeaponModel.ItemObject);
                    previousWeapons.Add(_rightBackSlot.UnequipItem());
                    previousWeapons.Add(_leftBackSlot.UnequipItem());
                    previousWeapons.Add(_twoHandBackSlot.UnequipItem());
                    _twoHandBackSlot.EquipItem(newWeaponModel);
                    break;
                case 3:
                    _leftHandSlot.SetWeaponPrefab(newWeaponModel.ItemObject);
                    previousWeapons.Add(_leftBackSlot.UnequipItem());
                    previousWeapons.Add(_twoHandBackSlot.UnequipItem());
                    _leftBackSlot.EquipItem(newWeaponModel);
                    break;
                default:
                    break;
            }

            OnAnyWeaponSlotChange?.Invoke();
            return previousWeapons;
        }

        public void SwapWeapon()
        {
            var oldRightSlot = _rightHandSlot.UnequipItem();
            var oldLeftSlot = _leftHandSlot.UnequipItem();
            var oldTwoHandSlot = _twoHandSlot.UnequipItem();
            var oldRightBack = _rightBackSlot.UnequipItem();
            var oldLeftBack = _leftBackSlot.UnequipItem();
            var oldTwoBack = _twoHandBackSlot.UnequipItem();

            EquipWeapon(oldRightBack);
            EquipWeapon(oldLeftBack);
            EquipWeapon(oldTwoBack);

            EquipWeaponOnBack(oldRightSlot);
            EquipWeaponOnBack(oldLeftSlot);
            EquipWeaponOnBack(oldTwoHandSlot);

            _isMainSlotActive = _isMainSlotActive ? false : true;
        }

        public void SwapActiveSlot()
        {
            if (_activeSlot.SecondSlot == null) return;
            if (_activeSlot.SecondSlot.ItemModel == null) return;
            if (_activeSlot.SecondSlot.ItemModel.IsShield == true) return;

            _activeSlot = _activeSlot.SecondSlot;
            OnAnyWeaponSlotChange?.Invoke();
        }

        public void Init()
        {
            _rightHandSlot.SecondSlot = _leftHandSlot;
            _leftHandSlot.SecondSlot = _rightHandSlot;
        }

        public void Release()
        {
            if (_leftBackSlot.ItemModel != null)
            {
                _leftBackSlot.ItemModel.LoadableElementsModel.Release();
            }

            if(_rightBackSlot.ItemModel != null)
            {
                _rightBackSlot.ItemModel.LoadableElementsModel.Release();
            }

            if (_rightHandSlot.ItemModel != null)
            {
                _rightHandSlot.ItemModel.LoadableElementsModel.Release();
            }

            if (_leftHandSlot.ItemModel != null)
            {
                _leftHandSlot.ItemModel.LoadableElementsModel.Release();
            }

            if (_twoHandSlot.ItemModel != null)
            {
                _twoHandSlot.ItemModel.LoadableElementsModel.Release();
            }

            if (_twoHandBackSlot.ItemModel != null)
            {
                _twoHandBackSlot.ItemModel.LoadableElementsModel.Release();
            }

            _activeSlot = null;
        }
    }
}