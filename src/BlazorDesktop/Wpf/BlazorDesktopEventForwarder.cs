// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.Win32;
using Windows.Win32.Foundation;

namespace BlazorDesktop.Wpf;

/// <summary>
/// The blazor desktop event forwarder.
/// </summary>
public class BlazorDesktopEventForwarder
{
    /// <summary>
    /// Posted when the user presses the left mouse button while the cursor is
    /// within the nonclient area of a window.
    /// </summary>
    private const int WM_NCLBUTTONDOWN = 0x00A1;

    /// <summary>
    /// In a title bar.
    /// </summary>
    private const int HTCAPTION = 2;

    /// <summary>
    /// The target
    /// </summary>
    private readonly IntPtr _target;

    /// <summary>
    /// Creates an instance of <see cref="BlazorDesktopEventForwarder"/>.
    /// </summary>
    /// <param name="target">The target handle.</param>
    public BlazorDesktopEventForwarder(IntPtr target)
    {
        _target = target;
    }

    /// <summary>
    /// Occurs when the mouse starts dragging.
    /// </summary>
    public void MouseDownDrag()
    {
        PInvoke.ReleaseCapture();
        PInvoke.SendMessage(new HWND(_target), WM_NCLBUTTONDOWN, HTCAPTION, 0);
    }
}
