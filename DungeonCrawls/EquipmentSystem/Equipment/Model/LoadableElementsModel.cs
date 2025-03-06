using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Equipment
{
    public class LoadableElementsModel
    {
        public AsyncOperationHandle<Material> MaterialHandle;
        public Material Material;

        private AsyncOperationHandle<GameObject> _itemHandle;
        private GameObject _itemObject;
        private AsyncOperationHandle<GameObject> _accessoryHandle;
        private GameObject _accessoryObject;
        private AsyncOperationHandle<GameObject> _lootItemHandle;
        private GameObject _lootItemObject;
        private LootDropItem _lootDropItem;
        private AsyncOperationHandle<GameObject> _backslotHandle;
        private GameObject _backslotObject;
        private AsyncOperationHandle<Sprite> _spriteHandle;
        private Sprite _sprite;

        public AsyncOperationHandle<GameObject> ItemHandle => _itemHandle;
        public GameObject ItemObject => _itemObject;
        public AsyncOperationHandle<Sprite> SpriteHandle => _spriteHandle;
        public Sprite Sprite => _sprite;
        public AsyncOperationHandle<GameObject> AcccessoryHandle => _accessoryHandle;
        public GameObject AccessoryObject => _accessoryObject;
        public AsyncOperationHandle<GameObject> BackslotHandle => _backslotHandle;
        public GameObject BackslotObject => _backslotObject;
        public AsyncOperationHandle<GameObject> LootItemHandle => _lootItemHandle;
        public GameObject LootItemObject => _lootItemObject;
        public LootDropItem LootDropItem => _lootDropItem;

        public LoadableElementsModel AddItemObject(AsyncOperationHandle<GameObject> itemHandle, GameObject itemObject)
        {
            _itemHandle = itemHandle;
            _itemObject = itemObject;
            return this;
        }

        public LoadableElementsModel AddLootItemObject(AsyncOperationHandle<GameObject> lootItemHandle, GameObject lootItemObject)
        {
            _lootItemHandle = lootItemHandle;
            _lootItemObject = lootItemObject;
            _lootDropItem = lootItemObject.GetComponent<LootDropItem>();
            return this;
        }

        public LoadableElementsModel AddAccessoryObject(AsyncOperationHandle<GameObject> accessoryHandle, GameObject accessoryObject)
        {
            _accessoryHandle = accessoryHandle;
            _accessoryObject = accessoryObject;
            return this;
        }

        public LoadableElementsModel AddBackSlotObject(AsyncOperationHandle<GameObject> backslotHandle, GameObject backslotObject)
        {
            _backslotHandle = backslotHandle;
            _backslotObject = backslotObject;
            return this;
        }

        public LoadableElementsModel AddSprite (AsyncOperationHandle<Sprite> spriteHandle, Sprite sprite)
        {
            _spriteHandle = spriteHandle;
            _sprite = sprite;
            return this;
        }

        public LoadableElementsModel AddMaterial(AsyncOperationHandle<Material> materialHandle, Material material)
        {
            MaterialHandle = materialHandle;
            Material = material;
            return this;
        }

        public void Release()
        {
            Addressables.Release(_itemHandle);
            if (_accessoryHandle.IsValid())
            {
                Addressables.Release(_accessoryHandle);
            }

            if (_lootItemHandle.IsValid())
            {
                Addressables.ReleaseInstance(_lootItemHandle);
            }
            
            if (_accessoryHandle.IsValid())
            {
                Addressables.Release(_backslotHandle);
            }
            Addressables.Release(_spriteHandle);
            Addressables.Release(MaterialHandle);
        }
    }
}