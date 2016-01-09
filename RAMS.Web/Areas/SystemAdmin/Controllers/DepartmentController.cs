using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AutoMapper;
using System.Net.Http;
using System.Collections.Generic;
using System.Text;
using RAMS.ViewModels;
using RAMS.Models;
using RAMS.Web.Controllers;
using RAMS.Helpers;

namespace RAMS.Web.Areas.SystemAdmin.Controllers
{
    public class DepartmentController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<PartialViewResult> DepartmentList()
        {
            var departmentList = new List<DepartmentListViewModel>();

            var response = await this.GetHttpClient().GetAsync("Department");
            
            if (response.IsSuccessStatusCode)
            {
                departmentList.AddRange(Mapper.Map<List<Department>, List<DepartmentListViewModel>>(await response.Content.ReadAsAsync<List<Department>>()));
            }

            return PartialView("_DepartmentList" , departmentList);
        }

        [HttpGet]
        public PartialViewResult NewDepartment()
        {
            var departmentAddViewModel = new DepartmentAddViewModel();

            return PartialView("_NewDepartment", departmentAddViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> NewDepartment(DepartmentAddViewModel model)
        {        
            if (ModelState.IsValid)
            {
                try
                {
                    var department = Mapper.Map<DepartmentAddViewModel, Department>(model);

                    var response = await this.GetHttpClient().PostAsJsonAsync("Department", department);

                    if (response.IsSuccessStatusCode)
                    {
                        department = await response.Content.ReadAsAsync<Department>();

                        var stringBuilder = new StringBuilder();

                        stringBuilder.AppendFormat("<div class='text-center'><h4><strong>Department {0} has been successfully created!</strong></h4></div>", department.Name);

                        stringBuilder.Append("<div class='row'><div class='col-md-offset-1'>Employees can now be assigned to this department.</div>");

                        stringBuilder.AppendFormat("<div class='col-md-12'><p></p></div><div class='col-md-offset-2 col-md-3'>Department Name: </div><div class='col-md-7'><strong>{0}</strong></div>", department.Name);

                        stringBuilder.Append("<div class='col-md-12'><p></p></div><div class='col-md-offset-1'><strong>NOTE:</strong> Employee can be assigned to this department from Edit User or New User interfaces.</div></div>");

                        var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString(), false, true);

                        return PartialView("_Success", confirmationViewModel);
                    }
                    else
                    {
                        // If department could not be created, throw DepartmentAddException exception
                        throw new DepartmentAddException("Department " + department.Name + " could not be created. Response: " + response.StatusCode);
                    }
                }
                catch(DepartmentAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Department could NOT be created.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update an employee profile. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_NewDepartment", model);
        }

        [HttpGet]
        public async Task<PartialViewResult> EditDepartment(int id)
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("Department?id={0}", id));

            if (response.IsSuccessStatusCode)
            {
                var department = await response.Content.ReadAsAsync<Department>();

                var departmentEditViewModel = Mapper.Map<Department, DepartmentEditViewModel>(department);

                return PartialView("_EditDepartment", departmentEditViewModel);
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='text-center'><h4><strong>Department information is NOT available at this time.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>User could have been deleted from the system.</div>");

            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'>Please refresh the list and try again in a moment.</div></div>");

            var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

            return PartialView("_Error", confirmationViewModel);
        }
    }
}