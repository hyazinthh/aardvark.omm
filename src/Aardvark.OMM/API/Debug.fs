namespace Aardvark.OMM.API

open System.Runtime.InteropServices

[<Struct; StructLayout(LayoutKind.Sequential)>]
type DebugSaveImagesDesc =
    struct
        val mutable path : string

        val mutable filePostfix : string

        /// The default behaviour is to dump the entire alpha texture with the OMM-triangle in it. Enabling detailedCutout will
        /// generate cropped version zoomed in on the OMM, and supersampled for detailed analysis
        val mutable detailedCutout : uint8

        /// Only dump index 0.
        val mutable dumpOnlyFirstOMM : uint8

        /// Will draw unknown transparent and unknown opaque in the same color.
        val mutable monochromeUnknowns : uint8

        /// true:Will draw all primitives to the same file. false: will draw each primitive separatley.
        val mutable oneFile : uint8

        new (path, filePostfix, detailedCutout, dumpOnlyFirstOMM, monochromeUnknowns, oneFile) =
            {
                path                = path
                filePostfix         = filePostfix
                detailedCutout      = if detailedCutout then 1uy else 0uy
                dumpOnlyFirstOMM    = if dumpOnlyFirstOMM then 1uy else 0uy
                monochromeUnknowns  = if monochromeUnknowns then 1uy else 0uy
                oneFile             = if oneFile then 1uy else 0uy
            }
    end

[<Struct; StructLayout(LayoutKind.Sequential)>]
type DebugStats =
    struct
        val totalOpaque : uint64
        val totalTransparent : uint64
        val totalUnknownTransparent : uint64
        val totalUnknownOpaque : uint64
        val totalFullyOpaque : uint32
        val totalFullyTransparent : uint32
        val totalFullyUnknownOpaque : uint32
        val totalFullyUnknownTransparent : uint32
        /// this is known area in uv space, divided by the total uv space. -1.f if unknown
        val knownAreaMetric : float32
    end
