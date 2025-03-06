namespace Equipment
{
    public class WeaponTypeParameter : ItemParameter<WeaponTypes>
    {
        public WeaponTypeParameter(WeaponTypes value)
        {
            _value = value;
            _name = "Тип оружия";
            _description = "";
        }
    }
}