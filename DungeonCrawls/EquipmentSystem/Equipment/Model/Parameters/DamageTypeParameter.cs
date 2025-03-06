namespace Equipment
{
    public class DamageTypeParameter : ItemParameter<DamageTypes>
    {
        public DamageTypeParameter(DamageTypes value, string name)
        {
            _value = value;
            _name = name;
            _description = "";
        }
    }
}