// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Components.WebView.Wpf;

namespace BlazorDesktop.Extensions;

/// <summary>
/// <see cref="RootComponentsCollection"/> extension methods.
/// </summary>
internal static class RootComponentsCollectionExtensions
{
    /// <summary>
    /// Adds a range of <see cref="RootComponent"/> to a <see cref="RootComponentsCollection"/>.
    /// </summary>
    /// <param name="collection">The <see cref="RootComponentsCollection"/>.</param>
    /// <param name="components">The <see cref="IEnumerable{RootComponent}"/>.</param>
    public static void AddRange(this RootComponentsCollection collection, IEnumerable<RootComponent> components)
    {
        foreach (var component in components)
        {
            collection.Add(component);
        }
    }
}
