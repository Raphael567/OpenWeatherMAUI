﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenWeatherAPP.Models
{
    public class WeatherResponse
    {
        public MainData main { get; set; }
        public Weather[] weather { get; set; }
        public string name { get; set; }
    }

    public class MainData
    {
        public float temp { get; set; }
        public float feels_Like { get; set; }
    }

    public class Weather 
    {
        public string description { get; set; }
    }
}
