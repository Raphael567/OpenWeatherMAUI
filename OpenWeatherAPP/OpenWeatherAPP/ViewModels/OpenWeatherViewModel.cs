﻿using CommunityToolkit.Mvvm.ComponentModel;
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
            LoadLocationDataAsync();
        }

        private string entryCidade;

        [ObservableProperty]
        private string _entryCidadeDisplay;

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
        private ObservableCollection<ForecastItem> _hourlyForecastList = new ObservableCollection<ForecastItem>();

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

                if (!string.IsNullOrWhiteSpace(EntryCidadeDisplay))
                {
                    entryCidade = EntryCidadeDisplay;
                    EntryCidadeDisplay = string.Empty;

                    var weatherData = await _service.GetWeather(entryCidade);

                    if (weatherData?.weather != null)
                    {
                        await LoadWeatherData(entryCidade);
                        await LoadHourlyForecastData(weatherData.coord.lat, weatherData.coord.lon);
                        await LoadForecastData(weatherData.coord.lat, weatherData.coord.lon);
                        await LoadDailyForecastData(weatherData.coord.lat, weatherData.coord.lon);
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Erro⚠", "Local não encontrado", "OK");
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Erro⚠", "Digite o nome do local", "OK");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                await Application.Current.MainPage.DisplayAlert("Erro⚠", "Erro ao buscar dados", "OK");
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
                Descricao = weatherData.weather[0].FormattedDescription;
                Temperatura = weatherData.main.temp;
                TemperaturaMinima = weatherData.main.temp_min;
                TemperaturaMaxima = weatherData.main.temp_max;
            }
        }

        private async Task LoadHourlyForecastData(double latitude, double longitude)
        {
            var hourlyForecastData = await _service.GetHourlyForecast(latitude, longitude);

            if (hourlyForecastData != null)
            {
                HourlyForecastList.Clear();
                foreach (var hourlyForecast in hourlyForecastData.list)
                {
                    HourlyForecastList.Add(hourlyForecast);
                }
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

        private async Task LoadLocationDataAsync()
        {
            try
            {
                var location = await Geolocation.GetLastKnownLocationAsync();

                if (location != null)
                {
                    IEnumerable<Placemark> placemarks = await Geocoding.GetPlacemarksAsync(location.Latitude, location.Longitude);

                    if (placemarks != null)
                    {
                        Placemark placemark = placemarks?.FirstOrDefault();
                        if (placemark != null)
                        {
                            EntryCidadeDisplay = placemark.AdminArea;
                            await GetWeatherAsync();
                        }
                    }
                }
                else
                {
                    location = await Geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.High,
                        Timeout = TimeSpan.FromSeconds(10)
                    });
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }
    }
}
