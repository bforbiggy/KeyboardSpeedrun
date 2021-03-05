using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class SaveData : MonoBehaviour
{
    public SwitchScene switcher;

    private static float bestTime = float.MaxValue;
    private static int bestClickCount = 0;

    private static readonly string path = "EncryptedResult.txt";
    private static readonly string key = "wow psyight is so freakin' cool!";

    public static string EncryptString(string key, string plainText)
    {
        byte[] iv = new byte[16];
        byte[] array;

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;

            ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream())
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                    {
                        streamWriter.Write(plainText);
                    }

                    array = memoryStream.ToArray();
                }
            }
        }

        return Convert.ToBase64String(array);
    }

    public static string DecryptString(string key, string cipherText)
    {
        byte[] iv = new byte[16];
        byte[] buffer = Convert.FromBase64String(cipherText);

        using (Aes aes = Aes.Create())
        {
            aes.Key = Encoding.UTF8.GetBytes(key);
            aes.IV = iv;
            ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

            using (MemoryStream memoryStream = new MemoryStream(buffer))
            {
                using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                    {
                        return streamReader.ReadToEnd();
                    }
                }
            }
        }
    }

    private void ReadData()
    {
        if(File.Exists(path))
        {
            //Read and decrypt data
            string data = File.ReadAllText(path);
            data = DecryptString(key, data);

            //Read time elapsed portion and convert
            int colonPos = data.IndexOf(":") + 1;
            bestTime = float.Parse(data.Substring(colonPos, data.IndexOf(",") - colonPos));

            //Read click count portion and convert
            bestClickCount = int.Parse(data.Substring(data.LastIndexOf(":") + 1));
        }

        //Update data to match high score
        if(DataHub.totalTime < bestTime)
        {
            bestTime = DataHub.totalTime;
            bestClickCount = DataHub.totalClicks;
        }
    }

    private void WriteData()
    {
        string text = $"Elapsed Time:{bestTime}, Total Clicks:{bestClickCount}";
        text = EncryptString(key, text);
        File.WriteAllText(path, text);
    }

    public void DoAll()
    {
        ReadData();
        WriteData();
        switcher.SwitchMenu();
    }
}
