using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppNet
{
    public static class SupportWeatherFunc
    {
        public static float KelvinToCelsius(float kelvin)
        {
            float celsius = (float)(kelvin - 273.15);
            return celsius;
        }
    }
}
