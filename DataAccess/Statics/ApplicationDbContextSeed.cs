using DataAccess.DBContext;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Statics
{
	public static class ApplicationDbContextSeed
	{
		public static async Task SeedCitiesAsync(WeatherDBContext context)
		{
			if (!context.Cities.Any())
			{
				var cities = new List<CityEntity>
			{
				new CityEntity { Name = "Adana" },
				new CityEntity { Name = "Adıyaman" },
				new CityEntity { Name = "Afyonkarahisar" },
				new CityEntity { Name = "Ağrı" },
				new CityEntity { Name = "Amasya" },
				new CityEntity { Name = "Ankara" },
				new CityEntity { Name = "Antalya" },
				new CityEntity { Name = "Artvin" },
				new CityEntity { Name = "Aydın" },
				new CityEntity { Name = "Balıkesir" },
				new CityEntity { Name = "Bilecik" },
				new CityEntity { Name = "Bingöl" },
				new CityEntity { Name = "Bitlis" },
				new CityEntity { Name = "Bolu" },
				new CityEntity { Name = "Burdur" },
				new CityEntity { Name = "Bursa" },
				new CityEntity { Name = "Çanakkale" },
				new CityEntity { Name = "Çankırı" },
				new CityEntity { Name = "Çorum" },
				new CityEntity { Name = "Denizli" },
				new CityEntity { Name = "Diyarbakır" },
				new CityEntity { Name = "Edirne" },
				new CityEntity { Name = "Elazığ" },
				new CityEntity { Name = "Erzincan" },
				new CityEntity { Name = "Erzurum" },
				new CityEntity { Name = "Eskişehir" },
				new CityEntity { Name = "Gaziantep" },
				new CityEntity { Name = "Giresun" },
				new CityEntity { Name = "Gümüşhane" },
				new CityEntity { Name = "Hakkari" },
				new CityEntity { Name = "Hatay" },
				new CityEntity { Name = "Isparta" },
				new CityEntity { Name = "Mersin" },
				new CityEntity { Name = "İstanbul" },
				new CityEntity { Name = "İzmir" },
				new CityEntity { Name = "Kars" },
				new CityEntity { Name = "Kastamonu" },
				new CityEntity { Name = "Kayseri" },
				new CityEntity { Name = "Kırklareli" },
				new CityEntity { Name = "Kırşehir" },
				new CityEntity { Name = "Kocaeli" },
				new CityEntity { Name = "Konya" },
				new CityEntity { Name = "Kütahya" },
				new CityEntity { Name = "Malatya" },
				new CityEntity { Name = "Manisa" },
				new CityEntity { Name = "Kahramanmaraş" },
				new CityEntity { Name = "Mardin" },
				new CityEntity { Name = "Muğla" },
				new CityEntity { Name = "Muş" },
				new CityEntity { Name = "Nevşehir" },
				new CityEntity { Name = "Niğde" },
				new CityEntity { Name = "Ordu" },
				new CityEntity { Name = "Rize" },
				new CityEntity { Name = "Sakarya" },
				new CityEntity { Name = "Samsun" },
				new CityEntity { Name = "Siirt" },
				new CityEntity { Name = "Sinop" },
				new CityEntity { Name = "Sivas" },
				new CityEntity { Name = "Tekirdağ" },
				new CityEntity { Name = "Tokat" },
				new CityEntity { Name = "Trabzon" },
				new CityEntity { Name = "Tunceli" },
				new CityEntity { Name = "Şanlıurfa" },
				new CityEntity { Name = "Uşak" },
				new CityEntity { Name = "Van" },
				new CityEntity { Name = "Yozgat" },
				new CityEntity { Name = "Zonguldak" },
				new CityEntity { Name = "Aksaray" },
				new CityEntity { Name = "Bayburt" },
				new CityEntity { Name = "Karaman" },
				new CityEntity { Name = "Kırıkkale" },
				new CityEntity { Name = "Batman" },
				new CityEntity { Name = "Şırnak" },
				new CityEntity { Name = "Bartın" },
				new CityEntity { Name = "Ardahan" },
				new CityEntity { Name = "Iğdır" },
				new CityEntity { Name = "Yalova" },
				new CityEntity { Name = "Karabük" },
				new CityEntity { Name = "Kilis" },
				new CityEntity { Name = "Osmaniye" },
				new CityEntity { Name = "Düzce" }
			};

				await context.Cities.AddRangeAsync(cities);
				await context.SaveChangesAsync();
			}
		}
	}
}
