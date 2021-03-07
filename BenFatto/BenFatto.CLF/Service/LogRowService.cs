using BenFatto.CLF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace BenFatto.CLF.Service
{
    public class LogRowService : ModelServiceBase<Model.LogRow>
    {
        public LogRowService(ClfContext context) : base(context) { }
        public override IEnumerable<Model.LogRow> Filter(Model.LogRow entity)
        {
            return Filter(entity, 0);
        }
        internal IEnumerable<LogRow> Filter(LogRow entity, int page)
        {
            return Filter(entity, page, ClfAppSettings.Current.PageSize);
        }
        internal IEnumerable<LogRow> Filter(LogRow entity, int page, int size)
        {
            return Context.LogRows.Include(e => e.Import).Where(e =>
                 (e.ImportId == entity.ImportId || 0 == entity.ImportId) &&
                 (string.IsNullOrEmpty(entity.IpAddress) || e.IpAddress.Contains(entity.IpAddress)) &&
                 (e.ResponseCode == entity.ResponseCode || 0 == entity.ResponseCode) &&
                 ((e.Date >= entity.Date.BeginningOfHour() && e.Date < entity.Date.NextHour()) || DateTime.MinValue == entity.Date) &&
                 (string.IsNullOrEmpty(entity.UserAgent) || e.UserAgent.Contains(entity.UserAgent)) &&
                 (e.Method == entity.Method || 0 == entity.Method)
            ).OrderBy(e => e.Date).ThenBy(e => e.RowNumber).ThenBy(e => e.ImportId).Skip(page * size).Take(size);
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
            if (LogRows.Count >= ClfAppSettings.Current.BatchSize && ClfAppSettings.Current.AutoFlush)
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
        public IEnumerable<DTO.UserAgent> GetDistinctUserAgents()
        {
            return Context.LogRows.Select(e => new DTO.UserAgent { Name = e.UserAgent }).Distinct().OrderBy(e => e.Name);
        }

    }
}
