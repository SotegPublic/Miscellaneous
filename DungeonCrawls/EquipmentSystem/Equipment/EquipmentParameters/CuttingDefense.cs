using Engine;

namespace Equipment.EquipmentParameters
{
    public class CuttingDefense : Parameter<float>, IDefenseParameter<float>
    {
        private DamageTypes _damageType = DamageTypes.Cutting;
        public DamageTypes DamageType => _damageType;

        public CuttingDefense(string name, string discription) : base(name, discription)
        {
        }
    }
}