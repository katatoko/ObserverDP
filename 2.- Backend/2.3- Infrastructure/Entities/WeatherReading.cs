using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    // molde o tipo de dato que agrupa los 4 datos del clima
    public class WeatherReading
    {
        public double TemperaturaCelsius { get; set; }
        public double PorcentajeHumedad { get; set; }
        public double PresionHpa { get; set; }
        public DateTime RecordedAt { get; set; } = DateTime.Now;
    }
}
