using Domain;
using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Weather.Services
{
    public class WeatherStationService : IWeatherSubject
    {
        // Lista privada de observadores conectados mediante la interfaz IWeatherObserver
        private readonly List<IWeatherObserver> _observers = new();

        // Estado actual de la estacion, permite también el enfoque pull
        public WeatherReading CurrentReading { get; private set; } = new()
        {
            TemperatureCelsius = 24.0,
            HumidityPercent = 60.0,
            PressureHpa = 1013.0,
            RecordedAt = DateTime.Now
        };

        // Cantidad de observadores conectados actualmente
        public int ConnectedObserversCount => _observers.Count;

        // Agrega un observador a la lista si aún no está registrado, suscribe
        public void Attach(IWeatherObserver observer)
        {
            if (!_observers.Contains(observer))
            {
                _observers.Add(observer);
            }
        }

        // Quita un observador de la lista para que deje de recibir avisos, desuscribe
        public void Detach(IWeatherObserver observer)
        {
            if (_observers.Contains(observer))
            {
                _observers.Remove(observer);
            }
        }

        //Recorre todos los observadores suscritos y ejecuta su método Update, notifica
        public void Notify()
        {
            // Se usa el tolist para recorrer una copia segura de la colección
            foreach (var observer in _observers.ToList())
            {
                observer.Update(CurrentReading);
            }
        }

        // Método que dispara el cambio de estado cuando se mueven los sliders o el simulador
        public void SetMeasurements(double temperatureCelsius, double humidityPercent, double pressureHpa)
        {
            CurrentReading = new WeatherReading
            {
                TemperatureCelsius = Math.Round(temperatureCelsius, 1),
                HumidityPercent = Math.Round(humidityPercent, 1),
                PressureHpa = Math.Round(pressureHpa, 1),
                RecordedAt = DateTime.Now
            };

            // Apenas cambia el estado, la estacion avisa automáticamente a todos los observadores
            Notify();
        }
    }
}
