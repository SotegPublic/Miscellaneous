using Abilities;
using PassiveAbilities;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;

namespace Equipment
{
    public interface IWeaponConfigurator : IItemConfigurator
    {
        public bool IsShield { get; }
        public WeaponGripTypes WeaponGripType { get; }
        public WeaponTypes WeaponType { get; }
        public float WeaponRange { get; }
        public AssetReferenceGameObject AccessoryReference { get; }
        public AssetReferenceGameObject BackSlotReference { get; }
        public List<AbilityConfigurator> AbilityConfigurators { get; }
        public List<PassiveAbilityConfigurator> PassiveAbilityConfigurators { get; }
    }
}