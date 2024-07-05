using Microsoft.AspNetCore.Components.Web;
using BlazorDesktop.Hosting;
using BlazorDesktop_CSharp.Components;

namespace BlazorDesktop_CSharp;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = BlazorDesktopHostBuilder.CreateDefault(args);

        builder.RootComponents.Add<Routes>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");

        if (builder.HostEnvironment.IsDevelopment())
        {
            builder.UseDeveloperTools();
        }

        await builder.Build().RunAsync();
    }
}
