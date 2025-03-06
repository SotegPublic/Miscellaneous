using System.Collections.Generic;

namespace Equipment
{
    public class WeaponDamageModel
    {
        private BaseDamageParameter _baseDamage;
        private SpeedParameter _speed;
        private TotalDamageParameter _totalDamage;
        private DPSParameter _damagePerSecond;
        private DamageParameter _chippingDamage;
        private DamageParameter _crushingDamage;
        private DamageParameter _piercingDamage;
        private DamageParameter _cuttingDamage;
        private List<DamageParameter> _damageParametersList = new List<DamageParameter>();

        public BaseDamageParameter BaseDamage => _baseDamage;
        public SpeedParameter Speed => _speed;
        public TotalDamageParameter TotalDamage => _totalDamage;
        public DPSParameter DamagePerSecond => _damagePerSecond;
        public DamageParameter ChippingDamage => _chippingDamage;
        public DamageParameter CrushingDamage => _crushingDamage;
        public DamageParameter PiercingDamage => _piercingDamage;
        public DamageParameter CuttingDamage => _cuttingDamage;
        public List<DamageParameter> DamageParametersList => _damageParametersList;

        public WeaponDamageModel()
        {
            _baseDamage = new BaseDamageParameter(0);
            _speed = new SpeedParameter(0);
            _totalDamage = new TotalDamageParameter(0);
            _damagePerSecond = new DPSParameter(0);

            _chippingDamage = new DamageParameter(0f, DamageTypes.Chipping, "Рубящий урон");
            _damageParametersList.Add(_chippingDamage);

            _crushingDamage = new DamageParameter(0f, DamageTypes.Crushing, "Дробящий урон");
            _damageParametersList.Add(_crushingDamage);

            _piercingDamage = new DamageParameter(0f, DamageTypes.Piercing, "Колящий урон");
            _damageParametersList.Add(_piercingDamage);

            _cuttingDamage = new DamageParameter(0f, DamageTypes.Cutting, "Режущий урон");
            _damageParametersList.Add(_cuttingDamage);
        }

        public WeaponDamageModel(WeaponModelParameters weaponModelParameters)
        {
            _baseDamage = new BaseDamageParameter(weaponModelParameters.Attack);
            _speed = new SpeedParameter(weaponModelParameters.Speed);
            _totalDamage = new TotalDamageParameter(weaponModelParameters.Damage);
            _damagePerSecond = new DPSParameter(weaponModelParameters.DamagePerSecond);

            _chippingDamage = GetDamage(weaponModelParameters.WeaponDamageParameters, DamageTypes.Chipping);
            _damageParametersList.Add(_chippingDamage);

            _crushingDamage = GetDamage(weaponModelParameters.WeaponDamageParameters, DamageTypes.Crushing);
            _damageParametersList.Add(_crushingDamage);

            _piercingDamage = GetDamage(weaponModelParameters.WeaponDamageParameters, DamageTypes.Piercing);
            _damageParametersList.Add(_piercingDamage);

            _cuttingDamage = GetDamage(weaponModelParameters.WeaponDamageParameters, DamageTypes.Cutting);
            _damageParametersList.Add(_cuttingDamage);
        }

        private DamageParameter GetDamage(List<DamageParameter> weaponDamageParameters, DamageTypes damageType)
        {
            return weaponDamageParameters.Find(x => x.DamageType == damageType);
        }
    }
}