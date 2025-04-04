﻿public interface IJsonData<T>
{
    void Save(T data, string path = null);
    T Load(string path = null);
}