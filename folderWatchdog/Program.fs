// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.
open System
open System.IO

let EnumFiles path exts = 
    Directory.GetFiles(path, exts, SearchOption.AllDirectories)

let ByteToMB szB = 
    szB * 0.000001

let updateFSize a b = 
    a + b

//CalcFolderSize - calculates the size of all files in a folder, returned in bytes
let DeleteFiles (files : string[]) =
    for eachFile in files do
        //printfn "Deleting: %s" eachFile
        File.Delete(eachFile)

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

let FolderWatchdogHandler folderToCheck maxSize = 
    let totalSize = CalcFolderSize folderToCheck
    printfn "[+] Folder Size: %iMB" (int64(ByteToMB ((float)totalSize)))
    let szInMb = ByteToMB ((float)totalSize)
    let files = EnumFiles folderToCheck "*"
    if szInMb >= maxSize then
        printfn "\t[!] Total size exceeds threshold: %i MB\nDeleting files!" (int64(szInMb))
        DeleteFiles files

let WatchdogTimer timerInterval wdh =
    let timer = new Timers.Timer(float timerInterval)
    
    timer.AutoReset <- true
    timer.Elapsed.Add wdh
    async {
        timer.Start()
        do! Async.Sleep timerInterval
        //DEBUG, in the program this will be infinite until program dies.
        //timer.Stop()
    }

[<EntryPoint>]
let main argv = 
    let numArgs = argv.Length
    if numArgs = 3 then
        printfn "%s %s %s" argv.[0] argv.[1] argv.[2]
        let wdHandler _ = FolderWatchdogHandler argv.[0] (argv.[1] |> float)
        let aTimer = WatchdogTimer (argv.[2] |> int) wdHandler
        Async.RunSynchronously aTimer
        
    else
        printfn "Usage: folderWatchdog.exe <folder> <Max MB size> <timer>"
        //1 // return an integer exit code
    
    0