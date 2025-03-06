using Equipment;

namespace Units.UnitsParameters
{
    public interface IUnitAttackTypeParameter<T>: IUnitParameter<T>
    {
        DamageTypes DamageType { get; }
    }
}