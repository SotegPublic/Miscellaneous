using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(PassiveAbilitiesProcChanceConfig), menuName = "PassiveAbility/PassiveAbilitiesProcChanceConfig", order = 3)]
public class PassiveAbilitiesProcChanceConfig : ScriptableObject
{
    [SerializeField] public List<ProcChancesByWeaponTypesConfigurator> ProcChancesByWeaponTypes = new List<ProcChancesByWeaponTypesConfigurator>();
}
