using RAMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    /// <summary>
    /// CategoryListViewModel view model declares properties for _CategoryList partial view
    /// </summary>
    public class CategoryListViewModel
    {
        public int CategoryId { get; set; }

        [Display(Name = "Category Id")]
        public string CategoryIdForDisplay { get; set; }

        [Display(Name = "Category Name")]
        public string Name { get; set; }

        [Display(Name = "Number of Positions")]
        public int NumOfPositions { get; set; }

        public virtual List<Position> Positions { get; set; }

        public CategoryListViewModel()
        {
            this.Positions = new List<Position>();
        }
    }

    /// <summary>
    /// CategoryAddViewModel view model declares properties for _NewCategory partial view
    /// </summary>
    public class CategoryAddViewModel
    {
        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// CategoryEditViewModel view model declares properties for _EditCategory partial view
    /// </summary>
    public class CategoryEditViewModel : CategoryAddViewModel
    {
        public int CategoryId { get; set; }

        public byte[] Timestamp { get; set; }

        public virtual List<Position> Positions { get; set; }

        public CategoryEditViewModel()
        {
            this.Positions = new List<Position>();
        }
    }

    /// <summary>
    /// CategoryAddEditConfirmationViewModel view model declares properties for _NewCategoryConfirmation and _EditCategoryConfirmation partial views
    /// </summary>
    public class CategoryAddEditConfirmationViewModel
    {
        [Display(Name = "Category Name")]
        public string Name { get; set; }
    }
}
