using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    public class WeatherStatisticsDto
    {
        public double AverageTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public double MinTemperature { get; set; }
        public int TotalReadings { get; set; }
    }
}
