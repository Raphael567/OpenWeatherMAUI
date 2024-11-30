﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherAPP.Models
{
    public class WeatherForecastResponse
    {
        public string cod { get; set; }
        public int message { get; set; }
        public int count { get; set; }
        public List<ForecastItem> list { get; set; }
        public City city { get; set; }
    }

    public class ForecastItem
    {
        public long dt { get; set; }
        public MainData main { get; set; }
        public List<Weather> weather { get; set; }
        public string dt_txt { get; set; }
    }

    public class City
    {
        public string name { get; set; }
        public string country { get; set; }
    }
}
