---
title: 用户使用说明书

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

# 安装

将压缩包解压即可。

# 使用

1. 运行 `SESim.exe`
2. 等待加载完成，按照屏幕所示开启游戏即可。

# 游戏内部操作

## 加载界面

在启动游戏之后，游戏会开始读取配置文件。在读取配置文件时，游戏会将遇到的问题（如果有）显示在屏幕上。如果您手动修改过配置文件，请按照提示进行修改。

## 主菜单

在读取完配置文件之后，游戏会进入主菜单。主菜单的按钮介绍和作用如下：

- `Game::new` - 新建游戏并进入。
- `Game::load` - 加载一个存档。在点击之后右侧会出现存档选择页面。
  - 在存档选择页面里点击存档标题选择一个存档；
  - 点击 `[ LOAD ]` 加载存档；
  - 点击 `[ DELETE ]` 删除存档。
- `Settings::show` - 显示设置界面。由于当前版本的游戏没有需要设置的内容，本按钮被禁用。
- `Game::exit` - 退出游戏。

## 游戏内

如图所示，游戏界面含有三个部分——左侧的一览面板、中间的公司视图、右侧的概况/提醒面板。

![SESim 界面简图](assets/sesim-ui.png)

## 一览面板

这个面板展示公司中各项资源的概况。它包含两个子面板：订单一览和人员一览。两个子面板可以通过最左侧的工具栏切换。

两个子面板都展示了当前公司中可以使用和可以接收的人员/订单资源。点击其中的按钮可以对资源执行相应的操作（接收/放弃等）。

## 公司视图

这个视图当前没有任何作用，仅作展示用。

## 概况面板

这个面板显示了公司的概况，包括当前时间、时间加速倍率、资金、声望和资源统计数据。

## 提醒面板和控制台

这个面板当前没有任何作用，是为以后的游戏内容预留的。

## 快捷键

| 快捷键                  | 用途                 |
| ----------------------- | -------------------- |
| `Esc`                   | 暂停/恢复游戏        |
| `,`（逗号）             | 降低时间加速倍率     |
| `.`（句号）             | 提高时间加速倍率     |
| `x`                     | 重制时间加速倍率为 1 |
| 鼠标右键拖动、`W/A/S/D` | 移动公司视图         |
