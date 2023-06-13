namespace WW.Application.Common.Interfaces;
public interface ICryptography
{
    bool vaildatePassword(string in1, string in2, string GUID);
    string generatePassword(string password, string GUID);
    byte[] PBKDF2Sha256GetBytes(int dklen, byte[] password, byte[] salt, int iterationCount);
}
