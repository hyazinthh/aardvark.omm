namespace Aardvark.OMM

open System
open System.Runtime.CompilerServices
open Aardvark.Base

#nowarn "51"

type Texture =
    val private baker : Baker
    val mutable private handle : API.CpuTexture

    internal new (baker, handle) = { baker = baker; handle = handle }

    member this.Handle = this.handle

    member this.Dispose() =
        if this.handle.IsValid then
            API.Omm.cpuDestroyTexture(this.baker.Handle, this.handle) |> Result.check "failed to destroy texture"
            this.handle <- API.CpuTexture.Null

    interface IDisposable with
        member this.Dispose() = this.Dispose()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Texture =

    let private getAlphaImage (pi: PixImage<'T>) : PixImage * int64 =
        let pi =
            if pi.ChannelCount = 1 then pi
            else
                let channels = pi.Format.ChannelsOfFormat()
                let channelIndex = channels |> Array.tryFindIndex ((=) Col.Channel.Alpha) |> Option.defaultValue 0
                PixImage<'T>(Col.Format.Alpha, pi.GetChannelInFormatOrder <| int64 channelIndex)

        pi, pi.VolumeInfo.FirstIndex * int64 sizeof<'T>

    let ofPixImage (baker: Baker) (pi: PixImage) =
        let (pi, offset), format =
            match pi with
            | :? PixImage<uint8> as pi   -> getAlphaImage pi, API.CpuTextureFormat.UNorm8
            | :? PixImage<float32> as pi -> getAlphaImage pi, API.CpuTextureFormat.Fp32
            | _ ->
                let pi = PixImage<float32>(pi.Format, pi)
                getAlphaImage pi, API.CpuTextureFormat.Fp32

        pi.Array |> NativeInt.pin (fun pData ->
            let mutable mip = API.CpuTextureMipDesc(uint32 pi.WidthL, uint32 pi.HeightL, uint32 pi.StrideL, pData + nativeint offset)
            let mutable desc = API.CpuTextureDesc(format, API.CpuTextureFlags.None, &&mip, 1u)

            let mutable handle = API.CpuTexture.Null
            API.Omm.cpuCreateTexture(baker.Handle, &desc, &handle) |> Result.check "failed to create texture"

            new Texture (baker, handle)
        )

[<Sealed; AbstractClass; Extension>]
type BakerTextureExtensions =

    static member CreateTexture(this: Baker, pi: PixImage) = pi |> Texture.ofPixImage this