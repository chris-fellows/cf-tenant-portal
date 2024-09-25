using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CFTenantPortal.Export.CSV
{
    /// <summary>
    /// Abstract class for exporting entities to CSV
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public abstract class CSVExportBase<TEntity>
    {
        protected Task WriteAsyncInternal(List<TEntity> entities, CSVExportSettings exportSettings)
        {
            if (File.Exists(exportSettings.File))
            {
                File.Delete(exportSettings.File);
            }

            using (var writer = new StreamWriter(exportSettings.File, false, exportSettings.Encoding))
            {
                // Write headers               
                writer.WriteLine(GetHeaderLine(exportSettings));

                // Write entities
                foreach (var entity in entities)
                {
                    var line = GetEntityLine(entity, exportSettings);
                    writer.WriteLine(line);
                }
                writer.Flush();
                writer.Close();
            }

            return Task.CompletedTask;
        }

        protected virtual string GetHeaderLine(CSVExportSettings exportSettings)
        {
            return string.Empty;
        }

        protected virtual string GetEntityLine(TEntity entity, CSVExportSettings exportSettings)
        {
            return string.Empty;
        }
    }
}
