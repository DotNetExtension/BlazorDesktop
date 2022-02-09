// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace BlazorDesktop.Hosting;

/// <summary>
/// The blazor desktop hosting environment.
/// </summary>
internal class BlazorDesktopHostingEnvironment : IWebHostEnvironment
{
    /// <summary>
    /// The environment name.
    /// </summary>
    public string EnvironmentName { get; set; } = Environments.Production;

    /// <summary>
    /// The application name.
    /// </summary>
    public string ApplicationName { get; set; } = string.Empty;

    /// <summary>
    /// The web root path.
    /// </summary>
    public string WebRootPath { get; set; } = string.Empty;

    /// <summary>
    /// The web root file provider.
    /// </summary>
    public IFileProvider WebRootFileProvider { get; set; } = new PhysicalFileProvider(Directory.GetCurrentDirectory());

    /// <summary>
    /// The content root path.
    /// </summary>
    public string ContentRootPath { get; set; } = string.Empty;

    /// <summary>
    /// The content root file provider.
    /// </summary>
    public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(Directory.GetCurrentDirectory());

    /// <summary>
    /// Loads configuration values.
    /// </summary>
    /// <param name="config">The configuration.</param>
    internal void LoadConfig(IConfiguration config)
    {
        var environmentName = config.GetValue<string?>(WebHostDefaults.EnvironmentKey);
        var applicationName = config.GetValue<string?>(WebHostDefaults.ApplicationKey);
        var webRootPath = config.GetValue<string?>(WebHostDefaults.WebRootKey);
        var contentRootPath = config.GetValue<string?>(WebHostDefaults.ContentRootKey);

        if (environmentName != null)
        {
            EnvironmentName = environmentName;
        }

        if (applicationName == null)
        {
            applicationName = Assembly.GetEntryAssembly()?.GetName()?.Name;

            if (applicationName != null)
            {
                ApplicationName = applicationName;
            }
        }
        else
        {
            ApplicationName = applicationName;
        }

        if (webRootPath == null)
        {
            webRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            WebRootPath = webRootPath;
            WebRootFileProvider = new PhysicalFileProvider(webRootPath);
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
                WebRootFileProvider = new PhysicalFileProvider(webRootPath);
            }
        }

        if (contentRootPath == null)
        {
            contentRootPath = Directory.GetCurrentDirectory();
            ContentRootPath = contentRootPath;
            ContentRootFileProvider = new PhysicalFileProvider(contentRootPath);
        }
        else
        {
            if (!Directory.Exists(contentRootPath))
            {
                throw new DirectoryNotFoundException(contentRootPath);
            }
            else
            {
                ContentRootPath = contentRootPath;
                ContentRootFileProvider = new PhysicalFileProvider(contentRootPath);
            }
        }
    }

    /// <summary>
    /// Saves configuration values.
    /// </summary>
    /// <param name="config">The configuration.</param>
    internal void SaveConfig(ConfigurationManager config)
    {
        config[WebHostDefaults.EnvironmentKey] = EnvironmentName;
        config[WebHostDefaults.ApplicationKey] = ApplicationName;
        config[WebHostDefaults.WebRootKey] = WebRootPath;
        config[WebHostDefaults.ContentRootKey] = ContentRootPath;
    }
}
