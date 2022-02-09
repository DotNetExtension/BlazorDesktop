using BlazorDesktop.Data;
using BlazorDesktop.Hosting;
using BlazorDesktop.Sample;
using Microsoft.AspNetCore.Components.Web;

var builder = BlazorDesktopHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<WeatherForecastService>();

builder.Window.UseFrame(false);

await builder.Build().RunAsync();
