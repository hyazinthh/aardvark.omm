namespace Aardvark.OMM.API

open System.Runtime.InteropServices

type TextureAddressMode =
    | Wrap       = 0
    | Mirror     = 1
    | Clamp      = 2
    | Border     = 3
    | MirrorOnce = 4

type TextureFilterMode =
    | Nearest = 0
    | Linear  = 1

[<StructLayout(LayoutKind.Sequential)>]
type SamplerDesc =
    struct
        val mutable addressMode : TextureAddressMode
        val mutable filterMode : TextureFilterMode
        val mutable borderAlpha : float32

        new (addressMode, filterMode, borderAlpha) = { addressMode = addressMode; filterMode = filterMode; borderAlpha = borderAlpha }
    end