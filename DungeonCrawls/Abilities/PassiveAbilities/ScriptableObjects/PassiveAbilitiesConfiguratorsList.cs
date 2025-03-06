using PassiveAbilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PassiveAbilitiesConfiguratorsList), menuName = "PassiveAbility/PassiveAbilitiesConfiguratorsList", order = 1)]
public class PassiveAbilitiesConfiguratorsList : ScriptableObject
{
    [SerializeField] private List<PassiveAbilityConfigurator> _passiveAbilityConfigurators = new List<PassiveAbilityConfigurator>();

    public List<PassiveAbilityConfigurator> PassiveAbilityConfigurators => _passiveAbilityConfigurators;
}
