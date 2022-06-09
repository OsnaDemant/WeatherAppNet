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
using WeatherServiceLibrary.DataDownload;
using WeatherServiceLibrary.Entities;
using WeatherServiceLibrary.Exceptions;


namespace WeatherServiceLibrary
{
    public class WeatherService : IWeatherService
    {

      
        private IWeatherDataRepository weatherDataRepository;
        private IDataDownload _dataDownload;

        public WeatherService(IDataDownload dataDownload, IWeatherDataRepository weatherDataRepository)
        {
           _dataDownload = dataDownload;
            this.weatherDataRepository = weatherDataRepository;

        }
        public async Task<WeatherData> GetWeather(string cityName, bool refreshData)
        {
            if(cityName == null) { cityName = ""; }
            cityName = cityName.ToLower();
            DateTime timeNow = DateTime.UtcNow;
            TimeSpan oneHour = new TimeSpan(1, 0, 0);
            var listOfAllElementsInRepo = weatherDataRepository.GetAll();
            var lessThenOneHourElements = listOfAllElementsInRepo.Where(x => x.Time >= (timeNow - oneHour) && x.CityName == cityName);
            var newestItem = lessThenOneHourElements.OrderByDescending(x => x.Time).FirstOrDefault();

            if (newestItem == null || refreshData == true)
            {
                newestItem = await _dataDownload.RefreshWeather(cityName);
                weatherDataRepository.AddData(newestItem);
            }
            return newestItem.WeatherData;
        }
        public IEnumerable<WeatherDataQuery> GetCacheData()
        {
            var listOfAllElementsInRepo = weatherDataRepository.GetAll();
            return listOfAllElementsInRepo.AsEnumerable();
        }
    }
}






