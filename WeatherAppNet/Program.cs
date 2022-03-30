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
            PropertisWeatherJson DataWeather = await Weather.GetWeather();
            Console.WriteLine("temperatura w szczecinie wynosi "+DataWeather.Main.Temp+" K");
        }


    }
}

