using System.ComponentModel;
using System.Windows;

namespace WPFWeatherAPI;

public class MainWindowViewModel : INotifyPropertyChanged
{
    private string _cityName = string.Empty;
    public string CityName {
        get => _cityName;
        set {
            _cityName = value;
            OnPropertyChanged(nameof(CityName));
        }
    }

    public MyCommand GetWeatherCommand { get; set; }

    public MainWindowViewModel()
    {
        var action = () =>
        {
            MessageBox.Show($"Wybrano: {CityName}");
        };
        GetWeatherCommand = new MyCommand(action);
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
