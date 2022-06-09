using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.Database;
using WeatherServiceLibrary.Entities;
using WeatherServiceLibrary.Exceptions;

namespace WeatherServiceLibrary.DataDownload
{
    public class DataDownload : IDataDownload
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string apikey;

        public DataDownload(IConfiguration config, IHttpClientFactory httpClientFactory )
        {
            _httpClientFactory = httpClientFactory;
            this.apikey = config["ApiKey"];
        }

        public async Task<WeatherDataQuery> RefreshWeather(string cityName)
        {
            var httpClient = _httpClientFactory.CreateClient();
            HttpResponseMessage contentResponse = await httpClient.GetAsync(GetWeatherApiUrl(cityName));
            if (contentResponse.StatusCode == HttpStatusCode.OK)
            {
                WeatherDataQuery query = new WeatherDataQuery();
                HttpContent content = contentResponse.Content;
                string data = await content.ReadAsStringAsync();
                query.CityName = cityName;
                query.Time = DateTime.Now;
                query.WeatherData = JsonConvert.DeserializeObject<WeatherData>(data);
                return query;
            }
            else if (contentResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("Wrong Apikey");
            }
            throw new ApplicationException("unnown error retriving data");
        }
        private string GetWeatherApiUrl(string cityName)
        {
            string response = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&appid=" + apikey;
            return response;
        }

    }
}
