using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RAMS.Models
{
    /// <summary>
    /// Abstract base class that allows all derived classes to be serialized
    /// </summary>
    [Serializable]
    public abstract class Base { }

    /// <summary>
    /// Abstract class that holds all common properties of all derived classes
    /// </summary>
    public abstract class BaseEntity : Base
    {
        [Timestamp]
        public byte[] Timestamp { get; set; } // Property that will allow to control data concurrency

        // Default constructor for setting certain properties
        public BaseEntity() { }
    }
}