using CFTenantPortal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Export Message list to CSV
    /// </summary>
    public class MessageCSVExport : CSVExportBase<Message>, IEntityExport<Message, CSVExportSettings>
    {
        public Task WriteAsync(List<Message> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                $"Text";                
        }

        protected override string GetEntityLine(Message entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.Text}";
        }
    }
}
