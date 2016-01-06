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
        /// Get first Category with matching name
        /// </summary>
        /// <param name="name">Name for comparing with Categories' name</param>
        /// <returns>First Category with matching name</returns>
        public Category GetOneByName(string name)
        {
            return this.GetContext.Categories.Where(c => c.Name == name).FirstOrDefault();
        }

        /// <summary>
        /// Get Category that matches Position's category
        /// </summary>
        /// <param name="position">Position for comparing it's Category to context Categories</param>
        /// <returns>Category that matches Position's category</returns>
        public Category GetOneByPosition(Position position)
        {
            return this.GetContext.Categories.Where(c => c.CategoryId == position.CategoryId).FirstOrDefault();
        }

        /// <summary>
        /// Get Category that matches Position's category
        /// </summary>
        /// <param name="id">Id of the Position</param>
        /// <returns>Category that matches Position's category</returns>
        public Category GetOneByPositionId(int id)
        {
            var position = this.GetContext.Positions.Where(p => p.PositionId == id).FirstOrDefault() ?? new Position();

            return this.GetContext.Categories.Where(c => c.CategoryId == position.CategoryId).FirstOrDefault();
        }
        #endregion
    }
}
