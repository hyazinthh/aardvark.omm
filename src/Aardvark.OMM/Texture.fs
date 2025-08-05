namespace Aardvark.OMM

open System
open System.Runtime.CompilerServices
open Aardvark.Base
open TypeMeta

#nowarn "51"

type Texture =
    val private baker : Baker
    val mutable private handle : API.CpuTexture

    internal new (baker, handle) = { baker = baker; handle = handle }

    member this.Handle = this.handle

    member this.Dispose() =
        if this.handle.IsValid then
            API.Omm.destroyCpuTexture(this.baker.Handle, this.handle) |> Result.check "failed to destroy texture"
            this.handle <- API.CpuTexture.Null

    interface IDisposable with
        member this.Dispose() = this.Dispose()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Texture =

    let ofPixImage (baker: Baker) (pi: PixImage) =
        let pi, format =
            match pi.PixFormat.Type with
            | UInt8   -> pi.ToPixImage Col.Format.Gray,          API.CpuTextureFormat.UNorm8
            | Float32 -> pi.ToPixImage Col.Format.Gray,          API.CpuTextureFormat.Fp32
            | _       -> PixImage<float32>(Col.Format.Gray, pi), API.CpuTextureFormat.Fp32

        pi.Array |> NativeInt.pin (fun pData ->
            let mutable mip = API.CpuTextureMipDesc(uint32 pi.WidthL, uint32 pi.HeightL, uint32 pi.StrideL, pData)
            let mutable desc = API.CpuTextureDesc(format, API.CpuTextureFlags.None, &&mip, 1u)

            let mutable handle = Unchecked.defaultof<API.CpuTexture>
            API.Omm.createCpuTexture(baker.Handle, &desc, &handle) |> Result.check "failed to create texture"

            new Texture (baker, handle)
        )

[<Sealed; AbstractClass; Extension>]
type BakerTextureExtensions =

    static member CreateTexture(this: Baker, pi: PixImage) = pi |> Texture.ofPixImage this