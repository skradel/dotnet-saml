using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

/// <summary>
/// Summary description for AppSettings
/// </summary>
namespace Saml2.Web.Lib
{
    public class AppSettings
    {
        public string assertionConsumerServiceUrl = "http://localhost:49573/Account/Login";
        public string issuer = "test-app";
    }
}