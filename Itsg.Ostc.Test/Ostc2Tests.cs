using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;

using ExtraStandard.Validation;

using Itsg.Ostc1;
using Itsg.Ostc1.Validator;
using Itsg.Ostc2;
using Itsg.Ostc2.Validator;

using Xunit;

namespace Itsg.Ostc.Test
{
    public class Ostc2Tests
    {
        private static readonly CultureInfo _cultureDe = new CultureInfo("de-DE");

        [Theory]
        [InlineData("Muster_1a_Antragsdaten_alle Elemente.xml")]
        [InlineData("Muster_1a_Antragsdaten_Pflichtelemente.xml")]
        public void ValidationTestSuccess(string orderResName)
        {
            byte[] data;
            using (var resStream = this.GetType().Assembly.GetManifestResourceStream(this.GetType(), $"Data.Ostc2.{orderResName}"))
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
            var order = new Ostc2.OstcAntrag()
            {
                Antragsteller = new Ostc2.OstcAntragAntragsteller()
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
    }
}
