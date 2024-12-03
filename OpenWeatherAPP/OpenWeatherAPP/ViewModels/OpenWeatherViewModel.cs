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
        private double _temperatura;

        [ObservableProperty]
        private double _temperaturaMinima;

        [ObservableProperty]
        private double _temperaturaMaxima;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private ObservableCollection<ForecastItem> _forecastList = new ObservableCollection<ForecastItem>();

        [ObservableProperty]
        private ObservableCollection<DailyForecast> _dailyForecastList = new ObservableCollection<DailyForecast>();

        [RelayCommand]
        private async Task GetWeatherAsync()
        {
            try
            {
                IsLoading = true;

                if (!string.IsNullOrWhiteSpace(EntryCidade))
                {
                    var weatherData = await _service.GetWeather(EntryCidade);

                    if (weatherData?.weather != null)
                    {
                        await LoadWeatherData(EntryCidade);
                        await LoadForecastData(weatherData.coord.lat, weatherData.coord.lon);
                        await LoadDailyForecastData(weatherData.coord.lat, weatherData.coord.lon);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro", "Local não encontrado", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro", "Digite o nome do local", "OK");
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Erro", "Erro ao buscar dados", "OK");
            }
            finally
            {
                IsLoading = false;
            }
        }

        private async Task LoadWeatherData(string cidade)
        {
            var weatherData = await _service.GetWeather(cidade);

            if (weatherData != null)
            {
                Cidade = weatherData.name;
                Descricao = weatherData.weather[0].description;
                Temperatura = weatherData.main.temp;
                TemperaturaMinima = weatherData.main.temp_min;
                TemperaturaMaxima = weatherData.main.temp_max;
            }
        }

        private async Task LoadForecastData(double latitude, double longitude)
        {
            var forecastData = await _service.GetForecast(latitude, longitude);

            if (forecastData != null)
            {
                ForecastList.Clear();
                foreach (var forecast in forecastData.list)
                {
                    ForecastList.Add(forecast);
                }
            }
        }

        private async Task LoadDailyForecastData(double latitude, double longitude)
        {
            var dailyForecastData = await _service.Get5DayForecast(latitude, longitude);

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
