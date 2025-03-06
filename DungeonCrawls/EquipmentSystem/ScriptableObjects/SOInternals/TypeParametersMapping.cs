using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class TypeParametersMapping
    {
        [SerializeField] private ArmorSlotTypes _slotType;
        [SerializeField] private List<GradeParametersMapping> _gradeDefenseParametersMappings;

        public ArmorSlotTypes SlotType => _slotType;
        public List<GradeParametersMapping> GradeDefenseParametersMappings => _gradeDefenseParametersMappings;
    }
}