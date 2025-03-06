using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class WeaponParametersMapping
    {
        [SerializeField] private WeaponTypes _weaponType;
        [SerializeField] private List<GradeParametersMapping> _gradeParametersMappings;

        public WeaponTypes WeaponType => _weaponType;
        public List<GradeParametersMapping> GradeParametersMappings => _gradeParametersMappings;
    }
}