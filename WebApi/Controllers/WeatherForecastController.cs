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
        private readonly ILogger<WeatherForecastController> logger;
        private readonly IWeatherService cityWeatherService;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherService weatherService)
        {
            this.logger = logger;
            cityWeatherService = weatherService;
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
                logger.LogError(e, "user was not authorized");
                return Unauthorized();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Weather couldnt be downloaded.");
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
                logger.LogError(e, "user was not authorized");
                return Unauthorized();
            }
            catch (Exception e)
            {
                logger.LogError(e, "Weather couldnt be downloaded.");
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

            dataCache = cityWeatherService.GetCacheData();

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
        private TemperatureScale GetTypeScaleForTemperature(string temperature)
        {
            TemperatureScale scaleTemperature;
            switch (temperature?.Substring(0, 1).ToUpper())
            {
                case "K":
                    scaleTemperature = TemperatureScale.Kelvin;
                    break;

                case "C":
                    scaleTemperature = TemperatureScale.Celsius;
                    break;

                case "F":
                    scaleTemperature = TemperatureScale.Fahrenheit;
                    break;

                default:
                    scaleTemperature = TemperatureScale.Kelvin;
                    break;
            }

            return scaleTemperature;
        }

        private TemperatureScale Test(string temperature)
            => temperature?.Substring(0, 1).ToUpper() switch
            {
                "F" => TemperatureScale.Kelvin,
                "C" => TemperatureScale.Celsius,
                _ => TemperatureScale.Kelvin
            };
    }
}