using System.IO;
using UnityEngine;

public sealed class JsonData<T> : IJsonData<T>
{
    private Encryptor _encryptor;
    public void Save(T data, string path = null)
    {
        var saveString = JsonUtility.ToJson(data);
        _encryptor = new Encryptor();
        File.WriteAllText(path, saveString);
        //File.WriteAllText(path, _encryptor.Encrypt(saveString)); //на случай необходимости кодировать
    }

    public T Load(string path = null)
    {
        var loadString = File.ReadAllText(path);
        return JsonUtility.FromJson<T>(loadString);
        //return JsonUtility.FromJson<T>(_encryptor.Decrypt(loadString)); //на случай необходимости кодировать
    }
}
