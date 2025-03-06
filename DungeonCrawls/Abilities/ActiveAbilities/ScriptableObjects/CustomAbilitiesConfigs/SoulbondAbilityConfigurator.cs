using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(SoulbondAbilityConfigurator), menuName = "Ability/SoulbondAbilityConfigurator")]
    public class SoulbondAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float CastRange;
        [field: SerializeField] public int TicksCount;
        [field: SerializeField] public float TickDamage;
        [field: SerializeField] public float LastTickDamage;
        [field: SerializeField] public float Duration;
    }
}