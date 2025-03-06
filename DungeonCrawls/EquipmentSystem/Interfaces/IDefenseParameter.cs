using System;
using Units.UnitsParameters;

namespace Equipment.EquipmentParameters
{
    public interface IDefenseParameter<T>: IParameter
    {
        DamageTypes DamageType { get; }
        T Value { get; }
        void SetValue(T value);
        void SubscribeOnValue(Action<T> subscribingAction);
        void UnsubscribeFromValue(Action<T> unsubscribingAction);
    }
}