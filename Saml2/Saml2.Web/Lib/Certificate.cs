using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography.X509Certificates;

namespace Saml2.Web.Lib
{
    public class Certificate
    {
        public X509Certificate2 cert;

        private static readonly System.Text.Encoding s_enc = System.Text.ASCIIEncoding.ASCII;

        public void LoadCertificate(string certificate)
        {
            cert = new X509Certificate2();

            cert.Import(s_enc.GetBytes(certificate));
        }
    }
}