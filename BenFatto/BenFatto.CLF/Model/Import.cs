using System;
using System.Collections.Generic;

#nullable disable

namespace BenFatto.CLF.Model
{
    public partial class Import
    {
        public Import()
        {
            LogRowMismatches = new HashSet<LogRowMismatch>();
            LogRows = new HashSet<LogRow>();
        }

        public int Id { get; set; }
        public DateTime When { get; set; }
        public string FileName { get; set; }
        public string MismatchRowsFileName { get; set; }
        public string UserName { get; set; }
        public int? RowCount { get; set; }
        public int? ErrorCount { get; set; }
        public int? SuccessCount { get; set; }

        public virtual ICollection<LogRowMismatch> LogRowMismatches { get; set; }
        public virtual ICollection<LogRow> LogRows { get; set; }
    }
}
