# SmartBundleTags
Specify bundles for your .Net Core MVC apps that automatically switch between source files and compressed production versions based on environment.

![Linking Javascript Before and After](https://github.com/BrandenMartin/SmartBundleTags/blob/master/readme/compare.png)

## This utility is designed to be used with the [Bundler & Minifier Extension for Visual Studio](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.BundlerMinifier)


## Features:

* Works with JS and CSS
* Keeps cshtml code clean and concise
* Presents developers with source files for quick and easy development
* Presents end users with only final minified code for fast loads time and increased security
* Set it once and forget it, no more fuss thinking about which files are where and who sees what


## How To Use:
If you have the Bundle & Minify extension setup and working with your bundleconfig.json, you can link your files in any cshtml page by simply specifying the bundle output file name in the new bundle tag!

```HTML
<bundle name="[your bundle name here]"/>
```

### To get the bundle tag working, follow these steps:

1. Add the following to your startup.cs file in the ConfigureServices method
```C#
  #if (DEBUG)
      services.AddBundles();
  #else
      services.AddBundles(options => {
          options.UseBundles = true;
          options.AppendVersion = true;
      });
  #endif
```


2. Add the following to the startup.cs file in the Configure method
```C#
  #if DEBUG
      //Allow direct access to un minified source files if debugging
      app.UseStaticFiles(new StaticFileOptions {
          FileProvider = new PhysicalFileProvider(Directory.GetCurrentDirectory()),
          RequestPath = ""
      });
  #endif
```


3. Add the following to your _ViewStart.cshtml file
```HTML+Razor
  @using SmartBundleTags.BundleTagHelpers
```


4. Add the following to your _ViewImports.cshtml file
```HTML+Razor
  @using SmartBundleTags
  @addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
  @addTagHelper *, SmartBundleTags
```

    
If the above steps were done correctly the `<bundle>` tag should now support syntax highlighting and will work properly.
