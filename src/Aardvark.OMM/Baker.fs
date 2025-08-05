namespace Aardvark.OMM

open System

type Baker private (messageCallback : Func<MessageSeverity, string, unit>) =
    let mutable handle =
        let mutable baker = Unchecked.defaultof<API.Baker>

        let messageCallback =
            if isNull messageCallback then
                null
            else
                API.MessageCallback(fun severity message _ -> messageCallback.Invoke(severity, message))

        let mutable desc = API.BakerCreationDesc(BakerType.CPU, messageCallback)
        API.Omm.createBaker(&desc, &baker) |> Result.check "failed to create baker"
        baker

    new () = new Baker(null)
    new (messageCallback: MessageSeverity -> string -> unit) = new Baker(Func<_, _, _> messageCallback)

    member _.Handle = handle

    static member LibraryVersion =
        let desc = API.Omm.getLibraryDesc()
        Version(int desc.versionMajor, int desc.versionMinor, int desc.versionBuild)

    member _.Dispose() =
        if handle.IsValid then
            API.Omm.destroyBaker handle |> Result.check "failed to destroy baker"
            handle <- API.Baker.Null

    interface IDisposable with
        member this.Dispose() = this.Dispose()