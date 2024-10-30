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
	internal class UserCityModelConfiguration : IEntityTypeConfiguration<UserCityEntity>
	{
		public void Configure(EntityTypeBuilder<UserCityEntity> builder)
		{
			builder.ToTable("TBL_USER_CITY").HasKey(x => x.Id);
		}
	}
}
