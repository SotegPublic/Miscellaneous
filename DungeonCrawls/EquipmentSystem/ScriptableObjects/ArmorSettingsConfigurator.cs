using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(ArmorSettingsConfigurator), menuName = "Equipment/Armors Settings Config", order = 5)]
    public class ArmorSettingsConfigurator : ScriptableObject
    {
        [Header("Таблица бонусной защиты по типам")]
        [SerializeField] private List<ArmorDefenseTypeMapping> _armorDefenseTypeMappings;
        [Header("Таблица брони и веса по типам")]
        [SerializeField] private List<ArmorParametersMapping> _armorParametersMappings;
        [Header("Модификаторы грейда для расчета Power")]
        [SerializeField] private List<ItemGradePowerModifiers> _itemGradePowerModifiers;
        [Header("Модификаторы типа защита")]
        [SerializeField] private List<ItemDamageTypeModifiers> _itemDamageTypeModifiers;

        public List<ArmorDefenseTypeMapping> ArmorDefenseTypeMappings => _armorDefenseTypeMappings;
        public List<ItemGradePowerModifiers> ItemGradePowerModifiers => _itemGradePowerModifiers;
        public List<ItemDamageTypeModifiers> ItemDamageTypeModifiers => _itemDamageTypeModifiers;
        public List<ArmorParametersMapping> ArmorParametersMappings => _armorParametersMappings;
    }
}