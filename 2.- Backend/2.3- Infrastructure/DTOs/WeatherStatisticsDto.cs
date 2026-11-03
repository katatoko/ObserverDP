using System;
using System.Collections.Generic;
using System.Text;

namespace DTOs
{
    // DTO que agrupa los datos estadísticos de la estación de clima
    // lo usa el observador StatisticsObserver para poder mostar la información en la pantalla
    public class WeatherStatisticsDto
    {
        public double TemperaturaPromedio { get; set; }
        public double TemperaturaMaxima { get; set; }
        public double TemperaturaMinima { get; set; }
        public int LecturasTotales { get; set; }
    }
}
