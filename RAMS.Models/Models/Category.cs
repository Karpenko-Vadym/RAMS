using RAMS.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// Category class represents a position category that will be used to search/filter positions
    /// </summary>
    public class Category : BaseEntity
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public virtual List<Position> Positions { get; set; }
    }
}