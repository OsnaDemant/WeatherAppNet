using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.Common;

namespace WeatherAppNet
{
    public class ProgramSettings
    {
        public string CityName { get; private set; }
        public TemperatureScale TemperatureScaleType { get; private set; }
        public bool Retrive { get; private set; }

        public ProgramSettings(string cityName, TemperatureScale temperatureScale = TemperatureScale.Kelvin, bool retrive = false)
        {
            this.CityName = cityName;
            this.TemperatureScaleType = temperatureScale;
            this.Retrive = retrive;
        }
    }
}
