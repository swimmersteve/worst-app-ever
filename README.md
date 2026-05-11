# Worst App Ever

A Windows desktop tool for deliberately triggering crashes, hangs, resource exhaustion, and system faults. Useful for testing crash reporters, watchdog processes, monitoring tools, and recovery workflows.

## Requirements

- Windows 10/11
- .NET 8 SDK
- Administrator privileges (required for BSOD only)

## Build & Run

```powershell
"C:\Program Files\dotnet\dotnet.exe" run --project src\WorstAppEver\WorstAppEver.csproj
```

Release build:

```powershell
"C:\Program Files\dotnet\dotnet.exe" build src\WorstAppEver\WorstAppEver.csproj -c Release
```

Output: `src\WorstAppEver\bin\Release\net8.0-windows\WorstAppEver.exe`

## Features

### Crash
Terminates the process immediately. Windows Event Viewer will log an application error entry.

| Scenario | Mechanism |
|---|---|
| Access Violation | Reads from address 0x0; SEH exception terminates via WER |
| Stack Overflow | Infinite recursion exhausts the call stack; CLR cannot handle |
| Unhandled Exception | `InvalidOperationException` with no catch block |
| Division by Zero | Integer divide-by-zero raises `DivideByZeroException` |
| FailFast / Abort | `Environment.FailFast()` — bypasses all managed handlers |
| Out of Memory | Allocates and touches 128 MB chunks until memory is exhausted |

### Hang
Blocks the UI thread. Use Task Manager (`Ctrl+Shift+Esc`) to kill the process.

| Scenario | Mechanism |
|---|---|
| Sleep Forever | `Thread.Sleep(Infinite)` on the UI thread |
| CPU Spin | `while(true)` on the UI thread |
| Deadlock | Classic two-lock deadlock between UI and background thread |
| Await Never Completes | `.GetAwaiter().GetResult()` on an unsignaled `TaskCompletionSource` |

### CPU Stress
Spawns worker threads that busy-spin for a configurable percentage of each 100ms window. Adjustable thread count (1–64) and load percentage (1–99%).

### Memory Pressure
Allocates byte arrays and touches every 4 KB page to commit physical RAM. Memory is held until explicitly released. Configurable target from 256 MB to 32 GB.

### Disk I/O Stress
Continuously writes then reads a temp file in a background loop. Configurable buffer size. Activity is visible in Resource Monitor.

### BSOD
Calls `NtRaiseHardError()` via `ntdll.dll` with `OptionShutdownSystem`. The OS immediately blue-screens and restarts. **All unsaved work across all applications will be lost.**

- Requires Administrator
- Type `BSOD` in the confirmation box before the button activates
- Test on a VM first — stop code will be `CRITICAL_PROCESS_DIED (0x000000EF)`
