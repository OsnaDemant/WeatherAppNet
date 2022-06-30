using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using WeatherServiceLibrary;
using WeatherServiceLibrary.Common;
using WeatherServiceLibrary.Database;
using WeatherServiceLibrary.Entities;
using WeatherServiceLibrary.Exceptions;

namespace WPFWeatherAPI;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private string _cityName = string.Empty;
    public string CityName
    {
        get => _cityName;
        set
        {
            _cityName = value;
            GetWeatherCommand?.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(CityName));
        }
    }
    private bool refreshCheckBoxIsChecked = false;

    public bool RefreshCheckBoxIsChecked
    {
        get => refreshCheckBoxIsChecked;
        set
        {
            refreshCheckBoxIsChecked = value;
            OnPropertyChanged(nameof(RefreshCheckBoxIsChecked));
        }
    }
    public ObservableCollection<TemperatureScale> TemperatureScalesList => new()
    {
        TemperatureScale.Celsius,
        TemperatureScale.Kelvin,
    };

    private TemperatureScale _selectedTemperatureScale;
    public TemperatureScale SelectedTemperatureScale
    {
        get => _selectedTemperatureScale;
        set
        {
            _selectedTemperatureScale = value;
            OnPropertyChanged(nameof(SelectedTemperatureScale));
        }
    }

    public MyCommand GetWeatherCommand { get; set; }

    private WeatherService _weatherService;

    public MainWindowViewModel()
    {
        InitializeGetWeatherButton();
        InitializeWeatherService();
    }
    private void InitializeGetWeatherButton()
    {
        var action = async () =>
        {
           var weatherData = await _weatherService.GetWeather(CityName, RefreshCheckBoxIsChecked);
           
           MessageBox.Show($"{weatherData.Name} + Weather is {weatherData.GetTemperature(SelectedTemperatureScale)} {SelectedTemperatureScale}");
        };
        var func = () =>
        {
            if (CityName != string.Empty) { return true; }
            return false;
        };
       
        GetWeatherCommand = new MyCommand(action, func);
    }

    private void InitializeWeatherService()
    {
        var config = new ConfigurationBuilder()
               .AddJsonFile("ApiKey.json")
               .AddUserSecrets<MainWindowViewModel>()
               .Build();

        var httpClientFactory = new ServiceCollection()
              .AddHttpClient()
              .BuildServiceProvider()
              .GetService<IHttpClientFactory>();

        WeatherDataRepository weatherDataRepository = new WeatherDataRepository();
        DataDownload dataDownload = new DataDownload(config, httpClientFactory);

        _weatherService = new WeatherService(dataDownload, weatherDataRepository);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
