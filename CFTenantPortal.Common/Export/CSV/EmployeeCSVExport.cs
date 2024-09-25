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
    public class EmployeeCSVExport : CSVExportBase<Employee>, IEntityExport<Employee, CSVExportSettings>
    {
        public Task WriteAsync(List<Employee> entities, CSVExportSettings exportSettings)
        {
            return WriteAsyncInternal(entities, exportSettings);
        }

        protected override string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return $"Id{exportSettings.ColumnDelimiter}" +
                $"Name{exportSettings.ColumnDelimiter}" +
                $"Email";
        }

        protected override string GetEntityLine(Employee entity, CSVExportSettings exportSettings)
        {
            return $"{entity.Id}{exportSettings.ColumnDelimiter}" +
                $"{entity.Name}{exportSettings.ColumnDelimiter}" +
                $"{entity.Email}";
        }
    }
}
