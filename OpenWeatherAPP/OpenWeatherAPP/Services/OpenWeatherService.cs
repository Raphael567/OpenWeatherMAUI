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
            HttpClientHandler handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true
            };

            _httpClient = new HttpClient(handler);
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

        public async Task<List<DailyForecast>> Get5DayForecast(double latitude, double longitude)
        {
            var detailedForecast = await GetForecast(latitude, longitude);

            if (detailedForecast == null || detailedForecast.list == null)
                return null;

            var groupedByDay = detailedForecast.list
                .GroupBy(item => DateTime.Parse(item.dt_txt).Date)
                .Take(5);

            var dailyForecasts = groupedByDay.Select(dayGroup =>
            {
                var minTemp = dayGroup.Min(item => item.main.temp_min);
                var maxTemp = dayGroup.Max(item => item.main.temp_max);

                var weather = dayGroup.First().weather;

                return new DailyForecast
                {
                    dt = new DateTimeOffset(dayGroup.Key).ToUnixTimeSeconds(),
                    temp = new Temp
                    {
                        min = minTemp,
                        max = maxTemp
                    },
                    weather = weather
                };
            }).ToList();

            return dailyForecasts;
        }
    }
}
