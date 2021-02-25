using System;
using System.Collections.Generic;

#nullable disable

namespace BenFatto.CLF.Model
{
    public partial class LogRowMismatch : IModelBase
    {
        public long Id { get; set; }
        public long ImportId { get; set; }
        public int OriginalRowNumber { get; set; }
        public int RowNumber { get; set; }
        public string Row { get; set; }
        public string ThrownException { get; set; }
        public bool Corrected { get; set; }
        public DateTime CorrectedAt { get; set; }

        public virtual Import Import { get; set; }
        public static LogRowMismatch Parse(string line, long importId, int originalLineNumber, int lineNumber, Exception exception)
        {
            LogRowMismatch logRow = new LogRowMismatch();
            logRow.Row = line;
            logRow.OriginalRowNumber = originalLineNumber;
            logRow.RowNumber = lineNumber;
            logRow.ImportId = importId;
            logRow.ThrownException = exception.ToString();
            logRow.Corrected = false;
            logRow.CorrectedAt = DateTime.Now.AddYears(20);
            return logRow;
        }
        public override string ToString()
        {
            return $"(...)\n\n(...)";
        }
        public LogRow TryConvert()
        {
            try
            {
                return LogRow.Parse(Row, ImportId, OriginalRowNumber);
            }
            catch (Exception)
            {
                //couldn't convert yet... keep correcting it
                //throw;
                return null;
            }
        }
    }
}
