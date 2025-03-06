using Equipment;
using System.Collections.Generic;

namespace PassiveAbilities
{
    public class ProcChancesKeeper
    {
        private Dictionary<WeaponTypes, ProcChancesByGradeModel> _procChancesByWeaponTypes;

        public ProcChancesKeeper(PassiveAbilitiesProcChanceConfig passiveAbilitiesProcChanceConfig)
        {
            _procChancesByWeaponTypes = new Dictionary<WeaponTypes, ProcChancesByGradeModel>(passiveAbilitiesProcChanceConfig.ProcChancesByWeaponTypes.Count);

            for(int i = 0; i < passiveAbilitiesProcChanceConfig.ProcChancesByWeaponTypes.Count; i++)
            {
                _procChancesByWeaponTypes.Add(passiveAbilitiesProcChanceConfig.ProcChancesByWeaponTypes[i].WeaponType,
                    new ProcChancesByGradeModel(passiveAbilitiesProcChanceConfig.ProcChancesByWeaponTypes[i].ProcChancesByGrades));
            }
        }

        public float GetProcChance(WeaponTypes weaponType, GradeTypes gradeType)
        {
            var chance = _procChancesByWeaponTypes[weaponType].GetProcChanceByGrageType(gradeType);
            return chance;
        }
    }
}
