# VSIX
visual studio  the Managed Package Framework (MPF)



## CodeMaid 10.6

基于CodeMaid二次开发的VS 2013插件项目，其中包含outline，Emmet语法，hightWord，TODO高亮等个性化的功能.

### 2020-01-06

* 新增支持文件夹右击清理html标签的样式和格式等；
* 新增支持html样式清理的容错性和性能等；

### 2019-09-01

* 新增关于嵌入嵌出文件的按钮功能，可以生成嵌入关系；
* 新增代码重构的按钮，代码未实现；
* 新增关于aspx页面的html标签的代码正则表达式解析和格式化等；





## WebEssentials2013

Web Essentials extends Visual Studio with lots of new features that web developers have been missing for many years. 

If you ever write CSS, HTML, JavaScript, TypeScript, CoffeeScript, LiveScript or LESS, then you will find many useful features that make your life as a developer easier. 

This is for all Web developers using Visual Studio.

To get the latest nightly build, follow [these instructions](http://vswebessentials.com/download#nightly).

##Getting started
To contribute to this project, you'll need to do a few things first:

1. Fork the project on GitHub
2. Clone it to your computer
3. Install the [Visual Studio 2013 SDK](https://www.microsoft.com/en-us/download/details.aspx?id=40758).
4. Open the solution in VS2013.

To install your local fork into your main VS instance, you will first need to open `Source.extension.vsixmanifest` and bump the version number to make it overwrite the (presumably) already-installed production copy. (alternatively, just uninstall Web Essentials from within VS first)

You can then build the project, then double-click the VSIX file from the bin folder to install it in Visual Studio.

### TODO

......正在扩展中...





## ZenCodingVS



[![Build status](https://ci.appveyor.com/api/projects/status/p6lyd0fetoa1amgy?svg=true)](https://ci.appveyor.com/project/madskristensen/zencodingvs)https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ZenCoding)

Download this extension from the [Marketplace](https://marketplace.visualstudio.com/items?itemName=MadsKristensen.ZenCoding)
or get the [CI build](http://vsixgallery.com/extension/9514d70e-a7b1-4876-847d-b0d2ad0962bf/).

------

Provides ZenCoding for the HTML Editor - full support for static HTML, Razor and WebForms.

See the [change log](CHANGELOG.md) for changes and road map.

## Features

- HTML ZenCoding
- Lorem Ipsum generator
- Lorem Pixel image generator
- PlaceHold.it image generator

### HTML ZenCoding

ZenCoding is a quick way to generate HTML markup by using a CSS based syntax.

To invoke ZenCoding, write the syntax and hit the `TAB` key to generate the markup.

#### Examples:

Syntax __ul>li\*3__ generates:

![Example1](E:/OpenSource/VSIX/ZenCodingVS/art/example1.png)

Syntax __#foo>span__ generates:

![Example2](E:/OpenSource/VSIX/ZenCodingVS/art/example2.png)

Syntax __ul>li\*4>a{test $}__ generates:

![Example3](E:/OpenSource/VSIX/ZenCodingVS/art/example3.png)

### Lorem Ipsum generator

As part of ZenCoding, you can now generate Lorem Ipsum code directly in the HTML editor. Simply type `lorem` and hit `TAB` and a 30 word Lorem Ipsum text is inserted. 

Type `lorem10` and a 10 word Lorem Ipsum text is inserted. 
This can be used in conjuction with ZenCoding like so: `ul>li*5>lorem3`

### Lorem Pixel generator

As part of ZenCoding, you can also generate Lorem Pixel code directly in the HTML editor. Simply type `pix-200x200-animals` and hit `TAB` and a img tag with a 200x200 image of an animal is inserted:

![Example4](E:/OpenSource/VSIX/ZenCodingVS/art/example4.png)

### PlaceHold.it generator

ZenCoding also support [PlaceHold.it](http://placehold.it/) if you prefer blank images. Type `place-50` and hit `TAB` to have a 50 pixels square image. Use `place-200x100` to insert a rectangular image:

![Example5](E:/OpenSource/VSIX/ZenCodingVS/art/example5.png)

You can choose the background color by adding the hexadecimal value after the size like this `place-150x240-EEEDDD`. You can even add text to the image by using `place-150x240-EEE-t=This%20is%20some%20text`.