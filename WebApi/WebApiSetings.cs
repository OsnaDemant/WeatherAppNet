using WeatherServiceLibrary.Common;

namespace WebApi
{
    public class WebApiSetings
    {
        public string CityName { get; private set; }
        public TemperatureScale TemperatureScaleType { get; private set; }
        public bool Retrive { get; private set; }

        public WebApiSetings(string cityName, TemperatureScale temperatureScale = TemperatureScale.Kelvin, bool retrive = false)
        {
            this.CityName = cityName;
            this.TemperatureScaleType = temperatureScale;
            this.Retrive = retrive;
        }
    }
}
