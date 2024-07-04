// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using BlazorDesktop.Wpf;

namespace BlazorDesktop.Services;

/// <summary>
/// The blazor desktop service.
/// </summary>
public partial class BlazorDesktopService : IHostedService, IDisposable
{
    private CancellationTokenRegistration _applicationStoppingRegistration;
    private readonly IHostApplicationLifetime _lifetime;
    private readonly IServiceProvider _services;
    private readonly ILogger<BlazorDesktopService> _logger;
    private readonly WebViewInstaller _webViewInstaller;

    /// <summary>
    /// Creates a <see cref="BlazorDesktopService"/> instance.
    /// </summary>
    /// <param name="lifetime">The <see cref="IHostApplicationLifetime"/>.</param>
    /// <param name="services">The <see cref="IServiceProvider"/>.</param>
    /// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
    /// <param name="webViewInstaller">The <see cref="WebViewInstaller"/>.</param>
    public BlazorDesktopService(IHostApplicationLifetime lifetime, IServiceProvider services, ILogger<BlazorDesktopService> logger, WebViewInstaller webViewInstaller)
    {
        _applicationStoppingRegistration = new();
        _lifetime = lifetime;
        _services = services;
        _logger = logger;
        _webViewInstaller = webViewInstaller;
    }

    /// <summary>
    /// Starts the service.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents starting the service.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationStoppingRegistration = _lifetime.ApplicationStopping.Register(OnApplicationStopping);

        await _webViewInstaller.EnsureInstalledAsync(cancellationToken);

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

    [LoggerMessage(0, LogLevel.Critical, "Unhandled exception rendering component: {Message}", EventName = "ExceptionRenderingComponent")]
    private static partial void LogUnhandledExceptionRenderingComponent(ILogger logger, string message, Exception exception);

    private void ApplicationThread()
    {
        var app = _services.GetRequiredService<Application>();
        var mainWindow = _services.GetRequiredService<Window>();

        app.Startup += OnApplicationStartup;
        app.Exit += OnApplicationExit;
        app.DispatcherUnhandledException += OnApplicationException;

        app.MainWindow = mainWindow;

        app.Run();

        app.DispatcherUnhandledException -= OnApplicationException;
        app.Exit -= OnApplicationExit;
        app.Startup -= OnApplicationStartup;
    }

    private void OnApplicationStartup(object sender, StartupEventArgs e)
    {
        var app = _services.GetRequiredService<Application>();

        app.MainWindow.WindowStyle = WindowStyle.None;
        app.MainWindow.WindowState = WindowState.Minimized;
        app.MainWindow.Show();
    }

    private void OnApplicationExit(object? sender, ExitEventArgs e)
    {
        _lifetime.StopApplication();
    }

    private void OnApplicationException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        if (e.Exception is TargetInvocationException && e.Exception.InnerException is not null)
        {
            LogUnhandledExceptionRenderingComponent(_logger, e.Exception.InnerException.Message, e.Exception.InnerException);
        }
        else
        {
            LogUnhandledExceptionRenderingComponent(_logger, e.Exception.Message, e.Exception);
        }

        e.Handled = true;
    }

    private void OnApplicationStopping()
    {
        var app = _services.GetRequiredService<Application>();

        app.Dispatcher.Invoke(app.Shutdown);
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
