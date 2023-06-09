﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WW.Application.Common.Interfaces;

namespace WW.Infrastructure.Services.Authentication;

public class CryptographyService : ICryptographyService
{
    public bool vaildatePassword(string in1, string in2, string GUID)
    {
        var resultPassword = this.generatePassword(in2, GUID);

        return resultPassword == in1;
    }
    public string generatePassword(string password, string GUID)
    {
        byte[] bpassword = Encoding.ASCII.GetBytes(password);
        byte[] salt = Encoding.ASCII.GetBytes(GUID);
        int dklen = 256;
        int iterationCount = 7072;
        byte[] result = this.PBKDF2Sha256GetBytes(
            dklen,
            bpassword,
            salt,
            iterationCount
        );
        return Convert.ToBase64String(result);
    }
    public byte[] PBKDF2Sha256GetBytes(int dklen, byte[] password, byte[] salt, int iterationCount)
    {
        using (var hmac = new System.Security.Cryptography.HMACSHA256(password))
        {
            int hashLength = hmac.HashSize / 8;
            if ((hmac.HashSize & 7) != 0)
                hashLength++;
            int keyLength = dklen / hashLength;
            if ((long)dklen > (0xFFFFFFFFL * hashLength) || dklen < 0)
                throw new ArgumentOutOfRangeException("dklen");
            if (dklen % hashLength != 0)
                keyLength++;
            byte[] extendedkey = new byte[salt.Length + 4];
            Buffer.BlockCopy(salt, 0, extendedkey, 0, salt.Length);
            using (var ms = new System.IO.MemoryStream())
            {
                for (int i = 0; i < keyLength; i++)
                {
                    extendedkey[salt.Length] = (byte)(((i + 1) >> 24) & 0xFF);
                    extendedkey[salt.Length + 1] = (byte)(((i + 1) >> 16) & 0xFF);
                    extendedkey[salt.Length + 2] = (byte)(((i + 1) >> 8) & 0xFF);
                    extendedkey[salt.Length + 3] = (byte)(((i + 1)) & 0xFF);
                    byte[] u = hmac.ComputeHash(extendedkey);
                    Array.Clear(extendedkey, salt.Length, 4);
                    byte[] f = u;
                    for (int j = 1; j < iterationCount; j++)
                    {
                        u = hmac.ComputeHash(u);
                        for (int k = 0; k < f.Length; k++)
                        {
                            f[k] ^= u[k];
                        }
                    }
                    ms.Write(f, 0, f.Length);
                    Array.Clear(u, 0, u.Length);
                    Array.Clear(f, 0, f.Length);
                }
                byte[] dk = new byte[dklen];
                ms.Position = 0;
                ms.Read(dk, 0, dklen);
                ms.Position = 0;
                for (long i = 0; i < ms.Length; i++)
                {
                    ms.WriteByte(0);
                }
                Array.Clear(extendedkey, 0, extendedkey.Length);
                return dk;
            }
        }
    }
}
