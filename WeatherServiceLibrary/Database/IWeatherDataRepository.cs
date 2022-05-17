using System.Linq;
using WeatherServiceLibrary.Entities;

namespace WeatherServiceLibrary.Database
{
    public interface IWeatherDataRepository
    {
        WeatherDataQuery AddData(WeatherDataQuery entity);
        IQueryable<WeatherDataQuery> GetAll();
    }
}