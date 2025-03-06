using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(RamAbilityConfigurator), menuName = "Ability/    RamAbilityConfigurator")]
    public class RamAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float RunDistance;
        [field: SerializeField] public float RunSpeed;
        [field: SerializeField] public float CollisionRadius;
        [field: SerializeField] public float CollisionDamage;
        [field: SerializeField] public float SelfDamage;
        [field: SerializeField] public float AnimationSpeedMultiplier;
        [field: SerializeField] public GameObject ChargeArrow;
    }
}