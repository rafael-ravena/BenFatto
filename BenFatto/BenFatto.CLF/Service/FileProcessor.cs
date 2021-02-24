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
        public FileProcessor(string fileName, int userId)
        {
            Import = new Import
            {
                FileName = fileName,
                UserId = userId,
                When = DateTime.Now
            };
        }
        public void ProcessFile()
        {
            using (ClfContext context = new ClfContext())
            {
                ImportService importService = new ImportService(context);
                importService.InsertOrUpdate(Import);

                List<string> errors = new List<string>();
                using (LogRowService service = new LogRowService(context))
                using (LogRowMismatchService mismatchService = new LogRowMismatchService(context))
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
                    using (StreamWriter file = File.CreateText(Import.FileName + ".err"))
                    {
                        foreach (string line in errors)
                        {
                            file.WriteLine(line);
                        }
                        file.Flush();
                    }
                }
            }
        }
    }
}
