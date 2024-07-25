[![GitHub license](https://img.shields.io/github/license/DotNetExtension/BlazorDesktop?style=for-the-badge&color=00bb00)](https://github.com/DotNetExtension/BlazorDesktop/blob/main/LICENSE.txt)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa?style=for-the-badge)](CODE_OF_CONDUCT.md)
[![GitHub issues](https://img.shields.io/github/issues/DotNetExtension/BlazorDesktop?style=for-the-badge)](https://github.com/DotNetExtension/BlazorDesktop/issues)

# Blazor Desktop
Blazor Desktop allows you to create desktop apps using Blazor. Apps run inside of a .NET generic host with a WPF window thats fully managed using a similar template to Blazor WASM.
![preview](https://github.com/DotNetExtension/BlazorDesktop/assets/2308261/7d025b49-e2f8-4b07-a57d-35f9a319d859)

# Getting Started
The easiest way to get started with Blazor Desktop is to install the templates, you can do so using the dotnet cli as follows:

```powershell
dotnet new install BlazorDesktop.Templates::8.0.0
```

Once you have the templates installed, you can either create a new project from the template either in Visual Studio in the template picker:
![create](https://github.com/DotNetExtension/BlazorDesktop/assets/2308261/5ac50c95-9b90-4d5f-bb4f-7aa8a242d823)

Or, you can create a new project using the cli as follows:
```powershell
dotnet new blazordesktop -n MyBlazorApp
```

# Tips & Tricks
The Blazor Desktop template is set up very similar to the Blazor WASM template, you can see the `Program.cs` file here:

```csharp
using BlazorDesktop.Hosting;
using HelloWorld.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = BlazorDesktopHostBuilder.CreateDefault(args);

builder.RootComponents.Add<Routes>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

if (builder.HostEnvironment.IsDevelopment())
{
    builder.UseDeveloperTools();
}

await builder.Build().RunAsync();
```

You can add root components just the same as well as add additional services your app may need just the same.

There are however a few additional APIs and services that have been made available to help when working in the context of a WPF window.

## Window Utilities
The window can have most of its common configuration done through the `BlazorDesktopHostBuilder.Window` APIs before you build your app in `Program.cs`.

To change your window title:
```csharp
builder.Window.UseTitle("Hello");
```

Window size:
```csharp
builder.Window
    .UseWidth(1920)
    .UseHeight(1080)
    .UseMinWidth(1280)
    .UseMinHeight(720)
    .UseMaxWidth(2560)
    .UseMaxHeight(1440);
```

Disable window resizing:
```csharp
builder.Window.UseResizable(false);
```

Disable the window frame (allows you to use your own window chrome inside of Blazor):
```csharp
builder.Window.UseFrame(false);
```

And change your window icon (uses `favicon.ico` as the default, base directory is `wwwroot`):
```csharp
builder.Window.UseIcon("myicon.ico");
```

It is also possible to configure these values through `appsettings.json` like so:
```json
{
  "Window": {
    "Title": "Hello Blazor",
    "Height": 1080,
    "Width": 1920,
    "MinHeight": 720,
    "MinWidth": 1280,
    "MaxHeight": 1440,
    "MaxWidth": 2560,
    "Frame": false,
    "Resizable": false,
    "Icon": "hello.ico"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

**The `Window` object itself is also made available inside of the DI container, so you can access all properties on it by using the inject Razor keyword or requesting it through the constructor of a class added as a service.**

## Custom Window Chrome & Draggable Regions
It is possible to make your own window chrome for Blazor Desktop apps. As an example base setup you could do the following:

Set up the window to have no frame in `Program.cs`:
```csharp
builder.Window.UseFrame(false);
```

Using the base template, if you were to edit `MainLayout.razor` and add a `-webkit-app-region: drag;` style to the top bar like so:
```razor
@inherits LayoutComponentBase

<div class="page">
    <div class="sidebar">
        <NavMenu />
    </div>

    <main>
        <div class="top-row px-4" style="-webkit-app-region: drag;">
            <a href="https://learn.microsoft.com/aspnet/core/" target="_blank">About</a>
        </div>

        <article class="content px-4">
            @Body
        </article>
    </main>
</div>
```
The top bar becomes draggable, applying the `-webkit-app-region: drag;` property to anything will make it able to be used to drag the window.

In terms of handling things such as the close button, you can inject the Window into any page and interact from it there.

Here is an example changing `MainLayout.razor`:
```razor
@using BlazorDesktop.Wpf

@inherits LayoutComponentBase
@inject BlazorDesktopWindow window

<div class="page">
    <div class="sidebar">
        <NavMenu />
    </div>

    <main>
        <div class="top-row px-4" style="-webkit-app-region: drag;">
            <a href="https://learn.microsoft.com/aspnet/core/" target="_blank">About</a>
        </div>

        <article class="content px-4">
            @Body
        </article>
    </main>
</div>

@code {
    void CloseWindow()
    {
        window.Close();
    }
}
```

To support fullscreen mode, you should also hide your custom window chrome when in fullscreen. You can check the current fullscreen status using the `IsFullscreen` property on the window. You can also monitor for it changing using the `OnFullscreenChanged` event.

## Issues
Under the hood, Blazor Desktop uses WebView2 which has limitations right now with composition. Due to this, if you disable the window border through the `Window.UseFrame(false)` API, the top edge of the window is unusable as a resizing zone for the window. However all the other corners and edges work.
