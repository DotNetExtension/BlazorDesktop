// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Sample.Data;

/// <summary>
/// The weather forecast service
/// </summary>
public class WeatherForecastService
{
    /// <summary>
    /// The list of summaries
    /// </summary>
    private static readonly string[] s_summaries =
    [
        "Freezing",
        "Bracing",
        "Chilly",
        "Cool",
        "Mild",
        "Warm",
        "Balmy",
        "Hot",
        "Sweltering",
        "Scorching"
    ];

    /// <summary>
    /// Gets the weather forecast asynchronously
    /// </summary>
    /// <param name="startDate">The starting date</param>
    /// <returns>An array of <see cref="WeatherForecast"/></returns>
    public Task<WeatherForecast[]> GetForecastAsync(DateTime startDate)
    {
        return Task.FromResult(Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = startDate.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = s_summaries[Random.Shared.Next(s_summaries.Length)]
        }).ToArray());
    }
}
