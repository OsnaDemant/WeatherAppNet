using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppNet
{
    public class WeatherService
    {

        private string cityName;
        private string apikey;
        public enum TemperatureScale { Celsius, fahrenheit }
        private WeatherData weatherData;
        

        public WeatherService(string cityName)
        {
            this.cityName = cityName;
            this.weatherData = null;

        }

        public void Initialize()
        {
            this.apikey = ReadApiKeyFromFile();
            if (apikey == null)
            {
                throw new UnauthorizedException("ApiKey not found.");
            }
        }
        public async Task RefreshWeather()
        {
            HttpClient client = new();
            HttpResponseMessage contentResponse = await client.GetAsync(GetWeatherApiUrl(cityName));
            if (contentResponse.StatusCode == HttpStatusCode.OK)
            {
                HttpContent content = contentResponse.Content;
                string data = await content.ReadAsStringAsync();
                weatherData = JsonConvert.DeserializeObject<WeatherData>(data);
            }
            else if (contentResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("Wrong Apikey");
            }

        }
        public async Task<WeatherData> GetWeather()
        {
            if (weatherData == null)
            {
                await RefreshWeather();
            }
            return weatherData;

        }
        public string GetWeatherApiUrl(string cityName)
        {
            string response = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&appid=" + apikey;
            return response;
        }
        public static string ReadApiKeyFromFile()
        {

            string pathToCurrentDirectory = Directory.GetCurrentDirectory();
            string pathToCurrentDirectoryApiKey = Path.Combine(pathToCurrentDirectory, "ApiKey.txt");
            if (File.Exists(pathToCurrentDirectoryApiKey))
            {
                string textFile = System.IO.File.ReadAllText(pathToCurrentDirectoryApiKey);
                return textFile;
            }
            return null;
            // what if is null or what if not exists
            //add path to apikey}
        }

        public float GetTemperature(TemperatureScale temperatureTypeScale)
        {
            {
                //WeatherData currentWeather = new WeatherData();
                switch (temperatureTypeScale)
                {
                    case TemperatureScale.Celsius:
                        return SupportWeatherFunc.KelvinToCelsius(weatherData.Main.Temp);

                    case TemperatureScale.fahrenheit:
                        return weatherData.Main.Temp;

                    default:
                        throw new NotImplementedException("Invalid Temperature Scale");
                }
            }


        }
        public TemperatureScale SetTypeScaleForTemperature(string[] args)
        {
            TemperatureScale ScaleTemperature;
            foreach (string arg in args)
            {
                
                switch (arg.Substring(0, 2).ToUpper())
                {
                    case "/F":
                        ScaleTemperature = TemperatureScale.fahrenheit;
                        return ScaleTemperature;

                    case "/C":
                        ScaleTemperature = TemperatureScale.Celsius;
                        return ScaleTemperature;    

                }
               
            }
            ScaleTemperature = TemperatureScale.fahrenheit;
            return ScaleTemperature;
        }

    }
}






