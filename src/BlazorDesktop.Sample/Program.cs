﻿// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BlazorDesktop.Hosting;
using BlazorDesktop.Sample;
using BlazorDesktop.Sample.Data;
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
