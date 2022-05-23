using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.Common;
using WeatherServiceLibrary.Database;
using WeatherServiceLibrary.Entities;
using WeatherServiceLibrary.Exceptions;


namespace WeatherServiceLibrary
{
    public class WeatherService : IWeatherService
    {

        private string apikey;
        private IWeatherDataRepository weatherDataRepository;
        private readonly IHttpClientFactory _httpClientFactory;

        public WeatherService(IConfiguration config, IWeatherDataRepository weatherDataRepository, IHttpClientFactory httpClientFactory )
        {
            this.weatherDataRepository = weatherDataRepository;
            this.apikey = config["ApiKey"];
            _httpClientFactory = httpClientFactory;
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
                weatherDataRepository.AddData(query);
                return query;
            }
            else if (contentResponse.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new UnauthorizedException("Wrong Apikey");
            }
            throw new ApplicationException("unnown error retriving data");
        }

        public async Task<WeatherData> GetWeather(string cityName, bool refreshData)
        {
            cityName = cityName.ToLower();
            DateTime timeNow = DateTime.UtcNow;
            TimeSpan oneHour = new TimeSpan(1, 0, 0);
            var listOfAllElementsInRepo = weatherDataRepository.GetAll();
            var lessThenOneHourElements = listOfAllElementsInRepo.Where(x => x.Time >= (timeNow - oneHour) && x.CityName == cityName);
            var newestItem = lessThenOneHourElements.OrderByDescending(x => x.Time).FirstOrDefault();

            if (newestItem == null || refreshData == true)
            {
                newestItem = await RefreshWeather(cityName);
            }
            return newestItem.WeatherData;
        }

        private string GetWeatherApiUrl(string cityName)
        {
            string response = "https://api.openweathermap.org/data/2.5/weather?q=" + cityName + "&appid=" + apikey;
            return response;
        }

        public IEnumerable<WeatherDataQuery> GetCacheData()
        {
            var listOfAllElementsInRepo = weatherDataRepository.GetAll();
            return listOfAllElementsInRepo.AsEnumerable();
        }
    }
}






