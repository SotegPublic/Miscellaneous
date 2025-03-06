using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Equipment
{
    public class WeaponFactory : IItemFactory<WeaponModel, IWeaponConfigurator>
    {
        private Transform _playerWeaponPoolTransform;
        private Transform _lootPrefabHolderTransform;
        private WeaponsSettingsConfigurator _weaponsSettings;
        private ShieldsSettingsConfigurator _shieldsSettings;

        public WeaponFactory(Transform playerWeaponPoolTransform, Transform lootPrefabHolderTransform, GlobalConfigLoader globalConfig)
        {
            _lootPrefabHolderTransform = lootPrefabHolderTransform;
            _playerWeaponPoolTransform = playerWeaponPoolTransform;
            _weaponsSettings = globalConfig.WeaponsSettingsConfigurator;
            _shieldsSettings = globalConfig.ShieldsSettingsConfigurator;
        }

        public async Task<WeaponModel> CreateItemAsync(IWeaponConfigurator configurator)
        {
            var loadableElements = new LoadableElementsModel();
            await GetWeaponObject(configurator, loadableElements);
            await GetLootObject(configurator, loadableElements);
            await GetBackSlotObject(configurator, loadableElements);
            await GetSprite(configurator, loadableElements);
            await GetMaterial(configurator, loadableElements);

            if (configurator.AccessoryReference.RuntimeKeyIsValid())
            {
                await GetAccessoryObject(configurator, loadableElements);
            }

            var weaponModelParameters = CreateWeaponModelParameters(configurator);

            return new WeaponModel(configurator, loadableElements, weaponModelParameters);
        }

        private WeaponModelParameters CreateWeaponModelParameters(IWeaponConfigurator configurator)
        {
            return new WeaponModelParameters(configurator, _weaponsSettings, _shieldsSettings);
        }

        private async Task GetMaterial(IWeaponConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var materialHandle = Addressables.LoadAssetAsync<Material>(configurator.MaterialReference);
            var material = await materialHandle.Task;
            loadableElements.AddMaterial(materialHandle, material);
        }

        private async Task GetSprite(IWeaponConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var spriteHandle = Addressables.LoadAssetAsync<Sprite>(configurator.IconReference);
            var sprite = await spriteHandle.Task;
            loadableElements.AddSprite(spriteHandle, sprite);
        }

        private async Task GetAccessoryObject(IWeaponConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var accessoryHandle = Addressables.InstantiateAsync(configurator.AccessoryReference, _playerWeaponPoolTransform);
            var accessoryObject = await accessoryHandle.Task;
            loadableElements.AddAccessoryObject(accessoryHandle, accessoryObject);
        }

        private async Task GetBackSlotObject(IWeaponConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var backslotHandle = Addressables.InstantiateAsync(configurator.BackSlotReference, _playerWeaponPoolTransform);
            var backslotObject = await backslotHandle.Task;
            loadableElements.AddBackSlotObject(backslotHandle, backslotObject);
        }

        private async Task GetLootObject(IWeaponConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var lootItemHandle = Addressables.InstantiateAsync(configurator.LootDropPrefabReference, _lootPrefabHolderTransform);
            var lootItemObject = await lootItemHandle.Task;
            loadableElements.AddLootItemObject(lootItemHandle, lootItemObject);
        }

        private async Task GetWeaponObject(IWeaponConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var weaponHandle = Addressables.InstantiateAsync(configurator.PrefabReference, _playerWeaponPoolTransform);
            var weaponObject = await weaponHandle.Task;
            loadableElements.AddItemObject(weaponHandle, weaponObject);
        }
    }
}