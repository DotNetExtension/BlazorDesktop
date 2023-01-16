// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Windows;
using BlazorDesktop.Wpf;
using WebView2.Runtime.AutoInstaller;

namespace BlazorDesktop.Services;

/// <summary>
/// The blazor desktop service.
/// </summary>
public class BlazorDesktopService : IHostedService, IDisposable
{
    /// <summary>
    /// The cancellation token registration.
    /// </summary>
    private CancellationTokenRegistration _applicationStoppingRegistration;

    /// <summary>
    /// The application lifetime.
    /// </summary>
    private readonly IHostApplicationLifetime _lifetime;

    /// <summary>
    /// The web view installer.
    /// </summary>
    private readonly WebViewInstaller _webViewInstaller;

    /// <summary>
    /// The services.
    /// </summary>
    private readonly IServiceProvider _services;

    /// <summary>
    /// Creates a <see cref="BlazorDesktopService"/> instance.
    /// </summary>
    /// <param name="lifetime">The <see cref="IHostApplicationLifetime"/>.</param>
    /// <param name="services">The <see cref="IServiceProvider"/>.</param>
    /// <param name="webViewInstaller">The <see cref="WebViewInstaller"/>.</param>
    public BlazorDesktopService(IHostApplicationLifetime lifetime, IServiceProvider services, WebViewInstaller webViewInstaller)
    {
        _applicationStoppingRegistration = new();
        _lifetime = lifetime;
        _services = services;
        _webViewInstaller = webViewInstaller;
    }

    /// <summary>
    /// Starts the service.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents starting the service.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationStoppingRegistration = _lifetime.ApplicationStopping.Register(() =>
        {
            OnApplicationStopping();
        });

        if (_webViewInstaller.Enabled)
        {
            await WebView2AutoInstaller.CheckAndInstallAsync(silentInstall: _webViewInstaller.SilentInstall, cancellationToken: cancellationToken);
        }

        var thread = new Thread(ApplicationThread);
        thread.SetApartmentState(ApartmentState.STA);
        thread.Start();
    }

    /// <summary>
    /// Stops the service.
    /// </summary>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/>.</param>
    /// <returns>A task representing the shutdown of the service.</returns>
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// The application thread.
    /// </summary>
    private void ApplicationThread()
    {
        var app = _services.GetRequiredService<Application>();
        var mainWindow = _services.GetRequiredService<Window>();

        app.Startup += OnApplicationStartup;
        app.Exit += OnApplicationExit;

        app.MainWindow = mainWindow;

        app.Run();
    }

    /// <summary>
    /// Occurs when the application is starting up.
    /// </summary>
    /// <param name="sender">The sending object.</param>
    /// <param name="e">The arguments.</param>
    private void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        var app = _services.GetRequiredService<Application>();

        app.MainWindow.WindowStyle = WindowStyle.None;
        app.MainWindow.WindowState = WindowState.Minimized;
        app.MainWindow.Show();
    }

    /// <summary>
    /// Occurs when the application exits.
    /// </summary>
    /// <param name="sender">The sending object.</param>
    /// <param name="e">The arguments.</param>
    private void OnApplicationExit(object? sender, ExitEventArgs e)
    {
        _lifetime.StopApplication();
    }

    /// <summary>
    /// Occurs when the application is stopping.
    /// </summary>
    private void OnApplicationStopping()
    {
        var app = _services.GetRequiredService<Application>();

        app.Dispatcher.Invoke(() =>
        {
            app.Shutdown();
        });
    }

    /// <summary>
    /// Disposes of this <see cref="BlazorDesktopService"/> instance.
    /// </summary>
    [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Not needed in this instance.")]
    public void Dispose()
    {
        _applicationStoppingRegistration.Dispose();
    }
}
