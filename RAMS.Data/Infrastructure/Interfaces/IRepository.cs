using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// IRepository interface declares the default repository operations
    /// </summary>
    /// <typeparam name="T">Class for which repository is implemented</typeparam>
    public interface IRepository<T> where T : class
    {
        void Add(T resource); // Add new resource

        void Update(T resource); // Update existing resource

        void Delete(T resource); // Delete resource

        void Delete(Expression<Func<T, bool>> predicate); // Delete resource

        T GetById(int id); // Get resource by id

        T Get(Expression<Func<T, bool>> predicate); // Get single resource

        IEnumerable<T> GetAll(); // Get all resources of type T

        IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate); // Get multiple resources of type T
    }
}
