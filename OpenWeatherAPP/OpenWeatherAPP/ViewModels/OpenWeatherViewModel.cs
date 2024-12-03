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
            GetWeatherAsync();
        }

        [ObservableProperty]
        private string _entryCidade;

        [ObservableProperty]
        private string _cidade;

        [ObservableProperty]
        private string _descricao;

        [ObservableProperty]
        private double _temperatura;

        [ObservableProperty]
        private double _temperaturaMinima;

        [ObservableProperty]
        private double _temperaturaMaxima;

        [ObservableProperty]
        private ObservableCollection<ForecastItem> _forecastList = new ObservableCollection<ForecastItem>();

        [ObservableProperty]
        private ObservableCollection<DailyForecast> _dailyForecastList = new ObservableCollection<DailyForecast>();

        [RelayCommand]
        private async Task GetWeatherAsync()
        {
            if (!string.IsNullOrWhiteSpace(EntryCidade))
            {
                var weatherData = await _service.GetWeather(EntryCidade);

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

                var dailyForecastData = await _service.Get5DayForecast(weatherData.coord.lat, weatherData.coord.lon);

                if (dailyForecastData != null)
                {
                    DailyForecastList.Clear();
                    foreach (var forecast in dailyForecastData)
                    {
                        DailyForecastList.Add(forecast);
                    }
                }
            }
        }
    }
}
