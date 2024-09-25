using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Export Employee list to CSV
    /// </summary>
    public class DocumentCSVExport : CSVExportBase<Document>, IEntityExport<Document, CSVExportSettings>
    {
        public Task WriteAsync(List<Document> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                $"Name";
        }

        protected override string GetEntityLine(Document entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.Name}";
        }
    }
}
