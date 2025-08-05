namespace Aardvark.OMM.API

open Aardvark.OMM
open System.Runtime.InteropServices

type MessageCallback = delegate of MessageSeverity * string * nativeint -> unit

[<StructLayout(LayoutKind.Sequential)>]
type MessageInterface =
    struct
        val mutable callback : MessageCallback
        val mutable userArg : nativeint
        new (callback, userArg) = { callback = callback; userArg = userArg }
        new (callback) = { callback = callback; userArg = 0n }
    end