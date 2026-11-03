using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weather.Services
{

    // Sujeto / Observable
    public class WeatherStationService : IWeatherSubject
    {
        // Lista privada de observadores conectados mediante la interfaz IWeatherObserver
        private readonly List<IWeatherObserver> observers = new();

        // Estado actual de la estacion
        // Propiedad de solo lectura get que guarda el estado actual del clima en la estación.
        // aqui los observadores con enfoque PULL pueden consultar directamente a la estación y sacar solo el dato que necesiten.
        public WeatherReading LecturaActual { get; private set; } = new()
        {
            TemperaturaCelsius = 24.0,
            PorcentajeHumedad = 60.0,
            PresionHpa = 1013.0,
            RecordedAt = DateTime.Now
        };

        // Cantidad de observadores conectados actualmente
        public int ObservadoresConectados => observers.Count;

        // Agrega un observador a la lista si aún no está registrado, suscribe
        public void Attach(IWeatherObserver observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }
        }

        // Quita un observador de la lista para que deje de recibir avisos, desuscribe
        public void Detach(IWeatherObserver observer)
        {
            if (observers.Contains(observer))
            {
                observers.Remove(observer);
            }
        }

        //Recorre todos los observadores suscritos y ejecuta su método Update, notifica
        public void Notify()
        {
            // Se usa el tolist para recorrer los observadores
            foreach (var observer in observers.ToList())
            {
                observer.Update(LecturaActual);
            }
        }

        // Método que dispara el cambio de estado cuando se mueven los sliders o el swagger
        public void SetMeasurements(double tempCelsius, double porcentHumedad, double presionHpa)
        {
            // Actualiza el estado de la estación con los nuevos datos recibidos
            LecturaActual = new WeatherReading
            {
                TemperaturaCelsius = Math.Round(tempCelsius, 1),
                PorcentajeHumedad = Math.Round(porcentHumedad, 1),
                PresionHpa = Math.Round(presionHpa, 1),
                RecordedAt = DateTime.Now
            };

            // Apenas cambia el estado, la estacion avisa automáticamente a todos los observadores suscritos
            Notify();
        }
    }
}
