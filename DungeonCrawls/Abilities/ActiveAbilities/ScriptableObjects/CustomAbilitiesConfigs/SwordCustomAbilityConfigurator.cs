using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(SwordCustomAbilityConfigurator), menuName = "Ability/SwordCustomAbilityConfigurator")]
    public class SwordCustomAbilityConfigurator : ScriptableObject
    {
        [field: SerializeField] public float Damage;
        [field: SerializeField] public float DamageMultiplier;
        [field: SerializeField] public float AdditivePercentage;
        [field: SerializeField] public float AbilityRange;
    }
}
