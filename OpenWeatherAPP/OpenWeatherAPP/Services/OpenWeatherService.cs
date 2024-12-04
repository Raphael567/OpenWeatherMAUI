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
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}&units=metric&lang=en";
        private const string WeatherForecastUrl = "https://api.openweathermap.org/data/2.5/forecast?lat={0}&lon={1}&appid={2}&units=metric&lang=en";

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

        public async Task<T> GetApiResponseAsync<T>(string url)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Resposta da API: {json}");

                    return JsonSerializer.Deserialize<T>(json);
                }

                Debug.WriteLine($"Erro na API. Status Code: {response.StatusCode}");
                return default;
            }
            catch (SystemException ex)
            {
                throw new Exception($"Erro na requisição dos dados: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao obter dados: {ex.Message}");
            }
        }

        public async Task<WeatherResponse> GetWeather(string cidade)
        {
            var url = string.Format(BaseUrl, cidade, ApiKey);
            return await GetApiResponseAsync<WeatherResponse>(url);
        }

        public async Task<WeatherForecastResponse> GetHourlyForecast(double latitude, double longitude)
        {
            var url = string.Format(WeatherForecastUrl, latitude, longitude, ApiKey);
            return await GetApiResponseAsync<WeatherForecastResponse>(url);
        }

        public async Task<WeatherForecastResponse> GetForecast(double latitude, double longitude)
        {
            var url = string.Format(WeatherForecastUrl, latitude, longitude, ApiKey);
            var forecastResponse = await GetApiResponseAsync<WeatherForecastResponse>(url);

            if (forecastResponse?.list != null)
            {
                var currentTimestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

                forecastResponse.list = forecastResponse.list
                    .Where(forecast =>
                        forecast.dt >= currentTimestamp &&
                        forecast.dt <= currentTimestamp + (24 * 60 * 60))
                    .ToList();
            }

            return forecastResponse;
        }

        public async Task<List<DailyForecast>> Get5DayForecast(double latitude, double longitude)
        {
            var detailedForecast = await GetApiResponseAsync<WeatherForecastResponse>(string.Format(WeatherForecastUrl, latitude, longitude, ApiKey));

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
