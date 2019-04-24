---
title: 软件开发计划书
---

[toc]

# 引言

## 目的

<!-- 不想说空话，就这样了吧 -->

为了使游戏《软件工程模拟器》的开发更具有条理性，我们编写了本计划书。本计划书主要确定：

- 软件开发的内容的、生命周期；
- 软件规范、方法和标准；
- 工作规模、工作量和成本的估计；
- 开发进度的制定；
- 风险的估计。

## 范围

软件开发计划书包括：

- 软件规模估计
- 软件模块规划
- 进度安排
- 质量保证计划

## 术语、缩写定义

| 术语/缩写 | 定义                      |
| --------- | ------------------------- |
| Unity     | [Unity3D][unity] 游戏引擎 |
| NPC       | 非玩家角色                |


[unity]: https://unity.com

## 参考资料

N/A

## 相关文档

N/A

## 版本历史

见本文件在 GitHub 的[版本历史][git_history]。

[git_history]: https://github.com/01010101lzy/software-engineering-simulator/commits/master/docs/homework/dev_plan.md

# 项目概述

软件工程模拟器（Software Engineering Simulator, 缩写为 SESim）是一款模拟软件开发过程的游戏。

**开发工具:** Unity3D  
**开发语言:** C#  
**开发周期:** 45 天

## 项目范围

### 主要功能点

- 游戏配置文件的读取
- 游戏存档的读取与存储
- 游戏内部运行机制，包括：
  - 资源随时间的变化
  - 虚拟开发人员的开发效率的计算
  - 虚拟项目的进度计算
  - 游戏事件的计算
- 游戏图形界面的设计与应用

# 项目组织

由于涉及到的开发人员较少，本项目的组织比较扁平、松散。其中：

- 项目管理人员将负责项目整体结构的设计、功能点的选取等工作；
- 软件开发人员将负责从代码编写、功能测试、游戏内部功能组合等全部工作；
- 美术工作人员将负责图形界面的设计、建模等工作；
- 音乐工作人员将负责游戏的配乐等工作。

人员与职务列表如下：

| 人员            | 职务             |
| --------------- | ---------------- |
| @01010101lzy    | 管理、软件、美术 |
| @awesomeztl     | 软件             |
| @y199387        | 软件             |
| @Maplecr        | 软件             |
| @MoonLight23333 | 软件             |
| @Dimpurr        | 音乐（外援）     |

# 生命周期

待补充。

# 规范、方法与标准

## 代码要求

### 格式

所有 C# 代码的编写需要遵循 [微软官方文档中推荐的格式][cs_coding_convensions] 编写。所有注释和提交说明（commit message）需尽量使用英语编写。

请尽量选用合适、简明的命名空间。项目的根命名空间是 `Sesim`。

请善用 `#region` 分割代码区域。

[cs_coding_convensions]: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/inside-a-program/coding-conventions

### 路径

所有的程序代码请尽量放在 `Assets/lib` 内部。测试代码请放在 `Assets/test` 内部。

# 任务与工作产品

待补充。

# 成本估计

由于开发工作是课程作业，没有人工费用。其余内容的费用待补充。

# 关键计算机资源计划

具体需求见[项目 Readme 文件][readme]。简要说明如下：

## 开发

- 性能高于 Intel i5-6300U 的 CPU
- 大于 8 GiB 的内存
- 至少 5 GiB 硬盘空间
- Unity Editor 2018.3.12f1
- Microsoft .NET Framework 4.7.1
- Nuget

## 运行（预计）

- 性能高于 Intel i5-6200U 的 CPU
- 大于 2 GiB 的内存
- 至少 1 GiB 硬盘空间

[readme]: https://github.com/01010101lzy/software-engineering-simulator/blob/master/readme.md

# 软件项目进度计划

待补充。

# 风险分析

N/A

# 设备工具计划

N/A

# 项目评审

N/A

# 度量

项目由管理人员每周对所有数据进行统计和记录，每天（自动化）运行测试记录测试数据。需要度量的数据包括：

- 项目各个功能的实现情况（整体完成度）；
- 单元测试和集成测试的覆盖率和错误数量；
- 项目中各类任务耗时；
