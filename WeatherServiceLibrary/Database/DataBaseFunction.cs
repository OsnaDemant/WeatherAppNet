using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherServiceLibrary.Database
{
   public class DataBaseFunction
    {
        static public async void AddData()
        {
            using (var context = new WeatherContext())
            {
                var wind = new Wind()
                {
                    Speed = 100
                };


                var std = new WeatherData()
                {
                    Wind = wind,
                    Timezone = 5,
                };
                context.WeatherData.Add(std);
                context.SaveChanges();
                var test = context.WeatherData.LastOrDefault((WeatherData m) => m.Wind != null && m.Wind.Speed > 0);
    
                // Console.WriteLine(test);
            }
        }
       
    }
}
