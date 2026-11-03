using Domain;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using ObserverDP;
using Weather.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Registra el Sujeto (Estación) y los 3 Observadores como Singleton en el Frontend
// para que la página Home.razor pueda usarlos e interactuar en vivo
builder.Services.AddSingleton<WeatherStationService>();
builder.Services.AddSingleton<IWeatherSubject>(sp => sp.GetRequiredService<WeatherStationService>());

builder.Services.AddSingleton<CurrentConditionsObserver>();
builder.Services.AddSingleton<StatisticsObserver>();
builder.Services.AddSingleton<ForecastObserver>();

await builder.Build().RunAsync();