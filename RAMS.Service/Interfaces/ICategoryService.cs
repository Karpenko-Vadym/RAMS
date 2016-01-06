using RAMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Service
{
    /// <summary>
    /// ICategoryService interface declares the Category specific service operations
    /// </summary>
    public interface ICategoryService
    {
        IEnumerable<Category> GetAllCategories();

        Category GetOneCategoryById(int id);

        Category GetOneCategoryByPositionId(int id);

        void CreateCategory(Category category);

        void UpdateCategory(Category category);

        void DeleteCategory(Category category);

        void SaveChanges();
    }
}
