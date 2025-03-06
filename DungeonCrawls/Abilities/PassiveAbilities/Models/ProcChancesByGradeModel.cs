using Equipment;
using System.Collections.Generic;

namespace PassiveAbilities
{
    public class ProcChancesByGradeModel
    {
        private Dictionary<GradeTypes, float> _procChancesByGradeTypes;

        public ProcChancesByGradeModel(List<ProcChancesByGradeConfigurator> procChancesByGradeConfigurators)
        {
            _procChancesByGradeTypes = new Dictionary<GradeTypes, float>(procChancesByGradeConfigurators.Count);

            for(int i = 0; i < procChancesByGradeConfigurators.Count; i++)
            {
                _procChancesByGradeTypes.Add(procChancesByGradeConfigurators[i].GradeType, procChancesByGradeConfigurators[i].ProcChance);
            }
        }

        public float GetProcChanceByGrageType(GradeTypes gradeType)
        {
            return _procChancesByGradeTypes[gradeType];
        }
    }
}
