# TASK-041: Global Top Bar

## Beskrivelse
Top bar (Cash/Income display) skal være synlig på alle tabs undtagen Settings, så brugeren kan følge med i sin økonomi mens de navigerer rundt i appen.

## Acceptance Criteria
- [ ] Top bar vises på Empire tab (allerede done)
- [ ] Top bar vises på Crime tab
- [ ] Top bar vises på Casino tab
- [ ] Top bar vises på Skills tab
- [ ] Top bar vises IKKE på Settings tab
- [ ] Top bar opdateres live på alle tabs (bindings til samme ViewModel)

## Teknisk Tilgang
Flyt top bar til en reusable component eller inkluder den i hver page's XAML.

**Option A:** Create `TopBarComponent.xaml` og inkluder i hver page
**Option B:** Brug Shell's HeaderTemplate (kan være tricky med custom styling)

Anbefalet: **Option A** - mere kontrol over styling og binding.

## Implementation
1. Opret `Views/Components/TopBar.xaml` component
2. Tilføj til OrgCrimePage, CasinoPage, SkillsTabPage
3. Bind til MainViewModel (singleton) for live updates
4. Verificer at data opdateres korrekt på tværs af tabs

## Dependencies
- TASK-032 (Top Bar) - COMPLETED
- TASK-031 (Navigation) - COMPLETED

## Bundle
Bundle C eller D (UI Polish)

## Status
READY
