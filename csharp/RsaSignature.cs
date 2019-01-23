using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Signing
{
    public static class RsaSignature
    {
        public static byte[] Sign(byte[] data)
        {
            // load certificate from file
            string fileName = @"C:\Temp\Tarball\Certs\certificate.p12";
            string pwd = "matsbader3694";

            // Exportable: used to get RSAParameters below; otherwise not needed.
            X509Certificate2 x509 = new X509Certificate2(fileName, pwd, X509KeyStorageFlags.Exportable);

            // https://dusted.codes/how-to-use-rsa-in-dotnet-rsacryptoserviceprovider-vs-rsacng-and-good-practise-patterns
            RSA rsa = (RSA)x509.PrivateKey;
            byte[] signature = rsa.SignData(data, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            // Export the key information to an RSAParameters object. You must pass true to export the private key for signing.
            // However, you do not need to export the private key for verification.
            RSAParameters key = rsa.ExportParameters(true);
            return signature;
        }

        public static bool Verify(byte[] data, byte[] signature)
        {
            // TODO: Load from cert (without private key!)
            // load certificate from file
            string fileName = @"C:\Temp\Tarball\Certs\certificate.p12";
            string pwd = "matsbader3694";

            // Exportable: used to get RSAParameters below; otherwise not needed.
            X509Certificate2 x509 = new X509Certificate2(fileName, pwd, X509KeyStorageFlags.Exportable);

            RSA publicKeyProvider = (RSA)x509.PublicKey.Key;

            bool isValid = publicKeyProvider.VerifyData(data, signature, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            return isValid;
        }
    }
}
