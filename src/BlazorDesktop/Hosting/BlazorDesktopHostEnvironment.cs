// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using Microsoft.Extensions.FileProviders;

namespace BlazorDesktop.Hosting;

/// <summary>
/// The blazor desktop host environment.
/// </summary>
internal sealed class BlazorDesktopHostEnvironment : IWebHostEnvironment
{
    /// <summary>
    /// The environment name.
    /// </summary>
    public string EnvironmentName
    {
        get => _hostEnvironment.EnvironmentName;
        set => _hostEnvironment.EnvironmentName = value;
    }

    /// <summary>
    /// The application name.
    /// </summary>
    public string ApplicationName
    {
        get => _hostEnvironment.ApplicationName;
        set => _hostEnvironment.ApplicationName = value;
    }

    /// <summary>
    /// The content root path.
    /// </summary>
    public string ContentRootPath
    {
        get => _hostEnvironment.ContentRootPath;
        set => _hostEnvironment.ContentRootPath = value;
    }

    /// <summary>
    /// The content root file provider.
    /// </summary>
    public IFileProvider ContentRootFileProvider
    {
        get => _hostEnvironment.ContentRootFileProvider;
        set => _hostEnvironment.ContentRootFileProvider = value;
    }

    /// <summary>
    /// The web root path.
    /// </summary>
    public string WebRootPath { get; set; }

    /// <summary>
    /// The web root file provider.
    /// </summary>
    public IFileProvider WebRootFileProvider { get; set; }

    private readonly IHostEnvironment _hostEnvironment;

    /// <summary>
    /// Creates an instance of <see cref="BlazorDesktopHostEnvironment"/> with a specified <see cref="IHostEnvironment"/>.
    /// </summary>
    /// <param name="hostEnvironment">The <see cref="IHostEnvironment"/> instance.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> instance.</param>
    public BlazorDesktopHostEnvironment(IHostEnvironment hostEnvironment, IConfiguration configuration)
    {
        _hostEnvironment = hostEnvironment;

        var webRootPath = configuration.GetValue<string?>(WebHostDefaults.WebRootKey);

        if (webRootPath == null)
        {
            WebRootPath = Path.Combine(ContentRootPath, "wwwroot");
        }
        else
        {
            if (!Directory.Exists(webRootPath))
            {
                throw new DirectoryNotFoundException(webRootPath);
            }
            else
            {
                WebRootPath = webRootPath;
            }
        }

        WebRootFileProvider = new PhysicalFileProvider(WebRootPath);
    }
}
