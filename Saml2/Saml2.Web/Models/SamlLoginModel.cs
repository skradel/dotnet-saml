using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web.Security;

namespace Saml2.Web.Models
{
    public class SamlLoginModel
    {
        [Required]
        [Display(Name = "SAMLResponse")]
        public string SAMLResponse { get; set; }
    }

}
