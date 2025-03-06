namespace Equipment
{
    public class ArmorMaterialParameter : ItemParameter<ArmorMaterialTypes>
    {
        public ArmorMaterialParameter(ArmorMaterialTypes value)
        {
            _value = value;
            _name = "Материал";
            _description = "";
        }
    }

}