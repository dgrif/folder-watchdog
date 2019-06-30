// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open System.IO
let EnumFiles path exts = Directory.GetFiles(path, exts, SearchOption.AllDirectories)
let ByteToMB szB = szB * 0.000001
let updateFSize a b = a + b

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
