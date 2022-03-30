using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WeatherAppNet
{
    class WeatherService
    {
        public async Task<PropertisWeatherJson> GetWeather()
        {
            HttpClient client = new();
            HttpResponseMessage response = await client.GetAsync("https://api.openweathermap.org/data/2.5/weather?q=Szczecin&appid=946ca6c9efb7e4936218ba5e826f9aab");
            HttpContent content = response.Content;
            string data = await content.ReadAsStringAsync();
            PropertisWeatherJson temp = JsonConvert.DeserializeObject<PropertisWeatherJson>(data);
            return temp;
        }

    }
}
