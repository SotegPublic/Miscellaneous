using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Utils;

namespace Equipment
{
    public class ArmorFactory: IItemFactory<ArmorModel, IArmorConfigurator>
    {
        private Transform _playerArmorPoolTransform;
        private Transform _lootPrefabHolderTransform;
        private ArmorSettingsConfigurator _armorSettings;

        public ArmorFactory(Transform playerArmorPoolTransform, Transform lootPrefabHolderTransform, GlobalConfigLoader globalConfig)
        {
            _playerArmorPoolTransform = playerArmorPoolTransform;
            _lootPrefabHolderTransform = lootPrefabHolderTransform;
            _armorSettings = globalConfig.ArmorSettingsConfigurator;
           // LoadArmorSettingsConfigAsync(); 
        }

    /*    private async Task LoadArmorSettingsConfigAsync()
        {
            _armorSettings = await AdressableAssetLoader.LoadAsset<ArmorSettingsConfigurator>("GameConfig");
        }*/

        public async Task<ArmorModel> CreateItemAsync(IArmorConfigurator configurator)
        {
            var loadableElements = new LoadableElementsModel();

            await GetArmorObject(configurator, loadableElements);
            await GetLootObject(configurator, loadableElements);
            await GetSprite(configurator, loadableElements);
            await GetMaterial(configurator, loadableElements);

            var armorModelParameters = CreateArmorModelParameters(configurator);

            return new ArmorModel(configurator, loadableElements, armorModelParameters);
        }

        private async Task GetMaterial(IArmorConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var materialHandle = Addressables.LoadAssetAsync<Material>(configurator.MaterialReference);
            var material = await materialHandle.Task;
            loadableElements.AddMaterial(materialHandle, material);
        }

        private async Task GetSprite(IArmorConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var spriteHandle = Addressables.LoadAssetAsync<Sprite>(configurator.IconReference);
            var sprite = await spriteHandle.Task;
            loadableElements.AddSprite(spriteHandle, sprite);
        }

        private async Task GetLootObject(IArmorConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var lootItemHandle = Addressables.InstantiateAsync(configurator.LootDropPrefabReference, _lootPrefabHolderTransform);
            var lootItemObject = await lootItemHandle.Task;
            loadableElements.AddLootItemObject(lootItemHandle, lootItemObject);
        }

        private async Task GetArmorObject(IArmorConfigurator configurator, LoadableElementsModel loadableElements)
        {
            var armorHandle = Addressables.InstantiateAsync(configurator.PrefabReference, _playerArmorPoolTransform);
            var armorObject = await armorHandle.Task;
            loadableElements.AddItemObject(armorHandle, armorObject);
        }

        private ArmorModelParameters CreateArmorModelParameters(IArmorConfigurator configurator)
        {
            var armorParameters = new ArmorModelParameters(configurator, _armorSettings);
            return armorParameters;
        }
    }
}