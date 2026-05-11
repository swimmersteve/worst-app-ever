# Install .NET 8 SDK on Windows (x64)
# Run this script as Administrator in PowerShell

$ErrorActionPreference = "Stop"

$installerUrl = "https://dot.net/v1/dotnet-install.ps1"
$installScript = "$env:TEMP\dotnet-install.ps1"

Write-Host "Downloading dotnet-install.ps1 from Microsoft..." -ForegroundColor Cyan
Invoke-WebRequest -Uri $installerUrl -OutFile $installScript -UseBasicParsing

Write-Host "Installing .NET 8 SDK..." -ForegroundColor Cyan
& $installScript -Channel 8.0 -InstallDir "$env:ProgramFiles\dotnet"

# Add to system PATH if not already there
$dotnetPath = "$env:ProgramFiles\dotnet"
$currentPath = [System.Environment]::GetEnvironmentVariable("PATH", "Machine")

if ($currentPath -notlike "*$dotnetPath*") {
    Write-Host "Adding $dotnetPath to system PATH..." -ForegroundColor Cyan
    [System.Environment]::SetEnvironmentVariable(
        "PATH",
        "$currentPath;$dotnetPath",
        "Machine"
    )
    Write-Host "PATH updated. Restart your terminal for it to take effect." -ForegroundColor Yellow
}

# Verify
$env:PATH = "$env:PATH;$dotnetPath"
Write-Host ""
Write-Host "Installed version:" -ForegroundColor Green
& "$dotnetPath\dotnet.exe" --version

Write-Host ""
Write-Host "Done! You can now run:" -ForegroundColor Green
Write-Host "  dotnet build src\WorstAppEver\WorstAppEver.csproj" -ForegroundColor White
Write-Host "  dotnet run --project src\WorstAppEver\WorstAppEver.csproj" -ForegroundColor White
