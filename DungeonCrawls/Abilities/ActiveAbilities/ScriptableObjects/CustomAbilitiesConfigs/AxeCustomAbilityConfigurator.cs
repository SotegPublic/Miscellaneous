using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(AxeCustomAbilityConfigurator), menuName = "Ability/AxeCustomAbilityConfigurator")]
    public class AxeCustomAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float AbilityDamage;
        [field: SerializeField] public float RunDistance;
        [field: SerializeField] public float RunSpeed;
        [field: SerializeField] public float CollisionRadius;
        [field: SerializeField] public float WallDamageMultiplier;
        [field: SerializeField] public float ConfuseDuration;
        [field: SerializeField] public float AbilityRange;
    }
}