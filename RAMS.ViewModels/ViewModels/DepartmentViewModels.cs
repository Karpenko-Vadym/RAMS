using RAMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.ViewModels
{
    public class DepartmentListViewModel
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public int NumOfAgents { get; set; }

        public virtual List<Agent> Agents { get; set; }

        public DepartmentListViewModel()
        {
            this.Agents = new List<Agent>();
        }
    }

    public class DepartmentAddViewModel
    {
        [Required]
        [StringLength(150, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        [Display(Name = "Department Name")]
        public string Name { get; set; }
    }

    public class DepartmentEditViewModel : DepartmentAddViewModel
    {
        public int DepartmentId { get; set; }

        public virtual List<Agent> Agents { get; set; }

        public DepartmentEditViewModel()
        {
            this.Agents = new List<Agent>();
        }
    }
}
