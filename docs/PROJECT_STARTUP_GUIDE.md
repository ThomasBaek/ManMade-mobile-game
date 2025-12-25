# 🚀 MADE MAN: IDLE EMPIRE
## Projekt Opstarts Guide

---

# FORMÅL

Denne guide hjælper dig med at:
1. Oprette et tomt .NET MAUI projekt
2. Konfigurere Claude Code struktur (CLAUDE.md + .claude/)
3. Verificere at projektet kører på Android emulator
4. Være klar til implementering

---

# FORUDSÆTNINGER

## Påkrævet Software

```bash
# Tjek .NET version (kræver 8.0+)
dotnet --version

# Tjek MAUI workload
dotnet workload list
```

### Installer MAUI Workload (hvis mangler)
```bash
dotnet workload install maui
```

### Android Setup
- Android Studio installeret (for emulator)
- Android SDK 33+ (API 33)
- En kørende Android emulator ELLER fysisk enhed med USB debugging

---

# TRIN 1: OPRET PROJEKT

## Kør disse kommandoer i terminal

```bash
# 1. Opret projekt mappe
mkdir MadeMan.IdleEmpire
cd MadeMan.IdleEmpire

# 2. Opret MAUI solution
dotnet new maui -n MadeMan.IdleEmpire

# 3. Gå ind i projekt mappen
cd MadeMan.IdleEmpire

# 4. Tilføj NuGet packages
dotnet add package CommunityToolkit.Mvvm --version 8.2.2

# 5. Verificer projekt struktur
ls -la
```

## Forventet Mappestruktur
```
MadeMan.IdleEmpire/
├── MadeMan.IdleEmpire.sln
└── MadeMan.IdleEmpire/
    ├── MadeMan.IdleEmpire.csproj
    ├── App.xaml
    ├── App.xaml.cs
    ├── AppShell.xaml
    ├── AppShell.xaml.cs
    ├── MainPage.xaml
    ├── MainPage.xaml.cs
    ├── MauiProgram.cs
    ├── Platforms/
    │   ├── Android/
    │   ├── iOS/
    │   ├── MacCatalyst/
    │   ├── Tizen/
    │   └── Windows/
    └── Resources/
        ├── AppIcon/
        ├── Fonts/
        ├── Images/
        ├── Raw/
        └── Splash/
```

---

# TRIN 2: OPRET CLAUDE CODE STRUKTUR

## Opret .claude mappe og filer

```bash
# Fra rod-mappen (MadeMan.IdleEmpire/)
cd ..  # Gå tilbage til rod hvis du er i projekt-mappen

# Opret .claude mappe
mkdir -p .claude

# Opret settings.json
cat > .claude/settings.json << 'EOF'
{
  "project_name": "MadeMan.IdleEmpire",
  "language": "da",
  "framework": ".NET MAUI",
  "target_platform": "Android",
  "coding_style": {
    "naming": "PascalCase for public, _camelCase for private fields",
    "pattern": "MVVM with CommunityToolkit.Mvvm",
    "null_handling": "Nullable reference types enabled"
  }
}
EOF

# Opret commands.md (ofte brugte kommandoer)
cat > .claude/commands.md << 'EOF'
# Ofte Brugte Kommandoer

## Build
```bash
dotnet build -f net8.0-android
```

## Run på Emulator
```bash
dotnet build -f net8.0-android -t:Run
```

## Clean Build
```bash
dotnet clean
dotnet build -f net8.0-android
```

## Publish APK
```bash
dotnet publish -f net8.0-android -c Release
```

## List Android Devices
```bash
adb devices
```
EOF

echo "✅ .claude struktur oprettet"
```

---

# TRIN 3: OPRET CLAUDE.md

## Opret hovedfilen for Claude Code kontekst

```bash
cat > CLAUDE.md << 'EOF'
# CLAUDE.md - Made Man: Idle Empire

## Projekt Oversigt
Dette er et **idle/incremental mobile game** bygget med .NET MAUI.
Spilleren bygger et kriminelt imperium i 1930'ernes New Porto.

## Tech Stack
- **Framework:** .NET 8 MAUI
- **Pattern:** MVVM med CommunityToolkit.Mvvm
- **UI:** XAML
- **Storage:** Preferences API (JSON serialization)
- **Target:** Android 8.0+ (API 26)

## Projekt Struktur (Planlagt)
```
MadeMan.IdleEmpire/
├── Models/           # Data modeller (GameState, Operation)
├── ViewModels/       # MVVM ViewModels
├── Views/            # XAML Pages
├── Services/         # Game logic, Save/Load
└── Resources/        # Icons, Styles, Fonts
```

## Kode Konventioner
- **Navngivning:** PascalCase for public, _camelCase for private fields
- **Async:** Brug async/await for I/O operationer
- **DI:** Registrer services i MauiProgram.cs
- **MVVM:** Brug [ObservableProperty] og [RelayCommand] attributter

## Build Kommandoer
```bash
# Build til Android
dotnet build -f net8.0-android

# Kør på emulator
dotnet build -f net8.0-android -t:Run

# Publish APK
dotnet publish -f net8.0-android -c Release
```

## Nuværende Status
- [x] Projekt oprettet
- [x] NuGet packages installeret
- [ ] Models implementeret
- [ ] Services implementeret
- [ ] ViewModels implementeret
- [ ] UI implementeret
- [ ] Test på emulator

## Næste Skridt
1. Opret mappestruktur (Models, ViewModels, Views, Services)
2. Implementer GameState og Operation models
3. Implementer GameEngine service
4. Implementer MainViewModel
5. Design MainPage UI
6. Test på Android emulator

## Vigtige Filer
- `MauiProgram.cs` - Dependency Injection setup
- `App.xaml.cs` - App lifecycle (save on sleep)
- `MainPage.xaml` - Hoved UI

## Game Design Reference
Se projektets design dokumenter for:
- Balance tal (unlock costs, income rates)
- UI mockups
- Prestige system regler
- Fremtidige features

## Fejlfinding
### Emulator starter ikke
```bash
# List tilgængelige emulatorer
emulator -list-avds

# Start specifik emulator
emulator -avd <emulator_name>
```

### Build fejler
```bash
# Clean og rebuild
dotnet clean
dotnet restore
dotnet build -f net8.0-android
```
EOF

echo "✅ CLAUDE.md oprettet"
```

---

# TRIN 4: OPRET MAPPESTRUKTUR

```bash
# Gå ind i projekt mappen
cd MadeMan.IdleEmpire

# Opret mapper til koden
mkdir -p Models
mkdir -p ViewModels  
mkdir -p Views
mkdir -p Services
mkdir -p Resources/Styles

echo "✅ Mappestruktur oprettet"
```

---

# TRIN 5: VERIFICER BUILD

```bash
# Build projektet (første gang tager længere tid)
dotnet build -f net8.0-android

# Forventet output: "Build succeeded"
```

---

# TRIN 6: TEST PÅ EMULATOR

## Start Android Emulator først
```bash
# List tilgængelige emulatorer
emulator -list-avds

# Start emulator (erstat med dit emulator navn)
emulator -avd Pixel_6_API_33 &
```

## Kør appen
```bash
# Deploy og kør på emulator
dotnet build -f net8.0-android -t:Run
```

## Forventet Resultat
- Appen starter på emulator
- Viser standard MAUI "Hello, World!" skærm
- Ingen crash = alt virker! ✅

---

# KOMPLET SCRIPT

## Alt-i-én setup script

Gem dette som `setup.sh` og kør det:

```bash
#!/bin/bash

echo "🎮 Setting up Made Man: Idle Empire..."

# Opret rod mappe
mkdir -p MadeMan.IdleEmpire
cd MadeMan.IdleEmpire

# Opret MAUI projekt
echo "📦 Creating MAUI project..."
dotnet new maui -n MadeMan.IdleEmpire

# Tilføj packages
echo "📚 Adding NuGet packages..."
cd MadeMan.IdleEmpire
dotnet add package CommunityToolkit.Mvvm --version 8.2.2

# Opret mapper
echo "📁 Creating folder structure..."
mkdir -p Models ViewModels Views Services Resources/Styles

# Gå tilbage til rod
cd ..

# Opret .claude mappe
echo "🤖 Setting up Claude Code structure..."
mkdir -p .claude

# Settings
cat > .claude/settings.json << 'SETTINGS'
{
  "project_name": "MadeMan.IdleEmpire",
  "language": "da",
  "framework": ".NET MAUI",
  "target_platform": "Android"
}
SETTINGS

# CLAUDE.md
cat > CLAUDE.md << 'CLAUDEMD'
# CLAUDE.md - Made Man: Idle Empire

## Projekt Oversigt
Idle/incremental mobile game bygget med .NET MAUI.
Spilleren bygger et kriminelt imperium i 1930'ernes New Porto.

## Tech Stack
- Framework: .NET 8 MAUI
- Pattern: MVVM med CommunityToolkit.Mvvm
- Target: Android 8.0+ (API 26)

## Build
```bash
dotnet build -f net8.0-android
dotnet build -f net8.0-android -t:Run
```

## Status
- [x] Projekt oprettet
- [ ] Implementering startet
CLAUDEMD

# Build test
echo "🔨 Building project..."
cd MadeMan.IdleEmpire
dotnet build -f net8.0-android

echo ""
echo "✅ Setup complete!"
echo ""
echo "📍 Projekt lokation: $(pwd)"
echo ""
echo "🚀 Næste skridt:"
echo "   1. Start Android emulator"
echo "   2. Kør: dotnet build -f net8.0-android -t:Run"
echo "   3. Begynd implementering!"
```

---

# EFTER SETUP: PROJEKT OVERBLIK

## Fil Struktur
```
MadeMan.IdleEmpire/
├── .claude/
│   └── settings.json
├── CLAUDE.md
├── MadeMan.IdleEmpire.sln
└── MadeMan.IdleEmpire/
    ├── Models/           (tom - klar til implementering)
    ├── ViewModels/       (tom - klar til implementering)
    ├── Views/            (tom - klar til implementering)
    ├── Services/         (tom - klar til implementering)
    ├── Resources/
    │   └── Styles/       (tom - klar til Theme.xaml)
    ├── App.xaml
    ├── App.xaml.cs
    ├── MainPage.xaml
    ├── MainPage.xaml.cs
    └── MauiProgram.cs
```

## Hvad er klar?
- ✅ Tomt MAUI projekt der bygger
- ✅ CommunityToolkit.Mvvm installeret
- ✅ Mappestruktur oprettet
- ✅ Claude Code kontekst (CLAUDE.md)
- ✅ Klar til at køre på emulator

## Hvad mangler? (Implementeringsplan)
1. **Models/** - GameState.cs, Operation.cs, GameConfig.cs
2. **Services/** - IGameEngine.cs, GameEngine.cs, SaveManager.cs
3. **ViewModels/** - MainViewModel.cs, OperationViewModel.cs
4. **Views/** - MainPage.xaml (redesign)
5. **Resources/Styles/** - Theme.xaml (farver)
6. **Resources/Images/** - SVG ikoner

---

# BRUG MED CLAUDE CODE

## Start implementering
Når projektet er oprettet, sig til Claude Code:

```
"Læs CLAUDE.md og projektfilerne. 
Lav en implementeringsplan for Made Man: Idle Empire.
Start med at implementere Models."
```

## Claude Code vil:
1. Læse CLAUDE.md for kontekst
2. Forstå projekt strukturen
3. Implementere filer i korrekt rækkefølge
4. Bygge og teste løbende

---

*Projekt Opstarts Guide v1.0*
*Made Man: Idle Empire*
