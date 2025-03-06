using System;

namespace Units.UnitsParameters
{
    public interface IUnitResource<T>: IUnitParameter<T>
    {
        T MaxValue { get; }
        void SetMaxValue(T value);
        void SubscribeOnMaxValue(Action<T> subscribingAction);
        void UnsubscribeFromMaxValue(Action<T> unsubscribingAction);
    }
}