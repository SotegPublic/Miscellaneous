using Equipment;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProcChancesByWeaponTypesConfigurator
{
    [SerializeField] public WeaponTypes WeaponType;
    [SerializeField] public List<ProcChancesByGradeConfigurator> ProcChancesByGrades = new List<ProcChancesByGradeConfigurator>();
}
