using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Export Property Owner list to CSV
    /// </summary>
    public class PropertyOwnerCSVExport : CSVExportBase<PropertyOwner>, IEntityExport<PropertyOwner, CSVExportSettings>
    {
        public Task WriteAsync(List<PropertyOwner> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +                
                $"Address";
        }

        protected override string GetEntityLine(PropertyOwner entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +                
                $"{entity.Address.ToSummary()}";
        }
    }
}
