using BenFatto.CLF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Service
{
    public class ImportService : ModelServiceBase<Model.Import>
    {
        public ImportService(ClfContext context) : base(context) { }
        public override IEnumerable<Import> Filter(Import entity)
        {
            return Context.Imports.Where(e =>
                ((e.When >= entity.When.BeginningOfDay() && e.When < entity.When.NextDay()) || DateTime.MinValue == entity.When) &&
                (e.SuccessCount > 0 || 0 == entity.SuccessCount) &&
                (e.ErrorCount > 0 || 0 == entity.ErrorCount) &&
                (string.IsNullOrEmpty(entity.FileName) || e.FileName.Contains(entity.FileName))
            );
        }
        public override Import Get(long entityId)
        {
            return Context.Imports.FirstOrDefault(e => e.Id == entityId);
        }
    }
}
