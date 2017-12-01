using System;
using System.Xml.Linq;

namespace Itsg.Ostc1.OrderStatusParser
{
    interface IOrderStatusParser
    {
        bool IsApplicable(XDocument responsePage);
        OstcOrderResult ExtractResult(Uri responseUri, XDocument responsePage);
    }
}
