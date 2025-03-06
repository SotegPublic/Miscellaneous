namespace Equipment
{
    public class ItemWeightParameter : ItemParameter<float>
    {
        public ItemWeightParameter(float value)
        {
            _value = value;
            _name = "Вес";
            _description = "";
        }
    }
}