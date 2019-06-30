open System
open System.IO
let EnumFiles path exts = Directory.GetFiles(path, exts, SearchOption.AllDirectories)

let DeleteFiles files =
    for eachFile in files do
        //printfn "Deleting: %s" eachFile
        File.Delete(eachFile)
        

let ByteToMB szB = szB * 0.000001
//let MBtoByte szM = szM / 0.000001 //TODO: verify this, it's prob incorrect
let updateFSize a b = a + b

//let files = EnumFiles "C:\\sr\\" "*"

let watchFolder folder maxSize timerInterval = 
//let totalSize =
    let files = EnumFiles folder "*"
    let mutable totalSize = int64 0
    let timer = new System.Timers.Timer(float timerInterval)

    for eachFile in files do
        let fInfo = FileInfo(eachFile)
        let bFSize = fInfo.Length
        let strFSize = fInfo.Length.ToString()
        printfn "[+]FILE: %s\n[--]SIZE: %s bytes" eachFile strFSize
        totalSize <- updateFSize totalSize bFSize

    let szInMb = ByteToMB ((float)totalSize)
    printfn "Total size: %i MB" (int64(szInMb))
    totalSize