using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherServiceLibrary.Common
{
    public static class WeatherHelper
    {
        public static float KelvinToCelsius(float kelvin)
        {
            float celsius = (float)(kelvin - 273.15);
            return celsius;
        }
    }
}
