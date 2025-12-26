# TASK-045: Interval-Based Offline Earnings

## Metadata
- **Phase**: 2 - Core Features
- **Dependencies**: TASK-043 (Model & Engine)
- **Estimated Time**: 1-2 hours
- **Status**: Pending
- **Design Reference**: CLAUDE_CODE_IMPLEMENTATION_GUIDE.md
- **Frequency Impact**: NO

---

## Formål

Opdater offline earnings beregning til at arbejde med interval-baseret yields.

**Hvorfor dette er vigtigt:**
- Spillere der vender tilbage efter timer skal have korrekt offline indtjening
- Beregningen skal matche online tick-logik for konsistens
- Welcome Back modal skal vise korrekte tal

---

## Risici

### Potentielle Problemer
1. **Beregningskompleksitet**:
   - Edge case: Partial cycles ved offline start/slut
   - Impact: Forkert earnings beregning

2. **Progress state ved resume**:
   - Edge case: Skal AccumulatedTime fortsætte fra hvor spiller slap?
   - Impact: Inkonsistent progress ved return

### Mitigering
- Simpel beregning: Antal komplette cycles × yield
- AccumulatedTime bevares fra sidst (partial progress ikke tabt)
- Offline earnings tilføjer IKKE til AccumulatedTime

---

## Analyse - Hvad Skal Implementeres

### CalculateOfflineEarnings Opdatering
**Description**: Ny offline beregning baseret på cycles
**Location**: `Services/GameEngine.cs`
**Key Requirements**:
- Beregn antal komplette cycles per operation i offline perioden
- Brug OfflineEfficiency multiplier
- Respekter MaxOfflineHours cap
- AccumulatedTime forbliver uændret (progress fortsætter)

**Business Rules**:
```csharp
// For hver operation:
var offlineSeconds = Math.Min(offlineTime.TotalSeconds, MaxOfflineHours * 3600);
var completeCycles = Math.Floor(offlineSeconds / operation.Interval);
var yieldPerCycle = operation.GetYield(level, prestigeBonus);
var operationEarnings = completeCycles * yieldPerCycle * efficiency;
```

---

## Dependencies Check

✅ **Required Before Starting**:
- [ ] TASK-043 completed (Interval/Yield model)

⚠️ **Assumptions**:
- OfflineEfficiency (50%) stadig gælder
- MaxOfflineHours (4 timer) stadig gælder
- Skills påvirker yield men ikke offline beregning direkte

❌ **Blockers**: TASK-043 skal være færdig først

---

## Implementation Guide

### Step 1: Opdater CalculateOfflineEarnings
Path: `Services/GameEngine.cs`

```csharp
private void CalculateOfflineEarnings()
{
    var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;
    LastOfflineTime = offlineTime;

    // Max hours with skill bonus
    var maxHours = GameConfig.MaxOfflineHours + _skillService.GetOfflineMaxHoursBonus();
    var offlineSeconds = Math.Min(offlineTime.TotalSeconds, maxHours * 3600);

    if (offlineSeconds < 60) // Less than 1 minute - no offline earnings
    {
        LastOfflineEarnings = 0;
        LastOfflineEfficiency = 0;
        State.LastPlayedUtc = DateTime.UtcNow;
        return;
    }

    // Base efficiency + skill bonus
    var efficiency = GameConfig.OfflineEfficiency + _skillService.GetOfflineEfficiencyBonus();
    efficiency = Math.Min(efficiency, 1.0);
    LastOfflineEfficiency = efficiency;

    // Calculate earnings per operation using interval-based logic
    double totalEarnings = 0;

    foreach (var op in GameConfig.Operations)
    {
        var opState = GetOperationState(op.Id);
        if (opState == null || opState.Level <= 0) continue;

        // How many complete cycles in offline time?
        var completeCycles = Math.Floor(offlineSeconds / op.Interval);

        // Base yield per cycle
        var yieldPerCycle = op.GetYield(opState.Level, State.PrestigeBonus);

        // Total for this operation (with efficiency)
        var operationEarnings = completeCycles * yieldPerCycle * efficiency;

        totalEarnings += operationEarnings;
    }

    // Apply offline earnings multiplier (Godfather's Cut skill)
    totalEarnings *= _skillService.GetOfflineEarningsMultiplier();

    LastOfflineEarnings = totalEarnings;
    State.Cash += totalEarnings;
    State.TotalEarned += totalEarnings;

    // Note: AccumulatedTime is NOT modified - progress continues from where player left
    State.LastPlayedUtc = DateTime.UtcNow;
}
```

---

## Verification Steps

### 1. Build Test
```bash
dotnet build src/MadeMan.IdleEmpire -f net10.0-android
```
Expected: 0 errors

### 2. Manual Test
- [ ] Luk app, vent 2 minutter, åbn igen
- [ ] Welcome Back modal viser korrekt beløb
- [ ] Beløb matcher forventet (cycles × yield × efficiency)
- [ ] Progress bars fortsætter fra hvor de var (ikke reset)
- [ ] MaxOfflineHours respekteres

---

## Acceptance Criteria

- [ ] Offline earnings beregnes korrekt med interval-based logic
- [ ] OfflineEfficiency (50%) anvendes
- [ ] MaxOfflineHours cap respekteres
- [ ] AccumulatedTime bevares (progress ikke tabt)
- [ ] Welcome Back modal viser korrekte tal
- [ ] Build succeeds with 0 errors

---

## Kode Evaluering

### Simplifikations-tjek
- **Simpel cycle-beregning**: Math.Floor(seconds / interval)
- **Ingen partial cycle credit**: Kun komplette cycles giver earnings
- **Bevarer eksisterende efficiency logic**: Samme multipliers som før

### Kendte begrænsninger
- **Ingen partial cycle credit**: Spilleren får ikke credit for 90% af en cycle
- **Skills ikke fuldt applied**: Kun base yield × prestige (simpelt for offline)

---

**Task Status**: ⏸️ Blocked by TASK-043
**Last Updated**: 2024-12-26
**Implemented By**: Pending
