using CFTenantPortal.Export;
using CFTenantPortal.Models;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Export Issue list to CSV
    /// </summary>
    public class IssueCSVExport : CSVExportBase<Issue>, IEntityExport<Issue, CSVExportSettings>
    {
        public Task WriteAsync(List<Issue> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                "TypeId";
        }

        protected override string GetEntityLine(Issue entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.TypeId}";
        }
    }
}
