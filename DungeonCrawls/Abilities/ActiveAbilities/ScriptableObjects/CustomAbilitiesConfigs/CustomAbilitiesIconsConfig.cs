using Abilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Abilities
{
    [CreateAssetMenu(fileName = nameof(CustomAbilitiesIconsConfig), menuName = "Ability/CustomAbilitiesIconsConfig")]

    public class CustomAbilitiesIconsConfig : ScriptableObject
    {
        [field: SerializeField] public Sprite AxeIcon;
        [field: SerializeField] public Sprite HammerIcon;
        [field: SerializeField] public Sprite SpearIcon;
        [field: SerializeField] public Sprite SwordIcon;
    }
}