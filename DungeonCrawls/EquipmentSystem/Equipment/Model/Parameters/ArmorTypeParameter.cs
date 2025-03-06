namespace Equipment
{
    public class ArmorTypeParameter : ItemParameter<ArmorTypes>
    {
        public ArmorTypeParameter(ArmorTypes value)
        {
            _value = value;
            _name = "Тип брони";
            _description = "";
        }
    }
}