// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using BlazorDesktop.Wpf;
using WebView2.Runtime.AutoInstaller;

namespace BlazorDesktop.Services;

/// <summary>
/// The blazor desktop service.
/// </summary>
/// <remarks>
/// Creates a <see cref="BlazorDesktopService"/> instance.
/// </remarks>
/// <param name="lifetime">The <see cref="IHostApplicationLifetime"/>.</param>
/// <param name="services">The <see cref="IServiceProvider"/>.</param>
/// <param name="logger">The <see cref="ILogger{TCategoryName}"/>.</param>
/// <param name="webViewInstaller">The <see cref="WebViewInstaller"/>.</param>
public partial class BlazorDesktopService(IHostApplicationLifetime lifetime, IServiceProvider services, ILogger<BlazorDesktopService> logger, WebViewInstaller webViewInstaller) : IHostedService, IDisposable
{
    /// <summary>
    /// The application lifetime.
    /// </summary>
    private readonly IHostApplicationLifetime _lifetime = lifetime;

    /// <summary>
    /// The services.
    /// </summary>
    private readonly IServiceProvider _services = services;

    /// <summary>
    /// The <see cref="ILogger{TCategoryName}"/>.
    /// </summary>
    private readonly ILogger<BlazorDesktopService> _logger = logger;

    /// <summary>
    /// The web view installer.
    /// </summary>
    private readonly WebViewInstaller _webViewInstaller = webViewInstaller;

    /// <summary>
    /// The cancellation token registration.
    /// </summary>
    private CancellationTokenRegistration _applicationStoppingRegistration;

    /// <summary>
    /// Starts the service.
    /// </summary>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns>A task that represents starting the service.</returns>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _applicationStoppingRegistration = _lifetime.ApplicationStopping.Register(OnApplicationStopping);

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
    /// Logs an unhanded component exception,
    /// </summary>
    /// <param name="logger">The <see cref="ILogger"/>.</param>
    /// <param name="message">The message.</param>
    /// <param name="exception">The <see cref="Exception"/>.</param>
    [LoggerMessage(0, LogLevel.Critical, "Unhandled exception rendering component: {Message}", EventName = "ExceptionRenderingComponent")]
    private static partial void LogUnhandledExceptionRenderingComponent(ILogger logger, string message, Exception exception);

    /// <summary>
    /// The application thread.
    /// </summary>
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
    /// Occurs when the application throws an exception.
    /// </summary>
    /// <param name="sender">The sending object.</param>
    /// <param name="e">The arguments.</param>
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

    /// <summary>
    /// Occurs when the application is stopping.
    /// </summary>
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
