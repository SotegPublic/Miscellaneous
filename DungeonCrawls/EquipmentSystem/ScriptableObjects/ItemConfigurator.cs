using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Equipment
{
    public class ItemConfigurator : ScriptableObject, IItemConfigurator
    {
        [Header("Item propeties")]
        [SerializeField] protected int _itemID;
        [SerializeField] protected string _name;
        [SerializeField] protected AssetReferenceGameObject _prefabReference;
        [SerializeField] protected AssetReferenceSprite _iconReference;
        [SerializeField] protected AssetReferenceMaterial materialReference;
        [SerializeField] protected AssetReferenceGameObject lootDropPrefabReference;
        [Space]
        [Header("Item parameters")]
        [SerializeField] protected GradeTypes _grade;
        [SerializeField] protected bool _isNewbieItem;

        public int ItemID => _itemID;
        public string Name => _name;
        public AssetReferenceGameObject LootDropPrefabReference => lootDropPrefabReference;
        public AssetReferenceGameObject PrefabReference => _prefabReference;
        public AssetReferenceSprite IconReference => _iconReference;
        public AssetReferenceT<Material> MaterialReference => materialReference;
        public GradeTypes Grade => _grade;
        public bool IsNewbieItem => _isNewbieItem;
    }
}