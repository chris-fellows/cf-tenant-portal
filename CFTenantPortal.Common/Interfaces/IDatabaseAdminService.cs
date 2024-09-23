using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Interfaces
{
    public interface IDatabaseAdminService
    { 
        /// <summary>
        /// Initialises shared database
        /// </summary>
        /// <returns></returns>
        Task InitialiseSharedAsync();
                
        /// <summary>
        /// Deletes shared data
        /// </summary>
        /// <returns></returns>
        Task DeleteSharedData();
      
        /// <summary>
        /// Loads shared data from specific group
        /// </summary>
        /// <param name="group"></param>
        /// <returns></returns>
        Task LoadSharedData(int group);
    }
}
