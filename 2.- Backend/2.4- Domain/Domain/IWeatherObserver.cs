using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace Domain
{
    // Interfaz del observador, es el contrato que la clase Observador debe cumplir
    public interface IWeatherObserver
    {
        // metodo que ejecuta la estación para notificar que hubo un cambio
        // para PUSH el observador usa el parametro reading para actualizar su estado interno
        // para PULL ignora el parametro reading y consulta directamente al sujeto para obtener solo el dato que necesita
        void Update(WeatherReading reading);
    }
}
