namespace Equipment
{
    public class ItemGradeParameter: ItemParameter<GradeTypes>
    {
        public ItemGradeParameter(GradeTypes value)
        {
            _value = value;
            _name = "Грейд";
            _description = "";
        }
    }
}