using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(LightningBoltAbilityConfigurator), menuName = "Ability/LightningBoltAbilityConfigurator")]
    public class LightningBoltAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float CastRange;
        [field: SerializeField] public float Damage;
        [field: SerializeField] public float StatusDuration;
    }
}