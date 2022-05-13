using CertificateGeneratorForOpenIddict;

var configEncryption = new Config(
TypeCertificate.Encryption,
"CN=CompanyName.ProjectName",
"SecretPassword",
@"C:\Certificates"
);

var configSignature = new Config(
TypeCertificate.Signature,
"CN=CompanyName.ProjectName",
"SecretPassword",
@"C:\Certificates"
);

Generator.GenerateCertificate(configEncryption);
Generator.GenerateCertificate(configSignature);
