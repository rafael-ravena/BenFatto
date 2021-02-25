using BenFatto.CLF.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BenFatto.CLF.Service
{
    public abstract class ModelServiceBase<TEntity>
        where TEntity : class, IModelBase
    {
        public ClfContext Context { get; set; }
        public ModelServiceBase(ClfContext context)
        {
            Context = context;
        }
        public void InsertOrUpdate(TEntity entity)
        {
            Context.Entry(entity).State = entity.Id == 0 ? EntityState.Added : EntityState.Modified;
            Context.SaveChanges();
        }
        public void Delete(long id)
        {
            TEntity entity = Get(id);
            Context.Entry(entity).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        public abstract TEntity Get(long entityId);
        public abstract IEnumerable<TEntity> Filter(TEntity entity);

    }
}
