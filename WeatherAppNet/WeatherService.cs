using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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
            this.apikey = "946ca6c9efb7e4936218ba5e826f9aab";
            this.cityName = cityName;

            this.weatherData = null;
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
    }
}

