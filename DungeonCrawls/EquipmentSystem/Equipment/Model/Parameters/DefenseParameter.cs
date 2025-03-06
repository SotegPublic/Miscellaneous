namespace Equipment
{
    public class DefenseParameter : ItemParameter<float>
    {
        public DefenseParameter(float value)
        {
            _value = value;
            _name = "Защита";
            _description = "Параметр защиты";
        }
    }
}