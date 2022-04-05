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

        private WeatherData weatherData;

        public WeatherService(string cityName)
        {
            this.cityName = cityName;
            this.weatherData = null;
        }

        public void Initialize()
        {
            this.apikey = ReadApiKeyFromFile();
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
            string pathToCatalog = pathToCurrentDirectory;
            string pathToCurrentDirectoryApiKey = pathToCatalog + "\\ApiKey.txt";
            string textFile = System.IO.File.ReadAllText(pathToCurrentDirectoryApiKey);
            return textFile;
            // what if is null or what if not exists
            //add path to apikey}
        }


    }
}






