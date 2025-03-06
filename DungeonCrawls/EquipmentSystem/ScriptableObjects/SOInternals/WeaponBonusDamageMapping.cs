using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class WeaponBonusDamageMapping
    {
        [SerializeField] private WeaponTypes _weaponType;
        [SerializeField] private List<GradeWeaponBonusMapping> _gradeBonusMappings;

        public WeaponTypes WeaponType => _weaponType;
        public List<GradeWeaponBonusMapping> GradeBonusMappings => _gradeBonusMappings;
    }
}