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
            GetWeatherCommand?.RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(CityName));
        }
    }

    public MyCommand GetWeatherCommand { get; set; }
    public My

    public MainWindowViewModel()
    {
        var action = () =>
        {
            MessageBox.Show($"Wybrano: {CityName}");
        };
        var func = () =>
        {
        
            if (CityName != string.Empty) { return true; }
            return false;
            
        };
        
        GetWeatherCommand = new MyCommand(action,func);
    }



    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
