// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Sample.Data;

/// <summary>
/// A weather forecast
/// </summary>
public class WeatherForecast
{
    /// <summary>
    /// The date
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// The temperature in celcius
    /// </summary>
    public int TemperatureC { get; set; }

    /// <summary>
    /// The temperature in fahrenheit
    /// </summary>
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    /// <summary>
    /// The summary
    /// </summary>
    public string? Summary { get; set; }
}
