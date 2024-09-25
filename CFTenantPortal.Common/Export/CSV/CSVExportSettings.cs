using System.Text;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Settings for export to CSV
    /// </summary>
    public class CSVExportSettings
    {
        public string File { get; set; } = string.Empty;

        public char ColumnDelimiter { get; set; } = (char)9;

        public Encoding Encoding { get; set; } = Encoding.UTF8;

        public string DefaultExtension { get; set; } = ".txt";
    }
}
