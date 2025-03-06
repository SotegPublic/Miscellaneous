using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class ItemDefenseConfigurator
    {
        [Header("Защита от рубящих атак")]
        [SerializeField] private int _minChippingDefenseCount;
        [SerializeField] private int _maxChippingDefenseCount;
        [Header("Защита от дробящих атак")]
        [SerializeField] private int _minCrushingDefenceCount;
        [SerializeField] private int _maxCrushingDefenceCount;
        [Header("Защита от колющих атак")]
        [SerializeField] private int _minPircingDefenceCount;
        [SerializeField] private int _maxPircingDefenceCount;
        [Header("Защита от режущих атак")]
        [SerializeField] private int _minCuttingDefenceCount;
        [SerializeField] private int _maxCuttingDefenceCount;

        public int MinChippingDefenseCount => _minChippingDefenseCount;
        public int MaxChippingDefenseCount => _maxChippingDefenseCount;
        public int MinCrushingDefenceCount => _minCrushingDefenceCount;
        public int MaxCrushingDefenceCount => _maxCrushingDefenceCount;
        public int MinPircingDefenceCount  => _minPircingDefenceCount;
        public int MaxPircingDefenceCount => _maxPircingDefenceCount;
        public int MinCuttingDefenceCount => _minCuttingDefenceCount;
        public int MaxCuttingDefenceCount => _maxCuttingDefenceCount;
    }
}