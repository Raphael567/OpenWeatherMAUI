using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenWeatherAPP.Models
{
    public class DailyForecast
    {
        public string Date { get; set; }
        public double Temperature { get; set; }
        public string Description { get; set; }
        public string IconUrl { get; set; }
    }
}
