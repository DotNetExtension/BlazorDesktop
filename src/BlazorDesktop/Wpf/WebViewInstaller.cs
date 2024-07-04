// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace BlazorDesktop.Wpf;

/// <summary>
/// Web view installer.
/// </summary>
public class WebViewInstaller
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Creates a <see cref="WebViewInstaller"/> instance.
    /// </summary>
    /// <param name="httpClient">The <see cref="HttpClient"/> used to download the installer.</param>
    public WebViewInstaller(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Checks if the web view is installed, and if not installs it.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> used to cancel the process.</param>
    /// <returns>A <see cref="Task"/> representing the installation.</returns>
    public async Task EnsureInstalledAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var version = CoreWebView2Environment.GetAvailableBrowserVersionString();

            if (!string.IsNullOrWhiteSpace(version))
            {
                return;
            }
        }
        catch
        {
            // If we land here, the web view isn't installed.
        }

        var installerPath = Path.Combine(Path.GetTempPath(), $"{Path.GetRandomFileName()}.exe");
        var installerStream = new FileStream(installerPath, FileMode.Create, FileAccess.Write);

        try
        {
            using var downloadStream = await _httpClient.GetStreamAsync("https://go.microsoft.com/fwlink/p/?LinkId=2124703", cancellationToken);
            await downloadStream.CopyToAsync(installerStream, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new HttpRequestException("Unable to download WebView2 installer.", ex);
        }
        finally
        {
            installerStream.Close();
        }

        try
        {
            var processStartInfo = new ProcessStartInfo()
            {
                FileName = installerPath,
                Arguments = "/install",
                Verb = "runas",
                UseShellExecute = true
            };

            var process = Process.Start(processStartInfo);

            if (process is not null)
            {
                await process.WaitForExitAsync(cancellationToken);
            }
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Unable to install WebView2.", ex);
        }
        finally
        {
            File.Delete(installerPath);
        }
    }
}
