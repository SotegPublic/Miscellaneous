using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(HammerCustomAbilityConfigurator), menuName = "Ability/HammerCustomAbilityConfigurator")]

    public class HammerCustomAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float AbilityDamage;
        [field: SerializeField] public float AbilityRadius;
        [field: SerializeField] public float AbilityRange;
    }
}