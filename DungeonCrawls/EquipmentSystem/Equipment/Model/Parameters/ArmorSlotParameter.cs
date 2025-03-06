namespace Equipment
{
    public class ArmorSlotParameter : ItemParameter<ArmorSlotTypes>
    {
        public ArmorSlotParameter(ArmorSlotTypes value)
        {
            _value = value;
            _name = "Слот для брони";
            _description = "";
        }
    }

}