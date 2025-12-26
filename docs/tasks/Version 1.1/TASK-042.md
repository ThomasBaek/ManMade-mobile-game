# TASK-042: Skills Tab Redesign

## Beskrivelse
Skills skal flyttes væk fra Empire tab og over i Skills tab. Dette giver mere plads til operations på hovedskærmen. Skills tab skal låses op så snart spilleren får sin første skill, og give et detaljeret overblik over alle unlockede skills.

## Acceptance Criteria
- [ ] Skills panel FJERNET fra MainPage/Empire tab
- [ ] Skills tab låses op når første skill unlocks (ikke ved Prestige level)
- [ ] Skills tab viser alle unlockede skills med detaljer
- [ ] Skills tab viser progress mod næste skill milestone
- [ ] Skills tab viser hvilke skills der er tilgængelige at vælge
- [ ] Locked state vises kun hvis ingen skills er unlocked endnu

## UI Design - Skills Tab

### Når LÅST (ingen skills endnu):
```
🔒
Skills
Unlock your first skill to access this tab

UNLOCK REQUIREMENT
Reach $1,000 Total Earned
[Progress bar: $X / $1,000]
```

### Når ULÅST (har mindst 1 skill):
```
[TOP BAR - Cash/Income]

⭐ YOUR SKILLS (2 active)

┌─────────────────────────────┐
│ 🎯 Quick Hands              │
│ +15% Pickpocket income      │
│ ████████░░ Tier 2           │
└─────────────────────────────┘

┌─────────────────────────────┐
│ 🚗 Getaway Driver           │
│ +20% Car Theft income       │
│ ██████████ Tier 3 (MAX)     │
└─────────────────────────────┘

── NEXT MILESTONE ──
$5,000 Total Earned
[Progress bar]
Choose your next skill!
```

## Teknisk Implementation

### 1. Unlock Logic
```csharp
// I SkillsTabPage eller ViewModel
public bool IsSkillsTabUnlocked => _skillService.ActiveSkills.Count > 0;
```

### 2. Fjern fra MainPage
- Fjern `<components:SkillsPanel>` fra MainPage.xaml
- Fjern `<components:SkillSelectionModal>` fra MainPage.xaml
- Behold SkillViewModel injection for andre formål

### 3. Skills Tab Content
- Vis liste over aktive skills med deres effekter
- Vis tier/level for hver skill
- Vis næste milestone og progress
- Trigger skill selection modal når milestone nås

## Dependencies
- TASK-041 (Global Top Bar) - for top bar on Skills tab
- Existing skill system (SkillService, SkillViewModel)

## Bundle
Bundle C eller separat Bundle F (Skills Rework)

## Status
READY
