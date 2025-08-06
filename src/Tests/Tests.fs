namespace Tests

open Aardvark.Base
open Aardvark.OMM
open System.Runtime.InteropServices

open FsUnit
open NUnit.Framework

module OmmTests =

    [<OneTimeSetUp>]
    let init() =
        Aardvark.Init()

    let private checkSize<'T> (expectedSize: int) =
        sizeof<'T> |> should equal expectedSize
        Marshal.SizeOf<'T>() |> should equal expectedSize

    let private checkHandleSize<'T>() =
        checkSize<'T> sizeof<nativeint>

    let printMessage (severity: MessageSeverity) (message: string) =
        printfn $"{severity}: {message}"

    [<Test>]
    let ``Struct and handle sizes``() =
        checkHandleSize<API.Baker>()
        checkHandleSize<API.CpuTexture>()
        checkHandleSize<API.CpuBakeResult>()
        checkHandleSize<API.CpuSerializedResult>()
        checkHandleSize<API.CpuDeserializedResult>()
        checkSize<API.CpuBakeInputDesc> 136
        checkSize<API.CpuBakeResultDesc> 80
        checkSize<API.DebugSaveImagesDesc> 24

    [<Test>]
    let ``Library version``() =
        let version = Baker.LibraryVersion
        printfn "Version: %A" version
        version.Major |> should greaterThan 0uy

    [<Test>]
    let ``Create baker``() =
        let baker = new Baker(printMessage)
        baker.Dispose()

    [<Test>]
    let ``Create texture``() =
        use baker = new Baker(printMessage)

        let pi = PixImage<uint8>(Col.Format.GrayAlpha, V2i(256))
        use texture = pi |> Texture.ofPixImage baker

        texture.Handle.IsValid |> should be True