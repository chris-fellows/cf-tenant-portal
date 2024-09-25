using CFTenantPortal.Export;
using CFTenantPortal.Models;
using System.IO;
using System.Text;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Export Property list to CSV
    /// </summary>
    public class PropertyCSVExport : CSVExportBase<Property>, IEntityExport<Property, CSVExportSettings>
    {
        public Task WriteAsync(List<Property> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                $"OwnerId{exportSettings.ColumnDelimiter}" +
                $"GroupId{exportSettings.ColumnDelimiter}" +
                $"Address";
        }

        protected override string GetEntityLine(Property entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.OwnerId}{exportSettings.ColumnDelimiter}" +
                $"{entity.GroupId}{exportSettings.ColumnDelimiter}" +
                $"{entity.Address.ToSummary()}";
        }
    }
}
