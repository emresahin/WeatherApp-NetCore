using Core.DataAccess.Interfaces;
using DataAccess.DBContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.DataAccess
{
	public class RepositoryContextManager
	{
		private readonly IRepository<WeatherDBContext> _context;
		public RepositoryContextManager(IRepository<WeatherDBContext> context)
		{
			_context = context;
		}

		public IRepository<WeatherDBContext> Repository => _context;
	}
}
