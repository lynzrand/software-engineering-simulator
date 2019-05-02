# Software Engineering Simulator

![logo](res/img/logo.png)

**WARNING: This project is in alpha phase. Use at your own risk.**

**软件工程模拟器** 是软件工程课的作业。这是一款模拟软件开发过程的游戏，完成时间 Soon™。在做完之后可能会上架 Steam。

**Software Engineering Simulator** is a homework project for the Software Engineering class in my college. This is a game to simulate (at least partially) the process of developing a software with a team. Will be complete Soon™. Might be uploaded to Steam after completion.

## Development Environment

- Unity 2018.3.12f1
- [DotNet Framework 4.7.1][dnf471]
- NuGet

*注：Unity 跟 NuGet 和 Paket 的兼容性都不太好，所以依赖项目前还是手动管理的。*

*Note: Because Unity3D does not go well with NuGet nor Paket, the dependencies are managed by hand for now.*

## Dependencies

由于 Unity3D 的某些特性（只认 `Assets/` 里面的文件），以下依赖项的二进制文件或源代码都被包含在了本仓库中。

Due to certain limitations of the Unity3D framework (only files in `Assets/` will be included for building), the binary release or source code of all dependencies below are contained in this repository.

- [Hocon][hocon_] ([Apache 2.0 License][hocon__lic])
- [Ceras][ceras_] ([MIT License][ceras__lic])
- [Wiry.Base32][base32] ([MIT License][base32_lic])
- [Ulid][ulid__] ([MIT License][ulid___lic])
- [Iosevka][iosvka] ([SIL OFL license][iosvka_lic])
- [Super Blur][sublur] ([MIT License][sublur_lic])

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

## License

见[协议文件][lic]。

See the [License file][lic].

[lic]: https://github.com/01010101lzy/software-engineering-simulator/blob/master/license.md
