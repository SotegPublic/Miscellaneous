using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Equipment
{
    [Serializable]
    public class AssetReferenceMaterial : AssetReferenceT<Material>
    {
        public AssetReferenceMaterial(string guid) : base(guid) { }
        public override bool ValidateAsset(Object obj)
        {
            var type = obj.GetType();
            return typeof(Material).IsAssignableFrom(type);
        }
    }
}