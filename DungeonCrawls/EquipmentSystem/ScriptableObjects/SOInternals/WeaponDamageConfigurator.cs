using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class WeaponDamageConfigurator
    {
        [Header("Рубящий урон")]
        [SerializeField] private int _minChippingDamage;
        [SerializeField] private int _maxChippingDamage;
        [Header("Дробящий урон")]
        [SerializeField] private int _minCrushingDamage;
        [SerializeField] private int _maxCrushingDamage;
        [Header("Колющий урон")]
        [SerializeField] private int _minPiercingDamage;
        [SerializeField] private int _maxPiercingDamage;
        [Header("Режущий урон")]
        [SerializeField] private int _minCuttingDamage;
        [SerializeField] private int _maxCuttingDamage;

        public int MinChippingDamage => _minChippingDamage;
        public int MaxChippingDamage => _maxChippingDamage;
        public int MinCrushingDamage => _minCrushingDamage;
        public int MaxCrushingDamage => _maxCrushingDamage;
        public int MinPiercingDamage => _minPiercingDamage;
        public int MaxPiercingDamage => _maxPiercingDamage;
        public int MinCuttingDamage => _minCuttingDamage;
        public int MaxCuttingDamage => _maxCuttingDamage;
    }
}