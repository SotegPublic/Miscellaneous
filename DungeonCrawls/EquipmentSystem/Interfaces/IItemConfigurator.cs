using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Equipment
{
    public interface IItemConfigurator
    {
        public int ItemID { get; }
        public string Name { get; }
        public GradeTypes Grade { get; }
        public AssetReferenceGameObject PrefabReference { get; }
        public AssetReferenceGameObject LootDropPrefabReference { get; }
        public AssetReferenceSprite IconReference { get; }
        public AssetReferenceT<Material> MaterialReference { get; }
    }
}