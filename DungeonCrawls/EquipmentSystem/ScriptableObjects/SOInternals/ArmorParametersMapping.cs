using System;
using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [Serializable]
    public class ArmorParametersMapping
    {
        [SerializeField] private ArmorMaterialTypes _armorMaterialType;
        [SerializeField] private List<TypeParametersMapping> _typeDefenseParametersMappings;

        public ArmorMaterialTypes ArmorMaterialType => _armorMaterialType;
        public List<TypeParametersMapping> TypeDefenseParametersMappings => _typeDefenseParametersMappings;
    }
}