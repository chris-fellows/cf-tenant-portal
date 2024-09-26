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
        /// Authenticates user. Returns either Employee, PropertyOwner or null (Invalid credentials)
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<object> AuthenticateAsync(string email, string password);        
    }
}
