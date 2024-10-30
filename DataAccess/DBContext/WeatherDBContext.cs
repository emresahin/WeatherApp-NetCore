using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DBContext
{
	public class WeatherDBContext : DbContext
	{
		public WeatherDBContext(DbContextOptions<WeatherDBContext> options) : base(options)
		{
			
		}


		public DbSet<CityEntity> Cities { get; set; }
		public DbSet<UserEntity> Users { get; set; }
		public DbSet<WeatherCityEntity> WeatherCities { get; set; }
		public DbSet<UserCityEntity> UserCities { get; set; }



		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

			base.OnModelCreating(modelBuilder);
		}

		public override int SaveChanges()
		{
			UpdateEntities();
			return base.SaveChanges();
		}

		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			UpdateEntities();
			return await base.SaveChangesAsync(cancellationToken);
		}
		private void UpdateEntities()
		{
			var entries = ChangeTracker.Entries<BaseEntity>();

			foreach (var entry in entries)
			{
				if (entry.State == EntityState.Added)
				{
					entry.Entity.CreatedDate = DateTime.UtcNow;
					entry.Entity.IsActive = true;
				}
			}
		}
	}
}
