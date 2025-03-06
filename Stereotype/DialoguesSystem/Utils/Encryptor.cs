using System.Collections.Generic;
using System;

public class Encryptor
{
    public string Encrypt(string saveString)
    {
        List<int> encryptKeyValues = GenerateOpenKey();

        List<Byte[]> bytes = new List<byte[]>();
        for (int i = 0; i < saveString.Length; i++)
        {
            bytes.Add(BitConverter.GetBytes(saveString[i]));
        }

        string encryptString = "";
        for (int i = 0; i < bytes.Count; i++)
        {
            if (i < bytes.Count * 0.3f)
            {
                bytes[i][1] = (byte)~(bytes[i][1] + encryptKeyValues[0]);
                encryptString = encryptString + BitConverter.ToChar(bytes[i], 0);
            }
            else if (i >= bytes.Count * 0.3f && i < bytes.Count * 0.6f)
            {
                bytes[i][1] = (byte)~(~(bytes[i][1] - encryptKeyValues[1]) + encryptKeyValues[2]);
                encryptString = encryptString + BitConverter.ToChar(bytes[i], 0);
            }
            else
            {
                bytes[i][1] = (byte)(~bytes[i][1] - encryptKeyValues[3]);
                encryptString = encryptString + BitConverter.ToChar(bytes[i], 0);
            }
        }

        encryptString = EncryptAndIncapsulateOpenKey(encryptKeyValues, encryptString);

        return encryptString;
    }

    public string Decrypt(string encryptedString)
    {
        List<Byte[]> bytes = new List<byte[]>();
        for (int i = 0; i < encryptedString.Length; i++)
        {
            bytes.Add(BitConverter.GetBytes(encryptedString[i]));
        }


        List<Byte[]> saveData = new List<byte[]>();
        List<Byte[]> decryptingData = new List<byte[]>();

        for (int i = 0; i < bytes.Count; i++)
        {
            if (i < 2 || i >= (bytes.Count - 2))
            {
                decryptingData.Add(bytes[i]);
            }
            else
            {
                saveData.Add(bytes[i]);
            }
        }

        List<int> decryptValues = DecryptOpenKey(decryptingData);

        string decryptedString = "";
        for (int i = 0; i < saveData.Count; i++)
        {
            if (i < saveData.Count * 0.3f)
            {
                saveData[i][1] = (byte)(~saveData[i][1] - decryptValues[1]);
                decryptedString = decryptedString + BitConverter.ToChar(saveData[i], 0);
            }
            else if (i >= saveData.Count * 0.3f && i < saveData.Count * 0.6f)
            {
                saveData[i][1] = (byte)(~(~saveData[i][1] - decryptValues[2]) + decryptValues[0]);
                decryptedString = decryptedString + BitConverter.ToChar(saveData[i], 0);
            }
            else
            {
                saveData[i][1] = (byte)~(saveData[i][1] + decryptValues[3]);
                decryptedString = decryptedString + BitConverter.ToChar(saveData[i], 0);
            }
        }

        return decryptedString;
    }

    private List<int> GenerateOpenKey()
    {
        List<int> values = new List<int>();

        for (int i = 0; i < 4; i++)
        {
            values.Add(UnityEngine.Random.Range(1, 20));
        }

        return values;
    }

    private string EncryptAndIncapsulateOpenKey(List<int> encryptValues, string encryptString) // Тут еще можно рандомизировать смещения, сделав закрытый ключ и сохранять его
                                                                                               // в директориях игры (либо тянуть с сервера)
    {
        List<Byte[]> encryptValuesBytes = new List<byte[]>();
        for (int i = 0; i < encryptValues.Count; i++)
        {
            encryptValuesBytes.Add(BitConverter.GetBytes(encryptValues[i]));
        }

        for (int i = 0; i < encryptValuesBytes.Count; i++)
        {
            if (i < 2)
            {
                encryptValuesBytes[i][1] = (byte)~(encryptValuesBytes[i][1] + 10);
                encryptString = BitConverter.ToChar(encryptValuesBytes[i], 0) + encryptString;
            }
            else
            {
                encryptValuesBytes[i][1] = (byte)~(encryptValuesBytes[i][1] - 15);
                encryptString = encryptString + BitConverter.ToChar(encryptValuesBytes[i], 0);
            }
        }

        return encryptString;
    }

    private List<int> DecryptOpenKey(List<byte[]> decryptingData)
    {
        List<int> decryptValues = new List<int>();
        for (int i = 0; i < decryptingData.Count; i++)
        {
            if (i < 2)
            {
                decryptingData[i][1] = (byte)(~(decryptingData[i][1]) - 10);
                decryptValues.Add(BitConverter.ToChar(decryptingData[i], 0));
            }
            else
            {
                decryptingData[i][1] = (byte)(~decryptingData[i][1] + 15);
                decryptValues.Add(BitConverter.ToChar(decryptingData[i], 0));
            }
        }

        return decryptValues;
    }
}