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
       // public string CityName { get; private set; }
       // public TemperatureScale TemperatureScaleType { get; private set; }

        // private string cityName;
        // private TemperatureScale temperatureScaleType;

        static async Task Main(string[] args)
        { 
            ProgramSettings programSettings = ParseArguments(args);
            if (programSettings == null)
            {
                return;
            }
            var cityWeatherService = new WeatherService(programSettings.CityName);
            WeatherData currentWeather;

            try
            {
                cityWeatherService.Initialize();
                currentWeather = await cityWeatherService.GetWeather();
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
                Console.WriteLine("temperatura w tym mieście wynosi: " + cityWeatherService.GetTemperature(programSettings.TemperatureScaleType));
                Console.WriteLine("wind: " + currentWeather.Wind.Speed + "\n" + "chmury: " + currentWeather.Clouds.All + "% zachmurzenia");
            }
        }

        static public TemperatureScale GetTypeScaleForTemperature(string arg)
        {
            TemperatureScale scaleTemperature = TemperatureScale.Fahrenheit;

            switch (arg.Substring(0, 2).ToUpper())
            {
                case "/F":
                    scaleTemperature = TemperatureScale.Fahrenheit;
                    break;

                case "/C":
                    scaleTemperature = TemperatureScale.Celsius;
                    break;
            }
            return scaleTemperature;
        }

        static ProgramSettings ParseArguments(string[] args)
        {
            
            if (args.Length <= 0)
            {
                Console.WriteLine("Proszę podać nazwę miasta jako pierwsz argument. Program domyślnie oblicza Fahrenhaity można ustawić celciusze dodając jako drugi argument po mieście /C");
                return null;
            }
            else if (args.Length == 1)
            {
               return new ProgramSettings(args[0]);
            }
            else if (args.Length == 2)
            {
                return new ProgramSettings(args[0], GetTypeScaleForTemperature(args[1]));
            }
            return null;
        }
    }
}

