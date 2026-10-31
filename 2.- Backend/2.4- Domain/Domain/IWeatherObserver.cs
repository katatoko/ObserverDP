using System;
using System.Collections.Generic;
using System.Text;
using Entities;

namespace Domain
{
    // para implementarlo en cualquier componente que necesite usar la interface, es el sensor
    public interface IWeatherObserver
    {
        // se maneja push, el sujero arroja el objeto con datos nuevos
        // se maneja pull por parte del observador puede consultar el sujero cuando recibe datos
        void Update(WeatherReading reading);
    }
}
