namespace Aardvark.OMM.API

open System.Runtime.InteropServices

#nowarn "9"

type BakerType =
    | GPU = 0
    | CPU = 1

[<StructLayout(LayoutKind.Explicit, Size = 56)>]
type BakerCreationDesc =
    struct
        [<FieldOffset(0)>]
        val mutable typ : BakerType
        [<FieldOffset(40)>]
        val mutable messageInterface : MessageInterface

        new (typ, msg) = { typ = typ; messageInterface = msg }
        new (typ, callback) = BakerCreationDesc(typ, MessageInterface(callback))
        new (typ) = BakerCreationDesc(typ, Unchecked.defaultof<MessageInterface>)
    end

[<Struct>]
type Baker =
    private | Baker of nativeint
    member this.IsValid = let (Baker ptr) = this in ptr <> 0n
    static member Null = Baker 0n