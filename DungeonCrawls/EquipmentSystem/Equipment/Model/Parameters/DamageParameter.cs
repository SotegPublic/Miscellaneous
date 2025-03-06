namespace Equipment
{
    public class DamageParameter : ItemParameter<float>
    {
        private DamageTypes _damageType;

        public DamageTypes DamageType => _damageType;
        public DamageParameter(float value, DamageTypes damageType, string name)
        {
            _value = value;
            _name = name;
            _description = "";
            _damageType = damageType;
        }
    }
}