using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class ItemDamageTypeModifiers
    {
        [SerializeField] private DamageTypes _damageType;
        [SerializeField] private float _modifier;

        public DamageTypes DamageType => _damageType;
        public float Modifier => _modifier;
    }
}