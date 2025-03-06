using Abilities;
using PassiveAbilities;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    
    public class WeaponModel: ItemModel
    {
        private WeaponDamageModel _weaponDamageModel;
        private ShieldModel _shieldModel;
        private bool _isShield;
        private WeaponGripParameter _weaponGripType;
        private WeaponTypeParameter _weaponType;
        private WeaponRangeParameter _weaponRange;
        private MeshRenderer _weaponRenderer;
        private MeshRenderer _backSlotRenderer;
        private MeshRenderer _accessoryRenderer;
        private Transform _shootPointTransform;
        private List<WeaponAbilityModel> _abilityModels = new List<WeaponAbilityModel>();
        private List<WeaponPassiveAbilityModel> _passiveAbilityModels = new List<WeaponPassiveAbilityModel>();

        public int WeaponGripTypeID => (int)_weaponGripType.Value;
        public WeaponGripParameter WeaponGripType => _weaponGripType;
        public WeaponTypeParameter WeaponType => _weaponType;
        public WeaponRangeParameter WeaponRange => _weaponRange;
        public GameObject Accessory => _loadableElementsModel.AccessoryObject;
        public GameObject BackSlotPrefab => _loadableElementsModel.BackslotObject;
        public WeaponDamageModel WeaponDamageModel => _weaponDamageModel;
        public ShieldModel ShieldModel => _shieldModel;
        public bool IsShield => _isShield;
        public MeshRenderer WeaponRenderer => _weaponRenderer;
        public MeshRenderer BackSlotRenderer => _backSlotRenderer;
        public MeshRenderer AccessoryRenderer => _accessoryRenderer;
        public Transform ShootPointTransform => _shootPointTransform;
        public List<WeaponAbilityModel> AbilityModels => _abilityModels;
        public List<WeaponPassiveAbilityModel> PassiveAbilityModels => _passiveAbilityModels;

        public WeaponModel(IWeaponConfigurator configurator, LoadableElementsModel loadableElements,
            WeaponModelParameters weaponModelParameters) : base(configurator, loadableElements)
        {
            _weaponGripType = new WeaponGripParameter(configurator.WeaponGripType);
            _weaponType = new WeaponTypeParameter(configurator.WeaponType);
            _weaponRange = new WeaponRangeParameter(configurator.WeaponRange);
            _itemPower = new ItemPowerParameter(weaponModelParameters.ItemPower);
            _itemWeight = new ItemWeightParameter(weaponModelParameters.Weight);

            for(int i = 0; i < configurator.AbilityConfigurators.Count; i++)
            {
                _abilityModels.Add(new WeaponAbilityModel(configurator.AbilityConfigurators[i]));
            }

            for (int i = 0; i < configurator.PassiveAbilityConfigurators.Count; i++)
            {
                _passiveAbilityModels.Add(new WeaponPassiveAbilityModel(configurator.PassiveAbilityConfigurators[i]));
            }

            base.LootDropItem.WeaponModel = this;

            if (configurator.IsShield)
            {
                _weaponDamageModel = new WeaponDamageModel();
                _shieldModel = new ShieldModel(weaponModelParameters);
                _isShield = true;
            } else
            {
                _weaponDamageModel = new WeaponDamageModel(weaponModelParameters);
                _shieldModel = new ShieldModel();
                _isShield = false;
            }

            _weaponRenderer = loadableElements.ItemObject.GetComponentInChildren<MeshRenderer>();
            _weaponRenderer.material = _material;
            _backSlotRenderer = loadableElements.BackslotObject.GetComponentInChildren<MeshRenderer>();
            _backSlotRenderer.material = _material;
            if (loadableElements.AccessoryObject != null)
            {
                _accessoryRenderer = loadableElements.AccessoryObject.GetComponentInChildren<MeshRenderer>();
            }

            _shootPointTransform = _itemObject.FindChildrenTransformByName("ShootPoint"); // TODO - потом убрать волшебную стрингу
        }
    }
}