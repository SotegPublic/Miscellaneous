namespace Equipment
{
    public class DamageTypeModifier
    {
        private DamageTypes _damageType;
        private float _modifier;

        public DamageTypes DamageType => _damageType;
        public float Modifier => _modifier;

        public DamageTypeModifier (float modifier, DamageTypes damageType)
        {
            _damageType = damageType;
            _modifier = modifier;
        }
    }
}