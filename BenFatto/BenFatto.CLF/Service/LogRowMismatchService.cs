using BenFatto.CLF.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Service
{
    public class LogRowMismatchService : ModelServiceBase<Model.LogRowMismatch>
    {
        public LogRowMismatchService(ClfContext context) : base(context) { }

        public override IEnumerable<LogRowMismatch> Filter(LogRowMismatch entity)
        {
            return Filter(entity, 0);
        }
        public IEnumerable<LogRowMismatch> Filter(LogRowMismatch entity, int page)
        {
            return Filter(entity, page, ClfAppSettings.Current.PageSize);
        }
        public IEnumerable<LogRowMismatch> Filter(LogRowMismatch entity, int page, int size)
        {
            return Context.LogRowMismatches.Include(e => e.Import).Where(e =>
                (e.ImportId == entity.ImportId || 0 == entity.ImportId) &&
                (string.IsNullOrEmpty(entity.ThrownException) || e.ThrownException.Contains(entity.ThrownException)) &&
                (string.IsNullOrEmpty(entity.Row) || e.Row.Contains(entity.Row))
            ).Skip(page * size).Take(size);
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
            if (LogRowMismatches.Count >= ClfAppSettings.Current.BatchSize && ClfAppSettings.Current.AutoFlush)
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

    }
}
