# CLAUDE CODE PROMPT: Crew System & Mission System Implementation

## Project Context

You are implementing **Feature 2: Crew System** and **Feature 3: Mission System** for "Made Man: Idle Empire" - a mafia-themed idle/incremental mobile game built with .NET MAUI targeting Android.

**Implementation Order:** Crew System FIRST, then Mission System. Both are documented here so you can design data models and interfaces that work together from the start.

**Before Implementation:** Analyze the existing project structure, especially:
- `/src/MadeMan.IdleEmpire/Models/` - Existing model patterns
- `/src/MadeMan.IdleEmpire/Services/` - Service architecture (GameEngine, SaveManager)
- `/src/MadeMan.IdleEmpire/ViewModels/` - MVVM patterns using CommunityToolkit.Mvvm
- `/src/MadeMan.IdleEmpire/Views/` - XAML UI patterns
- `/docs/EXPANSION_ROADMAP_MadeMan_v2.md` - Full feature specification

---

## What is the Crew System?

The Crew System allows players to **hire criminal associates** who:
- Have ongoing wage costs (expenses per second)
- Possess specialized skills needed for missions
- Can be sent on missions (becoming temporarily unavailable)
- Can be arrested (requiring bail payment or wait time)

**Core Design Philosophy:** Crew wages create a meaningful cost/benefit decision. The player must balance passive income from Operations against ongoing crew expenses, making crew hiring a strategic choice rather than pure accumulation.

---

## Crew Member Types (8 Types, 4 Tiers)

### Tier 1: Basics (Cheap, General Purpose)
| Type | Primary Skill | Role |
|------|---------------|------|
| **Thug** | Muscle | Basic enforcer, cheap labor |
| **Wheelman** | Driving | Getaway driver |

### Tier 2: Specialists (Mid-Cost, Specific Use)
| Type | Primary Skill | Role |
|------|---------------|------|
| **Safecracker** | Lockpicking | Opens safes and locks |
| **Hacker** | Electronics | Disables alarms and security |

### Tier 3: Experts (Expensive, High Value)
| Type | Primary Skill | Role |
|------|---------------|------|
| **Chemist** | Chemistry | Produces contraband |
| **Fixer** | Connections | Reduces mission risk |

### Tier 4: Elite (Premium, Strategic)
| Type | Primary Skill | Role |
|------|---------------|------|
| **Hitman** | Combat | Professional for dangerous jobs |
| **Consigliere** | Leadership | Advisor, provides global bonuses |

---

## Crew Member States

Each crew member can be in one of these states:

| State | Visual | Description |
|-------|--------|-------------|
| **Available** | ✅ Green | Ready to be assigned to missions |
| **OnMission** | 🎯 Yellow/Orange | Currently on a mission, unavailable |
| **InJail** | 🔒 Red | Arrested, requires bail or wait time |
| **Recovering** | ⏳ Gray | Temporarily unavailable after failed mission |

---

## Economy Design

### Balance Rule
> A crew member's wage should be coverable by 2-3 operation upgrades.

This means the player should feel that crew costs are significant but not crippling. The net income (income - expenses) should typically remain positive.

### Cost Structure (Suggested Starting Values)
| Crew Type | Hire Cost | Wage/min | Unlock Requirement |
|-----------|-----------|----------|-------------------|
| Thug | $500 | $5 | Start |
| Wheelman | $2,000 | $15 | Start |
| Safecracker | $5,000 | $35 | Prestige 1 |
| Hacker | $8,000 | $50 | Prestige 1 |
| Chemist | $20,000 | $100 | Prestige 2 |
| Fixer | $35,000 | $175 | Prestige 2 |
| Hitman | $75,000 | $350 | Prestige 3 |
| Consigliere | $150,000 | $500 | Prestige 4 |

**Note:** These values should be tuned during testing. The key is that wages feel meaningful without causing constant bankruptcy.

---

## Data Model Design

### What to Implement

**1. CrewType Configuration** (Static data)
```
- Id (string): "thug", "wheelman", etc.
- Name (string): Display name
- Description (string): Flavor text
- PrimarySkill (string): "Muscle", "Driving", etc.
- Tier (int): 1-4
- HireCost (double): One-time cost
- WagePerMinute (double): Ongoing cost
- UnlockPrestigeLevel (int): Minimum prestige to hire
- Icon (string): Icon resource name
```

**2. CrewMember Instance** (Player's hired crew)
```
- Id (string): Unique identifier (Guid)
- TypeId (string): Reference to CrewType
- Status (enum): Available, OnMission, InJail, Recovering
- StatusEndTime (DateTime?): When status changes back to Available
- CurrentMissionId (string?): If on mission, which one
- HiredAtUtc (DateTime): When hired (for stats)
```

**3. CrewState in GameState**
```
- OwnedCrew (List<CrewMember>): All hired crew
- TotalWagesPerMinute (computed): Sum of all crew wages
```

---

## Service Layer Design

### ICrewService Interface
```
Core Operations:
- GetAvailableCrewTypes() → List of crew types player can hire
- HireCrew(typeId) → Hire a new crew member
- FireCrew(memberId) → Remove crew member (no refund)
- GetCrewByType(typeId) → Get all crew of a specific type
- GetCrewByStatus(status) → Get all crew with a specific status

Economy:
- GetTotalWagesPerSecond() → Calculate total crew expenses
- CanAffordHire(typeId) → Check if player has enough cash
- ProcessWages(deltaSeconds) → Deduct wages from player cash

Status Management:
- SetCrewOnMission(memberId, missionId) → Mark as on mission
- ReleaseCrewFromMission(memberId) → Return to Available
- ArrestCrew(memberId, bailCost, waitHours) → Move to InJail
- PayBail(memberId) → Pay to release from jail immediately
- WaitOutSentence(memberId) → Start automatic release timer

Queries:
- GetAvailableCrewWithSkill(skill) → For mission assignment
- GetCrewCount(typeId) → How many of this type owned
- GetAvailableCount(typeId) → How many available for missions
```

---

## UI Design Specification

### Crew Tab Location
The Crew UI lives inside the **"Organized Crime" tab** (bottom navigation, slot 2).

**Tab Unlock Condition:** Prestige 2 + at least 1 crew member hired

### Organized Crime Tab Structure
Both Crew and Missions live within the same tab. Suggested layout:

```
┌─────────────────────────────────────────────────────────────┐
│  ORGANIZED CRIME                      🔥 Heat: 45%         │
├─────────────────────────────────────────────────────────────┤
│  [CREW]  [MISSIONS]  [ACTIVE]                              │  ← Sub-navigation
├─────────────────────────────────────────────────────────────┤
│                                                             │
│           Content area based on sub-tab                     │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

**Sub-tabs:**
- **CREW:** Hire/manage crew, jail section
- **MISSIONS:** Available missions to start
- **ACTIVE:** Currently running missions with progress

### Visual Design Theme
- Dark background (#0D0D0D / #1A1A2E)
- Gold accents (#D4AF37) for important elements
- Blood red (#8B0000) for warnings/costs
- Success green (#4ADE80) for available status

---

## UI Layout: Crew Overview Screen

```
┌─────────────────────────────────────────────────────────────┐
│                    CREW MANAGEMENT                          │
│                                                             │
│  Total Wages: $320/min                    [HIRE NEW +]      │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  👊 THUGS                                    (5)     │   │
│  │  ├── Available: 3 ✅                                │   │
│  │  ├── On Mission: 2 🎯                               │   │
│  │  └── Wage: $25/min                                  │   │
│  │                                         [+ HIRE $500]│   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  🚗 WHEELMEN                                 (2)     │   │
│  │  ├── Available: 2 ✅                                │   │
│  │  ├── On Mission: 0                                  │   │
│  │  └── Wage: $30/min                                  │   │
│  │                                       [+ HIRE $2,000]│   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  🔒 SAFECRACKER                         [LOCKED]     │   │
│  │  └── Unlock at Prestige 1                           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
├─────────────────────────────────────────────────────────────┤
│  🔒 IN JAIL (1)                                            │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Tony (Thug)                                        │   │
│  │  Bail: $5,000 [PAY]  or  Wait: 23h 45m             │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## UI Component: Crew Type Card

Each crew type is displayed as a collapsible card showing:

```
┌──────────────────────────────────────────────────────────┐
│  [Icon] CREW_NAME                           Count: (X)   │
│  ──────────────────────────────────────────────────────  │
│  Status Breakdown:                                       │
│    ● Available: X ✅                                     │
│    ● On Mission: X 🎯                                    │
│    ● In Jail: X 🔒                                       │
│                                                          │
│  Total Wage: $XXX/min                                    │
│                                                          │
│  ┌────────────────┐   ┌────────────────┐                │
│  │  [+ HIRE]      │   │  [MANAGE ▼]    │                │
│  │  $X,XXX        │   │                │                │
│  └────────────────┘   └────────────────┘                │
└──────────────────────────────────────────────────────────┘
```

### States for Hire Button
- **Enabled (Gold border):** Player can afford and meets prestige requirement
- **Disabled - Cost (Gray, red text):** "Need $X more"
- **Disabled - Locked (Gray, lock icon):** "Unlock at Prestige X"

---

## UI Component: Hire Modal

When player taps "HIRE NEW +" or individual hire buttons:

```
┌──────────────────────────────────────────────────────────┐
│                    HIRE NEW CREW                         │
│                                                          │
│  Select crew type to hire:                               │
│                                                          │
│  ┌────────────────────────────────────────────────────┐  │
│  │  👊 Thug                              $500         │  │
│  │  Basic enforcer, cheap labor                      │  │
│  │  Skill: Muscle  │  Wage: $5/min                   │  │
│  │  You have: 5    │  [HIRE +1]                      │  │
│  └────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌────────────────────────────────────────────────────┐  │
│  │  🚗 Wheelman                          $2,000       │  │
│  │  Getaway driver for quick escapes                 │  │
│  │  Skill: Driving  │  Wage: $15/min                 │  │
│  │  You have: 2     │  [HIRE +1]                     │  │
│  └────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌────────────────────────────────────────────────────┐  │
│  │  🔐 Safecracker                       🔒 LOCKED    │  │
│  │  Opens safes and locks                            │  │
│  │  Unlock at Prestige 1                             │  │
│  └────────────────────────────────────────────────────┘  │
│                                                          │
│                          [CLOSE]                         │
└──────────────────────────────────────────────────────────┘
```

---

## UI Component: Jail Section

When crew members are arrested:

```
┌──────────────────────────────────────────────────────────┐
│  🔒 CREW IN JAIL                                         │
│  ──────────────────────────────────────────────────────  │
│                                                          │
│  ┌────────────────────────────────────────────────────┐  │
│  │  👊 "Tony" (Thug)                                  │  │
│  │                                                    │  │
│  │  Options:                                          │  │
│  │  ┌──────────────────┐  ┌──────────────────────┐   │  │
│  │  │  💰 PAY BAIL     │  │  ⏳ WAIT IT OUT      │   │  │
│  │  │     $5,000       │  │     23h 45m left     │   │  │
│  │  └──────────────────┘  └──────────────────────┘   │  │
│  └────────────────────────────────────────────────────┘  │
│                                                          │
│  ┌────────────────────────────────────────────────────┐  │
│  │  🔐 "Vinnie" (Safecracker)                        │  │
│  │                                                    │  │
│  │  Options:                                          │  │
│  │  ┌──────────────────┐  ┌──────────────────────┐   │  │
│  │  │  💰 PAY BAIL     │  │  ⏳ WAIT IT OUT      │   │  │
│  │  │    $15,000       │  │     47h 12m left     │   │  │
│  │  └──────────────────┘  └──────────────────────┘   │  │
│  └────────────────────────────────────────────────────┘  │
│                                                          │
└──────────────────────────────────────────────────────────┘
```

---

## Integration Points

### 1. Top Bar Expenses
The Crew System must integrate with the top bar to show expenses:

```
┌─────────────────────────────────────────────────────────────┐
│  💰 $1,234,567  │  📈 +$456/s  │  📉 -$120/s  │ NET: +$336/s │
└─────────────────────────────────────────────────────────────┘
                                     ↑
                              Crew wages appear here
```

**Implementation:** 
- `GameEngine.TotalExpensesPerSecond` should include `CrewService.GetTotalWagesPerSecond()`
- Top bar binds to a computed `ExpensesDisplay` property

### 2. GameState Extension
Add crew data to the existing GameState for persistence:

```
GameState
├── Cash
├── Operations
├── PrestigeCount
├── ...existing fields...
└── Crew (NEW)
    ├── OwnedCrew: List<CrewMember>
    └── (other crew-related state)
```

### 3. Game Loop Integration
In `GameEngine.Tick(deltaSeconds)`:
1. Calculate income from operations
2. **Calculate and deduct crew wages** (NEW)
3. Update cash
4. Save periodically

### 4. Tab Unlock Logic
The "Organized Crime" tab should check:
- `GameState.PrestigeCount >= 2`
- AND `CrewService.GetTotalCrewCount() >= 1`

---

## XAML Styling Guidelines

### Use Existing Theme Colors
```xml
<!-- Reference from Resources/Styles/Colors.xaml -->
<Color x:Key="Background">#0D0D0D</Color>
<Color x:Key="Surface">#1A1A2E</Color>
<Color x:Key="Gold">#D4AF37</Color>
<Color x:Key="Primary">#8B0000</Color>
<Color x:Key="Success">#4ADE80</Color>
```

### Card Style Pattern
```xml
<Border 
    BackgroundColor="{StaticResource Surface}"
    StrokeShape="RoundRectangle 8"
    Padding="12"
    Margin="0,0,0,8">
    <!-- Card content -->
</Border>
```

### Status Badge Pattern
```xml
<!-- Available -->
<Label Text="✅" TextColor="{StaticResource Success}" />

<!-- On Mission -->
<Label Text="🎯" TextColor="{StaticResource Gold}" />

<!-- In Jail -->
<Label Text="🔒" TextColor="{StaticResource Primary}" />
```

---

## Implementation Phases

### Phase 1: Data Layer
1. Create `Models/Crew/CrewType.cs`
2. Create `Models/Crew/CrewMember.cs`
3. Create `Models/Crew/CrewStatus.cs` (enum)
4. Create `Config/CrewConfig.cs` (static crew type definitions)
5. Extend `GameState.cs` with crew collection

### Phase 2: Service Layer
1. Create `Services/ICrewService.cs`
2. Create `Services/CrewService.cs`
3. Register in DI (`MauiProgram.cs`)
4. Integrate wage calculation into `GameEngine.Tick()`

### Phase 3: ViewModel Layer
1. Create `ViewModels/Crew/CrewOverviewViewModel.cs`
2. Create `ViewModels/Crew/CrewTypeCardViewModel.cs`
3. Create `ViewModels/Crew/JailedCrewViewModel.cs`
4. Add crew expense properties to `MainViewModel` for top bar

### Phase 4: UI Layer
1. Create `Views/Crew/CrewPage.xaml` (main crew tab content)
2. Create `Views/Crew/HireCrewModal.xaml`
3. Integrate into Shell navigation
4. Update top bar to show expenses

### Phase 5: Testing & Polish
1. Test hire/fire flow
2. Test wage deduction over time
3. Test jail/bail mechanics
4. Test persistence (save/load)
5. Balance crew costs vs income

---

## Testing Checklist

### Functional Tests
- [ ] Can hire crew when meeting prestige + cost requirements
- [ ] Cannot hire crew when lacking cash
- [ ] Cannot hire locked crew types
- [ ] Wages deduct from cash every tick
- [ ] Crew count displays correctly
- [ ] Status changes reflect in UI immediately
- [ ] Jailed crew shows bail option
- [ ] Paying bail releases crew immediately
- [ ] Wait timer decreases and auto-releases crew
- [ ] Firing crew removes them and their wage

### Edge Cases
- [ ] Wages continue when cash goes to zero (crew stays, cash stays at 0)
- [ ] Multiple crew of same type track separately
- [ ] App restart restores crew state correctly
- [ ] Offline time advances jail wait timers

### UI Tests
- [ ] Smooth scrolling with many crew members
- [ ] Hire button states update correctly
- [ ] Modal opens/closes properly
- [ ] Top bar expenses update in real-time

---

## Notes for Claude Code

### Follow Existing Patterns
- Use `[ObservableProperty]` for bindable properties
- Use `[RelayCommand]` for button commands
- Use same naming conventions as existing code
- Register all services as Singleton
- Use Preferences API for persistence (existing SaveManager pattern)

### KISS Principle
- Start with basic implementation
- Crew hiring + wages working first
- Jail mechanics can be simplified initially (just a status, expand later)
- Don't over-engineer the UI - functional before fancy

### Dependencies
This task depends on:
- Navigation system (tabs) being in place
- Top bar showing income/expenses
- GameEngine tick loop working

**Implementation order within this prompt:**
1. Crew System (Feature 2) - FIRST
2. Mission System (Feature 3) - SECOND (requires Crew)

**Future features that build on this:**
- Police System (Feature 4) - Extends arrest/bail mechanics
- Skills System (Feature 5) - Can modify mission risk/rewards

---

## Questions to Resolve During Implementation

### Crew System
1. **Crew capacity:** Should there be a max number of crew? (Suggest: No cap for MVP)
2. **Firing refund:** Should firing crew refund any cost? (Suggest: No refund)
3. **Wage timing:** Per second or per minute display? (Suggest: Show per minute, calculate per second internally)
4. **Crew names:** Random generated or type-based? (Suggest: Random names from a list for personality)

### Mission System
5. **Concurrent missions:** How many missions can run at once? (Suggest: 3 max for MVP)
6. **Mission refresh:** Do missions refresh, or are they always available? (Suggest: Always available)
7. **Partial failure:** Include partial success outcome? (Suggest: Skip for MVP, just success/fail)
8. **Offline missions:** Should missions progress while app is closed? (Suggest: Yes, calculate on return)
9. **Crew selection:** Auto-assign or manual select? (Suggest: Manual for more engagement)

---

## File Naming Convention

```
Models/
├── Crew/
│   ├── CrewType.cs
│   ├── CrewMember.cs
│   └── CrewStatus.cs
├── Mission/
│   ├── MissionType.cs
│   ├── MissionCategory.cs
│   ├── MissionRequirement.cs
│   └── ActiveMission.cs

Config/
├── CrewConfig.cs
└── MissionConfig.cs

Services/
├── ICrewService.cs
├── CrewService.cs
├── IMissionService.cs
└── MissionService.cs

ViewModels/
├── Crew/
│   ├── CrewOverviewViewModel.cs
│   ├── CrewTypeCardViewModel.cs
│   └── HireCrewModalViewModel.cs
├── Mission/
│   ├── MissionListViewModel.cs
│   ├── MissionDetailViewModel.cs
│   └── ActiveMissionViewModel.cs

Views/
├── Crew/
│   ├── CrewPage.xaml
│   ├── CrewPage.xaml.cs
│   ├── HireCrewModal.xaml
│   └── HireCrewModal.xaml.cs
├── Mission/
│   ├── MissionListPage.xaml
│   ├── MissionListPage.xaml.cs
│   ├── MissionDetailPage.xaml
│   └── MissionDetailPage.xaml.cs
```

---

## Success Criteria

The Crew System is complete when:

1. ✅ Player can hire all 8 crew types (respecting unlock requirements)
2. ✅ Wages deduct automatically and show in top bar as "Expenses"
3. ✅ Crew status (Available/OnMission/InJail) tracks correctly
4. ✅ Jailed crew can be bailed out or waited out
5. ✅ All crew data persists across app restarts
6. ✅ UI is responsive and matches the dark mafia theme
7. ✅ Build succeeds with 0 errors
8. ✅ Manual testing passes in Android emulator

---

**End of Crew System Specification**

---

---

# FEATURE 3: MISSION SYSTEM

---

## What is the Mission System?

Missions are **active gameplay events** that:
- Require specific crew members with matching skills
- Take real-time to complete (minutes to hours)
- Have a risk percentage for failure
- Reward large one-time cash payouts on success
- Can result in crew arrest on failure

**Core Design Philosophy:** Missions transform passive idle income into active risk/reward decisions. Players must weigh potential rewards against the risk of losing valuable crew members to jail.

---

## Mission Flow

```
1. SELECT MISSION
   ↓
2. VIEW REQUIREMENTS (which crew skills are needed)
   ↓
3. ASSIGN CREW (select crew members for the job)
   ↓
4. VIEW RISK % (calculated from crew vs. mission difficulty)
   ↓
5. START MISSION (crew status → "OnMission")
   ↓
6. WAIT (real-time timer, minutes to hours)
   ↓
7. RESULT
   ├── SUCCESS: Full payout, crew returns to Available
   ├── PARTIAL: Partial payout, some crew arrested
   └── FAILURE: No payout, crew arrested
```

---

## Mission Categories (6 Categories)

### Petty Crime (Unlock: Prestige 2)
| Aspect | Value |
|--------|-------|
| Examples | Car theft, burglary, pickpocketing |
| Duration | 5-30 minutes |
| Base Risk | 5-20% |
| Reward | $1,000 - $10,000 |
| Crew Required | 1-2 members |

### Robbery (Unlock: Prestige 2)
| Aspect | Value |
|--------|-------|
| Examples | Store robbery, bank heist |
| Duration | 30-120 minutes |
| Base Risk | 15-35% |
| Reward | $10,000 - $50,000 |
| Crew Required | 2-4 members with varied skills |

### Bootlegging (Unlock: Prestige 3)
| Aspect | Value |
|--------|-------|
| Examples | Alcohol transport, speakeasy supply |
| Duration | 1-3 hours |
| Base Risk | 20-40% |
| Reward | $20,000 - $100,000 |
| Crew Required | Wheelmen + Muscle |

### Drug Running (Unlock: Prestige 3)
| Aspect | Value |
|--------|-------|
| Examples | Production, distribution |
| Duration | 2-6 hours |
| Base Risk | 25-50% |
| Reward | $50,000 - $200,000 |
| Crew Required | Chemist + Distribution crew |

### Assassination (Unlock: Prestige 5)
| Aspect | Value |
|--------|-------|
| Examples | Target elimination |
| Duration | 4-8 hours |
| Base Risk | 40-70% |
| Reward | $100,000 - $500,000 |
| Crew Required | Hitman + Support crew |

### Political Corruption (Unlock: Prestige 5)
| Aspect | Value |
|--------|-------|
| Examples | Bribery, blackmail of politicians |
| Duration | 6-12 hours |
| Base Risk | 30-60% |
| Reward | $200,000 - $1,000,000 |
| Crew Required | Fixer + Consigliere |

---

## Mission Requirements

Each mission specifies:
- **Required skill types** (Muscle, Driving, Lockpicking, etc.)
- **Number of crew** with each skill
- **Minimum skill level** (optional, for advanced missions)

### Example: Bank Heist
```
Requirements:
├── 2x Muscle (Thugs)      ✅ You have: 5
├── 1x Lockpicking         ✅ You have: 2 Safecrackers
├── 1x Electronics         ❌ You have: 0 Hackers
└── 1x Driving             ✅ You have: 2 Wheelmen

Status: CANNOT START (missing Electronics)
```

---

## Risk Calculation

Final risk is calculated dynamically:

```
  Base Risk (mission's inherent difficulty)
- Crew Skill Bonus (higher skill → lower risk)
+ Police Heat (higher heat → higher risk)
- Active Bribes (protection reduces risk)
- Fixer Bonus (Fixers in crew reduce risk)
- Consigliere Bonus (global reduction if owned)
────────────────────────────────────────────
= Final Risk %
```

**Important:** Player must see this breakdown BEFORE starting the mission.

### Risk Modifiers (Suggested Values)
| Modifier | Effect |
|----------|--------|
| Per excess crew member | -2% risk |
| Fixer in crew | -5% per Fixer |
| Consigliere owned | -10% global |
| Per 10% Heat | +3% risk |
| Max Heat (100%) | +30% risk total |

---

## Mission Outcomes

### Success (Risk roll passed)
- Full reward added to cash
- All crew return to "Available" status
- Small Heat increase (+5-15% depending on mission tier)

### Partial Success (Optional - for longer missions)
- 50% of reward
- Some crew arrested (random selection)
- Medium Heat increase

### Failure (Risk roll failed)
- No reward
- All assigned crew arrested (status → "InJail")
- Large Heat increase (+15-30%)

---

## Mission Data Model

### MissionType (Static configuration)
```
- Id (string): "bank_heist", "car_theft", etc.
- Name (string): Display name
- Description (string): Flavor text
- Category (enum): PettyCrime, Robbery, Bootlegging, etc.
- DurationMinutes (int): How long the mission takes
- BaseRisk (double): 0.0 - 1.0
- RewardMin (double): Minimum payout
- RewardMax (double): Maximum payout
- Requirements (List<MissionRequirement>): Skills needed
- UnlockPrestigeLevel (int): Minimum prestige
- HeatGenerated (double): Heat added on completion
- Icon (string): Icon resource
```

### MissionRequirement
```
- SkillType (string): "Muscle", "Driving", etc.
- Count (int): How many crew with this skill
- MinLevel (int?): Optional minimum skill level
```

### ActiveMission (Player's running missions)
```
- Id (string): Unique identifier
- MissionTypeId (string): Reference to MissionType
- AssignedCrewIds (List<string>): Crew member IDs assigned
- StartedAtUtc (DateTime): When mission started
- EndsAtUtc (DateTime): When mission completes
- CalculatedRisk (double): Risk at time of start
- Status (enum): InProgress, Completed, Failed
- Reward (double): Calculated reward (randomized at start)
```

---

## Mission Service Interface

### IMissionService
```
Mission Queries:
- GetAvailableMissions() → Missions player can access
- GetMissionRequirements(missionId) → What's needed
- CanStartMission(missionId) → Check requirements met
- GetActiveMissions() → Currently running missions

Mission Execution:
- CalculateRisk(missionId, crewIds) → Preview final risk
- StartMission(missionId, crewIds) → Begin mission
- CheckMissionCompletion() → Called in game tick
- CompleteMission(activeMissionId) → Process result

Helpers:
- GetMissingRequirements(missionId) → What crew is lacking
- GetEligibleCrew(missionId) → Crew that can be assigned
```

---

## Mission UI Design

### Mission List Screen

```
┌─────────────────────────────────────────────────────────────┐
│                    AVAILABLE MISSIONS                       │
├─────────────────────────────────────────────────────────────┤
│                                                             │
│  PETTY CRIME                                                │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  🚗 CAR THEFT                                         │  │
│  │  Reward: $2,500    Duration: 15 min    Risk: 12%     │  │
│  │  Requires: 1x Muscle                        [START]  │  │
│  └───────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  🏠 HOUSE BURGLARY                                    │  │
│  │  Reward: $5,000    Duration: 25 min    Risk: 18%     │  │
│  │  Requires: 1x Muscle, 1x Lockpicking    [START]      │  │
│  └───────────────────────────────────────────────────────┘  │
│                                                             │
│  ROBBERY                                                    │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  🏦 BANK HEIST                                        │  │
│  │  Reward: $50,000   Duration: 2 hr      Risk: 35%     │  │
│  │  Requires: 2x Muscle, 1x Lock, 1x Tech  [DETAILS]    │  │
│  └───────────────────────────────────────────────────────┘  │
│                                                             │
│  BOOTLEGGING 🔒                                             │
│  └── Unlock at Prestige 3                                  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Mission Detail / Assignment Screen

```
┌─────────────────────────────────────────────────────────────┐
│  🏦 BANK HEIST                                              │
│  ─────────────────────────────────────────────────────────  │
│  "The First National Bank of New Porto. Big vault,         │
│   bigger rewards. But security is tight."                   │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  💰 Reward: $45,000 - $55,000                       │   │
│  │  ⏱️ Duration: 2 hours                                │   │
│  │  🎯 Base Risk: 35%                                   │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  REQUIREMENTS:                                              │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  👊 2x Muscle                          ✅ 5 available│   │
│  │  🔐 1x Lockpicking                     ✅ 2 available│   │
│  │  💻 1x Electronics                     ❌ 0 available│   │
│  │  🚗 1x Driving                         ✅ 2 available│   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ASSIGN CREW:                                               │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Muscle:       [Tony ▼] [Vinnie ▼]                  │   │
│  │  Lockpicking:  [Fingers ▼]                          │   │
│  │  Electronics:  [None available]                      │   │
│  │  Driving:      [Wheels ▼]                           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  RISK BREAKDOWN:                                            │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Base Risk:           35%                           │   │
│  │  Crew Bonus:          -8%                           │   │
│  │  Heat (45%):          +14%                          │   │
│  │  Fixer Bonus:         -5%                           │   │
│  │  ────────────────────────                           │   │
│  │  FINAL RISK:          36%                           │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ┌─────────────────┐                                       │
│  │  ❌ CANNOT START │  Missing: 1x Electronics             │
│  └─────────────────┘                                       │
│                                                             │
│                    [BACK]        [START MISSION]            │
└─────────────────────────────────────────────────────────────┘
```

### Active Missions Panel

```
┌─────────────────────────────────────────────────────────────┐
│  🎯 ACTIVE MISSIONS (2)                                     │
│  ─────────────────────────────────────────────────────────  │
│                                                             │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  🚗 CAR THEFT                                         │  │
│  │  ████████████░░░░░░░░░░░░░░░░░░░░░░  45%             │  │
│  │  Time left: 8 min 23 sec                              │  │
│  │  Crew: Tony (Thug)                                    │  │
│  │  Reward: $2,500    Risk: 12%                         │  │
│  └───────────────────────────────────────────────────────┘  │
│                                                             │
│  ┌───────────────────────────────────────────────────────┐  │
│  │  🏠 HOUSE BURGLARY                                    │  │
│  │  ██████░░░░░░░░░░░░░░░░░░░░░░░░░░░░  18%             │  │
│  │  Time left: 20 min 45 sec                             │  │
│  │  Crew: Vinnie (Thug), Fingers (Safecracker)          │  │
│  │  Reward: $4,800    Risk: 18%                         │  │
│  └───────────────────────────────────────────────────────┘  │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Mission Complete Modal

```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│                    ✅ MISSION SUCCESS!                      │
│                                                             │
│                      🚗 CAR THEFT                           │
│                                                             │
│                      💰 +$2,500                             │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Crew returned safely:                              │   │
│  │  • Tony (Thug) ✅                                   │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ⚠️ Heat increased: +8%                                    │
│                                                             │
│                       [COLLECT]                             │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

### Mission Failed Modal

```
┌─────────────────────────────────────────────────────────────┐
│                                                             │
│                    ❌ MISSION FAILED!                       │
│                                                             │
│                    🏠 HOUSE BURGLARY                        │
│                                                             │
│                    💰 No reward                             │
│                                                             │
│  ┌─────────────────────────────────────────────────────┐   │
│  │  Crew arrested:                                     │   │
│  │  • Vinnie (Thug) 🔒 Bail: $5,000                   │   │
│  │  • Fingers (Safecracker) 🔒 Bail: $15,000          │   │
│  └─────────────────────────────────────────────────────┘   │
│                                                             │
│  ⚠️ Heat increased: +22%                                   │
│                                                             │
│                       [DISMISS]                             │
│                                                             │
└─────────────────────────────────────────────────────────────┘
```

---

## Heat System (Police Attention)

### Heat Mechanics
- **Range:** 0% - 100%
- **Increases:** When missions complete (success or fail)
- **Decreases:** Slowly over time (-1% per hour)
- **Effect:** Adds to mission risk (up to +30% at 100% heat)

### Heat Display
Show heat in the Organized Crime tab header:
```
┌─────────────────────────────────────────────────────────────┐
│  🔥 HEAT: 45%  ████████████░░░░░░░░░░░░░                   │
│  Risk modifier: +14%                    [BRIBE POLICE]      │
└─────────────────────────────────────────────────────────────┘
```

### Bribery System (Simplified for MVP)
| Bribe Type | Effect | Duration | Cost |
|------------|--------|----------|------|
| Single Mission | -15% risk | Next mission only | $5,000 |
| Short Protection | -20% risk | 4 hours | $25,000 |
| Long Protection | -30% risk | 24 hours | $100,000 |

---

## Integration: Crew ↔ Mission

### How They Connect

1. **Crew provides skills** → Missions require skills
2. **Crew status changes** → OnMission during, InJail on failure
3. **Crew wages continue** → Even during missions (except InJail)
4. **Mission completion** → Calls CrewService to update status

### Shared Data Points

```
CrewMember
├── TypeId → links to CrewType.PrimarySkill
├── Status → Set by MissionService
├── CurrentMissionId → Set when assigned
└── StatusEndTime → Used for jail release

ActiveMission
├── AssignedCrewIds → References CrewMember.Id
└── On complete → Updates all assigned crew status
```

---

## Implementation Phases (Combined)

### Phase 1: Crew Data Layer
1. Create Crew models (CrewType, CrewMember, CrewStatus)
2. Create CrewConfig with all 8 crew types
3. Extend GameState with crew collection

### Phase 2: Crew Service Layer
1. Create ICrewService and CrewService
2. Implement hire/fire functionality
3. Implement wage calculation
4. Integrate wages into GameEngine.Tick()

### Phase 3: Crew UI
1. Create CrewPage (within Organized Crime tab)
2. Create Hire modal
3. Create Jail section
4. Update top bar with expenses

### Phase 4: Mission Data Layer
1. Create Mission models (MissionType, ActiveMission, MissionRequirement)
2. Create MissionConfig with mission definitions
3. Extend GameState with active missions

### Phase 5: Mission Service Layer
1. Create IMissionService and MissionService
2. Implement mission start/complete logic
3. Implement risk calculation
4. Integrate mission timer checks into GameEngine.Tick()

### Phase 6: Mission UI
1. Create MissionListPage
2. Create MissionDetailPage with crew assignment
3. Create Active Missions panel
4. Create result modals (success/failure)

### Phase 7: Integration & Polish
1. Connect mission completion to crew status updates
2. Implement Heat system
3. Add bribery functionality
4. Test full flow: hire → assign → complete → jail → bail

---

## Balance Guidelines

### Mission Rewards vs. Time
- Shorter missions: Lower total reward, but faster turnaround
- Longer missions: Higher reward, but more risk exposure
- **Rule:** 2-hour mission should give MORE than 4× a 30-min mission

### Risk vs. Reward
- Higher risk missions must give proportionally higher rewards
- **Rule:** 50% risk mission should give MORE than 2.5× a 20% risk mission
- Player should feel risk is a real choice, not "always take highest"

### Mission Availability
- 3-5 missions available at Prestige 2
- New missions unlock with prestige
- Never overwhelm with too many choices

---

## Success Criteria (Combined)

### Crew System Complete When:
1. ✅ Can hire all 8 crew types (respecting unlock requirements)
2. ✅ Wages deduct automatically and show in top bar
3. ✅ Crew status tracks correctly
4. ✅ Jailed crew can be bailed or waited out
5. ✅ All crew data persists across restarts

### Mission System Complete When:
1. ✅ Can view available missions by category
2. ✅ Can see requirements and assign crew
3. ✅ Risk calculation displays correctly
4. ✅ Mission timer runs and completes
5. ✅ Success/failure determined by risk roll
6. ✅ Crew status updates on completion
7. ✅ Heat increases on mission completion
8. ✅ All mission data persists across restarts

### Integration Complete When:
1. ✅ Full flow works: hire crew → start mission → wait → result → crew status update
2. ✅ Arrested crew appears in jail section
3. ✅ Bailing out crew works
4. ✅ Heat affects mission risk visibly

---

**End of Prompt**
