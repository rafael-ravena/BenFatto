using BenFatto.CLF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BenFatto.CLF.Service
{
    public class LogRowService : ModelServiceBase<Model.LogRow>, IDisposable
    {
        public LogRowService(ClfContext context) : base(context) { }
        public override IEnumerable<Model.LogRow> Filter(Model.LogRow entity)
        {
            return Context.LogRows.Include(e => e.Import).Where(e =>
                (e.ImportId == entity.ImportId || entity.ImportId == 0) &&
                (e.IpAddress.Contains(entity.IpAddress) || string.Empty == entity.IpAddress) &&
                (e.ResponseCode == entity.ResponseCode || entity.ResponseCode == 0) &&
                ((e.Date >= entity.Date.BeginningOfHour() && e.Date < entity.Date.NextHour()) || DateTime.MinValue == entity.Date) &&
                (e.UserAgent.Contains(entity.UserAgent) || string.Empty == entity.UserAgent) &&
                (e.Method == entity.Method || entity.Method == 0)
            );
        }

        public override Model.LogRow Get(long entityId)
        {
            return Context.LogRows.FirstOrDefault(e => e.Id == entityId);
        }

        List<Model.LogRow> LogRows { get; set; }
        public void InsertCollection(Model.LogRow entity)
        {
            if (null == LogRows)
                LogRows = new List<LogRow>();
            LogRows.Add(entity);
            if (LogRows.Count >= AppSettings.Current.BatchSize && AppSettings.Current.AutoFlush)
                Flush();
        }
        public void Flush()
        {
            if (null == LogRows || LogRows.Count == 0)
                return;
            foreach (LogRow entity in LogRows)
                Context.LogRows.Add(entity);
            Context.SaveChanges();
            LogRows.Clear();
        }

        public void Dispose()
        {
            if (null != LogRows)
                LogRows.Clear();
            LogRows = null;
        }
    }
}
