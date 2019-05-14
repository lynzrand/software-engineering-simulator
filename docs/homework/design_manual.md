---
title: 软件设计说明书

toc: true
toc-title: 目录
toc-depth: 2
numbersections: true

lang: zh-CN

fontsize: 11pt
linestretch: 1.05

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
CJKmonofont: Source Han Sans SC
CJKmonofontoptions:
  - BoldFont=Source Han Sans SC Bold
  - ItalicFont=Source Han Sans SC Italic
  - BoldItalicFont=Source Han Sans SC Bold Italic

header-includes:
  - \usepackage{xeCJK}
  - \XeTeXlinebreaklocale "zh"
  - \XeTeXlinebreakskip = 0pt plus 1pt minus 0.1pt
---

<!-- [TOC] -->

# 引言

## 目的

本文档编写的目的是为了规范软件代码的结构等方面，防止在之后的开发中出现软件结构混乱、命名冲突等问题。

## 命名规范

代码的命名和编写规范见 [这篇关于命名规范的文档][cs_coding_standards]

[cs_coding_standards]: http://github.com/ktaranov/naming-convention/blob/master/C%23%20Coding%20Standards%20and%20Naming%20Convensions

## 定义

- N/A: Not Applicable，无、不适用
- 玩家: 指游玩此游戏的人
- 系统/程序: 指游戏本身
- Unity: [Unity3D][unity] 游戏引擎

[unity]: https://unity.com

## 参考资料

1. Unity Scripting Reference (<https://docs.unity3d.com/ScriptReference>)
2. Unity Manual (<https://docs.unity3d.com/Manual>)
3. Microsoft Docs (<https://docs.microsoft.com/zh-cn/>)


## 相关文档

1. 开发需求说明书 (<https://github.com/01010101lzy/software-engineering-simulator/blob/master/docs/homework/spec.md>)
2. 软件开发计划书 (<https://github.com/01010101lzy/software-engineering-simulator/blob/master/docs/homework/dev_plan.md>)

## 版本历史

见本文件在 GitHub 的[版本历史][git_history]。

[git_history]: https://github.com/01010101lzy/software-engineering-simulator/commits/master/docs/homework/design_manual.md

# 总体设计

待补充。

## 总体结构设计

待补充。

## 硬件运行环境设计

待补充。

## 软件运行环境设计

待补充。

## 功能模块清单

待补充。

# 模块功能分配

待补充。

## 公用模块功能分配

待补充。

## 专用模块功能分配

待补充。

# 存档结构设计

待补充。

# 其他设计

待补充。

# 系统错误处理机制

待补充。

# 测试计划

截至本次文档更新时，开发组已在 Travis CI 上设置了测试流水线。流水线在每一次代码提交之后都会对当前提交的版本进行单元测试和集成测试，并将测试结果发回代码所有者和组长的邮箱中。

[travis_repo]: https://travis-ci.com/01010101lzy/software-engineering-simulator/
