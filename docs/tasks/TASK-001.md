# Task 001: Install CommunityToolkit.Mvvm

## Metadata
- **Phase**: 1 - Foundation
- **Dependencies**: None
- **Estimated Time**: 15 min
- **Status**: READY
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Requires Design Input**: NO

---

## Formål

Installer CommunityToolkit.Mvvm NuGet package til MVVM support.

**Hvorfor dette er vigtigt:**
- CommunityToolkit.Mvvm er nødvendig for [ObservableProperty] og [RelayCommand]
- Reducerer boilerplate kode betydeligt
- Standard pattern for .NET MAUI apps

---

## Risici

### Potentielle Problemer
1. **Package Version Konflikt**:
   - Edge case: Inkompatibel version med .NET 10
   - Impact: Compile errors

### Mitigering
- Brug nyeste stabile version (8.2.2 eller højere)
- Test build efter installation

---

## Analyse - Hvad Skal Implementeres

### 1. Tilføj NuGet Package
**Placering**: `src/MadeMan.IdleEmpire/MadeMan.IdleEmpire.csproj`

**Ændringer:**
- Tilføj CommunityToolkit.Mvvm package reference

### 2. Verificer Build
**Kommando**: `dotnet build -f net10.0-android`

---

## Dependencies Check

**Krævet Før Start**:
- [x] Projekt eksisterer og bygger (verificeret 2024-12-25)

**Antagelser**:
- NuGet feeds er tilgængelige

**Blockers**: Ingen

---

## Implementation Guide

### Step 1: Tilføj CommunityToolkit.Mvvm Package

**Sti**: `src/MadeMan.IdleEmpire/MadeMan.IdleEmpire.csproj`

Tilføj i ItemGroup med andre PackageReference:

```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

### Step 2: Restore og Build

```bash
cd src/MadeMan.IdleEmpire
dotnet restore
dotnet build -f net10.0-android
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Forventet: 0 errors

### 2. Package Verification
```bash
dotnet list src/MadeMan.IdleEmpire package
```
Forventet: CommunityToolkit.Mvvm 8.2.2 (eller højere) listet

---

## Acceptance Criteria

- [x] CommunityToolkit.Mvvm package installeret
- [x] Build succeeds med 0 errors
- [x] Ingen nye warnings introduceret

---

## Kode Evaluering

### Simplifikations-tjek
- **Minimal ændring**: Kun én linje tilføjet til .csproj
- **Ingen ny kode**: Kun package reference

### Alternativer overvejet
Ingen - CommunityToolkit.Mvvm er standard for MAUI MVVM

### Kendte begrænsninger
- Ingen

---

## Kode Kvalitet Checklist

- [x] **KISS**: Simpleste løsning
- [x] **Læsbarhed**: Standard .csproj format
- [x] **DRY**: Ingen duplikering

---

## Design Files Reference

- **Spec Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Related Tasks**: TASK-002

---

## Notes

- CommunityToolkit.Mvvm 8.2.2 er kompatibel med .NET 10
- Source generators kræver partial classes

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Completed**: 2024-12-25
