using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain
{
    public interface IWeatherSubject
    {
        // Expone la lectura actual para soportar el enfoque pull, cuando el observador quiere consultar solo su dato correspondiente
        WeatherReading CurrentReading { get; }

        // Suscribe o conecta un nuevo observador a la estacion
        void Attach(IWeatherObserver observer);

        // Desuscribe un observador para que deje de recibir notificaciones
        void Detach(IWeatherObserver observer);

        // Notifica a todos los observadores suscritos que hubo un cambio de clima
        void Notify();
    }
}
