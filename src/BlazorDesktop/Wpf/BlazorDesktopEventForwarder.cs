// Licensed to the .NET Extension Contributors under one or more agreements.
// The .NET Extension Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace BlazorDesktop.Wpf;

/// <summary>
/// The blazor desktop event forwarder.
/// </summary>
public partial class BlazorDesktopEventForwarder
{
    private const int WM_NCLBUTTONDOWN = 0x00A1;
    private const int HTCAPTION = 2;

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
        ReleaseCapture();
        SendMessage(_target, WM_NCLBUTTONDOWN, HTCAPTION, 0);
    }

    [LibraryImport("User32", EntryPoint = "SendMessageW", SetLastError = true)]
    internal static partial nint SendMessage(IntPtr hWnd, uint msg, uint wParam, nint lParam);

    [LibraryImport("User32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ReleaseCapture();
}
