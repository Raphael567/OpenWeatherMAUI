using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using OpenWeatherAPP.Models;
using OpenWeatherAPP.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherAPP.ViewModels
{
    public partial class OpenWeatherViewModel : ObservableObject
    {
        private readonly OpenWeatherService _service;

        public OpenWeatherViewModel()
        {
            _service = new OpenWeatherService();
        }

        [ObservableProperty]
        private string _entryCidade;

        [ObservableProperty]
        private string _cidade;

        [ObservableProperty]
        private string _descricao;

        [ObservableProperty]
        private float _temperatura;

        [ObservableProperty]
        private float _temperaturaMinima;

        [ObservableProperty]
        private float _temperaturaMaxima;

        [ObservableProperty]
        private ObservableCollection<ForecastItem> _forecastList = new ObservableCollection<ForecastItem>();

        [RelayCommand]
        private async Task GetWeatherAsync()
        {
            if (!string.IsNullOrWhiteSpace(EntryCidade))
            {
                var weatherData = await _service.GetWeather(EntryCidade);

                //Debug.WriteLine($"Weather: {weatherData.weather}");
                //Debug.WriteLine($"Description: {weatherData.weather?[0]?.description}");
                //Debug.WriteLine($"Temperature: {weatherData.main?.temp}");

                if (weatherData?.weather != null)
                {
                    Cidade = weatherData.name;
                    Descricao = weatherData.weather[0].description;
                    Temperatura = weatherData.main.temp;
                    TemperaturaMinima = weatherData.main.temp_min;
                    TemperaturaMaxima = weatherData.main.temp_max;
                }
                else
                {
                    Descricao = "Cidade não encontrada";
                    Temperatura = 0;
                }

                var forecastData = await _service.GetForecast(weatherData.coord.lat, weatherData.coord.lon);

                if (forecastData?.list != null)
                {
                    ForecastList.Clear();
                    foreach (var forecast in forecastData.list)
                    {
                        ForecastList.Add(forecast);
                    }
                }
            }
        }
    }
}
