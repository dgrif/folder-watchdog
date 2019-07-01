// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open System.IO
let EnumFiles path exts = Directory.GetFiles(path, exts, SearchOption.AllDirectories)
let ByteToMB szB = szB * 0.000001
let updateFSize a b = a + b
//CalcFolderSize - calculates the size of all files in a folder, returned in bytes
let CalcFolderSize folder = 
//let totalSize =
    let files = EnumFiles folder "*"
    let mutable totalSize = int64 0
    
    for eachFile in files do
        let fInfo = FileInfo(eachFile)
        let bFSize = fInfo.Length
        let strFSize = fInfo.Length.ToString()
        printfn "[+]FILE: %s\n[--]SIZE: %s bytes" eachFile strFSize
        totalSize <- updateFSize totalSize bFSize

    totalSize

[<EntryPoint>]
let main argv = 
    printfn "%A" argv
    0 // return an integer exit code
