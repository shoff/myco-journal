namespace Zatoichi.Common.Infrastructure.Security
{
    public interface IEncryptor
    {
        byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv);
        string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv);
        string EncryptString(string text);
        string DecryptString(string text);
        byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] salt);
        byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt);
        byte[] GetRandomBytes();
        string Sha256Hash(string s);
        void SetPassword(string password);
    }
}