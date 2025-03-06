namespace Equipment
{
    public class SpeedParameter : ItemParameter<float>
    {
        public SpeedParameter(float value)
        {
            _value = value;
            _name = "Скорость атаки";
            _description = "";
        }
    }
}