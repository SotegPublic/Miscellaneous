namespace Equipment
{
    public class ArmorParameter : ItemParameter<float>
    {
        public ArmorParameter(float value)
        {
            _value = value;
            _name = "Броня";
            _description = "Параметр брони";
        }
    }
}