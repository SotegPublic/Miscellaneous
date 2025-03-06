namespace Equipment
{
    public class ShieldBlockChanceParameter : ItemParameter<float>
    {
        public ShieldBlockChanceParameter(float value)
        {
            _value = value;
            _name = "Шанс блока";
            _description = "";
        }
    }
}