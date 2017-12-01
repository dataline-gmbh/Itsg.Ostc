using System;
using System.Linq;
using System.Xml.Linq;

namespace Itsg.Ostc1.OrderStatusParser
{
    class OrderStatusParserV1 : IOrderStatusParser
    {
        public bool IsApplicable(XDocument responsePage)
        {
            return responsePage.Elements("html").Elements("body").Elements("form").Any();
        }

        public OstcOrderResult ExtractResult(Uri responseUri, XDocument responsePage)
        {

            var statusImageXml = responsePage
                .Elements("html")
                .Elements("body")
                .Elements("form")
                .Elements("table")
                .Elements("tr")
                .Elements("td").Where(x => x.Attributes("class").Any(y => y.Value== "sidebarFooter"))
                .Elements("img")
                .FirstOrDefault();
            if (statusImageXml == null)
                statusImageXml = responsePage
                    .Elements("html")
                    .Elements("body")
                    .Elements("blockquote")
                    .Elements("img")
                    .FirstOrDefault();
            if (statusImageXml == null)
                return null;

            var statusImageSrc = statusImageXml.Attributes("src").Select(x => x.Value).FirstOrDefault();
            switch (System.IO.Path.GetFileNameWithoutExtension(statusImageSrc))
            {
                case "ampel_gruen":
                    break;
                case "ampel_gelb":
                    return new OstcOrderResult() { Status = OstcOrderStatus.Processing };
                default:
                    return new OstcOrderResult() { Status = OstcOrderStatus.Failed, Message = statusImageXml.Value };
            }
            var certLinkXml = statusImageXml
                .Elements("p")
                .Elements("a")
                .FirstOrDefault();
            if (certLinkXml == null)
            {
                // Darf eigentlich nicht passieren...
                return new OstcOrderResult()
                {
                    Status = OstcOrderStatus.Unknown
                };
            }

            var hrefAttr = certLinkXml.Attribute("href");
            if (hrefAttr == null)
            {
                // Darf eigentlich nicht passieren...
                return new OstcOrderResult()
                {
                    Status = OstcOrderStatus.Unknown
                };
            }

            var certUri = new Uri(responseUri, hrefAttr.Value);

            return new OstcOrderResult { Status = OstcOrderStatus.Successful, DownloadUrl = certUri };
        }
    }
}
