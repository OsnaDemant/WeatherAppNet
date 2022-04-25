using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.Entities;

namespace WeatherServiceLibrary.Database
{
    public class WeatherDataRepository
    {
        private WeatherContext context;
        public WeatherDataRepository()
        {

            context = new WeatherContext();
        }
        public IQueryable<WeatherDataQuery> GetAll()
        {
            return context.WeatherDataQuerys
                .Include(x => x.WeatherData)
                .Include(x => x.WeatherData.Wind)
                .Include(x => x.WeatherData.Clouds)
                .Include(x => x.WeatherData.Coord)
                .Include(x => x.WeatherData.Main)
                .Include(x => x.WeatherData.Sys);
               // .Include(x => x.CityName);
                
        }

        public WeatherDataQuery AddData(WeatherDataQuery entity)
        {
            context.WeatherDataQuerys.Add(entity);
            context.SaveChanges();
            return entity;
        }


    }

}
