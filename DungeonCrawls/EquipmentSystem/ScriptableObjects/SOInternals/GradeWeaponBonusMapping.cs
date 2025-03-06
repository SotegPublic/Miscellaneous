using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class GradeWeaponBonusMapping
    {
        [SerializeField] private GradeTypes _grade;
        [SerializeField] private float _cuttingDamage;
        [SerializeField] private float _piercingDamage;
        [SerializeField] private float _chippingDamage;
        [SerializeField] private float _crushingDamage;

        public GradeTypes Grade => _grade;
        public float CuttingDamage => _cuttingDamage;
        public float PiercingDamage => _piercingDamage;
        public float ChippingDamage => _chippingDamage;
        public float CrushingDamage => _crushingDamage;
    }
}