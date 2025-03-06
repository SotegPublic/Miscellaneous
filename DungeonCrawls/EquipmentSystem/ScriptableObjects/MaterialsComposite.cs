using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(MaterialsComposite), menuName = "Equipment/MaterialsList", order = 10)]
    public class MaterialsComposite : ScriptableObject
    {
        [SerializeField] private AssetReferenceMaterial _standart;
        [SerializeField] private AssetReferenceMaterial _blue;
        [SerializeField] private AssetReferenceMaterial _red;
        [SerializeField] private AssetReferenceMaterial _green;
        [SerializeField] private AssetReferenceMaterial _white;
        [SerializeField] private AssetReferenceMaterial _brown;
        [SerializeField] private AssetReferenceMaterial _black;
        [SerializeField] private AssetReferenceMaterial _purple;
        [SerializeField] private AssetReferenceMaterial _tan;

        public AssetReferenceMaterial Standart => _standart;
        public AssetReferenceMaterial Blue => _blue;
        public AssetReferenceMaterial Red => _red;
        public AssetReferenceMaterial Green => _green;
        public AssetReferenceMaterial White => _white;
        public AssetReferenceMaterial Brown => _brown;
        public AssetReferenceMaterial Black => _black;
        public AssetReferenceMaterial Purple => _purple;
        public AssetReferenceMaterial Tan => _tan;
    }
}