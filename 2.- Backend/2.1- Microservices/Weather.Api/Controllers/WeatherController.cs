using Microsoft.AspNetCore.Mvc;
using Weather.Services;

namespace Weather.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherController : ControllerBase
    {
        // Referencias privadas al Sujeto (estación) y a los 3 Observadores que viven en memoria
        private readonly WeatherStationService station;
        private readonly CurrentConditionsObserver conditionsObserver;
        private readonly StatisticsObserver statisticsObserver;
        private readonly ForecastObserver forecastObserver;

        // Constructor con intyeccion de dependencias para recibir las instancias de la estación y los observadores
        // Recibe las instancias únicas, los singletons configurados en Program
        public WeatherController(
            WeatherStationService station,
            CurrentConditionsObserver conditionsObserver,
            StatisticsObserver statisticsObserver,
            ForecastObserver forecastObserver)
        {
            this.station = station;
            this.conditionsObserver = conditionsObserver;
            this.statisticsObserver = statisticsObserver;
            this.forecastObserver = forecastObserver;
        }

        // Consulta el estado actual del Sujeto y de los 3 Observadores
        [HttpGet("estado")]
        public IActionResult GetStatus()
        {
            // muestra que valores tiene la estacion y cada observador
            return Ok(new
            {
                // sujeto o estacion meteorologica
                StationSubject = new
                {
                    station.ObservadoresConectados,
                    station.LecturaActual
                },
                // observadores
                Observers = new
                {
                    CurrentConditions_Push = new
                    {
                        conditionsObserver.ActualizacionesRecibidas,
                        conditionsObserver.UltimaLectura
                    },
                    Statistics_Push = statisticsObserver.Estadisticas,
                    Forecast_Pull = new
                    {
                        forecastObserver.ActualizacionesRecibidas,
                        forecastObserver.PresionPrevia,
                        forecastObserver.PresionActual,
                        forecastObserver.MensajePronostico
                    }
                }
            });
        }

        // Envia nuevas mediciones a la estación
        [HttpPost("mediciones")]
        public IActionResult SetMeasurements(double temperatureCelsius = 28.5, double humidityPercent = 65.0, double pressureHpa = 1008.0)
        {
            // el post cambia los datos en la estacion
            // la estacion se encarga de notificar a los observadores suscritos
            station.SetMeasurements(temperatureCelsius, humidityPercent, pressureHpa);

            return Ok(new
            {
                Message = "Mediciones actualizadas en la estación. Se notificó a los observadores suscritos.",
                NewReading = station.LecturaActual,
                ConnectedObservers = station.ObservadoresConectados
            });
        }

        //Suscribe un observador
        [HttpPost("attach/{observerName}")]
        public IActionResult AttachObserver(string observerName)
        {
            // Normaliza el texto a minúsculas y sin espacios para identificar qué observador conectar
            switch (observerName.ToLower().Trim())
            {
                case "conditions":
                    station.Attach(conditionsObserver);
                    break;
                case "statistics":
                    station.Attach(statisticsObserver);
                    break;
                case "forecast":
                    station.Attach(forecastObserver);
                    break;
                default:
                    return BadRequest("Nombre inválido. Usa: 'conditions', 'statistics' o 'forecast'.");
            }

            return Ok(new
            {
                Message = $"Observador '{observerName}' SUSCRITO (Attach) exitosamente.",
                ConnectedObservers = station.ObservadoresConectados
            });
        }

        //Desuscribe un observador
        [HttpPost("detach/{observerName}")]
        public IActionResult DetachObserver(string observerName)
        {
            switch (observerName.ToLower().Trim())
            {
                case "conditions":
                    station.Detach(conditionsObserver);
                    break;
                case "statistics":
                    station.Detach(statisticsObserver);
                    break;
                case "forecast":
                    station.Detach(forecastObserver);
                    break;
                default:
                    return BadRequest("Nombre inválido. Usa: 'conditions', 'statistics' o 'forecast'.");
            }

            return Ok(new
            {
                Message = $"Observador '{observerName}' DESUSCRITO (Detach). Ya no recibirá notificaciones.",
                ConnectedObservers = station.ObservadoresConectados
            });
        }
    }
}