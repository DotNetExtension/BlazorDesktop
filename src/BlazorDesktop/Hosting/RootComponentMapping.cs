// Licensed to the Blazor Desktop Contributors under one or more agreements.
// The Blazor Desktop Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace BlazorDesktop.Hosting;

/// <summary>
/// Defines a mapping between a root <see cref="IComponent"/> and a DOM element selector.
/// </summary>
public readonly struct RootComponentMapping
{
    /// <summary>
    /// Gets the component type.
    /// </summary>
    [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)]
    public Type ComponentType { get; }

    /// <summary>
    /// Gets the DOM element selector.
    /// </summary>
    public string Selector { get; }

    /// <summary>
    /// Gets the parameters to pass to the root component.
    /// </summary>
    public ParameterView Parameters { get; }

    /// <summary>
    /// Creates a new instance of <see cref="RootComponentMapping"/> with the provided <paramref name="componentType"/>
    /// and <paramref name="selector"/>.
    /// </summary>
    /// <param name="componentType">The component type. Must implement <see cref="IComponent"/>.</param>
    /// <param name="selector">The DOM element selector or component registration id for the component.</param>
    /// <exception cref="ArgumentNullException">Occurs when the <paramref name="componentType"/> or <paramref name="selector"/> parameters are null.</exception>
    /// <exception cref="ArgumentException">Occurs when <paramref name="componentType"/> does not inherit from <see cref="IComponent"/>.</exception>
    public RootComponentMapping([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, string selector)
    {
        if (componentType is null)
        {
            throw new ArgumentNullException(nameof(componentType));
        }

        if (!typeof(IComponent).IsAssignableFrom(componentType))
        {
            throw new ArgumentException($"The type '{componentType.Name}' must implement {nameof(IComponent)} to be used as a root component.", nameof(componentType));
        }

        if (selector is null)
        {
            throw new ArgumentNullException(nameof(selector));
        }

        ComponentType = componentType;
        Selector = selector;
        Parameters = ParameterView.Empty;
    }

    /// <summary>
    /// Creates a new instance of <see cref="RootComponentMapping"/> with the provided <paramref name="componentType"/>,
    /// <paramref name="selector"/>, and <paramref name="parameters"/>.
    /// </summary>
    /// <param name="componentType">The component type. Must implement <see cref="IComponent"/>.</param>
    /// <param name="selector">The DOM element selector or component registration id for the component.</param>
    /// <param name="parameters">The parameters to pass to the component,</param>
    public RootComponentMapping([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.All)] Type componentType, string selector, ParameterView parameters) : this(componentType, selector)
    {
        Parameters = parameters;
    }
}
