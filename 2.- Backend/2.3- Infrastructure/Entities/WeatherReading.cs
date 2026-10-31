using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    public class WeatherReading
    {
        public double TemperatureCelsius { get; set; }
        public double HumidityPercent { get; set; }
        public double PressureHpa { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.Now;
    }
}
