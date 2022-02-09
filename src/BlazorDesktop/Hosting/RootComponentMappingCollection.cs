// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace BlazorDesktop.Hosting;

/// <summary>
/// Defines a collection of <see cref="RootComponentMapping"/> items.
/// </summary>
public class RootComponentMappingCollection : Collection<RootComponentMapping>
{
    /// <summary>
    /// Adds a component mapping to the collection.
    /// </summary>
    /// <typeparam name="TComponent">The component type.</typeparam>
    /// <param name="selector">The DOM element selector.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="selector"/> is null.</exception>
    public void Add<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] TComponent>(string selector) where TComponent : IComponent
    {
        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        Add(new RootComponentMapping(typeof(TComponent), selector));
    }

    /// <summary>
    /// Adds a component mapping to the collection.
    /// </summary>
    /// <param name="componentType">The component type. Must implement <see cref="IComponent"/>.</param>
    /// <param name="selector">The DOM element selector.</param>
    public void Add([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, string selector)
    {
        Add(componentType, selector, ParameterView.Empty);
    }

    /// <summary>
    /// Adds a component mapping to the collection.
    /// </summary>
    /// <param name="componentType">The component type. Must implement <see cref="IComponent"/>.</param>
    /// <param name="selector">The DOM element selector.</param>
    /// <param name="parameters">The parameters to the root component.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="componentType"/> or <paramref name="selector"/> is null.</exception>
    public void Add([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, string selector, ParameterView parameters)
    {
        if (componentType is null)
        {
            throw new ArgumentNullException(nameof(componentType));
        }

        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        Add(new RootComponentMapping(componentType, selector, parameters));
    }

    /// <summary>
    /// Adds a collection of items to this collection.
    /// </summary>
    /// <param name="items">The items to add.</param>
    /// <exception cref="ArgumentNullException">Occurs when <paramref name="items"/> is null.</exception>
    public void AddRange(IEnumerable<RootComponentMapping> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }

        foreach (var item in items)
        {
            Add(item);
        }
    }
}
