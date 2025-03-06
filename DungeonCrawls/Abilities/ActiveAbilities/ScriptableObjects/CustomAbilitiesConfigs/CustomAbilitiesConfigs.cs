using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(CustomAbilitiesConfigs), menuName = "Ability/CustomAbilitiesConfigs")]

    public class CustomAbilitiesConfigs : ScriptableObject
    {
        [field: SerializeField] public AxeCustomAbilityConfigurator AxeCustomAbilityConfigurator;
        [field: SerializeField] public HammerCustomAbilityConfigurator HammerCustomAbilityConfigurator;
        [field: SerializeField] public SpearCustomAbilityConfigurator SpearCustomAbilityConfigurator;
        [field: SerializeField] public SwordCustomAbilityConfigurator SwordCustomAbilityConfigurator;
        [field: SerializeField] public LightningBoltAbilityConfigurator LightningBoltAbilityConfigurator;
        [field: SerializeField] public SoulbondAbilityConfigurator SoulbondAbilityConfigurator;
        [field: SerializeField] public PosionBottleAbilityConfigurator PosionBottleAbilityConfigurator;
        [field: SerializeField] public RamAbilityConfigurator RamAbilityConfigurator;
        
    }
}