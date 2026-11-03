using Domain;
using Weather.Services;

// Inicializa el configurador de la aplicación web
var builder = WebApplication.CreateBuilder(args);

// Habilita el soporte para Controladores ([ApiController]) y el generador visual de Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registra el Sujeto y los 3 Observadores como Singleton
// lo hace en memoria hasta que se detenga la ejecución del programa para ver cómo cambian en vivo
builder.Services.AddSingleton<WeatherStationService>();

// si una clase pide la interfaz IWeatherSubject, se le inyecta la instancia de WeatherStationService
builder.Services.AddSingleton<IWeatherSubject>(sp => sp.GetRequiredService<WeatherStationService>());

builder.Services.AddSingleton<CurrentConditionsObserver>();
builder.Services.AddSingleton<StatisticsObserver>();
builder.Services.AddSingleton<ForecastObserver>();

// Configuracion de CORS permitiendo al frontend llamar la API
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// construye aplicacion con las configs especificacdas anteriormente
var app = builder.Build();

//Cunado inicia el servidor, se suscriben inicialmente los 3 observadores a la estación
using (var scope = app.Services.CreateScope())
{
    var station = scope.ServiceProvider.GetRequiredService<WeatherStationService>();
    var conditions = scope.ServiceProvider.GetRequiredService<CurrentConditionsObserver>();
    var stats = scope.ServiceProvider.GetRequiredService<StatisticsObserver>();
    var forecast = scope.ServiceProvider.GetRequiredService<ForecastObserver>();

    station.Attach(conditions);
    station.Attach(stats);
    station.Attach(forecast);

    // Se dispara una lectura inicial para que todos arranquen sincronizados y con datos
    station.SetMeasurements(24.0, 60.0, 1013.0);
}

// Activar la pantalla visual de Swagger siempre
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Weather Observer API v1");
});

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();

// Conecta las rutas HTTP con los métodos de WeatherController
app.MapControllers();

//Inicia el servidor
app.Run();