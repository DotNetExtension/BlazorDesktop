// Licensed to the .NET Extension Contributors under one or more agreements.
// The .NET Extension Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Hosting;

/// <summary>
/// Extension methods for configuring the <see cref="BlazorDesktopHostBuilder" />.
/// </summary>
public static class BlazorDesktopHostBuilderExtensions
{
    /// <summary>
    /// Adds chromium dev tools to the application.
    /// </summary>
    /// <param name="builder">The <see cref="BlazorDesktopHostBuilder"/>.</param>
    /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
    public static BlazorDesktopHostBuilder UseDeveloperTools(this BlazorDesktopHostBuilder builder)
    {
        builder.Services.AddBlazorWebViewDeveloperTools();

        return builder;
    }
}
