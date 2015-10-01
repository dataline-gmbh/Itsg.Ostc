using System;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

using Itsg.Ostc1;
using Itsg.Ostc1.Validator;

using RestSharp.Portable.WebRequest;

using Xunit;

namespace Itsg.Ostc.Test
{
    public class Ostc1Tests
    {
        private static readonly CultureInfo _cultureDe = new CultureInfo("de-DE");

        [Fact]
        public void ValidationTestSuccess()
        {
            var now = DateTime.Now;
            var order = new OstcAntrag()
            {
                Trustcenter = new OstcAntragTrustcenter()
                {
                    Eingangsnummer = string.Empty,
                    Returncode = string.Empty,
                },
                Antragsteller = new OstcAntragAntragsteller()
                {
                    IK_BN = "1234567890",
                    Firma = "Firma",
                    Anrede = "Anrede",
                    Nachname = "Nachname",
                    Strasse = "Strasse",
                    PLZ = "12345",
                    Ort = "Berlin",
                    Telefon = "0123456789",
                    Email = "email@company.com",
                    Kennwort = "customer password",
                },
                Antragsinfo = new OstcAntragAntragsinfo()
                {
                    Ruecksendung = "10",
                    Generierung = "40",
                    Sperrung = "1",
                    Softwarehaus = "manufacturer",
                    Fachanwendung = "product",
                    Datum = now.ToString("dd.MM.yyyy", _cultureDe),
                    Uhrzeit = now.ToString("HH:mm:ss", _cultureDe),
                },
                Rechnungsadresse = new OstcAntragRechnungsadresse(),
            };
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var data = OstcUtils.Serialize(order, encoding);

            var validator = new OstcAntragValidator();
            validator.Validate(data);
        }

        [Fact]
        [UseCulture("en-US")]
        public void ValidationTestFailure()
        {
            var order = new OstcAntrag()
            {
                Trustcenter = new OstcAntragTrustcenter()
                {
                    Eingangsnummer = string.Empty,
                    Returncode = string.Empty,
                },
                Antragsteller = new OstcAntragAntragsteller()
                {
                    IK_BN = "12345678",
                },
            };
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var data = OstcUtils.Serialize(order, encoding);

            var validator = new OstcAntragValidator();
            var ex = Assert.Throws<XmlSchemaValidationException>(() => validator.Validate(data));
            Assert.Equal("The 'IK_BN' element is invalid - The value '12345678' is invalid according to its datatype 'sType_an10_11' - The actual length is less than the MinLength value.", ex.Message);
        }

        [Theory]
        //[InlineData(OstcCertificateListType.Complete, OstcCertificateType.Sha1)]
        //[InlineData(OstcCertificateListType.Complete, OstcCertificateType.Sha256)]
        [InlineData(OstcCertificateListType.Receiver, OstcCertificateType.Sha1)]
        [InlineData(OstcCertificateListType.Receiver, OstcCertificateType.Sha256)]
        public async Task LoadCertificates(OstcCertificateListType certList, OstcCertificateType certType)
        {
            var restClient = new RestClient(Network.Base.Test);
            var ostcClient = new OstcClient(restClient, null);
            var certs = await ostcClient.DownloadCertificateListAsync(certList, certType);
            Assert.NotNull(certs);
            Assert.NotEqual(0, certs.Count);
        }

        [Theory]
        [InlineData(OstcCertificateType.Sha1)]
        [InlineData(OstcCertificateType.Sha256)]
        public async Task LoadCrl(OstcCertificateType certType)
        {
            var restClient = new RestClient(Network.Base.Test);
            var ostcClient = new OstcClient(restClient, null);
            var crl = await ostcClient.DownloadCrlAsync(certType);
            Assert.NotNull(crl);
            var revokedCerts = crl.GetRevokedCertificates();
            Assert.NotEqual(0, revokedCerts.Count);
        }
    }
}
