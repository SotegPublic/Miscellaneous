using Engine;

namespace Equipment.EquipmentParameters
{
    public class PircingDefense : Parameter<float>, IDefenseParameter<float>
    {
        private DamageTypes _damageType = DamageTypes.Piercing;
        public DamageTypes DamageType => _damageType;

        public PircingDefense(string name, string discription) : base(name, discription)
        {
        }
    }
}