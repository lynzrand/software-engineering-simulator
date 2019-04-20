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

由于 Unity3D 的某些特性（只认 `Assets/` 里面的文件），以下依赖项的二进制文件都被包含在了本仓库中。

Due to certain limitations of the Unity3D framework (only files in `Assets/` will be included for building), the binary release of all files below are contained in this repository.

- [protobuf-net][ptbnet] 3.0.0 ([BSD / Apache License][ptbnet_lic])
- [Hocon][hocon_] ([Apache 2.0 License][hocon__lic])
- [Wiry.Base32][base32] ([MIT License][base32_lic])
- [Ulid][ulid__] ([MIT License][ulid___lic])
- [Iosevka][iosvka] ([SIL OFL license][iosvka_lic])


[dnf471]: https://www.microsoft.com/en-us/download/details.aspx?id=56119
[nuget_]: https://www.nuget.org/
[paket_]: https://fsprojects.github.io/Paket/
[ptbnet]: https://github.com/mgravell/protobuf-net
[ptbnet_lic]: https://github.com/mgravell/protobuf-net/blob/master/Licence.txt
[hocon_]: https://github.com/akkadotnet/HOCON
[hocon__lic]: https://github.com/akkadotnet/HOCON/blob/dev/LICENSE
[base32]: https://github.com/wiry-net/Wiry.Base32
[base32_lic]: https://github.com/wiry-net/Wiry.Base32/blob/master/LICENSE
[ulid__]: https://github.com/Cysharp/Ulid
[ulid___lic]: https://github.com/Cysharp/Ulid/blob/master/LICENSE
[iosvka]: https://github.com/be5invis/Iosevka
[iosvka_lic]: https://github.com/be5invis/Iosevka/blob/master/LICENSE.md

## License

见[协议文件][lic]。

See the [License file][lic].

[lic]: https://github.com/01010101lzy/software-engineering-simulator/blob/master/license.md
