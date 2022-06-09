using System.Threading.Tasks;
using WeatherServiceLibrary.Entities;

namespace WeatherServiceLibrary.DataDownload
{
     public interface IDataDownload
    {
        Task<WeatherDataQuery> RefreshWeather(string cityName);
    }
}