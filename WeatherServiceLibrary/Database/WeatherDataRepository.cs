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
        public List<WeatherDataQuery> GetAll()
        {
            return context.WeatherDataQuerys.ToList();
        }

        public WeatherDataQuery AddData(WeatherDataQuery entity)
        {
            context.WeatherDataQuerys.Add(entity);
            context.SaveChanges();
            return entity;
        }


    }

}
