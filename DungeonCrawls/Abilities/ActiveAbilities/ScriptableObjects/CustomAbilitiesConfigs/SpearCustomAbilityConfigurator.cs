
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(SpearCustomAbilityConfigurator), menuName = "Ability/SpearCustomAbilityConfigurator")]
    public class SpearCustomAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float RunDistance;
        [field: SerializeField] public float RunSpeed;
        [field: SerializeField] public float CollisionRadius;
        [field: SerializeField] public float AbilityDamage;
    }
}