using System;

namespace Units.UnitsParameters
{
    public interface IUnitParameter<T>: IParameter
    {
        ParameterTypes ParameterType { get; }
        T Value { get; }
        void SetValue(T value);
        void SubscribeOnValue(Action<T> subscribingAction);
        void UnsubscribeFromValue(Action<T> unsubscribingAction);
    }
}