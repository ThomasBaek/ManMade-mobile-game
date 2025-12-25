# ============================================
# Made Man: Idle Empire - Windows Setup Script
# ============================================
# 
# KØR SÅDAN:
# 1. Åbn PowerShell som Administrator
# 2. Naviger til hvor du vil have projektet (fx: cd C:\Projects)
# 3. Kør: .\setup_mademan.ps1
#
# Hvis du får "execution policy" fejl, kør først:
# Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
# ============================================

Write-Host ""
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host "  MADE MAN: IDLE EMPIRE - Setup" -ForegroundColor Cyan
Write-Host "=======================================" -ForegroundColor Cyan
Write-Host ""

# Tjek .NET
Write-Host "Checking prerequisites..." -ForegroundColor Yellow

try {
    $dotnetVersion = dotnet --version
    Write-Host "  [OK] .NET SDK: $dotnetVersion" -ForegroundColor Green
} catch {
    Write-Host "  [ERROR] .NET SDK not found!" -ForegroundColor Red
    Write-Host "  Download from: https://dotnet.microsoft.com/download" -ForegroundColor Red
    exit 1
}

# Tjek MAUI workload
$workloads = dotnet workload list
if ($workloads -notmatch "maui") {
    Write-Host "  [INFO] Installing MAUI workload..." -ForegroundColor Yellow
    dotnet workload install maui
}
Write-Host "  [OK] MAUI workload ready" -ForegroundColor Green

Write-Host ""

# ============================================
# TRIN 1: Opret projekt
# ============================================
Write-Host "Step 1: Creating project..." -ForegroundColor Yellow

$projectRoot = "MadeMan.IdleEmpire"

# Fjern eksisterende
if (Test-Path $projectRoot) {
    Write-Host "  Removing existing folder..." -ForegroundColor Gray
    Remove-Item -Recurse -Force $projectRoot
}

# Opret MAUI projekt
dotnet new maui -n MadeMan.IdleEmpire -o $projectRoot

Write-Host "  [OK] MAUI project created" -ForegroundColor Green

# ============================================
# TRIN 2: Tilføj NuGet packages
# ============================================
Write-Host ""
Write-Host "Step 2: Adding NuGet packages..." -ForegroundColor Yellow

Set-Location "$projectRoot\MadeMan.IdleEmpire"
dotnet add package CommunityToolkit.Mvvm --version 8.2.2

Write-Host "  [OK] CommunityToolkit.Mvvm added" -ForegroundColor Green

# ============================================
# TRIN 3: Opret mappestruktur
# ============================================
Write-Host ""
Write-Host "Step 3: Creating folders..." -ForegroundColor Yellow

New-Item -ItemType Directory -Force -Path "Models" | Out-Null
New-Item -ItemType Directory -Force -Path "ViewModels" | Out-Null
New-Item -ItemType Directory -Force -Path "Views" | Out-Null
New-Item -ItemType Directory -Force -Path "Services" | Out-Null
New-Item -ItemType Directory -Force -Path "Resources\Styles" | Out-Null
New-Item -ItemType Directory -Force -Path "Resources\Images" | Out-Null

Write-Host "  [OK] Folders created" -ForegroundColor Green

# Gå tilbage til rod
Set-Location ..

# ============================================
# TRIN 4: Opret .claude struktur
# ============================================
Write-Host ""
Write-Host "Step 4: Creating Claude Code structure..." -ForegroundColor Yellow

New-Item -ItemType Directory -Force -Path ".claude" | Out-Null

# Settings.json
$settingsJson = @'
{
  "project": {
    "name": "MadeMan.IdleEmpire",
    "type": "mobile-game",
    "framework": ".NET MAUI",
    "version": "1.0.0-mvp"
  },
  "build": {
    "target": "net8.0-android",
    "min_sdk": 26
  },
  "commands": {
    "build": "dotnet build -f net8.0-android",
    "run": "dotnet build -f net8.0-android -t:Run",
    "clean": "dotnet clean"
  }
}
'@
$settingsJson | Out-File -FilePath ".claude\settings.json" -Encoding UTF8

Write-Host "  [OK] .claude\settings.json created" -ForegroundColor Green

# ============================================
# TRIN 5: Opret CLAUDE.md
# ============================================
Write-Host ""
Write-Host "Step 5: Creating CLAUDE.md..." -ForegroundColor Yellow

$claudeMd = @'
# CLAUDE.md - Made Man: Idle Empire

## Projekt Oversigt
**Idle/incremental mobile game** bygget med .NET MAUI.
Spilleren bygger et kriminelt imperium i 1930'ernes New Porto.

## Tech Stack
- **Framework:** .NET 8 MAUI
- **Pattern:** MVVM med CommunityToolkit.Mvvm
- **Storage:** Preferences API (JSON)
- **Target:** Android 8.0+ (API 26)

## Projekt Struktur
```
MadeMan.IdleEmpire/
├── Models/           # GameState, Operation, GameConfig
├── ViewModels/       # MainViewModel, OperationViewModel
├── Views/            # MainPage.xaml (single screen)
├── Services/         # GameEngine, SaveManager
└── Resources/        # Styles, Images
```

## Build Kommandoer
```powershell
dotnet build -f net8.0-android          # Build
dotnet build -f net8.0-android -t:Run   # Run on emulator
dotnet clean                             # Clean
```

## Implementerings Raekkefoelge
1. Models (GameState.cs, Operation.cs, GameConfig.cs)
2. Services (SaveManager.cs, GameEngine.cs)
3. ViewModels (OperationViewModel.cs, MainViewModel.cs)
4. Resources/Styles (Theme.xaml)
5. Views (MainPage.xaml)
6. MauiProgram.cs (DI setup)
7. App.xaml.cs (lifecycle)

## Balance Tal
| Operation | Income | Unlock |
|-----------|--------|--------|
| Pickpocket | $1/s | FREE |
| Car Theft | $4/s | $50 |
| Burglary | $15/s | $250 |
| Speakeasy | $50/s | $1,000 |
| Casino | $200/s | $5,000 |

## Farver
- Background: #1A1A2E
- Surface: #16213E  
- Primary: #E94560
- Gold: #FFD700
- Success: #4ADE80

## Status
- [x] Projekt oprettet
- [x] Packages installeret
- [x] Mappestruktur klar
- [ ] Models
- [ ] Services
- [ ] ViewModels
- [ ] UI
- [ ] Test
'@
$claudeMd | Out-File -FilePath "CLAUDE.md" -Encoding UTF8

Write-Host "  [OK] CLAUDE.md created" -ForegroundColor Green

# ============================================
# TRIN 6: Test build
# ============================================
Write-Host ""
Write-Host "Step 6: Testing build..." -ForegroundColor Yellow

Set-Location "MadeMan.IdleEmpire"
$buildResult = dotnet build -f net8.0-android 2>&1

if ($LASTEXITCODE -eq 0) {
    Write-Host "  [OK] Build successful!" -ForegroundColor Green
} else {
    Write-Host "  [WARN] Build had issues - check output" -ForegroundColor Yellow
}

Set-Location ..

# ============================================
# DONE!
# ============================================
Write-Host ""
Write-Host "=======================================" -ForegroundColor Green
Write-Host "  SETUP COMPLETE!" -ForegroundColor Green
Write-Host "=======================================" -ForegroundColor Green
Write-Host ""
Write-Host "Project location: $(Get-Location)\$projectRoot" -ForegroundColor Cyan
Write-Host ""
Write-Host "Folder structure:" -ForegroundColor White
Write-Host "  MadeMan.IdleEmpire\"
Write-Host "  +-- .claude\"
Write-Host "  |   +-- settings.json"
Write-Host "  +-- CLAUDE.md"
Write-Host "  +-- MadeMan.IdleEmpire\"
Write-Host "      +-- Models\"
Write-Host "      +-- ViewModels\"
Write-Host "      +-- Views\"
Write-Host "      +-- Services\"
Write-Host "      +-- Resources\"
Write-Host ""
Write-Host "NEXT STEPS:" -ForegroundColor Yellow
Write-Host "  1. Start Android emulator (from Android Studio)"
Write-Host "  2. cd MadeMan.IdleEmpire\MadeMan.IdleEmpire"
Write-Host "  3. dotnet build -f net8.0-android -t:Run"
Write-Host ""
Write-Host "For Claude Code, say:" -ForegroundColor Cyan
Write-Host '  "Laes CLAUDE.md og lav en implementeringsplan"'
Write-Host ""
