using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static WeatherAppNet.WeatherService;

namespace WeatherAppNet
{
    class Program 
    {

        static async Task Main(string[] args)
        {
            //jesli nie poda argumentu to zwroci instrukcje jak uzywac programu
            //jesli jeden argument podaj w farenhaitach
            // /c w celciuszach
            //  GetCityName();
            if (args.Length <= 0)
            {
                Console.WriteLine("nie podałeś nazwy miasta");
            }
            else
            {
                WeatherData currentWeather;
                try
                {
                    var szczecinWeatherService = new WeatherService(args[0]);
                    szczecinWeatherService.Initialize();

                    currentWeather = await szczecinWeatherService.GetWeather();
                }
                catch (UnauthorizedException e)
                {
                    Console.WriteLine(e.Message);
                    return;
                }

                if (currentWeather == null)
                {
                    Console.WriteLine("takiego miasta nie ma");
                }
                else
                {

                    Console.WriteLine("temperatura w tym mieście wynosi: " + WeatherService.GetTemperature(TemperatureScale.Celsius,currentWeather));
                    Console.WriteLine("wind: " + currentWeather.Wind.Speed + "\n" + "chmury: " + currentWeather.Clouds.All + "% zachmurzenia");

                }
            }
        }
    }
}

