# TASK-086: Milestone & Skill Descriptions

## Metadata
- **Phase**: Version 1.2.1
- **Bundle**: U (Stats & Documentation)
- **Dependencies**: None
- **Estimated Time**: 1 hour
- **Status**: READY
- **Priority**: Medium

---

## Purpose

1. Add descriptions to milestones with info icon popup
2. Always show "0%" for unselected skills (not blank)

**Why this matters:**
- Players don't know what skills do before choosing
- Blank percentage is confusing - is it 0% or not applicable?
- Info icons invite exploration

---

## Current Behavior

- Milestones have names but no detailed descriptions
- Unselected skills may show blank instead of 0%
- No way to preview skill effects before choosing

## Target Behavior

- Each milestone/skill has:
  - Short name (existing)
  - Description text (new)
  - Info icon (ℹ️) that opens detail popup
- Unselected skills always show "0%" clearly

---

## Technical Analysis

### Skill Descriptions

**Update SkillConfig.cs:**
```csharp
public record SkillDefinition(
    string Id,
    string Name,
    string Category,
    string Description,  // NEW
    double[] LevelMultipliers
);

// Example:
new SkillDefinition(
    "quick_hands",
    "Quick Hands",
    "Income",
    "Increases income from all operations. Each level adds +10% to your total income per second.",
    new[] { 1.0, 1.10, 1.20, 1.30, 1.40, 1.50 }
)
```

### Info Icon Popup

Use a simple modal or tooltip:
```xml
<ImageButton Source="icon_info"
             Command="{Binding ShowSkillInfoCommand}"
             CommandParameter="{Binding SkillId}"/>
```

### Always Show Percentage

**SkillViewModel or display logic:**
```csharp
public string EffectDisplay =>
    Level == 0 ? "0%" : $"+{(CurrentMultiplier - 1.0) * 100:0}%";
```

---

## UI Design

### Skill Selection Card

```
┌─────────────────────────────────────────────┐
│  Quick Hands                    [ℹ️]        │
│  ★★★☆☆  (+30%)                             │
│  ──────────────                             │
│  Increases all income                       │
└─────────────────────────────────────────────┘
```

### Info Popup (on ℹ️ tap)

```
┌─────────────────────────────────────────────┐
│           QUICK HANDS                       │
│                                             │
│  Increases income from all operations.      │
│  Each level adds +10% to your total         │
│  income per second.                         │
│                                             │
│  Level 1: +10%                              │
│  Level 2: +20%                              │
│  Level 3: +30%                              │
│  Level 4: +40%                              │
│  Level 5: +50%                              │
│                                             │
│              [ CLOSE ]                      │
└─────────────────────────────────────────────┘
```

### Unselected Skill Display

Before: `Quick Hands ★☆☆☆☆`
After: `Quick Hands ★☆☆☆☆ (0%)`

---

## Implementation Steps

1. Add `Description` field to `SkillDefinition`
2. Write descriptions for all 18 skills
3. Add info icon to skill cards
4. Create `SkillInfoModal.xaml` component
5. Update skill display to always show percentage
6. Update milestone display similarly

---

## Skill Descriptions (Draft)

| Skill | Description |
|-------|-------------|
| Quick Hands | Increases income from all operations. +10% per level. |
| Street Smarts | Reduces costs for operation upgrades. -5% per level. |
| Night Owl | Boosts offline earnings efficiency. +8% per level. |
| Smooth Talker | Increases prestige bonus multiplier. +5% per level. |
| ... | (complete for all 18 skills) |

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```

### 2. UI Tests
- [ ] All skills show descriptions
- [ ] Info icon opens popup with full details
- [ ] Unselected skills show "0%"
- [ ] Selected skills show correct percentage
- [ ] Popup displays level breakdown

---

## Acceptance Criteria

- [ ] All 18 skills have descriptions
- [ ] Info icon visible on skill cards
- [ ] Info popup shows skill details + level breakdown
- [ ] Unselected skills show "0%" not blank
- [ ] Build succeeds

---

## Test Impact

### New Tests
```csharp
[Fact]
public void Skill_AtLevel0_ShowsZeroPercent()
{
    var vm = new SkillViewModel(skillDef, level: 0);
    Assert.Equal("0%", vm.EffectDisplay);
}

[Fact]
public void AllSkills_HaveDescriptions()
{
    foreach (var skill in SkillConfig.AllSkills)
    {
        Assert.False(string.IsNullOrEmpty(skill.Description));
    }
}
```

---

**Task Status**: READY
