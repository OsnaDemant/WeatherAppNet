using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WeatherAppNet.WeatherService;

namespace WeatherAppNet
{
    public class ProgramSettings
    {
        public string CityName { get; private set; }
        public TemperatureScale TemperatureScaleType { get; private set; }

        public ProgramSettings(string cityName, TemperatureScale temperatureScaleType)
        {
            this.CityName = cityName;
            this.TemperatureScaleType = temperatureScaleType;

        }
       public ProgramSettings(string cityName): this(cityName,TemperatureScale.Fahrenheit)
        {
            
        }
    }
}
