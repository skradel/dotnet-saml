using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Saml2.Web.Models;
using Saml2.Web.Lib;

namespace Saml2.Web.Controllers
{

    [Authorize]
    public class AccountController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            AccountSettings accountSettings = new AccountSettings();

            AuthRequest req = new AuthRequest(new AppSettings(), accountSettings);

            return this.Redirect(accountSettings.idp_sso_target_url + "?SAMLRequest=" + Server.UrlEncode(req.GetRequest(AuthRequest.AuthRequestFormat.Base64)));
        }


        //
        // POST: /Account/Login
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(SamlLoginModel model)
        {
            if (ModelState.IsValid)
            {
                AccountSettings accountSettings = new AccountSettings();

                var samlResponse = new Response(accountSettings);
                samlResponse.LoadXmlFromBase64(model.SAMLResponse);

                if (samlResponse.IsValid())
                {
                    FormsAuthentication.SetAuthCookie(samlResponse.GetNameID(), true);

                    // Redirect to targetURL, or to /Home by default

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }
            else
            {
                ModelState.AddModelError("", "The user name or password provided is incorrect.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff

        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        private IEnumerable<string> GetErrorsFromModelState()
        {
            return ModelState.SelectMany(x => x.Value.Errors.Select(error => error.ErrorMessage));
        }
    }
}
