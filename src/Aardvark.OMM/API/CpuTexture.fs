namespace Aardvark.OMM.API

open System
open System.Runtime.InteropServices

type CpuTextureFormat =
   | UNorm8 = 0
   | Fp32   = 1

[<Flags>]
type CpuTextureFlags =
   | None          = 0u

   /// Controls the internal memory layout of the texture. does not change the expected input format, it does affect the baking
   /// performance and memory footprint of the texture object.
   | DisableZOrder = 1u

/// The baker supports conservativle baking from a MIP array when the runtime wants to pick freely between texture levels at
/// runtime without the need to update the OMM data. _However_ baking from mip level 0 only is recommended in the general
/// case for best performance the integration guide contains more in depth discussion on the topic
[<StructLayout(LayoutKind.Sequential)>]
type CpuTextureMipDesc =
   struct
      val mutable width : uint32
      val mutable height : uint32
      val mutable rowPitch : uint32
      val mutable textureData : nativeint
      new (width, height, rowPitch, data) = { width = width; height = height; rowPitch = rowPitch; textureData = data }
   end

[<StructLayout(LayoutKind.Sequential)>]
type CpuTextureDesc =
   struct
      val mutable format : CpuTextureFormat
      val mutable flags : CpuTextureFlags
      val mutable mips : nativeptr<CpuTextureMipDesc>
      val mutable mipCount : uint32
      /// Setting the alphaCutoff [0,1] allows the alpha cutoff to be embeded in the texture object which may accelerate the
      /// baking operation in some circumstances. Note: if set it must match the alphaCutoff in the bake desc exactly.
      val mutable alphaCutoff : float32

      new (format, flags, mips, mipCount, [<Optional; DefaultParameterValue(-1.0f)>] alphaCutOff) =
         {
            format = format
            flags = flags
            mips = mips
            mipCount = mipCount
            alphaCutoff = alphaCutOff
         }
   end

 [<Struct>]
 type CpuTexture =
    private | CpuTexture of nativeint
    member this.IsValid = let (CpuTexture ptr) = this in ptr <> 0n
    static member Null = CpuTexture 0n