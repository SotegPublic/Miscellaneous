namespace Equipment
{
    public interface IArmorConfigurator : IItemConfigurator
    {
        public ArmorBindingsTypes ArmorBindings { get; }
        public ArmorSlotTypes ArmorSlotType { get; }
        public ArmorTypes ArmorType { get; }
        public ArmorMaterialTypes ArmorMaterial { get; }
    }
}