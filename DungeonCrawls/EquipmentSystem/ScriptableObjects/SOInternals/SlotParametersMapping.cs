using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class SlotParametersMapping
    {
        [SerializeField] private ArmorSlotTypes _armorSlot;
        [SerializeField] private float _cuttingDamage;
        [SerializeField] private float _piercingDamage;
        [SerializeField] private float _chippingDamage;
        [SerializeField] private float _crushingDamage;

        public ArmorSlotTypes ArmorSlot => _armorSlot;
        public float CuttingDamage => _cuttingDamage;
        public float PiercingDamage => _piercingDamage;
        public float ChippingDamage => _chippingDamage;
        public float CrushingDamage => _crushingDamage;
    }
}