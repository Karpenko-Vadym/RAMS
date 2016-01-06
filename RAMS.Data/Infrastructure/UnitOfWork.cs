using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RAMS.Data.Infrastructure
{
    /// <summary>
    /// Implementation for IUnitOfWork interface
    /// </summary>
    public class UnitOfWork : IUnitOfWork
    {
        private DataContext Context;

        private readonly IDataFactory DataFactory; // Needed to initialize DataContext

        /// <summary>
        /// Constructor that sets IDataFactory
        /// </summary>
        /// <param name="dataFactory">Instance of IDataFactory that we need to initialize DataContext</param>
        public UnitOfWork(IDataFactory dataFactory)
        {
            this.DataFactory = dataFactory;
        }

        /// <summary>
        /// Method that sends commit to the database
        /// </summary>
        public void Commit()
        {
            try
            {
                this.GetContext.Commit();
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch (DbUpdateConcurrencyException ex)
            #pragma warning restore 0168 
            {
                throw;
            }
            #pragma warning disable 0168 // Supressing warning 0168 "The variable 'ex' is declared but never used"
            catch(DbUpdateException ex)
            #pragma warning restore 0168
            {  
                throw;
            }
        }

        /// <summary>
        /// DataContext getter
        /// </summary>
        public DataContext GetContext
        {
            get 
            { 
                return this.Context ?? (this.Context = this.DataFactory.Init());
            }
        }
    }
}
