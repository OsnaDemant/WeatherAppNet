using System.Threading.Tasks;
using WeatherServiceLibrary.Entities;

namespace WeatherServiceLibrary
{
     public interface IDataDownload
    {
        Task<WeatherDataQuery> RefreshWeather(string cityName);
    }
}