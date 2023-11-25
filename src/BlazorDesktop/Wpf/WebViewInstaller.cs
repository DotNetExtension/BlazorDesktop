// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace BlazorDesktop.Wpf;

/// <summary>
/// Web view installer options.
/// </summary>
public class WebViewInstaller
{
    /// <summary>
    /// If the installer should be used.
    /// </summary>
    public bool Enabled { get; set; }

    /// <summary>
    /// If the installer should be silent.
    /// </summary>
    public bool SilentInstall { get; set; }
}
