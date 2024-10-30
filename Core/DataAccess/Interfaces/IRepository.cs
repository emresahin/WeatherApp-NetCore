using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Interfaces
{
    public interface IRepository<TDbContext>
    {
        Task<T> AddAsync<T>(T entity, bool saveChanges = true) where T : class;
        IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool asNoTracking = true) where T : class;
        Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool asNoTracking = true) where T : class;

        Task<T> UpdateAsync<T>(T entity, bool saveChanges = true) where T : class;
        Task<bool> DeleteAsync<T>(T entity, bool saveChanges = true) where T : class;
    }
}
