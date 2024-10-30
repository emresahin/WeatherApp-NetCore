using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Configurations
{
	internal class WeatherCityModelConfiguration : IEntityTypeConfiguration<WeatherCityEntity>
	{
		public void Configure(EntityTypeBuilder<WeatherCityEntity> builder)
		{
			builder.ToTable("TBL_WEATHER_CITY").HasKey(x => x.Id);
			builder.Property(p => p.Degree).HasPrecision(18,2);
		}
	}
}
