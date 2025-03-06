using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(PosionBottleAbilityConfigurator), menuName = "Ability/PosionBottleAbilityConfigurator")]
    public class PosionBottleAbilityConfigurator: ScriptableObject
    {
        [field: SerializeField] public float ThrowRadius;
        [field: SerializeField] public int BottleCount;
        [field: SerializeField] public float TickDamage;
        [field: SerializeField] public float DamageRadius;
        [field: SerializeField] public float Duration;
        [field: SerializeField] public int TicksCount;
        [field: SerializeField] public GameObject VoidZonePref;
    }
}