﻿using Newtonsoft.Json;
using System;
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

        public void Initialize()
        {
            this.apikey = ReadApiKeyFromFile();
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
        public async Task<WeatherData> GetWeather(string cityName)
        {
             
        DateTime timeNow = DateTime.UtcNow;
            TimeSpan oneHour = new TimeSpan(1, 0, 0);
            var listOfAllElementsInRepo = weatherDataRepository.GetAll();
            var lessThenOneHourElements = listOfAllElementsInRepo.Where(x => x.Time >= (timeNow - oneHour)).ToList();

            var newestItem = lessThenOneHourElements.OrderByDescending(x => x.Time).ToList().FirstOrDefault();

            if (newestItem == null)
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
        }

    }
}






