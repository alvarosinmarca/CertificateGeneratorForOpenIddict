
namespace CertificateGeneratorForOpenIddict
{
    public class Config
    {
        public TypeCertificate TypeCertificate { get; private set; }
        public string DistinguishedName { get; private set; }
        public string Password { get; private set; }
        public string PathOutGenerated { get; private set; }

        public Config(TypeCertificate typeCertificate, string distinguishedName, string password, string pathOutGenerated)
        {
            TypeCertificate = typeCertificate;
            DistinguishedName = distinguishedName;
            Password = password;
            PathOutGenerated = pathOutGenerated;
        }
    }
}
