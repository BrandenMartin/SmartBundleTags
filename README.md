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
