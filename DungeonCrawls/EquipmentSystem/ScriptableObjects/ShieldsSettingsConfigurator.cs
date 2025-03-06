using System.Collections.Generic;
using UnityEngine;

namespace Equipment
{
    [CreateAssetMenu(fileName = nameof(ShieldsSettingsConfigurator), menuName = "Equipment/Shield Settings Config", order = 5)]
    public class ShieldsSettingsConfigurator : ScriptableObject
    {
        [Header("Таблица шанса блока и веса по типам")]
        [SerializeField] private List<GradeParametersMapping> gradeParametersMappings;
        [Header("Модификаторы грейда для расчета Power")]
        [SerializeField] private List<ItemGradePowerModifiers> _itemGradePowerModifiers;

        public List<GradeParametersMapping> GradeParametersMappings => gradeParametersMappings;
        public List<ItemGradePowerModifiers> ItemGradePowerModifiers => _itemGradePowerModifiers;
    }
}