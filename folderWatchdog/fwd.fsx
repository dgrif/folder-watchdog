open System.IO
let EnumFiles path exts = Directory.GetFiles(path, exts, SearchOption.AllDirectories)
let ByteToMB szB = szB * 0.000001
let files = EnumFiles "C:\\sr\\" "*"

let mutable totalSize = int64 0
let updateFSize a b = a + b
//let totalSize =
for eachFile in files do
    let fInfo = FileInfo(eachFile)
    let bFSize = fInfo.Length
    let strFSize = fInfo.Length.ToString()
    printfn "[+]FILE: %s\n[--]SIZE: %s bytes" eachFile strFSize
    totalSize <- updateFSize totalSize bFSize

let szInMb = ByteToMB ((float)totalSize)
printfn "Total size: %i MB" (int64(szInMb))
