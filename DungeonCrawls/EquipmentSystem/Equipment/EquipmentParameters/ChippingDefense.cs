using Engine;

namespace Equipment.EquipmentParameters
{
    public class ChippingDefense : Parameter<float>, IDefenseParameter<float>
    {
        private DamageTypes _damageType = DamageTypes.Chipping;
        public DamageTypes DamageType => _damageType;

        public ChippingDefense(string name, string discription) : base(name, discription)
        {
        }
    }
}