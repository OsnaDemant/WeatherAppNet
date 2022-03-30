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
    class WeatherService
    {
        private string apikey;
        public WeatherService()
        {
            apikey = "946ca6c9efb7e4936218ba5e826f9aab";
        }

        public async Task<PropertisWeatherJson> GetWeather(string cityName)
        {

            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync(GetWeatherApiUrl(cityName));
            if (response.StatusCode == HttpStatusCode.OK)
            {
                HttpContent content = response.Content;
                string data = await content.ReadAsStringAsync();
                PropertisWeatherJson temp = JsonConvert.DeserializeObject<PropertisWeatherJson>(data);
                return temp;
            }
            return null;
        }
        public string GetWeatherApiUrl(string cityName)
        {
            string response = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&appid="+apikey;
            return response;
        }
    }
}

    