using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;

namespace Itsg.Ostc.Pkcs
{
    /// <summary>
    /// X509EntryConverter für die ITSG, weil die ITSG kein DerUtf8String erlaubt, sondern nur DerPrintableString...
    /// </summary>
    class X509ItsgEntryConverter : X509DefaultEntryConverter
    {
        public override Asn1Object GetConvertedValue(DerObjectIdentifier oid, string value)
        {
            var result = base.GetConvertedValue(oid, value);
            if (result is DerUtf8String)
            {
                result = new DerPrintableString(value);
            }
            return result;
        }
    }
}
