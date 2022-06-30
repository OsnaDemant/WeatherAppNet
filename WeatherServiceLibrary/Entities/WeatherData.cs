using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherServiceLibrary.Common;

namespace WeatherServiceLibrary.Entities;

public class WeatherData
{

    public float GetTemperature(TemperatureScale temperatureTypeScale)
    {
        return temperatureTypeScale switch
        {
            TemperatureScale.Celsius => WeatherHelper.KelvinToCelsius(Main.Temp),
            TemperatureScale.Kelvin => Main.Temp,
            _ => throw new NotImplementedException("Invalid Temperature Scale"),
        };
    }
    [JsonIgnore]
    public int IdWeatherData { get; set; }
    public Coord Coord { get; set; }
    public Weather[] Weather { get; set; }
    public string _base { get; set; }
    public Main Main { get; set; }

    public int Visibility { get; set; }
    public Wind Wind { get; set; }
    public Clouds Clouds { get; set; }
    public int Dt { get; set; }
    public Sys Sys { get; set; }
    public int Timezone { get; set; }
    public int Id { get; set; }
    public string Name { get; set; }
    public int Cod { get; set; }
}

public class Coord
{
   
    public int Id { get; set; }
    public float Lon { get; set; }
    public float Lat { get; set; }
}

public class Main
{
    [JsonIgnore]
    public int Id { get; set; }
    public float Temp { get; set; }
    public float Feels_like { get; set; }
    public float Temp_min { get; set; }
    public float Temp_max { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
}

public class Wind
{
    [JsonIgnore]
    public int Id { get; set; }
    public float Speed { get; set; }
    public int Deg { get; set; }
}

public class Clouds
{
    [JsonIgnore]
    public int Id { get; set; }
    public int All { get; set; }
}

public class Sys
{
    [JsonIgnore]
    public int DataBaseId { get; set; }
    public int Type { get; set; }
    public int Id { get; set; }
    public string Country { get; set; }
    public int Sunrise { get; set; }
    public int Sunset { get; set; }
}

public class Weather
{
    public int Id { get; set; }
    public string Main { get; set; }
    public string Description { get; set; }
    public string Icon { get; set; }
}
