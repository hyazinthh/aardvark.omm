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

    let private checkHandleSize<'T>() =
        sizeof<'T> |> should equal sizeof<nativeint>
        Marshal.SizeOf<'T>() |> should equal sizeof<nativeint>

    let printMessage (severity: MessageSeverity) (message: string) =
        printfn $"{severity}: {message}"

    [<Test>]
    let ``Library Version``() =
        let version = Baker.LibraryVersion
        printfn "Version: %A" version
        version.Major |> should greaterThan 0uy

    [<Test>]
    let ``Create Baker``() =
        checkHandleSize<API.Baker>()
        let baker = new Baker(printMessage)
        baker.Dispose()

    [<Test>]
    let ``Create Texture``() =
        checkHandleSize<API.CpuTexture>()

        use baker = new Baker(printMessage)

        let data = Matrix<uint8>(V2i(256))
        let pi = PixImage<uint8>(data)
        use texture = pi |> Texture.ofPixImage baker

        texture.Handle.IsValid |> should be True