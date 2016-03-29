@echo off
md ..\..\Itsg.Ostc2\ApplicationForm\Request
md ..\..\Itsg.Ostc2\ApplicationForm\Response
md ..\..\Itsg.Ostc2\Order\Request
md ..\..\Itsg.Ostc2\Order\Response
md ..\..\Itsg.Ostc2\Key\Request
md ..\..\Itsg.Ostc2\Key\Response
md ..\..\Itsg.Ostc2\List\Request
md ..\..\Itsg.Ostc2\List\Response
xsd /c /edb /n:Itsg.Ostc2.ApplicationForm.Request /o:..\..\Itsg.Ostc2\ApplicationForm\Request eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-1a-components-1.xsd .\OSTC-1a-request-Antrag-1.xsd
xsd /c /edb /n:Itsg.Ostc2.ApplicationForm.Response /o:..\..\Itsg.Ostc2\ApplicationForm\Response eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-1a-components-1.xsd .\OSTC-1b-response-Antrag-1.xsd
xsd /c /edb /n:Itsg.Ostc2.Order.Request /o:..\..\Itsg.Ostc2\Order\Request eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-2a-components-1.xsd .\OSTC-2a-request-Auftrag-1.xsd
xsd /c /edb /n:Itsg.Ostc2.Order.Response /o:..\..\Itsg.Ostc2\Order\Response eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-2a-components-1.xsd .\OSTC-2b-response-Auftrag-1.xsd
xsd /c /edb /n:Itsg.Ostc2.Key.Request /o:..\..\Itsg.Ostc2\Key\Request eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-3a-components-1.xsd .\OSTC-3a-request-SchluesselAnfragen-1.xsd
xsd /c /edb /n:Itsg.Ostc2.Key.Response /o:..\..\Itsg.Ostc2\Key\Response eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-3a-components-1.xsd .\OSTC-3b-response-SchluesselAnfragen-1.xsd
xsd /c /edb /n:Itsg.Ostc2.List.Request /o:..\..\Itsg.Ostc2\List\Request eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-4a-components-1.xsd .\OSTC-4a-request-ListeAnfragen-1.xsd
xsd /c /edb /n:Itsg.Ostc2.List.Response /o:..\..\Itsg.Ostc2\List\Response eXTra-codelists-1.xsd xenc-schema.xsd xmldsig-core-schema.xsd OSTC-1a-plugins-1.xsd OSTC-4a-components-1.xsd .\OSTC-4b-response-ListeAnfragen-1.xsd
