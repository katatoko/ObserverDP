using Domain;
using DTOs;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weather.Services
{
    // Agrupamiento de Observadores en una clase

    // Observador 1 -> Condiciones actuales - Enfoque PUSH
    // Recibe el objeto WeatherReading directamente desde la estación o sujeto
    public class CurrentConditionsObserver : IWeatherObserver
    {
        // guarda la ultima lectura recibida desde la estación, para mostrarla en el dashboard
        // pero solo esta clase puede modificar
        public WeatherReading UltimaLectura { get; private set; } = new()
        {
            TemperaturaCelsius = 24.0,
            PorcentajeHumedad = 60.0,
            PresionHpa = 1013.0,
            RecordedAt = DateTime.Now
        };
        public int ActualizacionesRecibidas { get; private set; }
        public void Update(WeatherReading reading)
        {
            // Enfoque PUSH: Usa directamente el paquete de datos enviado por la estación
            UltimaLectura = reading;
            ActualizacionesRecibidas++;
        }
    }

    // Observador 2 -> Estadísticas - Enfoque PUSH
    // Acumula el historial de temperaturas y calcula max, min y promedio de las temperaturas recibidas
    public class StatisticsObserver : IWeatherObserver
    {
        //Lista que guarda el historial de las temperaturas recibidas
        private readonly List<double> historialTemperaturas = new();

        // dto que contiene el promedio, temp maxima, temp minima y cantidad de lecturas recibidas
        public WeatherStatisticsDto Estadisticas { get; private set; } = new();
        public void Update(WeatherReading reading)
        {
            // Enfoque PUSH: Toma la temperatura del parametro empujado por el sujeto
            historialTemperaturas.Add(reading.TemperaturaCelsius);

            // reclacula las estadisticas cada vez que llega una nueva lectura
            Estadisticas = new WeatherStatisticsDto
            {
                TemperaturaPromedio = Math.Round(historialTemperaturas.Average(), 1),
                TemperaturaMaxima = historialTemperaturas.Max(),
                TemperaturaMinima = historialTemperaturas.Min(),
                LecturasTotales = historialTemperaturas.Count
            };
        }
    }

    // Observador 3 -> Pronóstico - Enfoque PULL
    // Guarda referencias para solo extraer la presión atmosférica y calcular el pronóstico
    public class ForecastObserver : IWeatherObserver
    {
        // guarda una referencia al sujeto para poder consultarle la presión atmosférica cuando haya aviso de cambio
        private readonly IWeatherSubject station;
        public double PresionPrevia { get; private set; } = 1013.0;
        public double PresionActual { get; private set; } = 1013.0;
        public string MensajePronostico { get; private set; } = "Condiciones iniciales estables ⛅";
        public int ActualizacionesRecibidas { get; private set; }

        // Recibe el Sujeto por constructor para poder consultarle (PULL) cuando haya aviso
        public ForecastObserver(IWeatherSubject station)
        {
            this.station = station;
        }
        
        public void Update(WeatherReading reading)
        {
            // Enfoque PULL: En lugar de depender del parámetro empujado,
            // el observador consulta directamente al sujeto solo el dato que necesita (Presión)
            PresionPrevia = PresionActual;
            PresionActual = station.LecturaActual.PresionHpa;
            ActualizacionesRecibidas++;

            if (PresionActual > PresionPrevia)
            {
                MensajePronostico = "Mejorando el clima: cielo despejado y soleado ☀️ (Presión en ascenso)";
            }
            else if (PresionActual < PresionPrevia)
            {
                MensajePronostico = "Se aproxima lluvia o frente frío 🌧️ (Presión en descenso)";
            }
            else
            {
                MensajePronostico = "Sin cambios: se mantienen las condiciones actuales ⛅ (Presión estable)";
            }
        }
    }
}
