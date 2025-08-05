namespace Aardvark.OMM

open System
open System.Runtime.InteropServices

type OmmException(error : API.Result, message : string, [<Optional; DefaultParameterValue(null : Exception)>] innerException : Exception) =
    inherit Exception(message, innerException)

    new(message : string, [<Optional; DefaultParameterValue(null : Exception)>] innerException : Exception) =
        OmmException(API.Result.Failure, message, innerException)

    member x.Error = error
    override x.Message = $"{message} (Error: {error})"

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Result =
    let check (message: string) (result: API.Result) =
        if result <> API.Result.Success then
            let msg =
                if String.IsNullOrEmpty message then "An error occurred"
                else string (Char.ToUpper message.[0]) + message.Substring(1)

            raise <| OmmException(result, msg)