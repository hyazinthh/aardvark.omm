namespace Aardvark.OMM

open System.Runtime.InteropServices

[<StructLayout(LayoutKind.Sequential)>]
type LibraryDesc =
    struct
        val versionMajor : uint8
        val versionMinor : uint8
        val versionBuild : uint8
    end