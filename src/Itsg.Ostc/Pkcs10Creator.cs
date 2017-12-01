using System.IO;

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace Itsg.Ostc
{
    /// <summary>
    /// Erstellt die für die Zertifikatsanforderung notwendige PKCS#10-Datei
    /// </summary>
    public class Pkcs10Creator
    {
        /// <summary>
        /// Antragsteller
        /// </summary>
        public IRequester Requester { get; }

        /// <summary>
        /// Paßwort für den privaten Teil des RSA-Schlüssels
        /// </summary>
        public string Password { get; }

        /// <summary>
        /// RSA-Schlüssel
        /// </summary>
        public AsymmetricCipherKeyPair RSA { get; }

        /// <summary>
        /// Konstruktur
        /// </summary>
        /// <param name="requester">Antragsteller</param>
        /// <param name="password">Paßwort mit dem der private RSA-Schlüssel verschlüsselt werden soll</param>
        public Pkcs10Creator(IRequester requester, string password)
        {
            Requester = requester;
            Password = password;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="requester">Antragsteller</param>
        /// <param name="rsa">RSA-Schlüssel</param>
        public Pkcs10Creator(IRequester requester, AsymmetricCipherKeyPair rsa)
        {
            Requester = requester;
            RSA = rsa;
        }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="requester">Antragsteller</param>
        /// <param name="rsaPrivateKey">RSA-Schlüssel</param>
        public Pkcs10Creator(Requester requester, RsaPrivateCrtKeyParameters rsaPrivateKey)
        {
            Requester = requester;
            var rsaPublicKey = new RsaKeyParameters(false, rsaPrivateKey.Modulus, rsaPrivateKey.PublicExponent);
            RSA = new AsymmetricCipherKeyPair(rsaPublicKey, rsaPrivateKey);
        }

        /// <summary>
        /// Erstellt die für die PKCS#10-Datei notwendigen Daten
        /// </summary>
        /// <returns>Die für die PKCS#10-Datei notwendigen Daten</returns>
        public Pkcs10Data CreateRequest()
        {
            var sigAlgoName = "SHA256WITHRSA";
            var subject = new X509Name(
                true,
                $"CN={Requester.Surname}, OU={Requester.Number}, OU={Requester.CompanyName}, O={"ITSG TrustCenter fuer Arbeitgeber"}, C={"DE"}",
                new Pkcs.X509ItsgEntryConverter()
            );

            var rng = new SecureRandom();
            var rsaKeyPair = RSA;
            if (rsaKeyPair == null)
            {
                var keyPairGen = new RsaKeyPairGenerator();
                keyPairGen.Init(new KeyGenerationParameters(rng, 2048));
                rsaKeyPair = keyPairGen.GenerateKeyPair();
            }

            var csr = new Pkcs10CertificationRequest(new Asn1SignatureFactory(sigAlgoName, rsaKeyPair.Private), subject, rsaKeyPair.Public, null, rsaKeyPair.Private);
            
            var outputCsrPem = new StringWriter();
            {
                var pemWriter = new PemWriter(outputCsrPem);
                pemWriter.WriteObject(csr);
            }

            var outputPrivateKeyPem = new StringWriter();
            {
                var pemWriter = new PemWriter(outputPrivateKeyPem);
                if (string.IsNullOrEmpty(Password))
                {
                    pemWriter.WriteObject(rsaKeyPair.Private);
                }
                else
                {
                    pemWriter.WriteObject(
                        rsaKeyPair.Private,
                        "des-ede3-cbc",
                        Password.ToCharArray(),
                        rng
                    );
                }
            }

            var rsaPubKey = (RsaKeyParameters)rsaKeyPair.Public;
            var rawPubKeyData = OstcUtils.CalculatePublicKeyHash(rsaPubKey);

            return new Pkcs10Data(csr.GetEncoded(), outputCsrPem.ToString(), outputPrivateKeyPem.ToString(), rawPubKeyData);
        }
    }
}
