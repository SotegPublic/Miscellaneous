namespace Equipment
{
    public class ItemPowerParameter : ItemParameter<float>
    {
        public ItemPowerParameter(float value)
        {
            _value = value;
            _name = "Уровень";
            _description = "";
        }
    }
}