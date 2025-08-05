namespace Aardvark.OMM.API

open Aardvark.OMM
open System.Runtime.InteropServices
open System.Security

type Result =
    | Success                   = 0
    | Failure                   = 1
    | InvalidArgument           = 2
    | InsufficientScratchMemory = 3
    | NotImplemented            = 4
    | WorkloadTooBig            = 5

[<SuppressUnmanagedCodeSecurity>]
module internal Omm =

    [<Literal>]
    let private lib = "omm-lib"

    [<DllImport(lib, EntryPoint = "ommGetLibraryDesc")>]
    extern LibraryDesc getLibraryDesc()


    [<DllImport(lib, EntryPoint = "ommCreateBaker")>]
    extern Result createBaker([<In>] BakerCreationDesc& createInfo, [<Out>] Baker& outBaker)

    [<DllImport(lib, EntryPoint = "ommDestroyBaker")>]
    extern Result destroyBaker(Baker baker)


    [<DllImport(lib, EntryPoint = "ommCpuCreateTexture")>]
    extern Result createCpuTexture(Baker baker, [<In>] CpuTextureDesc& desc, [<Out>] CpuTexture& outTexture)

    [<DllImport(lib, EntryPoint = "ommCpuGetTextureDesc")>]
    extern Result createGetCpuTextureDesc(CpuTexture texture, [<Out>] CpuTextureDesc& outDesc)

    [<DllImport(lib, EntryPoint = "ommCpuDestroyTexture")>]
    extern Result destroyCpuTexture(Baker baker, CpuTexture texture)