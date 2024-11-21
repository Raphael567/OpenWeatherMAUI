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
        private string _cidade;

        [ObservableProperty]
        private string _descricao;

        [ObservableProperty]
        private float _temperatura;

        [RelayCommand]
        private async Task GetWeatherAsync()
        {
            if (!string.IsNullOrWhiteSpace(Cidade))
            {
                var weatherData = await _service.GetWeather(Cidade);

                Debug.WriteLine($"Weather: {weatherData.weather}");
                Debug.WriteLine($"Description: {weatherData.weather?[0]?.description}");
                Debug.WriteLine($"Temperature: {weatherData.main?.temp}");

                if (weatherData?.weather != null)
                {
                    Descricao = weatherData.weather[0].description;
                    Temperatura = weatherData.main.temp;
                }
                else
                {
                    Descricao = "Dados não disponíveis";
                    Temperatura = 0;
                }
            }
        }
    }
}
