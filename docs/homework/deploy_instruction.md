---
title: 部署文档

toc: true
toc-title: 目录
toc-depth: 2
numbersections: true

lang: zh-CN

fontsize: 11pt
linestretch: 1.05

geometry:
  - margin=1in

linkcolor: blue

documentclass: scrartcl 

mainfont: Tinos
mainfontoptions:
  - BoldFont=Tinos Bold
  - ItalicFont=Tinos Italic
  - BoldItalicFont=Tinos Bold Italic
CJKmainfont: Source Han Serif CN
CJKmainfontoptions:
  - BoldFont=Source Han Serif CN Bold
  - ItalicFont=Source Han Serif CN Italic
  - BoldItalicFont=Source Han Serif CN Bold Italic
monofont: Iosevka
monofontoptions:
  - BoldFont=Iosevka Bold
  - ItalicFont=Iosevka Italic
  - BoldItalicFont=Iosevka Bold Italic

figureTitle: 图
tableTitle: 表
figPrefix: 图
eqnPrefix: 式
tblPrefix: 表

header-includes:
  - \usepackage{xeCJK}
  - \XeTeXlinebreaklocale "zh"
  - \XeTeXlinebreakskip = 0pt plus 1pt minus 0.1pt
  - \setCJKmonofont{Sarasa Term SC}
---

# 系统需求

操作系统：Windows 7 及更高版本  
CPU：性能不低于 Intel Core i5-5200U  
显卡：性能不低于 Intel HD Graphics 520

# 部署步骤

## 可执行文件

如果您拿到的是包含可执行文件的压缩包，请按以下步骤部署：

1. 将压缩包解压至您想安装的位置；
2. 打开 `SESim.exe` 即可运行。

## 源代码

如果您拿到的是包含源代码的压缩包，请按照以下步骤部署：

1. 安装 Unity 2018.3f12 或更高版本，确保电脑本地有 Microsoft .NET Framework 4.7.1 的 SDK；
2. 将源代码文件夹解压缩到你认为合适的地方；
3. 使用 Unity 打开源代码文件夹，在菜单栏中找到 `Build > Build Windows`，点击；
4. 静待 Unity 编译完成；
5. 编译结果在 `build/windows` 文件夹中，将其移动至你想安装的位置；
6. 打开 `SESim.exe` 即可运行。
