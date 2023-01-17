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
    /// The configuration.
    /// </summary>
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Creates a instance of <see cref="ConfigureWindowBuilder"/>.
    /// </summary>
    /// <param name="configuration">The configuration.</param>
    internal ConfigureWindowBuilder(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Uses a specific title for the window.
    /// </summary>
    /// <param name="title">The title.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseTitle(string title)
    {
        _configuration[WindowDefaults.Title] = title;
        return this;
    }

    /// <summary>
    /// Uses a specific height for the window.
    /// </summary>
    /// <param name="height">The height.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseHeight(int height)
    {
        _configuration[WindowDefaults.Height] = height.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific width for the window.
    /// </summary>
    /// <param name="width">The width.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseWidth(int width)
    {
        _configuration[WindowDefaults.Width] = width.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific min height for the window.
    /// </summary>
    /// <param name="minHeight">The min height.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseMinHeight(int minHeight)
    {
        _configuration[WindowDefaults.MinHeight] = minHeight.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific min width for the window.
    /// </summary>
    /// <param name="minWidth">The min width.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseMinWidth(int minWidth)
    {
        _configuration[WindowDefaults.MinWidth] = minWidth.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific max height for the window.
    /// </summary>
    /// <param name="maxHeight">The max height.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseMaxHeight(int maxHeight)
    {
        _configuration[WindowDefaults.MaxHeight] = maxHeight.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific max width for the window.
    /// </summary>
    /// <param name="maxWidth">The max width.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseMaxWidth(int maxWidth)
    {
        _configuration[WindowDefaults.MaxWidth] = maxWidth.ToString();
        return this;
    }

    /// <summary>
    /// If the window should have a frame.
    /// </summary>
    /// <param name="frame">If the default frame should be used.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseFrame(bool frame)
    {
        _configuration[WindowDefaults.Frame] = frame.ToString();
        return this;
    }

    /// <summary>
    /// If the window can be resized.
    /// </summary>
    /// <param name="resizable">If the window is resizable.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseResizable(bool resizable)
    {
        _configuration[WindowDefaults.Resizable] = resizable.ToString();
        return this;
    }

    /// <summary>
    /// Uses a specific icon for the window.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <returns>The <see cref="ConfigureWindowBuilder"/>.</returns>
    public ConfigureWindowBuilder UseIcon(string icon)
    {
        _configuration[WindowDefaults.Icon] = icon;
        return this;
    }
}
