# Software Engineering Simulator

![logo](res/img/logo.png)

![](https://img.shields.io/travis/com/01010101lzy/software-engineering-simulator.svg?label=test&style=for-the-badge) ![](https://img.shields.io/travis/com/01010101lzy/software-engineering-simulator/master.svg?label=master&style=for-the-badge) ![](https://img.shields.io/travis/com/01010101lzy/software-engineering-simulator/stable.svg?label=stable&style=for-the-badge)

**WARNING: This project is in alpha phase. Use at your own risk.**

**软件工程模拟器** 是软件工程课的作业。这是一款模拟软件开发过程的游戏，完成时间 Soon™。在做完之后可能会上架 Steam。

**Software Engineering Simulator** is a homework project for the Software Engineering class in my college. This is a game to simulate (at least partially) the process of developing a software with a team. Will be complete Soon™. Might be uploaded to Steam after completion.

## 文档 Documents

供开发人员查看的文档（阶段性目标等）现在发布在 Notion 上。外部开发人员（如果有）可以参看以下文档（简体中文）：

Documents for developers (e.g. partial goals) are published on Notion. External developers (if any) can refer to the following docs (written in Simplified Chinese):

- [文档首页][sesim_notion_mainpage]
- [计划列表][sesim_notion_kanban]
- [文章、想法和碎碎念][sesim_notion_thoughts]

[sesim_notion_mainpage]: https://www.notion.so/4e4e8da938f64a1596c4beefd1ddaa4e
[sesim_notion_kanban]: https://www.notion.so/bdf8e75603534a38a816715151869410?v=2243fa7afb5c4630a72d78bbce567172
[sesim_notion_thoughts]: https://www.notion.so/06c779fd600e4ad2bdeb73069892108b?v=40c98345647c47d4aefa38e63d453daf

## 开发环境 Development Environment

- Unity 2018.3.12f1
- [DotNet Framework 4.7.1][dnf471]
- NuGet

*注：Unity 跟 NuGet 和 Paket 的兼容性都不太好，所以依赖项目前还是手动管理的。*

*Note: Because Unity3D does not go well with NuGet nor Paket, the dependencies are managed by hand for now.*

## 依赖项 Dependencies

由于 Unity3D 的某些特性（只认 `Assets/` 里面的文件），以下依赖项的二进制文件或源代码都被包含在了本仓库中。

Due to certain limitations of the Unity3D framework (only files in `Assets/` will be included for building), the binary release or source code of all dependencies below are contained in this repository.

- [Hocon][hocon_] ([Apache 2.0 License][hocon__lic])
- [Ceras][ceras_] ([MIT License][ceras__lic])
- [Wiry.Base32][base32] ([MIT License][base32_lic])
- [Ulid][ulid__] ([MIT License][ulid___lic])
- [Iosevka][iosvka] ([SIL OFL license][iosvka_lic])
- [Super Blur][sublur] ([MIT License][sublur_lic])
- MathNet.Numerics (MIT License)

已不再使用的依赖项：

Dependencies that are no longer used:

> Who needs ProtoBuf or MessagePack when he has the great Ceras serializer?

- [MessagePack-CSharp][msgpak] ([MIT License][msgpak_lic])
- [protobuf-net][ptbnet] 3.0.0 ([BSD / Apache License][ptbnet_lic])

[dnf471]: https://www.microsoft.com/en-us/download/details.aspx?id=56119
[nuget_]: https://www.nuget.org/
[paket_]: https://fsprojects.github.io/Paket/
[ceras_]: https://github.com/rikimaru0345/Ceras
[ceras__lic]: https://github.com/rikimaru0345/Ceras/blob/master/LICENSE.md
[hocon_]: https://github.com/akkadotnet/HOCON
[hocon__lic]: https://github.com/akkadotnet/HOCON/blob/dev/LICENSE
[base32]: https://github.com/wiry-net/Wiry.Base32
[base32_lic]: https://github.com/wiry-net/Wiry.Base32/blob/master/LICENSE
[ulid__]: https://github.com/Cysharp/Ulid
[ulid___lic]: https://github.com/Cysharp/Ulid/blob/master/LICENSE
[iosvka]: https://github.com/be5invis/Iosevka
[iosvka_lic]: https://github.com/be5invis/Iosevka/blob/master/LICENSE.md
[sublur]: https://github.com/PavelDoGreat/Super-Blur
[sublur_lic]: https://github.com/PavelDoGreat/Super-Blur/blob/master/LICENSE

[msgpak]: https://github.com/neuecc/MessagePack-CSharp
[msgpak_lic]: https://github.com/neuecc/MessagePack-CSharp/blob/master/LICENSE
[ptbnet]: https://github.com/mgravell/protobuf-net
[ptbnet_lic]: https://github.com/mgravell/protobuf-net/blob/master/Licence.txt

## 协议 License

见[协议文件][lic]。

See the [License file][lic].

[lic]: https://github.com/01010101lzy/software-engineering-simulator/blob/master/license.md
