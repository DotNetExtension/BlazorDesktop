// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shell;
using BlazorDesktop.Hosting;
using Microsoft.AspNetCore.Components.WebView.Wpf;
using Microsoft.Web.WebView2.Core;
using Windows.UI.ViewManagement;

namespace BlazorDesktop.Wpf;

/// <summary>
/// The blazor desktop window.
/// </summary>
public partial class BlazorDesktopWindow : Window
{
    /// <summary>
    /// The <see cref="BlazorWebView"/>.
    /// </summary>
    public BlazorWebView WebView { get; }

    /// <summary>
    /// The <see cref="Border"/> for the <see cref="BlazorWebView"/>.
    /// </summary>
    public Border WebViewBorder { get; }

    private readonly IServiceProvider _services;
    private readonly IConfiguration _config;
    private readonly IWebHostEnvironment _environment;
    private readonly UISettings _uiSettings;
    private const string DragScript =
@"
window.addEventListener('DOMContentLoaded', () => {
    document.body.addEventListener('mousedown', evt => {
        const { target } = evt;
        const appRegion = getComputedStyle(target)['-webkit-app-region'];

        if (appRegion === 'drag') {
            chrome.webview.hostObjects.sync.eventForwarder.MouseDownDrag();
            evt.preventDefault();
            evt.stopPropagation();
        }
    });
});
";

    /// <summary>
    /// Creates a <see cref="BlazorDesktopWindow"/> instance.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="config">The configuration.</param>
    /// <param name="environment">The hosting environment.</param>
    public BlazorDesktopWindow(IServiceProvider services, IConfiguration config, IWebHostEnvironment environment)
    {
        WebView = new BlazorWebView();
        WebViewBorder = new Border();
        _services = services;
        _config = config;
        _environment = environment;
        _uiSettings = new UISettings();

        InitializeWindow();
        InitializeWebViewBorder();
        InitializeWebView();
        InitializeTheming();
    }

    private void InitializeWindow()
    {
        Name = "BlazorDesktopWindow";
        Title = _config.GetValue<string?>(WindowDefaults.Title) ?? _environment.ApplicationName;
        Height = _config.GetValue<int?>(WindowDefaults.Height) ?? 768;
        Width = _config.GetValue<int?>(WindowDefaults.Width) ?? 1366;
        MinHeight = _config.GetValue<int?>(WindowDefaults.MinHeight) ?? 0;
        MinWidth = _config.GetValue<int?>(WindowDefaults.MinWidth) ?? 0;
        MaxHeight = _config.GetValue<int?>(WindowDefaults.MaxHeight) ?? double.PositiveInfinity;
        MaxWidth = _config.GetValue<int?>(WindowDefaults.MaxWidth) ?? double.PositiveInfinity;
        UseFrame(_config.GetValue<bool?>(WindowDefaults.Frame) ?? true);
        ResizeMode = (_config.GetValue<bool?>(WindowDefaults.Resizable) ?? true) ? ResizeMode.CanResize : ResizeMode.NoResize;
        UseIcon(_config.GetValue<string?>(WindowDefaults.Icon) ?? string.Empty);
        Content = WebViewBorder;
        StateChanged += WindowStateChanged;
    }

    private void InitializeWebViewBorder()
    {
        WebViewBorder.Name = "BlazorDesktopWebViewBorder";
        WebViewBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        WebViewBorder.Child = WebView;
    }

    private void InitializeWebView()
    {
        WebView.Name = "BlazorDesktopWebView";
        WebView.HostPage = Path.Combine(_environment.WebRootPath, "index.html");
        WebView.Services = _services;

        foreach (var rootComponent in _services.GetRequiredService<RootComponentMappingCollection>())
        {
            WebView.RootComponents.Add(new()
            {
                Selector = rootComponent.Selector,
                ComponentType = rootComponent.ComponentType,
                Parameters = (IDictionary<string, object?>)rootComponent.Parameters.ToDictionary()
            });
        }

        WebView.Loaded += WebViewLoaded;
    }

    private void InitializeTheming()
    {
        SourceInitialized += WindowSourceInitialized;
        _uiSettings.ColorValuesChanged += ThemeChanged;
    }

    private void WindowStateChanged(object? sender, EventArgs e)
    {
        var useFrame = _config.GetValue<bool?>(WindowDefaults.Frame) ?? true;

        if (WindowState == WindowState.Maximized && !useFrame)
        {
            WebViewBorder.BorderThickness = new Thickness(8, 8, 8, 0);
        }
        else
        {
            WebViewBorder.BorderThickness = new Thickness(0, 0, 0, 0);
        }
    }

    private void WindowSourceInitialized(object? sender, EventArgs e)
    {
        UpdateTheme();
    }

    private void ThemeChanged(UISettings sender, object args)
    {
        Dispatcher.Invoke(new(UpdateTheme));
    }

    private void UpdateTheme()
    {
        if (ShouldSystemUseDarkMode())
        {
            _ = DwmSetWindowAttribute(new WindowInteropHelper(this).Handle, 20, [1], Marshal.SizeOf(typeof(int)));
            Background = new SolidColorBrush(Color.FromRgb(25, 25, 25));
        }
        else
        {
            _ = DwmSetWindowAttribute(new WindowInteropHelper(this).Handle, 20, [0], Marshal.SizeOf(typeof(int)));
            Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
        }
    }

    private void UseFrame(bool frame)
    {
        if (!frame)
        {
            WindowChrome.SetWindowChrome(this, new() { NonClientFrameEdges = NonClientFrameEdges.Bottom | NonClientFrameEdges.Left | NonClientFrameEdges.Right });
        }
    }

    private void UseIcon(string icon)
    {
        var defaultIconPath = Path.Combine(_environment.WebRootPath, "favicon.ico");
        var userIconPath = Path.Combine(_environment.WebRootPath, icon);

        if (File.Exists(userIconPath))
        {
            Icon = BitmapFrame.Create(new Uri(userIconPath, UriKind.RelativeOrAbsolute));
        }
        else
        {
            if (File.Exists(defaultIconPath))
            {
                Icon = BitmapFrame.Create(new Uri(defaultIconPath, UriKind.RelativeOrAbsolute));
            }
        }
    }

    private void WebViewLoaded(object sender, RoutedEventArgs e)
    {
        WebView.WebView.CoreWebView2InitializationCompleted += WebViewInitializationCompleted;
    }

    private void WebViewInitializationCompleted(object? sender, CoreWebView2InitializationCompletedEventArgs e)
    {
        var eventForwarder = new BlazorDesktopEventForwarder(new WindowInteropHelper(this).Handle);

        WebView.WebView.CoreWebView2.AddHostObjectToScript("eventForwarder", eventForwarder);
        WebView.WebView.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(DragScript);
        WebView.WebView.CoreWebView2.NavigationCompleted += BlazorWebViewNavigationCompleted;

        WebView.WebView.DefaultBackgroundColor = System.Drawing.Color.Transparent;
    }

    private void BlazorWebViewNavigationCompleted(object? sender, CoreWebView2NavigationCompletedEventArgs e)
    {
        WindowState = WindowState.Normal;
        WindowStyle = WindowStyle.SingleBorderWindow;
        WebView.WebView.CoreWebView2.NavigationCompleted -= BlazorWebViewNavigationCompleted;
    }

    [LibraryImport("UXTheme.dll", EntryPoint = "#138", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool ShouldSystemUseDarkMode();

    [LibraryImport("dwmapi.dll")]
    private static partial int DwmSetWindowAttribute(IntPtr hwnd, int dwAttribute, [In] int[] pvAttribute, int cbAttribute);
}
