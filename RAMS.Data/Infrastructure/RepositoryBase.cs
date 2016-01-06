using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// Base class of all repositories
    /// </summary>
    /// <typeparam name="T">Class that the repository is implemented for</typeparam>
    public abstract class RepositoryBase<T> where T : class
    {
        private DataContext Context;

        private readonly IDbSet<T> Set; // DbSet

        protected IDataFactory DataFactory { get; private set; }

        /// <summary>
        /// Constructor that sets IDataFactory and DbSet
        /// </summary>
        /// <param name="dataFactory">Instance of IDataFactory that we need to initialize DataContext</param>
        protected RepositoryBase(IDataFactory dataFactory)
        {
            this.DataFactory = dataFactory;

            this.Set = this.GetContext.Set<T>();
        }

        /// <summary>
        /// Add new resource to the database
        /// </summary>
        /// <param name="resource">Instance of a class that is being added</param>
        public virtual void Add(T resource)
        {
            this.Set.Add(resource);
        }

        /// <summary>
        /// Update existing resource
        /// </summary>
        /// <param name="resource">Instance of a class that is being modified</param>
        public virtual void Update(T resource)
        {
            this.Set.Attach(resource);

            this.Context.Entry(resource).State = EntityState.Modified;
        }

        /// <summary>
        /// Delete existing resource
        /// </summary>
        /// <param name="resource">Instance of a class that is being deleted</param>
        public virtual void Delete(T resource)
        {
            this.Set.Remove(resource);
        }

        /// <summary>
        /// Delete existing resources by condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        public virtual void Delete(Expression<Func<T, bool>> predicate)
        {
            IEnumerable<T> data = this.Set.Where<T>(predicate).AsEnumerable();

            foreach(T item in data)
            {
                this.Set.Remove(item);
            }
        }

        /// <summary>
        /// Get resource by id
        /// </summary>
        /// <param name="id">Id of requested resource</param>
        /// <returns>Requested resource</returns>
        public virtual T GetById(int id)
        {
            return this.Set.Find(id);
        }

        /// <summary>
        /// Get first instance of a resource that meets the condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>First instance of a resource that meets the condition</returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return this.Set.Where(predicate).FirstOrDefault<T>();
        }

        /// <summary>
        /// Get all resources of type T
        /// </summary>
        /// <returns>All resources of type T</returns>
        public virtual IEnumerable<T> GetAll()
        {
            return this.Set.ToList();
        }

        /// <summary>
        /// Get multiple resources that meet the condition
        /// </summary>
        /// <param name="predicate">Condition</param>
        /// <returns>Multiple resources that meet the condition</returns>
        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            return this.Set.Where(predicate).ToList();
        }

        /// <summary>
        /// DataContext getter
        /// </summary>
        protected DataContext GetContext
        {
            get
            {
                return this.Context ?? (this.Context = this.DataFactory.Init());
            }
        }
    }
}
