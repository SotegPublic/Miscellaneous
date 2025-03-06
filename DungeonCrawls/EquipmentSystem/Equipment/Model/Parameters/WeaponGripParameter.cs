namespace Equipment
{
    public class WeaponGripParameter : ItemParameter<WeaponGripTypes>
    {
        public WeaponGripParameter(WeaponGripTypes value)
        {
            _value = value;
            _name = "Хват";
            _description = "";
        }
    }
}