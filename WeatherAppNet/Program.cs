using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WeatherAppNet
{
    class Program
    {

        static async Task Main(string[] args)
        {
          //  GetCityName();
            if (args.Length <= 0)
            {
                Console.WriteLine("nie podałeś nazwy miasta");
            }
            else
            {

                var szczecinWeatherService = new WeatherService(args[0]);

                var currentWeather = await szczecinWeatherService.GetWeather();
                if (currentWeather == null)
                {
                    Console.WriteLine("kolego takiego miasta nie ma");
                }
                else
                {
                    Console.WriteLine("temperatura w tym mieście wynosi: " + SupportWeatherFunc.KelvinToCelsius(currentWeather.Main.Temp));
                    Console.WriteLine("wind: " + currentWeather.Wind.Speed + "\n" + "chmury: " + currentWeather.Clouds.All + "% zachmurzenia");

                }
            }
        }
    }
}

