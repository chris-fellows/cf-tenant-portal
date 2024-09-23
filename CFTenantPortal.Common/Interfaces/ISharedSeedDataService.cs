using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Interfaces
{
    public interface ISharedSeedDataService
    {
        SharedSeed GetSeedData(int group);
    }
}
