using BenFatto.CLF.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Service
{
    public class LogRowMismatchService : ModelServiceBase<Model.LogRowMismatch>, IDisposable
    {
        public LogRowMismatchService(ClfContext context) : base(context) { }

        public override IEnumerable<LogRowMismatch> Filter(LogRowMismatch entity)
        {
            return Context.LogRowMismatches.Include(e => e.Import).Where(e =>
                (e.ImportId == entity.ImportId || entity.ImportId == 0) &&
                (e.ThrownException.Contains(entity.ThrownException) || string.Empty == entity.ThrownException) &&
                (e.Row.Contains(entity.Row) || string.Empty == entity.Row)
            );
        }

        public override LogRowMismatch Get(long entityId)
        {
            return Context.LogRowMismatches.FirstOrDefault(e => e.Id == entityId);
        }
        List<Model.LogRowMismatch> LogRowMismatches { get; set; }
        public void InsertCollection(Model.LogRowMismatch entity)
        {
            if (null == LogRowMismatches)
                LogRowMismatches = new List<LogRowMismatch>();
            LogRowMismatches.Add(entity);
            if (LogRowMismatches.Count >= AppSettings.Current.BatchSize && AppSettings.Current.AutoFlush)
                Flush();
        }
        public void Flush()
        {
            if (null == LogRowMismatches || LogRowMismatches.Count == 0)
                return;
            foreach (LogRowMismatch entity in LogRowMismatches)
                Context.LogRowMismatches.Add(entity);
            Context.SaveChanges();
            LogRowMismatches.Clear();
        }

        public void Dispose()
        {
            if (null != LogRowMismatches)
                LogRowMismatches.Clear();
            LogRowMismatches = null;
        }
    }
}
