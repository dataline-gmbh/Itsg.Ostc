using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;

using ExtraStandard;
using ExtraStandard.Extra11;
using ExtraStandard.Validation;

using Itsg.Ostc;

using JetBrains.Annotations;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;

namespace Itsg.Ostc2
{
    /// <summary>
    /// Client, über den auf die OSTC-Funktionen (V2) zugegriffen wird
    /// </summary>
    public class OstcClient
    {
        internal static readonly CultureInfo CultureDe = new CultureInfo("de-DE");
        internal static readonly Encoding Iso88591 = Encoding.GetEncoding("iso-8859-1");

        private static readonly string ExtraProfileOstc = "http://www.extra-standard.de/profile/DEUEVGKV/1.1";

        [NotNull]
        private readonly Uri _baseUrl;

        [CanBeNull]
        private readonly ICredentials _credentials;

        /// <summary>
        /// Basis-Informationen für den Client
        /// </summary>
        public OstcClientInfo Info { get; }

        /// <summary>
        /// Der Nutzer des OSTC-Client
        /// </summary>
        public OstcSender Sender { get; }

        /// <summary>
        /// Holt oder setzt eine Instanz einer OSTC-eXTra-Validator-Factory über die ein OSTC-eXTra-Validator erstellt werden kann.
        /// </summary>
        /// <remarks>
        /// Standardmäßig ist diese Eigenschaft nicht gesetzt.
        /// </remarks>
        [CanBeNull]
        public IValidatorFactory OstcExtraValidatorFactory { get; set; }

        /// <summary>
        /// Konstruktor
        /// </summary>
        /// <param name="sender">Absender, der den OSTC-Client nutzt (Informationen werden auch für die Zertifikat-Erstellung genutzt)</param>
        /// <param name="baseUrl">Basis-URL, die für die Kommunikation mit dem Server verwendet wird</param>
        /// <param name="credentials">Die Login-Informationen, damit der Client auf den OSTC-Server zugreifen darf</param>
        /// <param name="clientInfo">OSTC-Client-Informationen</param>
        public OstcClient([NotNull] OstcSender sender, [NotNull] Uri baseUrl, [CanBeNull] ICredentials credentials, [NotNull] OstcClientInfo clientInfo)
        {
            if (clientInfo == null)
                throw new ArgumentNullException(nameof(clientInfo), "Es müssen Client-Informationen angegeben werden");
            Sender = sender;
            Info = clientInfo;
            _baseUrl = baseUrl;
            _credentials = credentials;
        }

        private HttpWebRequest CreateRequest(Uri uri)
        {
            var request = WebRequest.CreateHttp(new Uri(_baseUrl, uri));
            request.Method = "POST";
            if (_credentials != null)
            {
                request.Credentials = _credentials;
            }
            request.Accept = "application/xml";
            request.ContentType = "application/xml";
            return request;
        }

        private void ValidateRequest(byte[] request, OstcMessageType messageType)
        {
            Debug.Assert(messageType == OstcMessageType.Application || messageType == OstcMessageType.Key || messageType == OstcMessageType.List || messageType == OstcMessageType.Order);
            if (OstcExtraValidatorFactory == null)
                return;
            var validator = OstcExtraValidatorFactory.Create(messageType, ExtraTransportDirection.Request);
            var document = XDocument.Load(new MemoryStream(request));
            validator.Validate(document);
        }

        private void ValidateRequest(TransportRequestType request, OstcMessageType messageType)
        {
            var data = OstcUtils.Serialize(request, Encoding.UTF8);
            ValidateRequest(data, messageType);
        }

        private void ValidateData(byte[] data, OstcMessageType messageType)
        {
            Debug.Assert(messageType == OstcMessageType.ApplicationData || messageType == OstcMessageType.KeyData || messageType == OstcMessageType.ListData || messageType == OstcMessageType.OrderData);
            if (OstcExtraValidatorFactory == null)
                return;
            var validator = OstcExtraValidatorFactory.Create(messageType, ExtraTransportDirection.Request);
            var document = XDocument.Load(new MemoryStream(data));
            validator.Validate(document);
        }

        private TransportRequestHeaderType CreateRequestHeader(DateTime now, string dataType, string scenario)
        {
            var receiverId = Sender.SenderId.CommunicationServerReceiver;
            return new TransportRequestHeaderType
            {
                Sender = new SenderType
                {
                    SenderID = new ClassifiableIDType
                    {
                        @class = Sender.SenderId.Type.ToString(),
                        Value = Sender.SenderId.Id,
                    },
                    Name = new TextType
                    {
                        Value = Sender.CompanyName,
                    },
                },
                Receiver = new ReceiverType
                {
                    ReceiverID = new ClassifiableIDType
                    {
                        @class = receiverId.Type.ToString(),
                        Value = receiverId.Id,
                    },
                },
                RequestDetails = new RequestDetailsType
                {
                    RequestID = new ClassifiableIDType
                    {
                        Value = Guid.NewGuid().ToString(),
                    },
                    TimeStamp = now,
                    TimeStampSpecified = true,
                    Application = new ApplicationType
                    {
                        Manufacturer = Info.Manufacturer,
                        Product = new TextType
                        {
                            Value = Info.Product,
                        },
                        RegistrationID = new ClassifiableIDType
                        {
                            Value = Info.RegistrationId.ToString(),
                        },
                    },
                    Procedure = "http://www.itsg.de/ostc",
                    DataType = dataType,
                    Scenario = scenario,
                },
            };
        }

        /// <summary>
        /// Antragstellung
        /// </summary>
        /// <param name="application">Antrag</param>
        /// <returns>Ergebnis der Antragstellung</returns>
        /// <remarks>
        /// Es wird für die PKCS#10-Erstellung ein neuer RSA-Schlüssel erstellt. Es ist zwingend notwendig, dass der private
        /// Teil des RSA-Schlüssels vom Aufrufer gespeichert wird, weil ohne diesen Bestandteil keine Entschlüsselung, bzw. Signatur
        /// mit dem von der OSTC zurückgelieferten Zertifikat durchgeführt werden kann.
        /// </remarks>
        public async Task<OstcApplicationResult> SendApplication([NotNull] OstcAntrag application)
        {
            return await SendApplicationAsync(application, null, null);
        }

        /// <summary>
        /// Antragstellung
        /// </summary>
        /// <param name="application">Antrag</param>
        /// <param name="certStore">Zertifikat-Speicher für die Ermittlung der Zertifikatskette des Absender-Zertifikats</param>
        /// <param name="pfx">Absender-Zertifikat für die Signierung des Antrags</param>
        /// <returns>Ergebnis der Antragstellung</returns>
        public async Task<OstcApplicationResult> SendApplicationAsync([NotNull] OstcAntrag application, [CanBeNull] IOstcCertificateStore certStore, [CanBeNull] Pkcs12Store pfx)
        {
            var now = DateTime.Now;

            application.Antragsteller.IK_BN = Sender.SenderId.ToString();

            RsaPrivateCrtKeyParameters rsaPrivateKey;
            AsymmetricCipherKeyPair rsaKeyPair;
            X509Certificate certificate;

            if (pfx != null)
            {
                var alias = pfx.Aliases.Cast<string>().First(pfx.IsKeyEntry);
                rsaPrivateKey = (RsaPrivateCrtKeyParameters)pfx.GetKey(alias).Key;
                var rsaPublicKey = new RsaKeyParameters(false, rsaPrivateKey.Modulus, rsaPrivateKey.PublicExponent);
                rsaKeyPair = new AsymmetricCipherKeyPair(rsaPublicKey, rsaPrivateKey);
                certificate = pfx.GetCertificate(alias).Certificate;
            }
            else
            {
                var keyPairGen = new RsaKeyPairGenerator();
                keyPairGen.Init(new KeyGenerationParameters(new SecureRandom(), 2048));
                rsaKeyPair = keyPairGen.GenerateKeyPair();
                rsaPrivateKey = (RsaPrivateCrtKeyParameters)rsaKeyPair.Private;
                certificate = null;
            }

            var requester = new Requester(application.Antragsteller.IK_BN, application.Antragsteller.Firma, application.Antragsteller.Nachname);
            var p10Creator = new Pkcs10Creator(requester, rsaKeyPair);
            var p10Data = p10Creator.CreateRequest();

            application.Antragsinfo.Requestschlüssel = p10Data.CertRequestDer;

            var applicationData = OstcUtils.Serialize(application, Iso88591);
            ValidateData(applicationData, OstcMessageType.ApplicationData);

            var receiver = Sender.SenderId.CommunicationServerReceiver;

            if (certificate != null)
            {
                Debug.Assert(certStore != null, "certStore != null");
                var certChain = certStore.GetChain(certificate).ToList();
                Debug.Assert(certChain[0].SubjectDN.Equivalent(certificate.SubjectDN));
                certChain.RemoveAt(0);
                applicationData = OstcUtils.SignData(applicationData, rsaPrivateKey, certificate, certChain);
                var receiverCert = certStore.GetCertificate(receiver);
                applicationData = OstcUtils.EncryptData(applicationData, receiverCert);
            }

            var fileName = $"{application.Antragsteller.IK_BN}_{now.Date:ddMMyyyy}.xml";
            var message = new TransportRequestType()
            {
                version = SupportedVersionsType.Item11,
                profile = ExtraProfileOstc,
                TransportHeader = CreateRequestHeader(now, OstcDataType.Application, ExtraScenario.RequestWithAcknowledgement),
                TransportBody = new TransportRequestBodyType
                {
                    Items = new object[]
                    {
                        new DataType 
                        {
                            Item = new Base64CharSequenceType()
                            {
                                Value = applicationData,
                            },
                        },
                    },
                },
            };

            if (certificate != null)
            {
                message.TransportPlugIns = new AnyPlugInContainerType
                {
                    Items = new object[]
                    { 
                        new DataTransformsType 
                        {
                            version = "1.1",
                            Encryption = new[]
                            {
                                new EncryptionType
                                {
                                    order = "1",
                                    Algorithm = new EncryptionAlgorithmType
                                    {
                                        id = ExtraEncryption.Pkcs7,
                                        Specification = new SpecificationType
                                        {
                                            url = "http://www.gkv-datenaustausch.de",
                                            name = "Security-Schnittstelle fuer den Datenaustausch im Gesundheitswesen",
                                        },
                                    },
                                },
                            },
                        },
                        new DataSourceType
                        {
                            version = "1.1",
                            DataContainer = new DataContainerType
                            {
                                type = ExtraContainerType.File,
                                name = fileName,
                                created = now,
                                createdSpecified = true,
                                encoding = ExtraContainerEncoding.Utf8,
                            }
                        },
                    },
                };
            }

            ValidateRequest(message, OstcMessageType.Application);

            var messageData = OstcExtraSerializer.Iso88591.Serialize(message);
            var request = CreateRequest(Network.Requests.Application);
            using (var requestStream = await Task.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            {
                requestStream.Write(messageData, 0, messageData.Length);
            }

            using (var response = await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))
            {
                var serializer = new XmlSerializer(typeof(TransportResponseType));
                var responseData = (TransportResponseType)serializer.Deserialize(response.GetResponseStream());

                var flags = responseData.TransportHeader.GetFlags().ToList();
                if (flags.Any(x => x.weight == ExtraFlagWeight.Error))
                    throw new Ostc2Exception(flags);

                return new OstcApplicationResult
                {
                    OrderId = responseData.TransportHeader.ResponseDetails.ResponseID.Value,
                    Pkcs10 = p10Data.CertRequestDer,
                    RSA = rsaPrivateKey,
                    Hash = p10Data.PublicKeyHashRaw,
                };
            }
        }

        /// <summary>
        /// Bestätigung des Auftrags
        /// </summary>
        /// <param name="orderId">ID des Auftrags</param>
        /// <param name="hash">Hash des öffentlichen Schlüssels</param>
        /// <returns></returns>
        public async Task AcknowledgeOrderAsync(string orderId, byte[] hash)
        {
            var query = new OstcAuftrag
            {
                Auftragsnummer = orderId,
                hash = string.Join(string.Empty, hash.Select(x => x.ToString("X2"))),
                ItemElementName = (CompanyNumberType)Enum.Parse(typeof(CompanyNumberType), Sender.SenderId.Type.ToString()),
                Item = Sender.SenderId.Id,
            };
            var queryData = OstcUtils.Serialize(query, Iso88591);

            ValidateData(queryData, OstcMessageType.OrderData);

            var now = DateTime.Now;
            var message = new TransportRequestType()
            {
                version = SupportedVersionsType.Item11,
                profile = ExtraProfileOstc,
                TransportHeader = CreateRequestHeader(now, OstcDataType.Order, ExtraScenario.RequestWithAcknowledgement),
                TransportBody = new TransportRequestBodyType
                {
                    Items = new object[]
                    {
                        new DataType 
                        {
                            Item = new Base64CharSequenceType()
                            {
                                Value = queryData,
                            },
                        },
                    },
                },
            };

            ValidateRequest(message, OstcMessageType.Order);

            var messageData = OstcExtraSerializer.Iso88591.Serialize(message);
            var request = CreateRequest(Network.Requests.Order);
            using (var requestStream = await Task.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            {
                requestStream.Write(messageData, 0, messageData.Length);
            }

            using (var response = await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))
            {
                var serializer = new XmlSerializer(typeof(TransportResponseType));
                var responseData = (TransportResponseType)serializer.Deserialize(response.GetResponseStream());

                var flags = responseData.TransportHeader.GetFlags().ToList();
                if (flags.Any(x => x.weight == ExtraFlagWeight.Error))
                    throw new Ostc2Exception(flags);
            }
        }

        /// <summary>
        /// Herunterladen des Zertifikats
        /// </summary>
        /// <param name="orderId">ID des Auftrags</param>
        /// <returns>Zertifikat von der OSTC (private Schlüssel fehlt hier!)</returns>
        /// <remarks>Es wird eine Exception ausgelöst, wenn noch kein Schlüssel verfügbar ist.</remarks>
        public async Task<IReadOnlyList<X509Certificate>> DownloadCertificateAsync(string orderId)
        {
            var query = new OstcSchluessel
            {
                Auftragsnummer = orderId,
                ItemElementName = (OstcKeyType)Enum.Parse(typeof(OstcKeyType), Sender.SenderId.Type.ToString()),
                Item = Sender.SenderId.Id,
            };
            var queryData = OstcUtils.Serialize(query, Iso88591);

            ValidateData(queryData, OstcMessageType.KeyData);

            var now = DateTime.Now;
            var message = new TransportRequestType()
            {
                version = SupportedVersionsType.Item11,
                profile = ExtraProfileOstc,
                TransportHeader = CreateRequestHeader(now, OstcDataType.Key, ExtraScenario.RequestWithResponse),
                TransportBody = new TransportRequestBodyType
                {
                    Items = new object[]
                    {
                        new DataType 
                        {
                            Item = new Base64CharSequenceType()
                            {
                                Value = queryData,
                            },
                        },
                    },
                },
            };

            ValidateRequest(message, OstcMessageType.Key);

            var messageData = OstcExtraSerializer.Iso88591.Serialize(message);
            var request = CreateRequest(Network.Requests.KeyRequest);
            using (var requestStream = await Task.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            {
                requestStream.Write(messageData, 0, messageData.Length);
            }

            using (var response = await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))
            {
                var serializer = new XmlSerializer(typeof(TransportResponseType));
                var responseData = (TransportResponseType)serializer.Deserialize(response.GetResponseStream());

                var flags = responseData.TransportHeader.GetFlags().ToList();
                if (flags.Any(x => x.weight == ExtraFlagWeight.Error))
                    throw new Ostc2Exception(flags);

                var certData = ((Base64CharSequenceType)((DataType)responseData.TransportBody.Items[0]).Item).Value;

                var parser = new X509CertificateParser();
                var certs = parser.ReadCertificates(certData).Cast<X509Certificate>().ToList();

                return certs;
            }
        }

        /// <summary>
        /// Herunterladen der Zertifikat-Liste
        /// </summary>
        /// <param name="list">Zertifikat-Liste die zu Laden ist</param>
        /// <returns>Zertifikat-Liste</returns>
        public async Task<IReadOnlyList<X509Certificate>> DownloadCertificateListAsync(OstcListeListe list)
        {
            var query = new OstcListe
            {
                Liste = list
            };
            var queryData = OstcUtils.Serialize(query, Encoding.UTF8);
            
            ValidateData(queryData, OstcMessageType.ListData);

            var now = DateTime.Now;
            now = new DateTime(now.Year, now.Month, now.Day, now.Hour, now.Minute, now.Second, DateTimeKind.Local).ToUniversalTime();
            var message = new TransportRequestType()
            {
                version = SupportedVersionsType.Item11,
                profile = ExtraProfileOstc,
                TransportHeader = CreateRequestHeader(now, OstcDataType.ListRequest, ExtraScenario.RequestWithResponse),
                TransportBody = new TransportRequestBodyType
                {
                    Items = new object[]
                    {
                        new DataType 
                        {
                            Item = new Base64CharSequenceType()
                            {
                                Value = queryData,
                            },
                        },
                    },
                },
            };

            ValidateRequest(message, OstcMessageType.List);

            var messageData = OstcExtraSerializer.Utf8.Serialize(message);
            var request = CreateRequest(Network.Requests.ListRequest);
            using (var requestStream = await Task.Factory.FromAsync(request.BeginGetRequestStream, request.EndGetRequestStream, null))
            {
                requestStream.Write(messageData, 0, messageData.Length);
            }

            using (var response = await Task.Factory.FromAsync(request.BeginGetResponse, request.EndGetResponse, null))
            {
                var serializer = new XmlSerializer(typeof(TransportResponseType));
                var responseData = (TransportResponseType)serializer.Deserialize(response.GetResponseStream());

                var flags = responseData.TransportHeader.GetFlags().ToList();
                if (flags.Any(x => x.weight == ExtraFlagWeight.Error))
                    throw new Ostc2Exception(flags);

                var data = ((Base64CharSequenceType)((DataType)responseData.TransportBody.Items[0]).Item).Value;
                var result = OstcUtils.ReadCertificates(new MemoryStream(data));
                return result;
            }
        }
    }
}
