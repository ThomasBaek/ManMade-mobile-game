# TASK-087: Info/Help Page

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: U (Stats & Documentation)
- **Dependencies**: TASK-086 (skill descriptions)
- **Estimated Time**: 2-3 hours
- **Status**: ✅ COMPLETED
- **Priority**: Low
- **Completed**: 2024-12-27

---

## Purpose

Create a Help/Info page accessible from Settings that documents:
- All operations (income, costs, unlock requirements)
- All skills (effects, categories)
- Prestige system explanation
- Game mechanics

**Why this matters:**
- New players understand mechanics
- Reference for returning players
- Transparency builds trust

---

## Content Sections

### 1. Operations Guide
```
═══════════ OPERATIONS ═══════════

Each operation generates income per second.
Upgrade to increase earnings.

┌────────────────────────────────┐
│ PICKPOCKET                     │
│ Base Income: $0.5/s            │
│ Unlock: FREE                   │
│ Upgrade multiplier: 1.5x       │
│                                │
│ Your first racket. Small but   │
│ steady money from the streets. │
└────────────────────────────────┘

┌────────────────────────────────┐
│ CAR THEFT                      │
│ Base Income: $2/s              │
│ Unlock: $75                    │
│ ...                            │
└────────────────────────────────┘
```

### 2. Skills Guide
```
═══════════ SKILLS ═══════════

Earn skill points at milestones.
Choose wisely - skills reset on prestige.

INCOME SKILLS
├─ Quick Hands: +10% income/level
├─ ...

OPERATION SKILLS
├─ ...

OFFLINE SKILLS
├─ ...

PRESTIGE SKILLS
├─ ...
```

### 3. Prestige Guide
```
═══════════ PRESTIGE ═══════════

Reset your empire for permanent bonuses.

• Available at $25,000 total earned
• Grants +20% income multiplier
• Resets: Cash, Operations, Skills
• Keeps: Prestige bonus, Titles, Stats

The more you prestige, the faster
you grow. Embrace the reset!
```

### 4. Titles Guide
```
═══════════ TITLES ═══════════

Earn titles through prestige.

Street Punk (0) - You are nobody... yet
Pickpocket (1) - Stealing to get by
Thug (2) - People start to fear you
...
Godfather (20) - You ARE the family
```

---

## Technical Design

### New Page: HelpPage.xaml

**Location:** `Views/HelpPage.xaml`

Structure:
```xml
<ContentPage>
    <ScrollView>
        <VerticalStackLayout Padding="16" Spacing="24">

            <!-- Operations Section -->
            <components:HelpSection Title="OPERATIONS">
                <components:OperationHelpCard ... />
            </components:HelpSection>

            <!-- Skills Section -->
            <components:HelpSection Title="SKILLS">
                ...
            </components:HelpSection>

            <!-- Prestige Section -->
            <components:HelpSection Title="PRESTIGE">
                ...
            </components:HelpSection>

        </VerticalStackLayout>
    </ScrollView>
</ContentPage>
```

### Navigation from Settings

**SettingsPage.xaml:**
```xml
<Button Text="Help & Info"
        Command="{Binding OpenHelpCommand}"
        ... />
```

### Data Source

Pull from existing configs:
- `GameConfig.Operations` → Operation details
- `SkillConfig.AllSkills` → Skill details
- `TitleConfig.Titles` → Title progression
- `GameConfig.PrestigeThreshold` etc → Prestige info

---

## Implementation Steps

1. Create `HelpPage.xaml` and `HelpPage.xaml.cs`
2. Create `HelpViewModel.cs` (or use code-behind)
3. Create reusable help section components
4. Add navigation from Settings
5. Register in Shell/DI
6. Style to match game theme

---

## UI Components

### HelpSection.xaml
Collapsible section with header:
```xml
<ContentView>
    <Frame>
        <VerticalStackLayout>
            <Label Text="{Binding Title}" ... />
            <ContentPresenter Content="{Binding Content}" />
        </VerticalStackLayout>
    </Frame>
</ContentView>
```

### OperationHelpCard.xaml
Displays operation info:
```xml
<Frame>
    <Grid>
        <Label Text="{Binding Name}" />
        <Label Text="{Binding IncomeDisplay}" />
        <Label Text="{Binding UnlockDisplay}" />
        <Label Text="{Binding Description}" />
    </Grid>
</Frame>
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. Navigation Tests
- [ ] Help button visible in Settings
- [ ] Tapping navigates to Help page
- [ ] Back button returns to Settings
- [ ] All sections display correctly

### 3. Content Tests
- [ ] All 5 operations documented
- [ ] All 18 skills documented
- [ ] Prestige system explained
- [ ] All titles listed

---

## Acceptance Criteria

- [x] Help page accessible from Settings (Info tab)
- [x] Operations section with all details
- [x] Skills section with summary (detailed in Skills Guide)
- [x] Prestige section with mechanics
- [x] Titles section with progression
- [x] Consistent styling with game theme
- [x] Build succeeds

---

## Completion Notes

**Implementation Details:**
- Created "How to Play" page with comprehensive game documentation
- Follows same pattern as SkillsGuidePage (header with back button, scrollable content)
- Content generated programmatically from GameConfig, TitleConfig

**Page Sections:**
1. Welcome to New Porto - Narrative intro
2. The Basics - Core gameplay bullet points
3. Operations - All 5 operations with income, costs, descriptions
4. Prestige - Full explanation with what resets/stays
5. Titles - All 9 title ranks with requirements
6. Skills - Summary with reference to Skills Guide
7. Pro Tips - Gameplay advice for different stages

**Files Created/Modified:**
- `src/MadeMan.IdleEmpire/Views/HelpPage.xaml` - Page layout
- `src/MadeMan.IdleEmpire/Views/HelpPage.xaml.cs` - Content generation
- `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml` - Added "How to Play" menu
- `src/MadeMan.IdleEmpire/Views/SettingsPage.xaml.cs` - Navigation handler
- `src/MadeMan.IdleEmpire/AppShell.xaml.cs` - Route registration
- `src/MadeMan.IdleEmpire/MauiProgram.cs` - DI registration

---

## Code Evaluation

### Simplicity Check
- Static content, no complex logic
- Pull from existing config objects
- Reusable components

### Performance Impact
- None (static page, lazy loaded)

### Maintenance
- Auto-updates when config changes
- Single source of truth

---

**Task Status**: ✅ COMPLETED (2024-12-27)
