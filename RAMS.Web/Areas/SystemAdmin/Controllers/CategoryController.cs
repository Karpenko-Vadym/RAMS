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
    /// CategoryController controller implements CRUD operations for categories
    /// </summary>
    public class CategoryController : BaseController
    {
        /// <summary>
        /// Index action method will be called as soon as user navigates (Or gets redirected) to /RAMS/Category
        /// This method displays the main view where all category related CRUD operations take place 
        /// User will be redirected to appropriate location depending on his/her UserType if user does not belong to this area
        /// </summary>
        /// <returns>Main view where all category related CRUD operations take place</returns>
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
        /// CategoryList method gets the list of all categories and passess it to _CategoryList partial view
        /// </summary>
        /// <returns>_CategoryList partial view with a list of categories as it's model</returns>
        [HttpGet]
        public async Task<PartialViewResult> CategoryList()
        {
            var categoryList = new List<CategoryListViewModel>();

            var response = await this.GetHttpClient().GetAsync("Category");

            if (response.IsSuccessStatusCode)
            {
                categoryList.AddRange(Mapper.Map<List<Category>, List<CategoryListViewModel>>(await response.Content.ReadAsAsync<List<Category>>()));
            }

            return PartialView("_CategoryList", categoryList);
        }


        
        /// <summary>
        /// NewCategory method returns _NewCategory partial view
        /// </summary>
        /// <returns>_NewCategory partial view</returns>
        [HttpGet]
        public PartialViewResult NewCategory()
        {
            var categoryAddViewModel = new CategoryAddViewModel();

            return PartialView("_NewCategory", categoryAddViewModel);
        }

        /// <summary>
        /// NewCategory method validates the model and attempts to persist it database if model state is valid
        /// </summary>
        /// <param name="model">Model containing properties required for creating new category</param>
        /// <returns>If model state is not valid, re-displays _NewCategory partial view with an input form and an error message. Otherwise returns success message or error message depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> NewCategory(CategoryAddViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    // If model state is valid, attempt to persist new category
                    var category = Mapper.Map<CategoryAddViewModel, Category>(model);

                    var response = await this.GetHttpClient().PostAsJsonAsync("Category", category);

                    if (response.IsSuccessStatusCode)
                    {
                        category = await response.Content.ReadAsAsync<Category>();

                        if (category != null)
                        {
                            var categoryAddEditConfirmationViewModel = Mapper.Map<Category, CategoryAddEditConfirmationViewModel>(category);

                            return PartialView("_NewCategoryConfirmation", categoryAddEditConfirmationViewModel);
                        }
                        else
                        {
                            throw new CategoryAddException("Null is returned after creating new category. Status Code: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        // If category could not be created, throw CategoryAddException exception
                        throw new CategoryAddException("Category " + category.Name + " could not be created. Response: " + response.StatusCode);
                    }
                }
                catch (CategoryAddException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Category could NOT be created.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update an existing category. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_NewCategory", model);
        }

        /// <summary>
        /// EditCategory method retrieves one category by it's id and displays it's information in _EditCategory partial view
        /// </summary>
        /// <param name="id">Id of the category to be retrieved</param>
        /// <returns>_EditCategory partial view with details of retrieved category if category information could be retrieved, otherwise an error message</returns>
        [HttpGet]
        public async Task<PartialViewResult> EditCategory(int id)
        {
            var response = await this.GetHttpClient().GetAsync(String.Format("category?id={0}", id));

            if (response.IsSuccessStatusCode)
            {
                var category = await response.Content.ReadAsAsync<Category>();

                var categoryEditViewModel = Mapper.Map<Category, CategoryEditViewModel>(category);

                return PartialView("_EditCategory", categoryEditViewModel);
            }

            var stringBuilder = new StringBuilder();

            stringBuilder.Append("<div class='text-center'><h4><strong>Category information is NOT available at this time.</strong></h4></div>");

            stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>Category could have been deleted from the system.</div>");

            stringBuilder.Append("<div class='col-md-offset-1 col-md-11'>Please refresh the list and try again in a moment.</div></div>");

            var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

            return PartialView("_Error", confirmationViewModel);
        }

        /// <summary>
        /// EditCategory method validates the model and attempts to persist it database if model state is valid
        /// </summary>
        /// <param name="model">Model containing properties required for editing existing category</param>
        /// <returns>If model state is not valid, re-displays _EditCategory partial view with an input form and an error message. Otherwise returns success message or error message depending on the outcome of this method</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> EditCategory(CategoryEditViewModel model)
        {
            var response = new HttpResponseMessage();

            if (ModelState.IsValid)
            {
                try
                {
                    // Attempt to persist updated category
                    var category = Mapper.Map<CategoryEditViewModel, Category>(model);

                    response = await this.GetHttpClient().PutAsJsonAsync("Category", category);

                    if (response.IsSuccessStatusCode)
                    {
                        category = await response.Content.ReadAsAsync<Category>();

                        if (category != null)
                        {
                            var categoryAddEditConfirmationViewModel = Mapper.Map<Category, CategoryAddEditConfirmationViewModel>(category);

                            return PartialView("_EditCategoryConfirmation", categoryAddEditConfirmationViewModel);
                        }
                        else
                        {
                            throw new CategoryUpdateException("Null is returned after updating category. Status Code: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        throw new CategoryUpdateException("Category could not be updated. Status Code: " + response.StatusCode);
                    }

                }
                catch (CategoryUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    var stringBuilder = new StringBuilder();

                    stringBuilder.Append("<div class='text-center'><h4><strong>Category could NOT be updated.</strong></h4></div>");

                    stringBuilder.Append("<div class='row'><div class='col-md-12'><p></p></div><div class='col-md-offset-1 col-md-11'>An exception has been caught while attempting to update category details. Please review an exception log for more details about the exception.</div></div>");

                    var confirmationViewModel = new ConfirmationViewModel(stringBuilder.ToString());

                    return PartialView("_Error", confirmationViewModel);
                }
            }

            return PartialView("_EditCategory", model);
        }
    }
}