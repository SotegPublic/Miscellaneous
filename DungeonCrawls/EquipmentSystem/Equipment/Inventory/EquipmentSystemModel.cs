using System;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class EquipmentSystemModel
    {
        [SerializeField] private ArmorComposite _armorComposite;
        [SerializeField] private WeaponComposite _weaponComposite;

        public ArmorComposite ArmorComposite => _armorComposite;
        public WeaponComposite WeaponComposite => _weaponComposite;
    }
}