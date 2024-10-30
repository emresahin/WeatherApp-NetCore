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
	internal class CityModelConfiguration : IEntityTypeConfiguration<CityEntity>
	{
		public void Configure(EntityTypeBuilder<CityEntity> builder)
		{
			builder.ToTable("TBL_CITY").HasKey(x => x.Id);
		}
	}
}
