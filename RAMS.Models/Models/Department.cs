using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Models
{
    /// <summary>
    /// Department is a collection of Agents and one manager
    /// </summary>
    public class Department : BaseEntity
    {
        public int DepartmentId { get; set; }

        public string Name { get; set; }

        public virtual List<Agent> Agents { get; set; }

        public virtual List<Position> Positions { get; set; }

        public Department()
        {
            this.Agents = new List<Agent>();

            this.Positions = new List<Position>();
        }
    }
}
