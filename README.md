[![GitHub license](https://img.shields.io/github/license/AndrewBabbitt97/BlazorDesktop?style=for-the-badge&color=00bb00)](https://github.com/AndrewBabbitt97/BlazorDesktop/blob/main/LICENSE.txt)
[![Contributor Covenant](https://img.shields.io/badge/Contributor%20Covenant-2.0-4baaaa?style=for-the-badge)](CODE_OF_CONDUCT.md)
[![GitHub issues](https://img.shields.io/github/issues/AndrewBabbitt97/BlazorDesktop?style=for-the-badge)](https://github.com/AndrewBabbitt97/BlazorDesktop/issues)

# Blazor Desktop
Blazor Desktop allows you to create desktop apps using Blazor. Apps run inside of a .NET generic host with a WPF window thats fully managed using a similar template to Blazor WASM.
![app](https://user-images.githubusercontent.com/2308261/153133429-7e1cdebd-72d0-4d61-91d9-eb14089cf9fc.png)

# Getting Started
The easiest way to get started with Blazor Desktop is to install the templates, you can do so using the dotnet cli as follows:

```powershell
dotnet new --install BlazorDesktop.Templates::1.0.4
```

Once you have the templates installed, you can either create a new project from the template either in Visual Studio in the template picker:
![desktop](https://user-images.githubusercontent.com/2308261/153132853-5430710e-a4d7-434d-a46c-1269b9865711.png)

Or, you can create a new project using the cli as follows:
```powershell
dotnet new blazordesktop -n MyBlazorApp
```

# Tips & Tricks
The Blazor Desktop template is set up very similar to the Blazor WASM template, you can see the `Program.cs` file here:

```csharp
using BlazorDesktop.Hosting;
using HelloWorld;
using HelloWorld.Data;
using Microsoft.AspNetCore.Components.Web;

var builder = BlazorDesktopHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton<WeatherForecastService>();

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
builder.Window.UseWidth(1920)
    .UseHeight(1080);
```

Disable window resizing:
```csharp
builder.Window.UseResizable(false);
```

Disable the window frame (allows you to use your own window chrome inside of Blazor):
```csharp
builder.Window.UseFrame(false);
```

And change your window icon (uses `wwwroot/favicon.ico` as the default):
```csharp
builder.Window.UseIcon("myicon.ico");
```

It is also possible to configure these values through `appsettings.json` like so:
```json
{
  "Window": {
    "Title": "Hello Blazor",
    "Height": 900,
    "Width": 400,
    "Frame": false,
    "Resizable": false,
    "Icon": "hello.ico"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```

Blazor Desktop will automatically install WebView2 for the user if they do not already have it installed, you can disable this if you wish:
```csharp
builder.Window.UseWebView2Installer(false);
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
            <a href="https://docs.microsoft.com/aspnet/" target="_blank">About</a>
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
@using System.Windows
@inherits LayoutComponentBase
@inject Window window

<div class="page">
    <div class="sidebar">
        <NavMenu />
    </div>

    <main>
        <div class="top-row px-4" style="-webkit-app-region: drag;">
            <a href="https://docs.microsoft.com/aspnet/" target="_blank">About</a>
        </div>

        <article class="content px-4">
            <button @onclick="CloseWindow">Close Window</button>
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

## Issues
Under the hood, Blazor Desktop uses WebView2 which has limitations right now with composition. Due to this, if you disable the window border through the `Window.UseFrame(false)` API, the top edge of the window is unusable as a resizing zone for the window. However all the other corners and edges work.
