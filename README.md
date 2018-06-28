# RudiBech.Parcel.AspNetCore

AspNet Core middelware for [Parcel](https://parceljs.org/) allowing you to start [Parcel](https://parceljs.org/) when running your AspNet Core application. It is based on- and works the same way as [WebPack middelware](https://github.com/aspnet/JavaScriptServices/tree/dev/src/Microsoft.AspNetCore.SpaServices/Webpack).

This package will ensure that Parcel runs when you start your AspNetCore application, and that it runs `parcel build index.html` when you build it with `release` configuration. It will also ensure that the generated files are added to your application as part of the `publish` phase.

The defaults in this package assumes that you have an `index.html` in the root of your application (same level as .csproj), and that you want the generated output to be placed in `wwwroot`. The debug files will be placed in `wwwroot/debug` and served by parcel through the middelware. When building for release it will put the files in `wwwroot`.

## Requirements

* [Node.js](https://nodejs.org/en/)
  * To test if this is installed and can be found, run `node -v` on a command line
  * Note: If you're deploying to an Azure web site, you don't need to do anything here - Node is already installed and available in the server environments
* [.NET Core](https://dot.net/) 2.1 or later

## Getting started

```powershell
mkdir AspNetCoreWithParcel
cd AspNetCoreWithParcel
dotnet new web
npm init -y
npm install parcel-bundler --save-dev
dotnet add package RudiBech.Parcel.AspNetCore
```
Then change your `Startup.cs` similar to this:

```c#
public void Configure(IApplicationBuilder app, IHostingEnvironment env)
{
  app.UseMvc();

  if (env.IsDevelopment())
  {
    app.UseDeveloperExceptionPage();
    //Remember to add using RudiBech.AspNetCore.SpaServices.Parcel;
    app.UseParcelDevMiddleware();
  }
  else
  {
    app.UseStaticFiles();
  }
}
```

Then add an `index.html` as your [Parcel](https://parceljs.org/) entry point. You can use a different entry point by using the available override on `UseParcelDevMiddleware`. You will then also need to add a couple of properties to your csproj file:

```xml
<PropertyGroup>
  <!-- These values must match whatever you pass in to UseParcelDevMiddleware -->
  <!-- Default to index.html -->
  <ParcelAspNetCoreEntryPoint>somePath/index.js</ParcelAspNetCoreEntryPoint>
  <!-- Default to wwwroot -->
  <ParcelAspNetCoreOutDir>dist</ParcelAspNetCoreOutDir>
</PropertyGroup>
```

**Please note:** This only gives you a starting point. You still need to set up your project with other dependencies. E.g. to get up and running with [Vue](https://vuejs.org/) and HMR you should add the following dependencies:

```powershell
npm install vue --save-dev
npm install vue-template-compiler --save-dev
npm install @vue/component-compiler-utils --save-dev
npm install babel-preset-env --save-dev
```

Basic index.html (to get you started):

```html
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <meta http-equiv="X-UA-Compatible" content="IE=edge">
    <title>Vue js Setup With Parcel</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">
</head>
<body>
    <div>
        <h2>Simple Vue.js Parcel Starter Kit</h2>
        <div id="app"></div>
    </div>
    <script src="./src/app.js"></script>
</body>
</html>
```

Sample `src/app.js`:

```js
import Vue from 'vue';
import App from './App.vue';

new Vue({
  el: '#app',
  render: h => h(App)
});
```

Sample `src/App.vue`:

```vue
<template>
  <div>
    <h3>{{ data }}</h3>
  </div>
</template>

<script>
export default {
  data () {
    return {
      data: 'Welcome to parcel bundler'
    }
  }
}
</script>
```