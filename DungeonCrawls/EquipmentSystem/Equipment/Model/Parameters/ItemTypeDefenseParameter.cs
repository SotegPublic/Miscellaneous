namespace Equipment
{
    public class ItemTypeDefenseParameter : ItemParameter<float>
    {
        private DamageTypes _defenceType;
        public DamageTypes DefenceType => _defenceType;
        public ItemTypeDefenseParameter(float value, DamageTypes defenceType, string name)
        {
            _value = value;
            _name = name;
            _description = "";
            _defenceType = defenceType;
        }
    }
}