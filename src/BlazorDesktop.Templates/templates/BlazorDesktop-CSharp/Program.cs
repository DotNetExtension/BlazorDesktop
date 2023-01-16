using BlazorDesktop_CSharp;
using BlazorDesktop_CSharp.Data;
using BlazorDesktop.Hosting;
using Microsoft.AspNetCore.Components.Web;

var builder = BlazorDesktopHostBuilder.CreateDefault(args);

builder.UseWebViewInstaller();

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<WeatherForecastService>();

if (builder.HostEnvironment.IsDevelopment())
{
    builder.UseDeveloperTools();
}

await builder.Build().RunAsync();
