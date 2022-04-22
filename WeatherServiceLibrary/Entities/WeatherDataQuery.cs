using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherServiceLibrary.Entities
{
    public class WeatherDataQuery
    {
        public int Id { get; set; }
        public WeatherData WeatherData { get; set; }
        public string CityName { get; set; }
        public DateTime Time { get; set; }
    }
}
