using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherAppNet
{
    class Program
    {

        static async Task Main(string[] args)
        {
            var szczecinWeatherService = new WeatherService("Szczecin");
            await szczecinWeatherService.RefreshWeather();
            var currentWeather = szczecinWeatherService.GetWeather();
            if (currentWeather == null)
            {
                Console.WriteLine("kolego takiego miasta nie ma");
            }
            else
            {
                Console.WriteLine("temperatura w tym mieście wynosi: " + szczecinWeatherService.KelvinToCelsius(currentWeather.Main.Temp));
                Console.WriteLine("wind: "+ currentWeather.Wind.Speed+"\n"+ "chmury: "+ currentWeather.Clouds.All );
                    
            }

        }
        public static string GetCityName()
        {
            string CityName;
            CityName = Console.ReadLine();
            return CityName;
        }

    }
}

