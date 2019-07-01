open System
open System.IO
open System.Threading

let EnumFiles path exts = Directory.GetFiles(path, exts, SearchOption.AllDirectories)

let DeleteFiles (files : string[]) =
    for eachFile in files do
        //printfn "Deleting: %s" eachFile
        File.Delete(eachFile)
        

let ByteToMB szB = szB * 0.000001
//let MBtoByte szM = szM / 0.000001 //TODO: verify this, it's prob incorrect
let updateFSize a b = a + b

//let files = EnumFiles "C:\\sr\\" "*"

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

let FolderWatchdogHandler folderToCheck maxSize = 
    let totalSize = CalcFolderSize folderToCheck
    printfn "[ ] Folder Size: %i" (int64(ByteToMB ((float)totalSize)))
    let szInMb = ByteToMB ((float)totalSize)
    let files = EnumFiles folderToCheck "*"
    if szInMb >= maxSize then
        printfn "\t[!] Total size exceeds threashold: %i MB\nDeleting files!" (int64(szInMb))
        DeleteFiles files

let WatchdogTimer timerInterval wdh =
    let timer = new Timers.Timer(float timerInterval)
    
    timer.AutoReset <- true
    timer.Elapsed.Add wdh
    async {
        timer.Start()
        do! Async.Sleep timerInterval
    }

// folder and time should be variables, input as cli args or prompted for if none
let wdHandler _ = FolderWatchdogHandler "C:\\temp" 100000.0
let aTimer = WatchdogTimer 30000 wdHandler
Async.RunSynchronously aTimer
//let wdt = WatchdogTimer 30 (FolderWatchdogHandler "C:\\temp" 100000.0)