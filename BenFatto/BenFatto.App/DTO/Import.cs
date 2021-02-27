using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.App.DTO
{
    public class Import
    {
        public Import()
        {
            LogRowMismatches = new List<LogRowMismatch>();
            LogRows = new List<LogRow>();
        }

        public long Id { get; set; }
        [DisplayName("Imported at")]
        public DateTime When { get; set; }
        [DisplayName("File")]
        public string FileName { get; set; }
        [DisplayName("Errors File")]
        public string MismatchRowsFileName { get; set; }
        public int UserId { get; set; }
        [DisplayName("Rows in file")]
        public int RowCount { get; set; }
        [DisplayName("Errors found")]
        public int ErrorCount { get; set; }
        [DisplayName("Successfuly processed")]
        public int SuccessCount { get; set; }
        public List<LogRowMismatch> LogRowMismatches { get; set; }
        public List<LogRow> LogRows { get; set; }

        public string FileNameDisplay
        {
            get
            {
                string[] parts = FileName.Split('\\');
                return parts[parts.Length - 1];
            }
        }
    }
}
