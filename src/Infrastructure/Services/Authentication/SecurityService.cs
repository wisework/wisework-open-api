using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WW.Application.Common.Interfaces;

namespace WW.Infrastructure.Services.Authentication;
public class SecurityService : ISecurityService
{
    private string Password { get; set; } = "Secured";


    private int SaltLength { get; set; } = 16;


    public byte[] GetRandomBytes()
    {
        try
        {
            byte[] array = new byte[SaltLength];
            RandomNumberGenerator.Create().GetBytes(array);
            return array;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
    {
        try
        {
            byte[] result = null;
            byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using RijndaelManaged rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.KeySize = 256;
                rijndaelManaged.BlockSize = 128;
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                rijndaelManaged.Mode = CipherMode.CBC;
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cryptoStream.Close();
                }

                result = memoryStream.ToArray();
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
    {
        try
        {
            byte[] result = null;
            byte[] salt = new byte[8] { 1, 2, 3, 4, 5, 6, 7, 8 };
            using (MemoryStream memoryStream = new MemoryStream())
            {
                using RijndaelManaged rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.KeySize = 256;
                rijndaelManaged.BlockSize = 128;
                Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                rijndaelManaged.Key = rfc2898DeriveBytes.GetBytes(rijndaelManaged.KeySize / 8);
                rijndaelManaged.IV = rfc2898DeriveBytes.GetBytes(rijndaelManaged.BlockSize / 8);
                rijndaelManaged.Mode = CipherMode.CBC;
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, rijndaelManaged.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cryptoStream.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                    cryptoStream.Close();
                }

                result = memoryStream.ToArray();
            }

            return result;
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string Encrypt(string data)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Password);
            byte[] passwordBytes = SHA256.Create().ComputeHash(bytes);
            byte[] bytes2 = Encoding.UTF8.GetBytes(data);
            byte[] randomBytes = GetRandomBytes();
            byte[] array = new byte[randomBytes.Length + bytes2.Length];
            for (int i = 0; i < randomBytes.Length; i++)
            {
                array[i] = randomBytes[i];
            }

            for (int j = 0; j < bytes2.Length; j++)
            {
                array[j + randomBytes.Length] = bytes2[j];
            }

            array = AES_Encrypt(array, passwordBytes);
            return Convert.ToBase64String(array);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string Decrypt(string data)
    {
        try
        {
            byte[] bytes = Encoding.UTF8.GetBytes(Password);
            byte[] passwordBytes = SHA256.Create().ComputeHash(bytes);
            data = data.Replace(" ", "+");
            byte[] bytesToBeDecrypted = Convert.FromBase64String(data);
            byte[] array = AES_Decrypt(bytesToBeDecrypted, passwordBytes);
            int saltLength = SaltLength;
            byte[] array2 = new byte[array.Length - saltLength];
            for (int i = 0; i < array2.Length; i++)
            {
                array2[i] = array[i + saltLength];
            }

            return Encoding.UTF8.GetString(array2);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string RandomPassword(int passwordLength)
    {
        try
        {
            string text = "123456789ABCDEFGHIJKLMNPQRSTUVWXYZabcdefghijkmnpqrstuvwxyz";
            Random random = new Random();
            char[] array = new char[passwordLength];
            for (int i = 0; i < passwordLength; i++)
            {
                int index = random.Next(text.Length);
                array[i] = text[index];
            }

            return new string(array);
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string MD5HashCode(string data)
    {
        string empty = string.Empty;
        using MD5 md5Hash = MD5.Create();
        empty = GetMd5Hash(md5Hash, data);
        return empty.ToUpper();
    }

    public string GetMd5Hash(MD5 md5Hash, string input)
    {
        byte[] array = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < array.Length; i++)
        {
            stringBuilder.Append(array[i].ToString("x2"));
        }

        return stringBuilder.ToString();
    }

    public bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
    {
        string md5Hash2 = GetMd5Hash(md5Hash, input);
        StringComparer ordinalIgnoreCase = StringComparer.OrdinalIgnoreCase;
        if (ordinalIgnoreCase.Compare(md5Hash2, hash) == 0)
        {
            return true;
        }

        return false;
    }
}
