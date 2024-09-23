using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Interfaces
{
    public interface ISharedDatabaseConfigurer
    {
        /// <summary>
        /// Initialise shared database
        /// </summary>
        /// <returns></returns>
        Task InitialiseAsync();
    }
}
