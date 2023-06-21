using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WW.Application.Common.Interfaces;
public interface ISecurityService
{
    byte[] GetRandomBytes();
    byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes);
    byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes);
    string Encrypt(string data);
    string Decrypt(string data);
    string RandomPassword(int passwordLength);
    string MD5HashCode(string data);
    string GetMd5Hash(MD5 md5Hash, string input);
    bool VerifyMd5Hash(MD5 md5Hash, string input, string hash);
}
