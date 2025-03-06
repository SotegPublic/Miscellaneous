using Engine;
using Units.UnitsParameters;

namespace Equipment.EquipmentParameters
{
    public class UnitDefenseParameter : Parameter<float>, IUnitParameter<float>
    {
        private ParameterTypes _parameterType = ParameterTypes.TotalDefence;
        public ParameterTypes ParameterType => _parameterType;
        public UnitDefenseParameter(string name, string discription) : base(name, discription)
        {
        }
    }
}