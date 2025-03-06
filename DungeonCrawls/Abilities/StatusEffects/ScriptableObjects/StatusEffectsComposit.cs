using PassiveAbilities;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(StatusEffectsComposit), menuName = "StatusEffects/StatusEffectsComposit", order = 5)]
public class StatusEffectsComposit : ScriptableObject
{
    [SerializeField] private List<StatusEffectConfigurator> statusEffectConfigurators;

    public List<StatusEffectConfigurator> StatusEffectConfigurators => statusEffectConfigurators;
}
