using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Interfaces
{
    public interface ILoginService
    {
        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns>Employee, PropertyOwner or failure message</returns>
        Task<object> AuthenticateAsync(string email, string password);        
    }
}
