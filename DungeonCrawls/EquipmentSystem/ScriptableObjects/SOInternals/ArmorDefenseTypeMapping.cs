using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class ArmorDefenseTypeMapping
    {
        [SerializeField] private ArmorMaterialTypes _armorMaterialType;
        [SerializeField] private List<GradeArmorBonusMapping> _gradeDefenseParametersMappings;

        public ArmorMaterialTypes ArmorMaterialType => _armorMaterialType;
        public List<GradeArmorBonusMapping> GradeDefenseParametersMappings => _gradeDefenseParametersMappings;
    }
}