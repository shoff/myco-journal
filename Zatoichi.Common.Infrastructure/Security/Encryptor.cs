namespace Zatoichi.Common.Infrastructure.Security
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;
    using ChaosMonkey.Guards;
    using Configuration;
    using Microsoft.Extensions.Options;

    public class Encryptor : IEncryptor
    {
        private readonly byte[] baSalt;
        private readonly IOptions<Crypt> options;

        public Encryptor(IOptions<Crypt> options)
        {
            Guard.IsNotNull(options, nameof(options));
            Guard.IsNotNull(options.Value, nameof(options.Value));
            this.options = options;
            this.baSalt = Encoding.UTF8.GetBytes(options.Value.Key);
        }

        public byte[] EncryptStringToBytes(string plainText, byte[] key, byte[] iv)
        {
            byte[] encrypted;
            // Create an Rijndael object
            // with the specified key and IV.
            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create an encryptor to perform the stream transform.
                var encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using var msEncrypt = new MemoryStream();
                using var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
                using (var swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }

                encrypted = msEncrypt.ToArray();
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            string plaintext;

            // Create an Rijndael object
            // with the specified key and IV.
            using (var rijAlg = Rijndael.Create())
            {
                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decryptor to perform the stream transform.
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using var msDecrypt = new MemoryStream(cipherText);
                using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
                using var srDecrypt = new StreamReader(csDecrypt);
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                plaintext = srDecrypt.ReadToEnd();
            }

            return plaintext;
        }

        public string EncryptString(string text)
        {
            var baPwd = Encoding.UTF8.GetBytes(this.options.Value.Password);
            var baPwdHash = HashPassword(baPwd, this.baSalt, this.options.Value.Iterations);

            var baText = Encoding.UTF8.GetBytes(text);

            if (this.options.Value.CompressText)
            {
                baText = baText.Compress();
            }

            var baEncrypted = AES_Encrypt(baText, baPwdHash, this.baSalt);
            var result = Convert.ToBase64String(baEncrypted);
            return result;
        }

        public string DecryptString(string text)
        {
            var baPwd = Encoding.UTF8.GetBytes(this.options.Value.Password);
            var baPwdHash = HashPassword(baPwd, this.baSalt, this.options.Value.Iterations);

            var baText = Convert.FromBase64String(text);
            var baDecrypted = AES_Decrypt(baText, baPwdHash, this.baSalt);

            var result = Encoding.UTF8.GetString(baDecrypted);
            return result;
        }

        public byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] salt)
        {
            byte[] encryptedBytes;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            //byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (var ms = new MemoryStream())
            {
                using var aes = new RijndaelManaged { KeySize = 256, BlockSize = 128 };

                var key = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
                aes.Key = key.GetBytes(aes.KeySize / 8);
                aes.IV = key.GetBytes(aes.BlockSize / 8);

                aes.Mode = CipherMode.CBC;

                using (var cs = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                    cs.Close();
                }

                encryptedBytes = ms.ToArray();
            }

            return encryptedBytes;
        }

        public byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] salt)
        {
            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            using var ms = new MemoryStream();
            using var aes = new RijndaelManaged { KeySize = 256, BlockSize = 128 };
            var key = new Rfc2898DeriveBytes(passwordBytes, salt, 1000);
            aes.Key = key.GetBytes(aes.KeySize / 8);
            aes.IV = key.GetBytes(aes.BlockSize / 8);
            aes.Mode = CipherMode.CBC;
            using var cs = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write);
            cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
            cs.Close();
            var decryptedBytes = ms.ToArray();

            return decryptedBytes;
        }

        public byte[] GetRandomBytes()
        {
            var saltLength = GetSaltLength();
            var ba = new byte[saltLength];
            RandomNumberGenerator.Create().GetBytes(ba);
            return ba;
        }

        internal int GetSaltLength()
        {
            return 8;
        }

        internal byte[] GenerateSalt()
        {
            using var randomNumberGenerator = new RNGCryptoServiceProvider();
            var randomNumber = new byte[32];
            randomNumberGenerator.GetBytes(randomNumber);

            return randomNumber;
        }

        internal byte[] HashPassword(byte[] toBeHashed, byte[] salt, int numberOfIterations)
        {
            using var rfc2898 = new Rfc2898DeriveBytes(toBeHashed, salt, numberOfIterations);
            return rfc2898.GetBytes(32);
        }

        /// <summary>
        ///     Creates a one way hash for string passed using the SHA256 hashing algorithm.
        /// </summary>
        /// <param name="s"> The string to hash. </param>
        /// <returns> </returns>
        public string Sha256Hash(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return s;
            }

            var sha256 = SHA256.Create();

            var dataSha256 = sha256.ComputeHash(Encoding.Default.GetBytes(s));
            var sb = new StringBuilder();
            for (var i = 0; i < dataSha256.Length; i++)
            {
                sb.AppendFormat("{0:x2}", dataSha256[i]);
            }

            return sb.ToString();
        }

        public void SetPassword(string password)
        {
            Guard.IsNotNullOrEmpty(password, nameof(password));
            this.options.Value.Password = password;
        }
    }
}