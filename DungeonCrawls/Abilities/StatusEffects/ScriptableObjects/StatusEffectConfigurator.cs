using System;
using UnityEngine;

namespace PassiveAbilities
{
    [Serializable]
    [CreateAssetMenu(fileName = nameof(StatusEffectConfigurator), menuName = "StatusEffects/StatusEffectConfigurator", order = 3)]
    public class StatusEffectConfigurator : ScriptableObject
    {
        [SerializeField] public StatusEffectTypesMappingTable StatusEffectTypesMappingTable;
        [SerializeField] public int StatusEffectID;
        [SerializeField] public StatusTypes StatusType;
        [SerializeField] public float StatusComboValue; // Значение комбо эффекта, например значение усилиение урона или снижения брони 
        [SerializeField] public float StatusComboRadius; // Радиус АоЕ комбо эффекта, если требуется
        [SerializeField] public float StatusEffectValue; // Значение статус эффекта, например урон доты
    }
}