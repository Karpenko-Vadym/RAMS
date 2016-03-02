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
    /// <summary>
    /// DepartmentController controller implements CRUD operations for departments
    /// </summary>
    public class DepartmentController : BaseController
    {
        /// <summary>
        /// Index action method will be called as soon as user navigates (Or gets redirected) to /RAMS/Department
        /// This method displays the main view where all department related CRUD operations take place 
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view where all department related CRUD operations take place</returns>
        [HttpGet]
        public ActionResult Index()
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity.HasClaim("UserType", "Agent"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Agency" });
            }
            else if (identity.HasClaim("UserType", "Client"))
            {
                return RedirectToAction("Index", "Home", new { Area = "Customer" });
            }
            else if (identity.HasClaim("UserType", "Admin"))
            {
                return View();
            }

            return RedirectToAction("Index", "Home", new { Area = "" });
        }

        /// <summary>
        /// DepartmentList method gets the list of all departments and passess it to _DepartmentList partial view
        /// </summary>
        /// <returns>_DepartmentList partial view with a list of departments as it's model</returns>
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

        /// <summary>
        /// NewDepartment method returns _NewDepartment partial view
        /// </summary>
        /// <returns>_NewDepartment partial view</returns>
        [HttpGet]
        public PartialViewResult NewDepartment()
        {
            var departmentAddViewModel = new DepartmentAddViewModel();

            return PartialView("_NewDepartment", departmentAddViewModel);
        }

        /// <summary>
        /// NewDepartment method validates the model and attempts to persist it database if model state is valid
        /// </summary>
        /// <param name="model">Model containing properties required for creating new department</param>
        /// <returns>If model state is not valid, re-displays _NewDepartment partial view with an input form and an error message. Otherwise returns success message or error message depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> NewDepartment(DepartmentAddViewModel model)
        {        
            if (ModelState.IsValid)
            {
                try
                {
                    // If model state is valid, attempt to persist new department
                    var department = Mapper.Map<DepartmentAddViewModel, Department>(model);

                    var response = await this.GetHttpClient().PostAsJsonAsync("Department", department);

                    if (response.IsSuccessStatusCode)
                    {
                        department = await response.Content.ReadAsAsync<Department>();

                        if (department != null)
                        {
                            var departmentAddEditConfirmationViewModel = Mapper.Map<Department, DepartmentAddEditConfirmationViewModel>(department);

                            return PartialView("_NewDepartmentConfirmation", departmentAddEditConfirmationViewModel);
                        }
                        else
                        {
                            throw new DepartmentAddException("Null is returned after creating new department. Status Code: " + response.StatusCode);
                        }
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

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update an existing department. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_NewDepartment", model);
        }

        /// <summary>
        /// EditDepartment method retrieves one department by it's id and displays it's information in _EditDepartment partial view
        /// </summary>
        /// <param name="id">Id of the department to be retrieved</param>
        /// <returns>_EditDepartment partial view with details of retrieved department if department information could be retrieved, otherwise an error message</returns>
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

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Department could have been deleted from the system.</div>");

            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'>Please refresh the list and try again in a moment.</div></div>");

            var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

            return PartialView("_Error", confirmationViewModel);
        }

        /// <summary>
        /// EditDepartment method validates the model and attempts to persist it database if model state is valid
        /// </summary>
        /// <param name="model">Model containing properties required for editing existing department</param>
        /// <returns>If model state is not valid, re-displays _EditDepartment partial view with an input form and an error message. Otherwise returns success message or error message depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditDepartment(DepartmentEditViewModel model)
        {
            var response = new HttpResponseMessage();
            
            if (ModelState.IsValid)
            {
                try
                {
                    // Attempt to persist updated department
                    var department = Mapper.Map<DepartmentEditViewModel, Department>(model);

                    response = await this.GetHttpClient().PutAsJsonAsync("Department", department);

                    if (response.IsSuccessStatusCode)
                    {
                        department = await response.Content.ReadAsAsync<Department>();

                        if (department != null)
                        {
                            var departmentAddEditConfirmationViewModel = Mapper.Map<Department, DepartmentAddEditConfirmationViewModel>(department);

                            return PartialView("_EditDepartmentConfirmation", departmentAddEditConfirmationViewModel);
                        }
                        else
                        {
                            throw new DepartmentUpdateException("Null is returned after updating department. Status Code: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        throw new DepartmentUpdateException("Department could not be updated. Status Code: " + response.StatusCode);
                    }
                    
                }
                catch (DepartmentUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Department could NOT be updated.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update depatment details. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_EditDepartment", model);
        }
    }
}