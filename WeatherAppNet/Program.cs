using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherServiceLibrary;
using WeatherServiceLibrary.Common;
using WeatherServiceLibrary.Exceptions;
using WeatherServiceLibrary.Database;
using WeatherServiceLibrary.Entities;


namespace WeatherAppNet
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ProgramSettings programSettings = ParseArguments(args);
            if (programSettings == null)
            {
                return;
            }
            var cityWeatherService = new WeatherService();
            WeatherData currentWeather;
            // DataBaseFunction.AddData();
            try
            {
                cityWeatherService.Initialize();


                currentWeather = await cityWeatherService.GetWeather(programSettings.CityName, programSettings.Retrive);
            }
            catch (UnauthorizedException e)
            {
                Console.WriteLine(e.Message);
                return;
            }

            if (currentWeather == null)
            {
                Console.WriteLine("This city dont exist");
            }
            else
            {
                Console.WriteLine("Weather in: " + programSettings.CityName + "\n" + "Temperature: " + currentWeather.GetTemperature(programSettings.TemperatureScaleType));
                Console.WriteLine("Wind: " + currentWeather.Wind.Speed + " meter/sec" + "\n" + "Clouds: " + currentWeather.Clouds.All + " %");
                Console.WriteLine("Humidity: " + currentWeather.Main.Humidity + " %" + "\n" + "Visibility: " + currentWeather.Visibility + " meter");
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
                Console.WriteLine("Please enter the name of the city as the first argument. The program calculates Fahrenhaity by default. you can set celsius by adding /C as the second argument after the city");
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
            else if (args.Length == 3)
            {
                return new ProgramSettings(args[0], GetTypeScaleForTemperature(args[1]), RetriveData(args[2]));
            }
            return null;
        }
        static public bool RetriveData(string arg)
        {
            switch (arg.Substring(0, 2).ToUpper())
            {
                case "/R":
                    return true;
               
            }
            return false;

        }

    }
}

