namespace Aardvark.OMM.API

open System.Runtime.InteropServices

[<Struct; StructLayout(LayoutKind.Sequential)>]
type CpuOpacityMicromapDesc =
    struct
        /// Byte offset into the opacity micromap map array.
        val offset           : uint32

        /// Micro triangle count is 4^N, where N is the subdivision level.
        val subdivisionLevel : uint16

        /// OMM input format.
        val format           : uint16
    end

[<Struct; StructLayout(LayoutKind.Sequential)>]
type CpuOpacityMicromapUsageCount =
    struct
       /// Number of OMMs with the specified subdivision level and format.
       val count            : uint32

       /// Micro triangle count is 4^N, where N is the subdivision level.
       val subdivisionLevel : uint16

       /// OMM input format.
       val format           : uint16
    end

[<Struct; StructLayout(LayoutKind.Sequential)>]
type CpuBakeResultDesc =
    struct
        val arrayData               : nativeint
        val arrayDataSize           : uint32
        val descArray               : nativeptr<CpuOpacityMicromapDesc>
        val descArrayCount          : uint32
        val descArrayHistogram      : nativeptr<CpuOpacityMicromapUsageCount>
        val descArrayHistogramCount : uint32
        val indexBuffer             : nativeint
        val indexCount              : uint32
        val indexFormat             : IndexFormat
        val indexHistogram          : nativeptr<CpuOpacityMicromapUsageCount>
        val indexHistogramCount     : uint32
    end

[<Struct>]
type CpuBakeResult =
    private | CpuBakeResult of nativeint
    member this.IsValid = let (CpuBakeResult ptr) = this in ptr <> 0n
    static member Null = CpuBakeResult 0n