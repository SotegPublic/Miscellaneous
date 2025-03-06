namespace Equipment
{
    public class WeaponRangeParameter : ItemParameter<float>
    {
        public WeaponRangeParameter(float value)
        {
            _value = value;
            _name = "Радиус атаки";
            _description = "";
        }
    }
}