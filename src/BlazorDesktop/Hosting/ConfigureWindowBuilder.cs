// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Hosting;

/// <summary>
/// Used to configure window options on a <see cref="BlazorDesktopHostBuilder"/>.
/// </summary>
public class ConfigureWindowBuilder
{
    /// <summary>
    /// The config.
    /// </summary>
    private readonly IConfiguration _config;

    /// <summary>
    /// Creates a instance of <see cref="ConfigureWindowBuilder"/>.
    /// </summary>
    /// <param name="config">The config.</param>
    internal ConfigureWindowBuilder(ConfigurationManager config)
    {
        _config = config;
    }

    /// <summary>
    /// Uses a specific title for the window.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseTitle(string title)
    {
        _config[WindowDefaults.Title] = title;
        return this;
    }

    /// <summary>
    /// Uses a specific height for the window.
    /// </summary>
    /// <param name="height">The height.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseHeight(int height)
    {
        _config[WindowDefaults.Height] = height.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific width for the window.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseWidth(int width)
    {
        _config[WindowDefaults.Width] = width.ToString();
        return this;
    }

    /// <summary>
    /// If the window should have a frame.
    /// </summary>
    /// <param name="frame">If the default frame should be used.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseFrame(bool frame)
    {
        _config[WindowDefaults.Frame] = frame.ToString();
        return this;
    }

    /// <summary>
    /// If the window can be resized.
    /// </summary>
    /// <param name="isResizable">If the window is resizable.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseResizable(bool resizable)
    {
        _config[WindowDefaults.Resizable] = resizable.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific icon for the window.
    /// </summary>
    /// <param name="isResizable">The icon.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseIcon(string icon)
    {
        _config[WindowDefaults.Icon] = icon;
        return this;
    }
}
