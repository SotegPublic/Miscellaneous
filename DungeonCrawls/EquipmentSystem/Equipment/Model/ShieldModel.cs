namespace Equipment
{
    public class ShieldModel
    {
        private ShieldBlockChanceParameter _blockChance;

        public ShieldBlockChanceParameter BlockChance => _blockChance;

        public ShieldModel()
        {
            _blockChance = new ShieldBlockChanceParameter(0);
        }

        public ShieldModel(WeaponModelParameters weaponModelParameters)
        {
            _blockChance = new ShieldBlockChanceParameter(weaponModelParameters.BlockChance);
        }
    }
}