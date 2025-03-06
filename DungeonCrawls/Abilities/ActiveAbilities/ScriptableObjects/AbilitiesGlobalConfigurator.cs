using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(AbilitiesGlobalConfigurator), menuName = "Ability/AbilitiesGlobalConfigurator", order = 1)]
    public class AbilitiesGlobalConfigurator : ScriptableObject
    {
        [SerializeField] private float _globalCooldown;
        [SerializeField] private List<AbilityConfigurator> _abilityConfigurators;

        public List<AbilityConfigurator> AbilityConfigurators => _abilityConfigurators;
        public float GlobalCooldown => _globalCooldown;
    }
}