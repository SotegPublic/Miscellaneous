namespace Equipment
{
    public class BaseDamageParameter : ItemParameter<float>
    {
        public BaseDamageParameter(float value)
        {
            _value = value;
            _name = "Базовый урон";
            _description = "";
        }
    }
}