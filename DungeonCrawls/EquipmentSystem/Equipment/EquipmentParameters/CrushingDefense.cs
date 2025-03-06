using Engine;

namespace Equipment.EquipmentParameters
{
    public class CrushingDefense : Parameter<float>, IDefenseParameter<float>
    {
        private DamageTypes _damageType = DamageTypes.Crushing;
        public DamageTypes DamageType => _damageType;

        public CrushingDefense(string name, string discription) : base(name, discription)
        {
        }
    }
}