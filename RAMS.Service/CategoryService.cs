using RAMS.Data.Infrastructure;
using RAMS.Data.Repositories;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// CategoryService implements Category specific services
    /// </summary>
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository CategoryRepository;

        private readonly IPositionRepository PositionRepository;

        private readonly IUnitOfWork UnitOfWork;

        /// <summary>
        /// Constructor that sets required repositories and unit of work for this service
        /// </summary>
        /// <param name="categoryRepository">Parameter for setting CategoryRepository</param>
        /// <param name="positionRepository">Parameter for setting PositionRepository</param>
        /// <param name="unitOfWork">Parameter for setting UnitOfWork</param>
        public CategoryService(ICategoryRepository categoryRepository, IPositionRepository positionRepository, IUnitOfWork unitOfWork) 
        {
            this.CategoryRepository = categoryRepository;

            this.PositionRepository = positionRepository;

            this.UnitOfWork = unitOfWork;
        }

        #region Getters
        /// <summary>
        /// Get all the categories
        /// </summary>
        /// <returns>All the categories</returns>
        public IEnumerable<Category> GetAllCategories()
        {
            return this.CategoryRepository.GetAll();
        }

        /// <summary>
        /// Get a category with matching id
        /// </summary>
        /// <param name="id">Id of the category to be compared with the context categories' data</param>
        /// <returns>A category with matching id</returns>
        public Category GetOneCategoryById(int id)
        {
            return this.CategoryRepository.GetById(id);
        }

        /// <summary>
        /// Get a category for specific position
        /// </summary>
        /// <param name="id">Id of the position for which category is being retrieved</param>
        /// <returns>A category for specific position</returns>
        public Category GetOneCategoryByPositionId(int id)
        {
            var position = this.PositionRepository.GetById(id);

            return position.Category;
        }
        #endregion

        /// <summary>
        /// Create new category
        /// </summary>
        /// <param name="category">Category to be created</param>
        public void CreateCategory(Category category)
        {
            this.CategoryRepository.Add(category);
        }

        /// <summary>
        /// Update existing category
        /// </summary>
        /// <param name="category">Category to be updated</param>
        public void UpdateCategory(Category category)
        {
            this.CategoryRepository.Update(category);
        }

        /// <summary>
        /// Delete existing category
        /// </summary>
        /// <param name="category">Category to be deleted</param>
        public void DeleteCategory(Category category)
        {
            this.CategoryRepository.Delete(category);
        }

        /// <summary>
        /// Save changes by using UnitOfWork's Commit method
        /// </summary>
        public void SaveChanges()
        {
            try
            {
                this.UnitOfWork.Commit();
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateConcurrencyException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateException ex)
            #pragma warning restore 0168
            {
                throw;
            }
        }
    }
}
