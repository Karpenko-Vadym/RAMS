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
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using RAMS.Web.Identity;
using RAMS.Enums;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;

namespace RAMS.Web.Areas.Customer.Controllers
{
    public class ProfileController : BaseController
    {
        [HttpGet]
        public ActionResult Index(string message = "")
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity.HasClaim("UserType", "Agent"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Agency" });
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                return View("Index", null, message);
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return RedirectToAction("Index", "User", new { Area = "SystemAdmin" });
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        #region Profile Details

        [HttpGet]
        public async Task<PartialViewResult> ProfileDetails()
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("Client?userName={0}", User.Identity.Name));

            if (response.IsSuccessStatusCode)
            {
                var clientProfileDetailsViewModel = Mapper.Map<Client, ClientProfileDetailsViewModel>(await response.Content.ReadAsAsync<Client>());

                return PartialView("_ProfileDetails", clientProfileDetailsViewModel);
            }

            return PartialView("_ProfileDetails");
        }

        [HttpGet]
        public PartialViewResult UploadProfilePicture()
        {
            var clientProfilePictureViewModel = new ClientProfilePictureViewModel(User.Identity.Name);

            return PartialView("_UploadProfilePicture", clientProfilePictureViewModel);
        }

        [HttpPost]
        public ActionResult UploadProfilePicture(ClientProfilePictureViewModel model)
        {
            if(ModelState.IsValid)
            {
                try
                {
                    if (model.ProfilePicture.ContentType == "image/jpeg")
                    {
                        model.ProfilePicture.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg"));

                        if(System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png"));
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif"));
                        }

                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully updated." });
                    }
                    else if (model.ProfilePicture.ContentType == "image/gif")
                    {
                        model.ProfilePicture.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif"));

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg"));
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png"));
                        }

                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully updated." });
                    }
                    else if (model.ProfilePicture.ContentType == "image/png")
                    {
                        model.ProfilePicture.SaveAs(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".png"));

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".jpg"));
                        }

                        if (System.IO.File.Exists(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif")))
                        {
                            System.IO.File.Delete(System.IO.Path.Combine(Server.MapPath("~/Content/ProfilePictures/"), model.UserName + ".gif"));
                        }

                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Profile picture has been successfully updated." });
                    }
                    else
                    {
                        return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "Unsupported image type. Please try again using supported image type." });
                    }
                }
                catch(UnauthorizedAccessException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "File could NOT be saved due to lack of permissions. Please review exception log for more details." });
                }
                catch(System.IO.IOException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "An exception has occured. Please review exception log for more details." });
                }

            }

            return RedirectToAction("Index", "Profile", new { Area = "Customer", message = "An error occured while attmpting to upload a profile picture. Profile picture has NOT been updated." });
        }


        #endregion
    }
}