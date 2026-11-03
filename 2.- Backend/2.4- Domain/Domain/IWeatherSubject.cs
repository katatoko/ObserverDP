using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    // Interfaz del sujeto, es el contrato que la clase Sujeto debe cumplir
    public interface IWeatherSubject
    {
        // Propiedad de solo lectura get que guarda el estado actual del clima en la estación.
        // Está aquí en la interfaz para que los observadores con enfoque PULL (como ForecastObserver)
        // puedan consultar directamente a la estación (station.LecturaActual) y sacar solo el dato que necesiten.
        WeatherReading LecturaActual { get; }

        // Suscribe o conecta un nuevo observador a la estacion
        void Attach(IWeatherObserver observer);

        // Desuscribe un observador para que deje de recibir notificaciones
        void Detach(IWeatherObserver observer);

        // Notifica a todos los observadores suscritos que hubo un cambio de clima
        void Notify();
    }
}
