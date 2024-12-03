using OpenWeatherAPP.Models;
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
        private const string WeatherForecastUrl = "https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid={2}&units=metric&lang=pt";
        private const string ApiKey = "01badcc1fa8fb95a73f8eece4a40c8de";

        private readonly HttpClient _httpClient;

        public OpenWeatherService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<WeatherForecastResponse> GetForecast(double latitude, double longitude)
        {
            var url = string.Format(WeatherForecastUrl, latitude, longitude, ApiKey);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Resposta da API: {json}");

                    var weatherForecastResponse = JsonSerializer.Deserialize<WeatherForecastResponse>(json);

                    return weatherForecastResponse;
                }

                Debug.WriteLine($"Erro na API. Status Code: {response.StatusCode}");
                return null;
            }
            catch (SystemException ex)
            {
                throw new Exception($"Erro na requisição dos dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter previsão do tempo: {ex.Message}");
            }
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
                
                Debug.WriteLine($"Erro na API. Status Code: {response.StatusCode}");
                return null;
            }
            catch(SystemException ex)
            {
                throw new Exception($"Erro na requisição dos dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter clima: {ex.Message}");
            }
        }

        public async Task<List<DailyForecast>> GetDailyForecast(double latitude, double longitude)
        {
            var url = string.Format(WeatherForecastUrl, latitude, longitude, ApiKey);

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var forecastResponse = JsonSerializer.Deserialize<WeatherForecastResponse>(json);

                    // Processando a previsão para retornar 10 dias
                    var dailyForecasts = forecastResponse.list.Take(10).Select(f => new DailyForecast
                    {
                        Date = f.dt_txt,
                        Temperature = f.main.temp,
                        Description = f.weather[0].description,
                        IconUrl = $"https://openweathermap.org/img/wn/{f.weather[0].icon}.png"
                    }).ToList();

                    return dailyForecasts;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter previsão do tempo: {ex.Message}");
            }
        }
    }
}
