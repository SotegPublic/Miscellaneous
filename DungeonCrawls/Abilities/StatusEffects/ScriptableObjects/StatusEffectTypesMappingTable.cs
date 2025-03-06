using System.Collections.Generic;
using UnityEngine;

namespace PassiveAbilities
{
    [CreateAssetMenu(fileName = nameof(StatusEffectTypesMappingTable), menuName = "StatusEffects/StatusEffectTypesMappingTable", order = 1)]
    public class StatusEffectTypesMappingTable : ScriptableObject
    {
        [SerializeField] public List<StatusEffectTypesMapping> StatusEffectMappings = new List<StatusEffectTypesMapping>();
    }
}