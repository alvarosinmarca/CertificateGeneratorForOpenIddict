using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace CertificateGeneratorForOpenIddict
{
    public class Generator
    {
        public static void GenerateCertificate(Config config)
        {
            var subject = new X500DistinguishedName(config.DistinguishedName);

            using var algorithm = RSA.Create(keySizeInBits: 2048);

            var request = new CertificateRequest(subject, algorithm, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);

            switch (config.TypeCertificate)
            {

                case TypeCertificate.Encryption:
                    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: true));
                    break;

                case TypeCertificate.Signature:
                    request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.DigitalSignature, critical: true));
                    break;

                default:
                    throw new Exception("Case TypeCertificate not implemented");
            }

            var certificate = request.CreateSelfSigned(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow.AddYears(2));

            // Note: setting the friendly name is not supported on Unix machines (including Linux and macOS). 
            // To ensure an exception is not thrown by the property setter, an OS runtime check is used here. 
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                certificate.FriendlyName = $"OpenIddict Server Development {config.TypeCertificate.ToString()} Certificate";
            }

            // Note: CertificateRequest.CreateSelfSigned() doesn't mark the key set associated with the certificate 
            // as "persisted", which eventually prevents X509Store.Add() from correctly storing the private key. 
            // To work around this issue, the certificate payload is manually exported and imported back 
            // into a new X509Certificate2 instance specifying the X509KeyStorageFlags.PersistKeySet flag. 
            var data = certificate.Export(X509ContentType.Pfx, config.Password);

            if (!Directory.Exists(config.PathOutGenerated))
                Directory.CreateDirectory(config.PathOutGenerated);

            var fullPath = @$"{config.PathOutGenerated}\{config.TypeCertificate.ToString()}.pfx";

            System.IO.File.WriteAllBytes(fullPath, data);
            Console.WriteLine($"Certificate generated in: {fullPath}");
        }
    }
}
