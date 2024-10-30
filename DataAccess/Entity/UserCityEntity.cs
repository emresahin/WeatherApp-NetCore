using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models
{
	public class UserCityEntity : BaseEntity
	{
		public int UserId { get; set; }
		public int CityId { get; set; }
		public UserEntity User { get; set; }
		public CityEntity City { get; set; }
	}
}
