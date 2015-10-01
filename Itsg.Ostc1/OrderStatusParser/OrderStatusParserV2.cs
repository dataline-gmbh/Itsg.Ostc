using System;
using System.Linq;
using System.Xml.Linq;

namespace Itsg.Ostc1.OrderStatusParser
{
    class OrderStatusParserV2 : IOrderStatusParser
    {
        private const string ProcessFinishedText = "Bearbeitung ist abgeschlossen";
        private const string ProcessNotStarted = "noch nicht in Bearbeitung";

        public bool IsApplicable(XDocument responsePage)
        {
            return true;
        }

        public OstcOrderResult ExtractResult(Uri responseUri, XDocument responsePage)
        {
            var ns = XNamespace.Get("http://www.w3.org/1999/xhtml");
            var statusElement = responsePage
                .Elements(ns+"html")
                .Elements(ns+"body")
                .Elements(ns+"div").Where(x => x.Attributes("id").Any(y => y.Value == "tab1"))
                .Elements(ns+"div")
                .Elements(ns + "table")
                .Elements(ns + "tr").Skip(3).Take(1)
                .Elements(ns + "td").Skip(1).FirstOrDefault();
            if (statusElement == null)
                return null;

            var message = statusElement.Value.Trim();
            switch (message)
            {
                case ProcessFinishedText:
                {
                    var downloadElement = responsePage
                        .Elements(ns + "html")
                        .Elements(ns + "body")
                        .Elements(ns + "div").Where(x => x.Attributes("id").Any(y => y.Value == "tab3"))
                        .Elements(ns + "div")
                        .Elements(ns + "table")
                        .Elements(ns + "tr").Skip(0).Take(1)
                        .Elements(ns + "td")
                        .Elements(ns + "a").FirstOrDefault();
                    if (downloadElement == null)
                    {
                        // Darf eigentlich nicht passieren...
                        return new OstcOrderResult
                        {
                            Status = OstcOrderStatus.Unknown,
                        };
                    }
                    var hrefAttrib = downloadElement.Attribute("href");
                    if (hrefAttrib == null)
                    {
                        // Darf eigentlich nicht passieren...
                        return new OstcOrderResult
                        {
                            Status = OstcOrderStatus.Unknown,
                        };
                    }

                    var certUri = new Uri(responseUri, hrefAttrib.Value);
                    return new OstcOrderResult
                    {
                        Status = OstcOrderStatus.Successful,
                        DownloadUrl = certUri,
                    };
                }
                case ProcessNotStarted:
                    return new OstcOrderResult()
                    {
                        Status = OstcOrderStatus.NotProcessed,
                        Message = message,
                    };
            }

            return new OstcOrderResult()
            {
                Status = OstcOrderStatus.NotFound,
                Message = message,
            };
        }
    }
}
