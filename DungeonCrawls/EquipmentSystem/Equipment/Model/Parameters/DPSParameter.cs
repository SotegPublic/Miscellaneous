namespace Equipment
{
    public class DPSParameter : ItemParameter<float>
    {
        public DPSParameter(float value)
        {
            _value = value;
            _name = "ДПС";
            _description = "";
        }
    }
}