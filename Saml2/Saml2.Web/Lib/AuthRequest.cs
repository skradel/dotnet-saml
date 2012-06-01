using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;

namespace Saml2.Web.Lib
{
    public class AuthRequest
    {
        public string id;
        private string issue_instant;
        private AppSettings appSettings;
        private AccountSettings accountSettings;

        public enum AuthRequestFormat
        {
            Base64 = 1
        }

        public AuthRequest(AppSettings appSettings, AccountSettings accountSettings)
        {
            this.appSettings = appSettings;
            this.accountSettings = accountSettings;

            id = "_" + System.Guid.NewGuid().ToString();
            issue_instant = DateTime.UtcNow.ToString("yyyy-mm-ddTH:mm:ssZ");
        }

        public string GetRequest(AuthRequestFormat format)
        {
            using (StringWriter sw = new StringWriter())
            {
                using (XmlWriter xw = XmlWriter.Create(sw, new XmlWriterSettings()
                {
                    OmitXmlDeclaration = true
                }))
                {
                    xw.WriteStartElement("samlp", "AuthnRequest", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("ID", id);
                    xw.WriteAttributeString("Version", "2.0");
                    xw.WriteAttributeString("IssueInstant", issue_instant);
                    xw.WriteAttributeString("ProtocolBinding", "urn:oasis:names:tc:SAML:2.0:bindings:HTTP-POST");
                    xw.WriteAttributeString("AssertionConsumerServiceURL", appSettings.assertionConsumerServiceUrl);

                    xw.WriteStartElement("saml", "Issuer", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString(appSettings.issuer);
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "NameIDPolicy", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Format", "urn:oasis:names:tc:SAML:2.0:nameid-format:unspecified");
                    xw.WriteAttributeString("AllowCreate", "true");
                    xw.WriteEndElement();

                    xw.WriteStartElement("samlp", "RequestedAuthnContext", "urn:oasis:names:tc:SAML:2.0:protocol");
                    xw.WriteAttributeString("Comparison", "exact");
                    xw.WriteEndElement();

                    xw.WriteStartElement("saml", "AuthnContextClassRef", "urn:oasis:names:tc:SAML:2.0:assertion");
                    xw.WriteString("urn:oasis:names:tc:SAML:2.0:ac:classes:PasswordProtectedTransport");
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                }

                switch (format)
                {
                    case AuthRequestFormat.Base64:
                        return Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(sw.ToString()));

                    default:
                        throw new ApplicationException();
                }
            }
        }
    }
}