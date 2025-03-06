namespace Equipment
{
    public class TotalDamageParameter : ItemParameter<float>
    {
        public TotalDamageParameter(float value)
        {
            _value = value;
            _name = "Общий урон";
            _description = "";
        }
    }
}