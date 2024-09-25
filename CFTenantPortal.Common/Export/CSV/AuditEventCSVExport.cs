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
    public class AuditEventCSVExport : CSVExportBase<AuditEvent>, IEntityExport<AuditEvent, CSVExportSettings>
    {
        public Task WriteAsync(List<AuditEvent> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                $"CreatedDateTime{exportSettings.ColumnDelimiter}" + 
                $"EventTypeId{exportSettings.ColumnDelimiter}";
        }

        protected override string GetEntityLine(AuditEvent entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.CreatedDateTime}{exportSettings.ColumnDelimiter}" +
                $"{entity.EventTypeId}";
        }
    }
}
