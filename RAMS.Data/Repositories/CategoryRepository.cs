using RAMS.Data.Infrastructure;
using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Repositories
{
    /// <summary>
    /// Category repository implements Category specific repository operations, inherits basic repository operations from RepositoryBase class
    /// </summary>
    public class CategoryRepository : RepositoryBase<Category>, ICategoryRepository
    {
        public CategoryRepository(IDataFactory dataFactory) : base(dataFactory) { }

        #region Getters
        /// <summary>
        /// Get first category with matching category id
        /// </summary>
        /// <param name="id">Id to match with categoryies' data</param>
        /// <returns>First category with matching id</returns>
        public Category GetOneByCategoryId(int id)
        {
            return this.GetContext.Categories.Include("Positions").FirstOrDefault(c => c.CategoryId == id);
        }

        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>All categories</returns>
        public IEnumerable<Category> GetAllCategories()
        {
            return this.GetContext.Categories.Include("Positions");
        }

        /// <summary>
        /// Get first category with matching name
        /// </summary>
        /// <param name="name">Name for comparing with Categories' name</param>
        /// <returns>First category with matching name</returns>
        public Category GetOneByName(string name)
        {
            return this.GetContext.Categories.Include("Positions").FirstOrDefault(c => c.Name == name);
        }

        /// <summary>
        /// Get Category that matches Position's category
        /// </summary>
        /// <param name="position">Position for comparing it's Category to context Categories</param>
        /// <returns>Category that matches Position's category</returns>
        public Category GetOneByPosition(Position position)
        {
            return this.GetContext.Categories.Include("Positions").FirstOrDefault(c => c.CategoryId == position.CategoryId);
        }

        /// <summary>
        /// Get Category that matches Position's category
        /// </summary>
        /// <param name="id">Id of the Position</param>
        /// <returns>Category that matches Position's category</returns>
        public Category GetOneByPositionId(int id)
        {
            var position = this.GetContext.Positions.FirstOrDefault(p => p.PositionId == id) ?? new Position();

            return this.GetContext.Categories.Include("Positions").FirstOrDefault(c => c.CategoryId == position.CategoryId);
        }
        #endregion
    }
}
