using System;
using System.Security.Cryptography;

namespace Encryption
{
    public class AesEncryptor
    {
        public AesEncryptor(byte[] key)
        {
            if (key.Length != 32)
            {
                throw new ArgumentException("Key for AEs-CBC-256 has to have length 32 bytes.");
            }
            Key = key;
        }

        private byte[] Key { get; }

        public EncryptionResult Encrypt(byte[] originalBytes)
        {
            // create initialization-vector (IV): 16 bytes (128 bits)
            // iv is not secret (but unique). Likely to be prepended to the data... 
            byte[] iv = new byte[16];
            new RNGCryptoServiceProvider().GetBytes(iv);

            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Key = Key;
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                byte[] encryptedBytes = encryptor.TransformFinalBlock(originalBytes, 0, originalBytes.Length);
                return new EncryptionResult(iv, encryptedBytes);
            }
        }

        public byte[] Decrypt(EncryptionResult encryptionResult)
        {
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                aesAlg.BlockSize = 128;
                aesAlg.KeySize = 256;
                aesAlg.Key = Key;
                aesAlg.IV = encryptionResult.Iv;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                byte[] decryptedBytes = decryptor.TransformFinalBlock(encryptionResult.EncryptedBytes, 0, encryptionResult.EncryptedBytes.Length);
                return decryptedBytes;
            }
        }

        public class EncryptionResult
        {
            public EncryptionResult(byte[] iv, byte[] encryptedBytes)
            {
                Iv = iv;
                EncryptedBytes = encryptedBytes;
            }
            public byte[] Iv { get; }
            public byte[] EncryptedBytes { get; }
        }
    }
}
