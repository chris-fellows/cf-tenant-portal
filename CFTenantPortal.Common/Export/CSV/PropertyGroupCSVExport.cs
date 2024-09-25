using CFTenantPortal.Export.CSV;
using CFTenantPortal.Export;
using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Export Property Group list to CSV
    /// </summary>
    public class PropertyGroupCSVExport : CSVExportBase<PropertyGroup>, IEntityExport<PropertyGroup, CSVExportSettings>
    {
        public Task WriteAsync(List<PropertyGroup> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                $"Name{exportSettings.ColumnDelimiter}" +
                $"Description";
        }

        protected override string GetEntityLine(PropertyGroup entity, CSVExportSettings exportSettings)
        {            
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.Name}{exportSettings.ColumnDelimiter}" +
                $"{entity.Description}";
        }
    }
}
