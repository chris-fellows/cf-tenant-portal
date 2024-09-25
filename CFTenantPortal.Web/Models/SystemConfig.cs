using CFTenantPortal.Export.CSV;

namespace CFTenantPortal.Web.Models
{
    /// <summary>
    /// Static config settings
    /// </summary>
    public static class SystemConfig
    {        
        public static CSVExportSettings DefaultCSVExportSettings => new CSVExportSettings()
        {            
            ColumnDelimiter = (Char)9,
            Encoding = System.Text.Encoding.UTF8,
            DefaultExtension = ".txt"
        };        
    }
}
