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
    public class WeatherService
    {

        private string apikey;
        private WeatherDataRepository weatherDataRepository;
      

        public WeatherService()
        {
            this.weatherDataRepository = new WeatherDataRepository();
          
        }

        public void Initialize(string apiKey)
        {
            this.apikey = apiKey;
            if (apikey == null)
            {
                throw new UnauthorizedException("ApiKey not found.");
            }
        }
        public async Task<WeatherDataQuery> RefreshWeather(string cityName)
        {
            HttpClient client = new();
            HttpResponseMessage contentResponse = await client.GetAsync(GetWeatherApiUrl(cityName));
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
        public string GetWeatherApiUrl(string cityName)
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






