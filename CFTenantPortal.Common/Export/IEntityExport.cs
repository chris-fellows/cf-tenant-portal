using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Export
{
    /// <summary>
    /// Interface for exporting entities
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEntityExport<TEntity, TExportSettings>
    {
        Task WriteAsync(List<TEntity> entities, TExportSettings exportSettings);           
    }
}
