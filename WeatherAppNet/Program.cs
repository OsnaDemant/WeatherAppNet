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
            WeatherService Weather = new();
            PropertisWeatherJson DataWeather = await Weather.GetWeather(GetCityName());
            if (DataWeather == null)
            {
                Console.WriteLine("kolego takiego miasta nie ma");
            }
            else
            {
                Console.WriteLine("temperatura w tym mieście wynosi: " + DataWeather.Main.Temp + " K");
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

