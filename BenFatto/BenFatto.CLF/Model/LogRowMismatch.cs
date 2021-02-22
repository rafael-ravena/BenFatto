using System;
using System.Collections.Generic;

#nullable disable

namespace BenFatto.CLF.Model
{
    public partial class LogRowMismatch
    {
        public long Id { get; set; }
        public int ImportId { get; set; }
        public int OriginalRowNumber { get; set; }
        public int RowNumber { get; set; }
        public string Row { get; set; }
        public string ThrownException { get; set; }

        public virtual Import Import { get; set; }
    }
}
