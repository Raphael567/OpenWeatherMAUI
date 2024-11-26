﻿using OpenWeatherAPP.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace OpenWeatherAPP.Services
{
    public class OpenWeatherService
    {
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";
        private const string ApiKey = "CHAVE_API";

        private readonly HttpClient _httpClient;

        public OpenWeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherResponse> GetWeather(string cidade)
        {
            var url = $"{BaseUrl}?q={cidade}&appid={ApiKey}&units=metric&lang=pt";

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Resposta da API: {json}");

                    var weatherResponse = JsonSerializer.Deserialize<WeatherResponse>(json);
                    Debug.WriteLine($"Description: {weatherResponse?.weather?[0]?.description}");
                    Debug.WriteLine($"Temperature: {weatherResponse?.main?.temp}");

                    return weatherResponse;
                }

                throw new Exception($"Erro na API. Status Code: {response.StatusCode}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter clima: {ex.Message}");
            }
        }
    }
}
