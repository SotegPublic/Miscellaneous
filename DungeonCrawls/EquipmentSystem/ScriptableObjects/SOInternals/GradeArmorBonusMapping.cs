using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class GradeArmorBonusMapping
    {
        [SerializeField] private GradeTypes _grade;
        [SerializeField] private List<SlotParametersMapping> _slotDefenseParametersMappings;

        public GradeTypes Grade => _grade;
        public List<SlotParametersMapping> SlotDefenseParametersMappings => _slotDefenseParametersMappings;
    }
}