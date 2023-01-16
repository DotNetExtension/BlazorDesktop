// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using BlazorDesktop.Wpf;

namespace BlazorDesktop.Hosting;

/// <summary>
/// Extension methods for configuring the <see cref="BlazorDesktopHostBuilder" />.
/// </summary>
public static class BlazorDesktopHostBuilderExtensions
{
    /// <summary>
    /// Installs a web view on the machine.
    /// </summary>
    /// <param name="builder">The <see cref="BlazorDesktopHostBuilder"/>.</param>
    /// <param name="useInstaller">If the installer should be used.</param>
    /// <param name="silentInstall">If the installer should be silent.</param>
    /// <returns>A reference to the <paramref name="builder"/> after the operation has completed.</returns>
    public static BlazorDesktopHostBuilder UseWebViewInstaller(this BlazorDesktopHostBuilder builder, bool useInstaller = true, bool silentInstall = false)
    {
        builder.Services.AddSingleton(new WebViewInstaller { Enabled = useInstaller, SilentInstall = silentInstall });

        return builder;
    }

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
