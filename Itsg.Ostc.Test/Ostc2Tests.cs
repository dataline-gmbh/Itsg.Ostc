using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

using ExtraStandard.Validation;

using Itsg.Ostc2;
using Itsg.Ostc2.Validator;

using Xunit;

namespace Itsg.Ostc.Test
{
    public class Ostc2Tests
    {
        [Theory]
        [InlineData("Muster_1a_Antragsdaten_alle Elemente.xml")]
        [InlineData("Muster_1a_Antragsdaten_Pflichtelemente.xml")]
        public void ValidationTestSuccess(string orderResName)
        {
            byte[] data;
            using (var resStream = GetType().Assembly.GetManifestResourceStream(GetType(), $"Data.Ostc2.{orderResName}"))
            {
                Assert.NotNull(resStream);
                using (var temp = new MemoryStream())
                {
                    resStream.CopyTo(temp);
                    data = temp.ToArray();
                }
            }

            var validator = new OstcExtraValidator(OstcMessageType.ApplicationData, ExtraTransportDirection.Request);
            validator.Validate(data);
        }

        [Fact]
        [UseCulture("en-US")]
        public void ValidationTestFailure()
        {
            var order = new OstcAntrag()
            {
                Antragsteller = new OstcAntragAntragsteller()
                {
                    IK_BN = "12345678",
                },
            };
            var encoding = Encoding.GetEncoding("iso-8859-1");
            var data = OstcUtils.Serialize(order, encoding);

            var validator = new OstcExtraValidator(OstcMessageType.ApplicationData, ExtraTransportDirection.Request);
            var ex = Assert.Throws<XmlSchemaValidationException>(() => validator.Validate(data));
            Assert.Equal("The 'IK_BN' element is invalid - The value '12345678' is invalid according to its datatype 'sType_an10_11' - The actual length is less than the MinLength value.", ex.Message);
        }

        [Theory()]
        [InlineData(OstcListeListe.annahmepkcsagv)]
        [InlineData(OstcListeListe.annahmepkcskey)]
        [InlineData(OstcListeListe.annahmesha256agv)]
        [InlineData(OstcListeListe.annahmesha256key)]
        public async Task LoadCertificatesAsync(OstcListeListe certList)
        {
            var sender = new OstcSender(SenderId.FromBnr("99300006"), "Test");
            var cred = new NetworkCredential("dataline", "a5pY_4cm");
            var client = new OstcClient(sender, Network.Base.Test, cred, new OstcClientInfo("Dataline", "Dataline Office", 21412))
            {
                OstcExtraValidatorFactory = OstcExtraValidator.Factory
            };
            var certs = await client.DownloadCertificateListAsync(certList);
            Assert.NotNull(certs);
            Assert.NotEqual(0, certs.Count);
        }
    }
}
