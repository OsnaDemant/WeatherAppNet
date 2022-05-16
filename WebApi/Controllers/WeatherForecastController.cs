using Microsoft.AspNetCore.Mvc;
using WeatherServiceLibrary;
using WeatherServiceLibrary.Common;
using WeatherServiceLibrary.Entities;
using WeatherServiceLibrary.Exceptions;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration config;
        private readonly WeatherService cityWeatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IConfiguration config)
        {
            _logger = logger;
            this.config = config;
            cityWeatherService = new WeatherService();
            var apiKey = this.config["ApiKey"];
            cityWeatherService.Initialize(apiKey);

        }


        [HttpGet("{cityName}")]
        public async Task<ActionResult<WeatherData>> GetCurentWeather(string cityName)
        {

            WeatherData currentWeather;
            // DataBaseFunction.AddData();
            try
            {
                currentWeather = await cityWeatherService.GetWeather(cityName, false);
            }
            catch (UnauthorizedException e)
            {
                _logger.LogError(e, "user was not authorized");
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Weather couldnt be downloaded.");
                return NotFound();
            }

            if (currentWeather == null)
            {
                return NotFound();
            }
            else
            {

                return Ok(currentWeather);
            }

        }
        [HttpGet("{cityName}/temperature")]
        public async Task<ActionResult<float>> GetTemperature(string cityName, string scaleTemperature, bool refresh)
        {

            WeatherData currentWeather;
            // DataBaseFunction.AddData();
            try
            {

                currentWeather = await cityWeatherService.GetWeather(cityName, refresh);
            }
            catch (UnauthorizedException e)
            {
                _logger.LogError(e, "user was not authorized");
                return Unauthorized();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Weather couldnt be downloaded.");
                return NotFound();
            }

            if (currentWeather == null)
            {
                return NotFound();
            }
            else
            {

                var typeOfTemperature = GetTypeScaleForTemperature(scaleTemperature);
                var curentTemperature = currentWeather.GetTemperature(typeOfTemperature);
                return Ok(curentTemperature);
            }
        }
        [HttpGet("/cache")]
        public ActionResult<IEnumerable<WeatherDataQuery>> GetCache()
        {
            IEnumerable<WeatherDataQuery> dataCache;
            WeatherService weatherService = new WeatherService();

            dataCache = weatherService.GetCacheData();

            if (dataCache == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(dataCache);
            }

        }
        //to do asp.net core mvc read
        //to do null handling
        static public TemperatureScale GetTypeScaleForTemperature(string temperature)
        {
            TemperatureScale scaleTemperature = TemperatureScale.Fahrenheit;

            switch (temperature.Substring(0, 1).ToUpper())
            {
                case "F":
                    scaleTemperature = TemperatureScale.Fahrenheit;
                    break;

                case "C":
                    scaleTemperature = TemperatureScale.Celsius;
                    break;
            }
            return scaleTemperature;
        }
        static public string CoverSpaceInCityName(string cityName)
        {
            cityName = cityName.Replace(" ", "%20");

            return cityName;
        }
    }
}