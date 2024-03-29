﻿# VSIX
visual studio  the Managed Package Framework (MPF)    测试



## CodeMaid 10.8

基于CodeMaid二次开发的VS 2013插件项目，其中包含outline，Emmet语法，OverviewMargin2012预览方法虚线连接，hightWord，TODO高亮等个性化的功能.

### 2020-01-20

* 新增BrowserLink浏览器自动刷新html页面的功能；



### 2020-01-09

* 新增特性文件比较功能，支持两个文件比较；
* 新增特性打开VS加载完全加载解决方案后会隐藏菜单栏；
* 新增插件OverviewMargin功能；
* 新增第三方包代码段添加和编辑等；
* 优化outline和高亮代码显示并且显示选区长度；
* 新增ZenCoding语法插件；

### 2020-01-06

* 新增支持文件夹右击清理html标签的样式和格式等；
* 新增支持html样式清理的容错性和性能等；
* 优化清理已选代码的性能，清理可对部分文件类型有效；
* 通过配置选项科配置只清理aspx文件部分或全部html标签样式；
* 修复outling功能，支持选中单词会高亮显示当前文档中相同的字符；

### 2019-09-01

* 新增关于嵌入嵌出文件的按钮功能，可以生成嵌入关系；
* 新增代码重构的按钮，代码未实现；
* 新增关于aspx页面的html标签的代码正则表达式解析和格式化等；



## CodeAnalysis 项目

新增代码分析项目,支持在vs2019项目中可以分析代码是否符合自己的要求，不符合自动出现警告或错误提示，

然后还有小灯泡修复提示等功能；

- 运行环境

  VisualStudio 2019 版本   VSSDK

- 功能说明

  - 1. 项目中代码的提醒分析
  - 2. 对应的分析结果解决方案

  - 3. 待续。。。。









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



# SideWaffle for Visual Studio

[SideWaffle.com](http://sidewaffle.com) - download the extension 自定义模板项目

## The ultimate web developer template pack

A collection of Item Templates for Visual Studio 2012/2013/2015 
that makes any web developer's life much easier.

![The result of a search for "angular" in the "Add new item" dialog](C:/Users/Administrator/Desktop/side-waffle/screenshot.png)
The result of a search for "angular" in the "Add new item" dialog

## Add new templates

1. Fork the project
2. Clone it to your computer
3. Install the [Visual Studio 2012 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=30668), 
   [Visual Studio 2013 SDK](http://www.microsoft.com/en-us/download/details.aspx?id=40758), or 
   [Visual Studio 2015 SDK](https://msdn.microsoft.com/en-us/library/bb166441(v=vs.140).aspx).
4. Open the solution in Visual Studio
5. Watch [this video tutorial](http://youtu.be/h4VaORKgrOw)
6. After adding your templates, send us a pull request

 **Only high quality templates with broad appeal will be accepted**

SideWaffle templates can be installed in Visual Studio 2012, 2013 and 2015, regardless of the version you use for creating new templates.

Learn more about on MSDN about [customizing item templates]



## AllMargins

#### Introduction:

​	This is a compilation of the OverviewMargin and several other extensions that use the OverviewMargin. The OverviewMargin shows
​    a margin on the right side of the editor that logically maps to the entire file (similar the the vertical scroll bar). Unlike
​    the scroll bar, it maps to the entire file and can contain other margins that provide more information about the file.

#### History:

   * v1.0    David Pugh  2/26/2010

     ​        	Initial release

   * V1.1    David Pugh  3/4/2010
     ​        	Updated included extensions.
     
   * V1.2    David Pugh  4/19/2010
     ​        	Updated included extensions.
     
   * V1.3    David Pugh  4/20/2010
     ​        	Updated included extensions.
     
   * V1.4    David Pugh  4/22/2010
     ​        	Updated included extensions.
     
   * V1.5    David Pugh  4/27/2010
     ​        	Updated included extensions.
     
   * V1.6    David Pugh  4/28/2010
     ​        	Updated included extensions to fix VB parser error.
     
   * V1.7    David Pugh  4/28/2010
     ​        	Updated version number to work around install issue.
     
   * V1.8    David Pugh  4/28/2010
     ​        	More VB parse fixes (Public, REM, Interface, Structure).
     
   * V1.9    David Pugh  4/28/2010
     ​        	Updated CaretMargin, VB, C# and C parsers.
     
   * V2.0    David Pugh  5/03/2010
     ​        	Bumped version number to fix installation problem.
     
   * V2.1    David Pugh  6/10/2010
     ​        	Updated OverviewMargin, BlockTagger, StructureAdornment & StructureMargin.
     ​        	Changed namespaces and DLL names to Microsoft.VisualStudio.Extensions.....
     
   * V2.2    David Pugh  6/29/2010
     ​        	Updated StructureAdornment.
     
   * V2.3    David Pugh  6/30/2010
     ​        	Picked up fix for VB parser, tweaks for structure adornment & margin.
     
   * V3.0    Jeff Valore 9/06/2012
     ​        	Updated for VisualStudio 2012. All extensions now included in single project.

#### Included extensions:

* CaretMargin         http://visualstudiogallery.msdn.microsoft.com/en-us/a893687b-f488-49eb-ad91-c59d86daad34.
* ErrorsToMarks       http://visualstudiogallery.msdn.microsoft.com/en-us/0fc52c83-0ab3-485d-a917-2006966eec7a.
* MarkersToMarks      http://visualstudiogallery.msdn.microsoft.com/en-us/89deee06-0ed0-4347-81a6-942a3f2874af.
* OverviewMarginImpl  http://visualstudiogallery.msdn.microsoft.com/en-us/2e9f37b7-5a1f-4c47-930b-379b2d0fd596.
* StructureAdornment  http://visualstudiogallery.msdn.microsoft.com/en-us/203f22f4-3e9f-4dbb-befc-f2606835834e.
*  StructureMargin     http://visualstudiogallery.msdn.microsoft.com/en-us/fe432eb5-c538-47a9-9919-fba1a8f5b261. 

* BlockTagger         The definition of an API to get the structure of a code file.

* BlockTaggerImpl     An implementation of the BlockTaggerAPI for C/C# and VB files.

* OverviewMargin      The definition of an API for creating margins that map to the entire file.

*  SettingsStore       The definition of an API to load and save editor options across sessions.

* SettingsStoreImpl   An implementation of the SettingStore API that uses IVsSettingsStore to access the system registry.

  

  * Usage:
        See other extensions.

  * Options:
        None.

#### Notes:

​    The full source for this extension is at http://code.msdn.microsoft.com/OverviewMargin/Release/ProjectReleases.aspx?ReleaseId=3957

