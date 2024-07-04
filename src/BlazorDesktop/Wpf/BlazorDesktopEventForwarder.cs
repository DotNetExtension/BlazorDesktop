// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Runtime.InteropServices;

namespace BlazorDesktop.Wpf;

/// <summary>
/// The blazor desktop event forwarder.
/// </summary>
public partial class BlazorDesktopEventForwarder
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
        ReleaseCapture();
        SendMessage(_target, WM_NCLBUTTONDOWN, HTCAPTION, 0);
    }

    /// <summary>
    /// Sends the specified message to a window or windows. The SendMessage function calls the window procedure for the specified window and does not return until the window procedure has processed the message.
    /// </summary>
    /// <param name="hWnd">A handle to the window whose window procedure will receive the message.</param>
    /// <param name="msg">The message to be sent.</param>
    /// <param name="wParam">Additional message-specific information.</param>
    /// <param name="lParam">Additional message-specific information.</param>
    /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
    [LibraryImport("User32", EntryPoint = "SendMessageW", SetLastError = true)]
    internal static partial nint SendMessage(IntPtr hWnd, uint msg, uint wParam, nint lParam);

    /// <summary>
    /// Releases the mouse capture from a window in the current thread and restores normal mouse input processing.
    /// </summary>
    /// <returns>If the function succeeds, the return value is true. If the function fails, the return value is false.</returns>
    [LibraryImport("User32", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    internal static partial bool ReleaseCapture();
}
