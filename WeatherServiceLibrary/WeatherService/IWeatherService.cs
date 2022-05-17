using System.Collections.Generic;
using System.Threading.Tasks;
using WeatherServiceLibrary.Entities;

namespace WeatherServiceLibrary
{
    public interface IWeatherService
    {
        IEnumerable<WeatherDataQuery> GetCacheData();
        Task<WeatherData> GetWeather(string cityName, bool refreshData);
        Task<WeatherDataQuery> RefreshWeather(string cityName);
    }
}