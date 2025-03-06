namespace Equipment
{
    public class ItemParameter<T>
    {
        protected T _value;
        protected string _name;
        protected string _description;

        public T Value 
        { 
            get => _value;
            set => _value = value;
        }
        public string Name => _name;
        public string Description => _description;
    }
}