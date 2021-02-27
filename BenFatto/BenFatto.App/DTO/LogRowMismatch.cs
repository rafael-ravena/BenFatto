using System;
using System.ComponentModel;

namespace BenFatto.App.DTO
{
    public class LogRowMismatch
    {
        public long Id { get; set; }
        public long ImportId { get; set; }
        [DisplayName("Line in original file")]
        public int OriginalRowNumber { get; set; }
        [DisplayName("Line in errors file")]
        public int RowNumber { get; set; }
        [DisplayName("Line")]
        public string Row { get; set; }
        [DisplayName("Exception thrown")]
        public string ThrownException { get; set; }
        [DisplayName("Corrected?")]
        public bool Corrected { get; set; }
        [DisplayName("Corrected at")]
        public DateTime CorrectedAt { get; set; }
        public Import Import { get; set; }
    }
}