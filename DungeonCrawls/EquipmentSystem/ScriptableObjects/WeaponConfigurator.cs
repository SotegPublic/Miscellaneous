using Abilities;
using PassiveAbilities;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Equipment
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(WeaponConfigurator), menuName = "Equipment/WeaponConfig", order = 0)]
    public class WeaponConfigurator : ItemConfigurator, IWeaponConfigurator
    {
        [Space]
        [Header("Weapon properties")]
        [SerializeField] private bool _isShield;
        [SerializeField] private WeaponGripTypes _weaponGripType;
        [SerializeField] private WeaponTypes _weaponType;
        [SerializeField] private float _weaponRange;
        [SerializeField] private AssetReferenceGameObject _accessoryReference;
        [SerializeField] private AssetReferenceGameObject _backSlotReference;
        [SerializeField] [HideInInspector] private List<AbilityConfigurator> _abilityConfigurators = new List<AbilityConfigurator>();
        [SerializeField] [HideInInspector] private List<PassiveAbilityConfigurator> _passiveAbilityConfigurators = new List<PassiveAbilityConfigurator>();

        public bool IsShield => _isShield;
        public WeaponGripTypes WeaponGripType => _weaponGripType;
        public WeaponTypes WeaponType => _weaponType;
        public AssetReferenceGameObject AccessoryReference => _accessoryReference;
        public AssetReferenceGameObject BackSlotReference => _backSlotReference;
        public float WeaponRange => _weaponRange;
        [SerializeField] public List<AbilityConfigurator> AbilityConfigurators => _abilityConfigurators;
        [SerializeField] public List<PassiveAbilityConfigurator> PassiveAbilityConfigurators => _passiveAbilityConfigurators;

        public void AddAbilityConfigurator()
        {
            _abilityConfigurators.Add(null);
        }

        public void AddPassiveAbilityConfigurator()
        {
            _passiveAbilityConfigurators.Add(null);
        }

        public void RemoveLastAbilityConfigurator()
        {
            if (_abilityConfigurators.Count == 0) return;

            int lastIndex = _abilityConfigurators.Count - 1;
            _abilityConfigurators.Remove(_abilityConfigurators[lastIndex]);
        }

        public void RemoveLastPassiveAbilityConfigurator()
        {
            if (_passiveAbilityConfigurators.Count == 0) return;

            int lastIndex = _passiveAbilityConfigurators.Count - 1;
            _passiveAbilityConfigurators.Remove(_passiveAbilityConfigurators[lastIndex]);
        }

        public void ClearAbilityConfigurators()
        {
            _abilityConfigurators.Clear();
        }

        public void ClearPassiveAbilityConfigurators()
        {
            _passiveAbilityConfigurators.Clear();
        }
    }
}