using RAMS.Helpers;
using RAMS.Models;
using RAMS.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace RAMS.Web.Controllers.WebAPI
{
    /// <summary>
    /// CategoryController is an api controller that allows to access context resources by sending http requests and responces
    /// </summary>
    public class CategoryController : ApiController
    {
        private readonly ICategoryService CategoryService;

        /// <summary>
        /// Controller that sets category service in order to access context resources
        /// </summary>
        /// <param name="categoryService">Parameter for setting category service</param>
        public CategoryController(ICategoryService categoryService)
        {
            this.CategoryService = categoryService;
        }

        /// <summary>
        /// Get the list of all categories
        /// </summary>
        /// <returns>The list of all categories</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<Category>))]
        public IHttpActionResult GetAllCategorys()
        {
            var categories = this.CategoryService.GetAllCategories();

            if (categories.Count() > 0)
            {
                return Ok(categories);
            }

            return NotFound();
        }

        /// <summary>
        /// Get a category by id
        /// </summary>
        /// <param name="id">Id of a category to be fetched</param>
        /// <returns>A category with matching id</returns>
        [HttpGet]
        [ResponseType(typeof(Category))]
        public IHttpActionResult GetCategory(int id)
        {
            if (id > 0)
            {
                var category = this.CategoryService.GetOneCategoryById(id);

                if (category != null)
                {
                    return Ok(category);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="category">A category to be created</param>
        /// <returns>The Uri of newly created category</returns>
        [HttpPost]
        [ResponseType(typeof(Category))]
        public IHttpActionResult PostCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                this.CategoryService.CreateCategory(category);

                try
                {
                    this.CategoryService.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }

                return CreatedAtRoute("DefaultApi", new { id = category.CategoryId }, category);

            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Update existing category
        /// </summary>
        /// <param name="category">Category to be updated</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpPut]
        [ResponseType(typeof(Category))]
        public IHttpActionResult PutCategory(Category category)
        {
            if (ModelState.IsValid)
            {
                this.CategoryService.UpdateCategory(category);

                try
                {
                    this.CategoryService.SaveChanges();

                    return Ok(category);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    return Conflict();
                }
                catch (DbUpdateException ex)
                {
                    // Log exception
                    ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                    if (!this.CategoryExists(category.CategoryId))
                    {
                        return NotFound();
                    }

                    return Conflict();
                }
            }

            return BadRequest(ModelState);
        }

        /// <summary>
        /// Delete existing category
        /// </summary>
        /// <param name="id">Id of the category to be deleted</param>
        /// <returns>HttpResponseMessage with status code dependning on the outcome of this method</returns>
        [HttpDelete]
        [ResponseType(typeof(Category))]
        public IHttpActionResult DeleteCategory(int id)
        {
            if (id > 0)
            {
                var category = this.CategoryService.GetOneCategoryById(id);

                if (category != null)
                {
                    this.CategoryService.DeleteCategory(category);

                    try
                    {
                        this.CategoryService.SaveChanges();
                    }
                    catch (DbUpdateException ex)
                    {
                        // Log exception
                        ErrorHandlingUtilities.LogException(ErrorHandlingUtilities.GetExceptionDetails(ex));

                        if (!this.CategoryExists(category.CategoryId))
                        {
                            return NotFound();
                        }

                        return Conflict();
                    }

                    return Ok(category);
                }
            }

            return NotFound();
        }

        /// <summary>
        /// CategoryExists is used to check whether the category is present in data context
        /// </summary>
        /// <param name="id">Id of the category to check against</param>
        /// <returns>True if category is present in data context, false otherwise</returns>
        private bool CategoryExists(int id)
        {
            return this.CategoryService.GetAllCategories().Count(c => c.CategoryId == id) > 0;
        }
    }
}
