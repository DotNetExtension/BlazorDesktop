// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Hosting;

/// <summary>
/// The logging builder.
/// </summary>
internal class LoggingBuilder : ILoggingBuilder
{
    /// <summary>
    /// The service colection.
    /// </summary>
    public IServiceCollection Services { get; }

    /// <summary>
    /// Creates an instance of <see cref="LoggingBuilder"/>.
    /// </summary>
    /// <param name="services">The service collection.</param>
    public LoggingBuilder(IServiceCollection services)
    {
        Services = services;
    }
}
