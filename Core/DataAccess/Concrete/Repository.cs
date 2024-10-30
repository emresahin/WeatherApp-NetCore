using Core.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.DataAccess.Concrete
{
	public class Repository<TDbContext>(TDbContext dbContext) : IRepository<TDbContext>
	where TDbContext : DbContext
	{
		private readonly TDbContext context = dbContext;

		public async Task<T> AddAsync<T>(T entity, bool saveChanges = true) where T : class
		{
			await context.AddAsync(entity);

			if (saveChanges)
				await context.SaveChangesAsync();

			return entity;
		}

		public async Task<List<T>> GetListAsync<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool asNoTracking = true) where T : class
		{
			IQueryable<T> query = context.Set<T>();

			if (include != null)
			{
				query = asNoTracking ? include(query).AsNoTrackingWithIdentityResolution() : include(query);
			}
			else
			{
				if (asNoTracking)
				{
					query = query.AsNoTracking();
				}
			}

			if (filter != null)
				return await query.Where(filter).ToListAsync();

			return await query.ToListAsync();
		}

		public IQueryable<T> GetQueryable<T>(Expression<Func<T, bool>> filter = null, Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null, bool asNoTracking = true) where T : class
		{
			IQueryable<T> query = context.Set<T>();

			if (include != null)
			{
				query = include(query).AsNoTrackingWithIdentityResolution();
			}
			else
			{
				if (asNoTracking)
				{
					query = query.AsNoTracking();
				}
			}

			return filter != null ? query.Where(filter) : query;
		}

        public async Task<T> UpdateAsync<T>(T entity, bool saveChanges = true) where T : class
        {
            context.Update(entity);

            if (saveChanges)
                await context.SaveChangesAsync();

            return entity;
        }

        public async Task<bool> DeleteAsync<T>(T entity, bool saveChanges = true) where T : class
        {
            context.Remove(entity);

            if (saveChanges)
            {
                int rowEffected = await context.SaveChangesAsync();

                return rowEffected > 0;
            }

            return true;
        }
    }
}
