using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace RAMS.ViewModels
{
    public class ClientProfileDetailsViewModel
    {
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Job Title")]
        public string JobTitle { get; set; }

        [Display(Name = "Company Name")]
        public string Company { get; set; }

        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
    }

    public class ClientProfileImageViewModel
    {
        public HttpPostedFileBase Image { get; set; }

        public List<string> ImageContentTypes { get; set; }

        public ClientProfileImageViewModel()
        {
            this.ImageContentTypes = new List<string> { "image/jpeg", "image/gif", "image/png" };
        }
    }
}
