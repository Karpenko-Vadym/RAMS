using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using RAMS.ViewModels;
using RAMS.Web.Identity;
using System.Net.Http;
using AutoMapper;
using RAMS.Models;

namespace RAMS.Web.Controllers
{
    // TODO - Uncomment [Authorize]
    // [Authorize] 
    public class AccountController : BaseController
    {
        private ApplicationSignInManager _signInManager;

        private ApplicationUserManager _userManager;

        public AccountController() { }

        public AccountController(ApplicationUserManager userManager, ApplicationSignInManager signInManager )
        {
            UserManager = userManager;

            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get { return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>(); }

            private set { _signInManager = value; }
        }

        public ApplicationUserManager UserManager
        {
            get { return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>(); }

            private set { _userManager = value; }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // This doesn't count login failures towards account lockout
            // To enable password failures to trigger account lockout, change to shouldLockout: true
            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, shouldLockout: false);

            switch (result)
            {
                case SignInStatus.Success:

                    return RedirectToLocal(returnUrl);

                case SignInStatus.LockedOut:

                    return View("Lockout");

                case SignInStatus.RequiresVerification:

                    return RedirectToAction("SendCode", new { ReturnUrl = returnUrl, RememberMe = model.RememberMe });

                case SignInStatus.Failure:

                default:

                    ModelState.AddModelError("", "Invalid login attempt.");

                    return View(model);
            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }

        public async Task<PartialViewResult> EditUserProfile()
        {
            var identity = User.Identity as ClaimsIdentity;

            var response = new HttpResponseMessage();

            var editUserProfileViewModel = new EditUserProfileViewModel();

            if (identity.HasClaim("UserType", "Agent"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Agent?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    editUserProfileViewModel = Mapper.Map<Agent, EditUserProfileViewModel>(await response.Content.ReadAsAsync<Agent>());


                    if (editUserProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    editUserProfileViewModel = Mapper.Map<Client, EditUserProfileViewModel>(await response.Content.ReadAsAsync<Client>());


                    if (editUserProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                response = await this.GetHttpClient().GetAsync(String.Format("Admin?userName={0}", this.User.Identity.Name));

                if (response.IsSuccessStatusCode)
                {
                    editUserProfileViewModel = Mapper.Map<Admin, EditUserProfileViewModel>(await response.Content.ReadAsAsync<Admin>());


                    if (editUserProfileViewModel == null)
                    {
                        return PartialView("_EditUserProfile");
                    }

                }
            }

            return PartialView("_EditUserProfile", editUserProfileViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditUserProfile(EditUserProfileViewModel model)
        {

            return null;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_userManager != null)
                {
                    _userManager.Dispose();

                    _userManager = null;
                }

                if (_signInManager != null)
                {
                    _signInManager.Dispose();

                    _signInManager = null;
                }
            }

            base.Dispose(disposing);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null) {}

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;

                RedirectUri = redirectUri;

                UserId = userId;
            }

            public string LoginProvider { get; set; }

            public string RedirectUri { get; set; }

            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}