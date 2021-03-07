using BenFatto.CLF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Service
{
    public class FileProcessor
    {
        private Import Import;
        public FileProcessor(string fileName)
        {
            Import = new Import
            {
                FileName = fileName,
                When = DateTime.Now
            };
        }
        public Import ProcessFile(ClfContext context)
        {
            ImportService importService = new ImportService(context);
            importService.InsertOrUpdate(Import);

            List<string> errors = new List<string>();
            LogRowService service = new LogRowService(context);
            LogRowMismatchService mismatchService = new LogRowMismatchService(context);
            using (StreamReader file = new StreamReader(Import.FileName))
            {
                string line;
                while (null != (line = file.ReadLine()))
                {
                    try
                    {
                        Import.RowCount++;
                        service.InsertCollection(Model.LogRow.Parse(line, Import.Id, Import.RowCount));
                        Import.SuccessCount++;
                    }
                    catch (Exception ex)
                    {
                        Import.ErrorCount++;
                        errors.Add(line);
                        mismatchService.InsertCollection(LogRowMismatch.Parse(line, Import.Id, Import.RowCount, Import.ErrorCount, ex));
                    }
                }
            }
            if (Import.ErrorCount > 0)
            {
                Import.MismatchRowsFileName = Import.FileName + ".err";
                context.SaveChanges();
                using (StreamWriter file = File.CreateText(Import.MismatchRowsFileName))
                {
                    foreach (string line in errors)
                    {
                        file.WriteLine(line);
                    }
                    file.Flush();
                }
            }
            return Import;
        }
    }
}
