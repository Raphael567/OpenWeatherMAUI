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
            //OnLoadImageCommand = new RelayCommand(OnLoadImageAsync);
        }

        [ObservableProperty]
        private string _entryCidade = "São Paulo";

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

        [ObservableProperty]
        private ObservableCollection<DailyForecast> _dailyForecastList = new ObservableCollection<DailyForecast>();

        [ObservableProperty]
        private Image myImage;

        //public IRelayCommand OnLoadImageCommand { get; }

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

            //private async void OnLoadImageAsync()
            //{
            //    await LoadImageManually();
            //}

            //async Task LoadImageManually()
            //{
            //    try
            //    {
            //        var handler = new HttpClientHandler()
            //        {
            //            SslProtocols = System.Security.Authentication.SslProtocols.Tls12 |
            //                           System.Security.Authentication.SslProtocols.Tls13
            //        };

            //        using var client = new HttpClient(handler);
            //        var response = await client.GetAsync("https://openweathermap.org/img/wn/10d@2x.png");

            //        if (response.IsSuccessStatusCode)
            //        {
            //            var stream = await response.Content.ReadAsStreamAsync();
            //            var imageSource = ImageSource.FromStream(() => stream);
            //            MyImage.Source = ImageSource.FromStream(() => stream);
            //        }
            //        else
            //        {
            //            Console.WriteLine($"Erro ao carregar imagem: {response.StatusCode}");
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"Erro: {ex.Message}");
            //    }
            //}
        }
    }
}
