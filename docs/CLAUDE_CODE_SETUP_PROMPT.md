# Claude Code Setup Prompt: Made Man - Idle Empire

## Din Rolle

Du er lead developer og projektarkitekt på "Made Man: Idle Empire" - et idle/incremental mobile game i .NET MAUI. 

**Din opgave har 3 faser:**
1. **ANALYSÉR** projektet grundigt (struktur, kode, design docs)
2. **OPRET** komplet implementeringsplan med detaljerede tasks
3. **IMPLEMENTÉR** task-for-task med streng kvalitetskontrol

---

## KRITISKE REGLER

### File-Based Workflow
1. **Al langsigtet hukommelse SKAL leve i filer, IKKE i chatten**
2. Antag at fremtidige chats starter fra scratch - stol aldrig på chat history
3. Efter hver ændring: Opdater relevante filer med eksakte diffs

### Code Principper (SKAL følges i AL kode)

**KISS (Keep It Simple, Stupid)**
- Simpleste løsning der virker
- Ingen over-engineering
- Læsbar kode over "clever" kode

**YAGNI (You Aren't Gonna Need It)**
- Implementér KUN det der er nødvendigt NU
- Ingen spekulative features
- Tilføj kompleksitet når det er bevist nødvendigt

**SOLID**
- **S**ingle Responsibility: Én klasse, ét ansvar
- **O**pen-Closed: Åben for udvidelse, lukket for ændring
- **L**iskov Substitution: Subtypes skal være substituerbare
- **I**nterface Segregation: Små, specifikke interfaces
- **D**ependency Inversion: Depend on abstractions

### Task Workflow
```
1. Jeg siger: "Start [TASK-ID]"
2. Du arbejder og stiller spørgsmål hvis nødvendigt
3. Du tester MAKSIMALT inden færdigmelding (build, kør hvis muligt)
4. Du færdigmelder med: hvad der er implementeret + hvad jeg kan teste
5. Jeg tester og godkender
6. Ved godkendelse: Du laver commit med "[TASK-ID]: Beskrivelse"
7. Du opdaterer TASKS.md, STATE.md, og task filen
8. Næste task er klar
```

---

## FASE 1: PROJEKT ANALYSE

### Udfør denne analyse FØRST

**1. Læs ALLE design dokumenter:**
```
/mnt/project/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
/mnt/project/MVP_Specification_MAUI.md
/mnt/project/GAME_DESIGN_DOCUMENT.md
/mnt/project/Market_Analysis_MadeMan.md
/mnt/project/idle_mafia_game_brainstorm.md
```

**2. Analyser eksisterende kodebase:**
```bash
# Find projekt root og struktur
ls -la
find . -type f -name "*.csproj" 2>/dev/null
find . -type f -name "*.cs" 2>/dev/null | head -30
find . -type f -name "*.xaml" 2>/dev/null | head -20

# Tjek NuGet packages hvis projekt eksisterer
cat *.csproj 2>/dev/null | grep -i packagereference

# Tjek mappestruktur
find . -type d -not -path "*/\.*" | head -30
```

**3. Verificer build status:**
```bash
dotnet build -f net8.0-android 2>&1 | tail -20
```

**4. Dokumenter analyse i STATE.md:**
- Hvad eksisterer allerede?
- Hvad mangler i forhold til MVP spec?
- Hvilke filer skal oprettes vs. modificeres?
- Er der eksisterende kode der skal refaktoreres?
- Eventuelle problemer eller blockers?

---

## FASE 2: OPRET PROJECT MANAGEMENT STRUKTUR

### Opret `docs/` mappe med følgende struktur:

```
docs/
├── PLAN.md           ← Arkitektur + roadmap (stabil, ændres sjældent)
├── TASKS.md          ← Task oversigt med status
├── STATE.md          ← Nuværende projekt status
├── NOTES.md          ← Scratchpad til noter
└── tasks/            ← Individuelle detaljerede task-filer
    ├── TASK-001.md
    ├── TASK-002.md
    └── ...
```

---

## FASE 3: TASK TEMPLATE

**KRITISK: Hver task fil i `docs/tasks/` SKAL følge denne template:**

```markdown
# Task [XXX]: [Task Navn]

## Metadata
- **Phase**: [1-5] - [Phase Name]
- **Dependencies**: [Task IDs eller "None"]
- **Estimated Time**: [X-Y timer]
- **Status**: 🔵 Ready | ⚪ Blocked | 🟡 In Progress | ✅ Completed
- **Design Reference**: [Sti til design fil eller "N/A"]
- **Requires Design Input**: YES/NO

---

## Formål

[Klar beskrivelse af hvad denne task skal opnå]

**Hvorfor dette er vigtigt:**
- [Grund 1]
- [Grund 2]
- [Forbindelse til brugerværdi]

---

## Risici

### Potentielle Problemer
1. **[Problem kategori]**:
   - Edge case: [Beskrivelse]
   - Impact: [Hvad kan gå galt]

2. **[Andet problem]**:
   - Beskrivelse
   - Impact

### Mitigering
- [Hvordan forebygge/håndtere problem 1]
- [Hvordan forebygge/håndtere problem 2]

---

## Analyse - Hvad Skal Implementeres

### [Komponent/Feature 1]
**Beskrivelse**: [Hvad det er]
**Placering**: [Fil sti]
**Nøgle Krav**:
- [Krav 1]
- [Krav 2]

### [Komponent/Feature 2]
[Samme struktur]

**Business Rules** (hvis relevant):
```csharp
// Eksempel kode der viser forretningslogik
```

---

## Dependencies Check

✅ **Krævet Før Start**:
- [ ] Dependency 1 færdig
- [ ] Dependency 2 færdig

⚠️ **Antagelser**:
- [Antagelse om eksisterende kode]
- [Antagelse om design]

❌ **Blockers** (hvis nogen):
- [Hvad ville forhindre start af denne task]

---

## Implementation Guide

### Step 1: [Første Opgave]
**Sti**: `[fil sti]`

```csharp
// Komplet kode eksempel
```

**Forklaring**: [Hvorfor denne kode]

### Step 2: [Anden Opgave]
[Samme struktur]

### Step N: Register/Integrate
[DI registration, routing, etc.]

---

## Verification Steps

### 1. Build Test
```bash
dotnet build -f net8.0-android
```
Forventet: 0 errors

### 2. Unit Tests (hvis relevant)
```csharp
[Fact]
public async Task [TestName]()
{
    // Test kode
}
```

### 3. Manuel Test i Emulator
- [ ] Test scenarie 1
- [ ] Test scenarie 2
- [ ] Edge case testing

---

## Acceptance Criteria

- [ ] [Kriterie 1]
- [ ] [Kriterie 2]
- [ ] Build succeeds med 0 errors
- [ ] Ingen nye warnings introduceret
- [ ] Manuel testing passed

---

## Kode Evaluering

### Simplifikations-tjek
Denne implementation følger KISS princippet ved at:
- **[Simplifikation 1]**: [Beskrivelse]
- **[Simplifikation 2]**: [Beskrivelse]
- **Ingen [X]**: [Hvilken kompleksitet blev undgået]

### Alternativer overvejet

**Alternativ 1: [Navn]**
```csharp
// Alternativ tilgang kode
```
**Hvorfor fravalgt**: [Grund - typisk "for komplekst" eller "over-engineering"]

### Potentielle forbedringer (v2)
- [Forbedring 1] - Ikke nødvendig for MVP
- [Forbedring 2] - Kan tilføjes senere hvis nødvendigt

### Kendte begrænsninger
- **[Begrænsning 1]**: [Beskrivelse og hvorfor det er acceptabelt for MVP]

---

## Kode Kvalitet Checklist

- [ ] **KISS**: Er dette den simpleste løsning?
- [ ] **Læsbarhed**: Kan en anden udvikler forstå koden uden forklaring?
- [ ] **Navngivning**: Er alle navne beskrivende og konsistente?
- [ ] **Funktioner**: Er alle funktioner korte og fokuserede? (max 20-30 linjer)
- [ ] **DRY**: Er der nogen duplikeret kode?
- [ ] **Error handling**: Er fejl håndteret korrekt?
- [ ] **Edge cases**: Er edge cases identificeret og håndteret?
- [ ] **Performance**: Er der åbenlyse performance problemer?
- [ ] **Testbarhed**: Kan koden nemt testes?

---

## Design Files Reference

- **Spec Reference**: [Sti til relevant spec fil]
- **Related Tasks**: [Andre relevante task IDs]

---

## Notes

- [Implementation note]
- [Design beslutning]
- [Fremtidig overvejelse]

---

**Task Status**: 🔵 Ready to implement
**Last Updated**: [DATO]
```

---

## FASE 4: OPRET TASKS BASERET PÅ ANALYSE

### Forventede Faser og Tasks

Baseret på CLAUDE_CODE_IMPLEMENTATION_GUIDE.md, opret tasks for:

**Phase 1: Foundation**
- Project setup & cleanup
- Models (GameState, Operation, GameConfig)
- SaveManager service
- GameEngine service
- DI & Lifecycle setup

**Phase 2: UI**
- Theme & colors
- OperationViewModel
- MainViewModel
- MainPage.xaml
- Icons

**Phase 3: Gameplay**
- Game loop verification
- Upgrade/unlock logic verification
- Prestige system verification

**Phase 4: Persistence**
- Save/load testing
- Offline earnings testing

**Phase 5: Polish**
- UI polish
- Bug fixes
- Final testing

### Task Nummerering
- TASK-001, TASK-002, etc.
- Klar dependency chain
- Realistiske tidsestimater

---

## PLAN.md INDHOLD

```markdown
# Made Man: Idle Empire - Implementation Plan

## Vision
Et idle/incremental mobile game hvor spilleren bygger et kriminelt imperium i 1930'ernes New Porto. Fra pickpocket til Godfather.

## Tech Stack (Låst)
| Komponent | Teknologi |
|-----------|-----------|
| Framework | .NET 8 MAUI |
| Sprog | C# |
| UI | XAML |
| Pattern | MVVM med CommunityToolkit.Mvvm |
| Storage | Preferences API (JSON) |
| Target | Android 8.0+ (API 26) |

## Arkitektur Beslutninger

### Single Page Design
- AL gameplay på MainPage.xaml
- Ingen navigation mellem sider i MVP
- Simplicitet over features

### Unified Operation Model
- Crimes og Businesses bruger samme Operation klasse
- Ingen separate modeller for forskellige typer
- Nemmere at udvide senere

### Game Loop
- IDispatcherTimer til 10 ticks/sekund
- GameEngine håndterer al logik
- ViewModel er kun presentation layer

## Projekt Struktur
```
MadeMan.IdleEmpire/
├── Models/
│   ├── GameState.cs
│   ├── Operation.cs
│   └── GameConfig.cs
├── ViewModels/
│   ├── MainViewModel.cs
│   └── OperationViewModel.cs
├── Views/
│   └── MainPage.xaml
├── Services/
│   ├── IGameEngine.cs
│   ├── GameEngine.cs
│   └── SaveManager.cs
└── Resources/
    ├── Styles/Theme.xaml
    └── Images/*.png
```

## Kode Standarder

### Navngivning
- PascalCase: Public members, classes, methods
- _camelCase: Private fields
- camelCase: Local variables, parameters

### CommunityToolkit.Mvvm
- [ObservableProperty] for alle bindable properties
- [RelayCommand] for alle commands
- Partial classes for source generators

### DI Registration
- Alle Services som Singleton
- Alle ViewModels som Transient
- Alle Pages som Transient

### Error Handling
- Try-catch i alle public methods der kan fejle
- Log til Debug.WriteLine i MVP
- Graceful degradation - app må ALDRIG crashe

## Faser
1. Foundation (TASK-001 til TASK-005)
2. UI (TASK-006 til TASK-010)
3. Gameplay (TASK-011 til TASK-012)
4. Persistence (TASK-013 til TASK-014)
5. Polish (TASK-015 til TASK-017)

## Ikke i MVP (YAGNI)
- Heat system
- Crew members
- Territories
- PvP/Raids
- Notifications
- Ads/IAP
- Cloud save
- Achievements
- Story/narrative
- Animationer
- Lyd

## Success Metrics
- Første upgrade: < 15 sekunder
- Første unlock (Car Theft): < 2 minutter
- Prestige mulighed: < 15 minutter
- Build uden errors
- Kører stabilt på Android emulator
```

---

## TASKS.md INDHOLD

```markdown
# Made Man: Idle Empire - Tasks Overview

## Aktuel Task
**[TASK-ID]**: [Navn] - [Status]

---

## Task Status Legend
- 🔵 Ready - Klar til implementation
- ⚪ Blocked - Venter på dependencies
- 🟡 In Progress - Under arbejde
- ✅ Completed - Færdig og godkendt

---

## Phase 1: Foundation

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-001 | Project Setup & Cleanup | 🔵 | None | 30 min |
| TASK-002 | Implement Models | ⚪ | TASK-001 | 1 time |
| TASK-003 | Implement SaveManager | ⚪ | TASK-002 | 30 min |
| TASK-004 | Implement GameEngine | ⚪ | TASK-003 | 1.5 time |
| TASK-005 | Setup DI & Lifecycle | ⚪ | TASK-004 | 30 min |

## Phase 2: UI

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-006 | Implement Theme | ⚪ | TASK-005 | 30 min |
| TASK-007 | OperationViewModel | ⚪ | TASK-006 | 45 min |
| TASK-008 | MainViewModel | ⚪ | TASK-007 | 1 time |
| TASK-009 | MainPage UI | ⚪ | TASK-008 | 1.5 time |
| TASK-010 | Add Icons | ⚪ | TASK-009 | 30 min |

## Phase 3: Gameplay

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-011 | Verify Game Loop | ⚪ | TASK-010 | 1 time |
| TASK-012 | Verify Prestige | ⚪ | TASK-011 | 30 min |

## Phase 4: Persistence

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-013 | Test Save/Load | ⚪ | TASK-012 | 30 min |
| TASK-014 | Test Offline Earnings | ⚪ | TASK-013 | 30 min |

## Phase 5: Polish

| Task | Navn | Status | Dependencies | Estimat |
|------|------|--------|--------------|---------|
| TASK-015 | UI Polish | ⚪ | TASK-014 | 1 time |
| TASK-016 | Bug Fixes | ⚪ | TASK-015 | Variable |
| TASK-017 | Final Testing | ⚪ | TASK-016 | 1 time |

---

## Completed Tasks

| Task | Navn | Completed | Commit |
|------|------|-----------|--------|
| - | - | - | - |

---

## Total Estimat
**~12-14 timer** for fuld MVP implementation
```

---

## STATE.md INDHOLD

```markdown
# Made Man: Idle Empire - Current State

## Sidst Opdateret
[INDSÆTTES AF CLAUDE EFTER ANALYSE]

## Projekt Analyse

### Eksisterende Filer
[Liste over hvad der allerede eksisterer]

### Manglende Filer
[Liste over hvad der skal oprettes]

### Build Status
[Resultat af dotnet build]

### Identificerede Issues
[Eventuelle problemer fundet under analyse]

---

## Nuværende Status
🔴 Ikke startet | 🟡 I gang | 🟢 MVP Færdig

## Aktiv Task
[TASK-ID eller "Ingen"]

## Sidst Completed Task
[TASK-ID eller "Ingen"]

---

## Beslutninger Taget
1. Single-page design (kun MainPage)
2. Unified Operation model
3. File-based workflow
4. KISS/YAGNI/SOLID principper

## Antagelser
[Opdateres baseret på analyse]

## Blockers
[Eventuelle blockers]

## Næste Skridt
[Hvad skal ske næst]

---

## Kode Metrics
- **Build Status**: [Success/Fail]
- **Errors**: [Antal]
- **Warnings**: [Antal]
- **Files Created**: [Antal]
- **Lines of Code**: [Cirka]
```

---

## OPDATER CLAUDE.md

Tilføj følgende sektion til den eksisterende CLAUDE.md fil:

```markdown

---

## Task Workflow (Claude Code)

### Kommandoer

| Kommando | Beskrivelse |
|----------|-------------|
| `Start TASK-XXX` | Start specifik task |
| `Godkend TASK-XXX` | Godkend og commit færdig task |
| `Ændr TASK-XXX: [beskrivelse]` | Bed om ændringer |
| `Status` | Få overblik over nuværende state |
| `Næste` | Hvad er næste task? |

### Task Flow
1. Sig: `Start TASK-XXX`
2. Claude implementerer og tester
3. Claude færdigmelder med test instruktioner
4. Du tester i emulator
5. Sig: `Godkend TASK-XXX`
6. Claude committer og opdaterer docs

---

## Code Principper

### KISS
- Simpleste løsning der virker
- Ingen over-engineering

### YAGNI
- Kun det nødvendige NU
- Ingen spekulative features

### SOLID
- Single Responsibility
- Open-Closed
- Liskov Substitution
- Interface Segregation
- Dependency Inversion

---

## Git Commit Convention
```
[TASK-XXX]: Kort beskrivelse

- Detalje 1
- Detalje 2
```

---

## Kvalitetskrav Før Task Godkendelse
1. `dotnet build -f net8.0-android` succeeds
2. Ingen nye warnings
3. Manuel test i emulator (hvis UI)
4. Alle acceptance criteria opfyldt
5. Kode evaluering udfyldt i task fil
```

---

## EKSEKVÉR NU

### Step 1: Analyse
1. Læs alle projektfiler i `/mnt/project/`
2. Analyser eksisterende kodebase med kommandoerne ovenfor
3. Kør build test

### Step 2: Opret Struktur
1. Opret `docs/` mappen
2. Opret PLAN.md, TASKS.md, STATE.md, NOTES.md
3. Opret `docs/tasks/` mappen

### Step 3: Opret Detaljerede Tasks
1. Opret hver TASK-XXX.md fil med fuld template
2. Tilpas baseret på analyse (hvad eksisterer allerede?)
3. Sørg for korrekte dependencies

### Step 4: Opdater CLAUDE.md
1. Tilføj task workflow sektion

### Step 5: Rapporter
Giv mig en rapport med:
- Analyse resultater
- Build status
- Antal tasks oprettet
- Første task der er klar
- Eventuelle issues eller spørgsmål

---

Når du er færdig, er jeg klar til at starte med: `Start TASK-001`
