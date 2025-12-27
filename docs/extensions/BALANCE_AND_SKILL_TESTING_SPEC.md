# BALANCE & SKILL TESTING SPECIFICATION

## Purpose

This document provides Claude Code with instructions for:
1. **Rebalancing** the economy (cash, prestige, skills) to make the game more challenging
2. **Implementing comprehensive tests** for the skill system to verify all skills work correctly

---

# PART 1: ECONOMY REBALANCING

## Current Problem Analysis

The game currently progresses too quickly:
- First prestige is achievable in ~15 minutes (should be 30-45 minutes)
- Skills provide too much power too early
- Not enough "grind" before progression feels meaningful
- Crew wages and Casino (coming soon) need room in the economy

## Design Philosophy

**Make the game slower but not boring:**
- Early game should feel challenging but rewarding
- Each prestige should feel like a real accomplishment
- Skills should be meaningful choices, not automatic power
- Leave economic "headroom" for crew expenses and casino risk

---

## BALANCE CHANGES REQUIRED

### 1. Operation Income Adjustments

**Reduce base income for all operations:**

| Operation | OLD Income | NEW Income | OLD Unlock | NEW Unlock |
|-----------|------------|------------|------------|------------|
| Pickpocket | $1/s | $0.5/s | FREE | FREE |
| Car Theft | $4/s | $2/s | $50 | $75 |
| Burglary | $15/s | $8/s | $250 | $400 |
| Speakeasy | $50/s | $30/s | $1,000 | $2,000 |
| Casino | $200/s | $100/s | $5,000 | $10,000 |

**Rationale:** Halving base income doubles time-to-progression. Combined with higher unlock costs, this creates more meaningful choices.

### 2. Upgrade Cost Adjustments

**Increase upgrade cost multiplier:**

| Setting | OLD Value | NEW Value |
|---------|-----------|-----------|
| Base upgrade multiplier | 1.4-1.5 | 1.6 |
| Max operation level | 10 | 10 (unchanged) |

**New upgrade formula:**
```
UpgradeCost = BaseUnlockCost × 1.6^(currentLevel)
```

### 3. Prestige System Adjustments

| Setting | OLD Value | NEW Value |
|---------|-----------|-----------|
| Prestige threshold | $10,000 | $25,000 |
| Prestige bonus | +25% | +20% |
| Bonus calculation | Additive | Additive (unchanged) |

**Rationale:** Higher threshold = more playtime per prestige. Slightly lower bonus = more prestiges needed for same power level.

### 4. Milestone Threshold Adjustments

**Spread milestones further apart:**

| # | OLD Threshold | NEW Threshold |
|---|---------------|---------------|
| 1 | $1,000 | $2,500 |
| 2 | $5,000 | $10,000 |
| 3 | $15,000 | $30,000 |
| 4 | $50,000 | $75,000 |
| 5 | $150,000 | $200,000 |
| 6 | $500,000 | $600,000 |
| 7 | $1,500,000 | $2,000,000 |
| 8 | $5,000,000 | $7,500,000 |
| 9 | $15,000,000 | $25,000,000 |
| 10 | $50,000,000 | $100,000,000 |

**Rationale:** Skills come slower, making each choice more impactful. First milestone at $2,500 means player has experienced more of the base game first.

### 5. Skill Effect Rebalancing

**Reduce skill power across the board:**

#### Income Skills

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Cash Flow | +4%/lvl (max 20%) | +3%/lvl (max 15%) |
| Street Smarts | +10%/lvl (max 50%) | +8%/lvl (max 40%) |
| Business Acumen | +12%/lvl (max 60%) | +8%/lvl (max 40%) |
| Lucky Break | +4% chance/lvl | +3% chance/lvl (max 15%) |
| The Skim | +5%/lvl (max 25%) | +4%/lvl (max 20%) |
| Compound Interest | +3%/lvl (max 15%) | +2%/lvl (max 10%) |

#### Operation Skills

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Quick Hands (Pickpocket) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |
| Chop Shop (Car Theft) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |
| Inside Man (Burglary) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |
| Happy Hour (Speakeasy) | +15%/lvl (max 75%) | +12%/lvl (max 60%) |

#### Offline Skills

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Night Owl | +8%/lvl efficiency | +6%/lvl (max 30%) |
| Extended Shift | +1hr/lvl (max 5) | +0.5hr/lvl (max 2.5 extra) |
| Passive Income | +$1/s/lvl | +$0.5/s/lvl (max $2.5/s) |
| Godfather's Cut | +8%/lvl (max 40%) | +6%/lvl (max 30%) |

#### Prestige Skills

| Skill | OLD Effect | NEW Effect |
|-------|------------|------------|
| Old Connections | +$100/lvl | +$75/lvl (max $375) |
| Reputation | +6%/lvl prestige bonus | +4%/lvl (max +20%) |
| Fast Learner | -4%/lvl upgrade cost | -3%/lvl (max -15%) |
| Early Bird | -5%/lvl unlock cost | -4%/lvl (max -20%) |

### 6. Offline Earnings Adjustment

| Setting | OLD Value | NEW Value |
|---------|-----------|-----------|
| Base offline efficiency | 50% | 40% |
| Max offline hours | 4h (8h premium) | 4h (unchanged) |

---

## IMPLEMENTATION INSTRUCTIONS FOR CLAUDE CODE

### Files to Modify

1. **GameConfig.cs** - Contains all balance constants
   - Update operation definitions (income, unlock costs)
   - Update prestige threshold and bonus
   - Update upgrade multiplier

2. **SkillConfig.cs** - Contains skill definitions
   - Update all skill effect values per the tables above

3. **MilestoneService.cs** or equivalent - Contains milestone thresholds
   - Update the threshold array

4. **GameEngine.cs** - Contains offline calculation
   - Update base offline efficiency from 0.5 to 0.4

### Verification After Changes

After implementing balance changes, verify:
- [ ] Build succeeds without errors
- [ ] Game starts and Pickpocket works at new income rate
- [ ] First unlock (Car Theft) costs $75
- [ ] Prestige panel appears at $25,000 total earned
- [ ] First milestone triggers at $2,500 total earned

---

# PART 2: SKILL SYSTEM TESTING

## Testing Strategy

Create automated tests that verify each skill's effect is correctly applied. Tests should cover:
1. **Individual skill effects** - Each of 18 skills works as specified
2. **Skill stacking** - Multiple skills combine correctly
3. **Level progression** - Effects scale correctly with levels
4. **Edge cases** - Max levels, prestige reset, etc.

## Test Framework Setup

### Location
`tests/MadeMan.IdleEmpire.Tests/` or `src/MadeMan.IdleEmpire.Tests/`

### Dependencies
```xml
<PackageReference Include="xunit" Version="2.6.6" />
<PackageReference Include="xunit.runner.visualstudio" Version="2.5.6" />
<PackageReference Include="Moq" Version="4.20.70" />
```

### Base Test Class

```csharp
public abstract class SkillTestBase
{
    protected SkillService SkillService { get; }
    protected GameState GameState { get; }
    
    protected SkillTestBase()
    {
        GameState = new GameState();
        SkillService = new SkillService(GameState);
    }
    
    protected void AddSkill(string skillId, int level = 1)
    {
        // Helper to add skill to game state
        GameState.Skills.Add(new SkillState { Id = skillId, Level = level });
    }
}
```

---

## REQUIRED TEST CASES

### Income Skills Tests

```csharp
public class IncomeSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 1.03)]  // Level 1: 3%
    [InlineData(3, 1.09)]  // Level 3: 9%
    [InlineData(5, 1.15)]  // Level 5: 15% (max)
    public void CashFlow_AppliesCorrectMultiplier(int level, double expected)
    {
        // Arrange
        AddSkill("cash_flow", level);
        
        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();
        
        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 1.08)]  // Level 1: 8%
    [InlineData(5, 1.40)]  // Level 5: 40% (max)
    public void StreetSmarts_BoostsTier1Only(int level, double expected)
    {
        // Arrange
        AddSkill("street_smarts", level);
        
        // Act
        var tier1Multiplier = SkillService.GetTierMultiplier(1);
        var tier2Multiplier = SkillService.GetTierMultiplier(2);
        
        // Assert
        Assert.Equal(expected, tier1Multiplier, precision: 2);
        Assert.Equal(1.0, tier2Multiplier);  // Tier 2 unaffected
    }
    
    [Theory]
    [InlineData(1, 1.08)]
    [InlineData(5, 1.40)]
    public void BusinessAcumen_BoostsTier2Only(int level, double expected)
    {
        // Arrange
        AddSkill("business_acumen", level);
        
        // Act
        var tier1Multiplier = SkillService.GetTierMultiplier(1);
        var tier2Multiplier = SkillService.GetTierMultiplier(2);
        
        // Assert
        Assert.Equal(1.0, tier1Multiplier);  // Tier 1 unaffected
        Assert.Equal(expected, tier2Multiplier, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 0.03)]  // 3% chance
    [InlineData(5, 0.15)]  // 15% chance max
    public void LuckyBreak_HasCorrectChance(int level, double expectedChance)
    {
        // Arrange
        AddSkill("lucky_break", level);
        
        // Act
        var chance = SkillService.GetLuckyBreakChance();
        
        // Assert
        Assert.Equal(expectedChance, chance, precision: 2);
    }
    
    [Fact]
    public void LuckyBreak_DoublesDamageWhenTriggered()
    {
        // Arrange
        AddSkill("lucky_break", 5);
        var baseIncome = 100.0;
        
        // Act - Force a "lucky" roll (mock random)
        var critIncome = SkillService.ApplyLuckyBreak(baseIncome, forceSuccess: true);
        
        // Assert
        Assert.Equal(200.0, critIncome);
    }
    
    [Theory]
    [InlineData(1, 0.04)]  // 4% cashback
    [InlineData(5, 0.20)]  // 20% cashback max
    public void TheSkim_ReturnsCorrectCashback(int level, double expectedRate)
    {
        // Arrange
        AddSkill("the_skim", level);
        var purchaseCost = 1000.0;
        
        // Act
        var cashback = SkillService.CalculateCashback(purchaseCost);
        
        // Assert
        Assert.Equal(purchaseCost * expectedRate, cashback, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 1.02)]  // 2% per hour
    [InlineData(5, 1.10)]  // 10% per hour max
    public void CompoundInterest_ScalesWithSessionTime(int level, double hourlyRate)
    {
        // Arrange
        AddSkill("compound_interest", level);
        
        // Act - After 1 hour
        var multiplier = SkillService.GetCompoundMultiplier(hours: 1.0);
        
        // Assert
        Assert.Equal(hourlyRate, multiplier, precision: 2);
    }
    
    [Fact]
    public void CompoundInterest_CapsAtMaxMultiplier()
    {
        // Arrange
        AddSkill("compound_interest", 5);  // 10%/hr
        
        // Act - After 10 hours (would be 2x without cap)
        var multiplier = SkillService.GetCompoundMultiplier(hours: 10.0);
        
        // Assert - Should cap at reasonable value (e.g., 1.5x)
        Assert.True(multiplier <= 1.5);
    }
}
```

### Operation Skills Tests

```csharp
public class OperationSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData("quick_hands", "pickpocket", 1, 1.12)]
    [InlineData("quick_hands", "pickpocket", 5, 1.60)]
    [InlineData("chop_shop", "cartheft", 1, 1.12)]
    [InlineData("chop_shop", "cartheft", 5, 1.60)]
    [InlineData("inside_man", "burglary", 1, 1.12)]
    [InlineData("inside_man", "burglary", 5, 1.60)]
    [InlineData("happy_hour", "speakeasy", 1, 1.12)]
    [InlineData("happy_hour", "speakeasy", 5, 1.60)]
    public void OperationSkill_BoostsCorrectOperation(
        string skillId, string operationId, int level, double expected)
    {
        // Arrange
        AddSkill(skillId, level);
        
        // Act
        var multiplier = SkillService.GetOperationMultiplier(operationId);
        
        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }
    
    [Fact]
    public void QuickHands_DoesNotAffectOtherOperations()
    {
        // Arrange
        AddSkill("quick_hands", 5);
        
        // Act
        var pickpocketMultiplier = SkillService.GetOperationMultiplier("pickpocket");
        var carTheftMultiplier = SkillService.GetOperationMultiplier("cartheft");
        
        // Assert
        Assert.Equal(1.60, pickpocketMultiplier, precision: 2);
        Assert.Equal(1.0, carTheftMultiplier);
    }
}
```

### Offline Skills Tests

```csharp
public class OfflineSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 0.46)]  // Base 40% + 6% = 46%
    [InlineData(5, 0.70)]  // Base 40% + 30% = 70% (max)
    public void NightOwl_IncreasesOfflineEfficiency(int level, double expected)
    {
        // Arrange
        AddSkill("night_owl", level);
        
        // Act
        var efficiency = SkillService.GetOfflineEfficiency();
        
        // Assert
        Assert.Equal(expected, efficiency, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 4.5)]   // Base 4h + 0.5h
    [InlineData(5, 6.5)]   // Base 4h + 2.5h max
    public void ExtendedShift_IncreasesMaxOfflineHours(int level, double expected)
    {
        // Arrange
        AddSkill("extended_shift", level);
        
        // Act
        var maxHours = SkillService.GetMaxOfflineHours();
        
        // Assert
        Assert.Equal(expected, maxHours, precision: 1);
    }
    
    [Theory]
    [InlineData(1, 0.5)]   // $0.5/s
    [InlineData(5, 2.5)]   // $2.5/s max
    public void PassiveIncome_AddsBaselineIncome(int level, double expected)
    {
        // Arrange
        AddSkill("passive_income", level);
        
        // Act
        var passive = SkillService.GetPassiveIncomePerSecond();
        
        // Assert
        Assert.Equal(expected, passive, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 1.06)]  // +6%
    [InlineData(5, 1.30)]  // +30% max
    public void GodfathersCut_MultipliesOfflineEarnings(int level, double expected)
    {
        // Arrange
        AddSkill("godfathers_cut", level);
        var baseEarnings = 1000.0;
        
        // Act
        var boosted = SkillService.ApplyOfflineBonus(baseEarnings);
        
        // Assert
        Assert.Equal(baseEarnings * expected, boosted, precision: 2);
    }
}
```

### Prestige Skills Tests

```csharp
public class PrestigeSkillsTests : SkillTestBase
{
    [Theory]
    [InlineData(1, 75)]    // $75
    [InlineData(5, 375)]   // $375 max
    public void OldConnections_SetsStartingCash(int level, double expected)
    {
        // Arrange
        AddSkill("old_connections", level);
        
        // Act
        var startCash = SkillService.GetStartingCashAfterPrestige();
        
        // Assert
        Assert.Equal(expected, startCash);
    }
    
    [Theory]
    [InlineData(1, 0.24)]  // Base 20% + 4% = 24%
    [InlineData(5, 0.40)]  // Base 20% + 20% = 40% max
    public void Reputation_IncreasesPrestigeBonus(int level, double expected)
    {
        // Arrange
        AddSkill("reputation", level);
        
        // Act
        var bonusRate = SkillService.GetPrestigeBonusRate();
        
        // Assert
        Assert.Equal(expected, bonusRate, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 0.97)]  // -3%
    [InlineData(5, 0.85)]  // -15% max
    public void FastLearner_ReducesUpgradeCosts(int level, double expected)
    {
        // Arrange
        AddSkill("fast_learner", level);
        
        // Act
        var multiplier = SkillService.GetUpgradeCostMultiplier();
        
        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }
    
    [Theory]
    [InlineData(1, 0.96)]  // -4%
    [InlineData(5, 0.80)]  // -20% max
    public void EarlyBird_ReducesUnlockCosts(int level, double expected)
    {
        // Arrange
        AddSkill("early_bird", level);
        
        // Act
        var multiplier = SkillService.GetUnlockCostMultiplier();
        
        // Assert
        Assert.Equal(expected, multiplier, precision: 2);
    }
}
```

### Skill Stacking Tests

```csharp
public class SkillStackingTests : SkillTestBase
{
    [Fact]
    public void MultipleIncomeSkills_StackCorrectly()
    {
        // Arrange
        AddSkill("cash_flow", 3);        // +9%
        AddSkill("street_smarts", 2);    // +16% to tier 1
        AddSkill("quick_hands", 2);      // +24% to pickpocket
        
        // Act - Calculate total pickpocket income
        var baseIncome = 0.5;  // Pickpocket base
        var operationMultiplier = SkillService.GetOperationMultiplier("pickpocket");
        var tierMultiplier = SkillService.GetTierMultiplier(1);
        var totalMultiplier = SkillService.GetTotalIncomeMultiplier();
        
        var finalIncome = baseIncome * operationMultiplier * tierMultiplier * totalMultiplier;
        
        // Assert - Expected: 0.5 * 1.24 * 1.16 * 1.09 = 0.784
        Assert.True(finalIncome > baseIncome);
        Assert.True(finalIncome < baseIncome * 2);  // Not too overpowered
    }
    
    [Fact]
    public void CostReductionSkills_StackCorrectly()
    {
        // Arrange
        AddSkill("fast_learner", 3);   // -9%
        AddSkill("early_bird", 3);     // -12%
        
        // Act
        var upgradeMult = SkillService.GetUpgradeCostMultiplier();
        var unlockMult = SkillService.GetUnlockCostMultiplier();
        
        // Assert
        Assert.Equal(0.91, upgradeMult, precision: 2);
        Assert.Equal(0.88, unlockMult, precision: 2);
    }
    
    [Fact]
    public void MaxSkillLimit_EnforcedAtFive()
    {
        // Arrange
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);
        
        // Act
        var canAddMore = SkillService.CanAddNewSkill();
        
        // Assert
        Assert.False(canAddMore);
        Assert.Equal(5, GameState.Skills.Count);
    }
    
    [Fact]
    public void ExistingSkill_CanBeUpgraded()
    {
        // Arrange
        AddSkill("cash_flow", 1);
        AddSkill("street_smarts", 1);
        AddSkill("business_acumen", 1);
        AddSkill("quick_hands", 1);
        AddSkill("night_owl", 1);
        
        // Act
        var canUpgrade = SkillService.CanUpgradeSkill("cash_flow");
        
        // Assert
        Assert.True(canUpgrade);
    }
    
    [Fact]
    public void MaxLevelSkill_CannotBeUpgraded()
    {
        // Arrange
        AddSkill("cash_flow", 5);  // Max level
        
        // Act
        var canUpgrade = SkillService.CanUpgradeSkill("cash_flow");
        
        // Assert
        Assert.False(canUpgrade);
    }
}
```

### Prestige Reset Tests

```csharp
public class PrestigeResetTests : SkillTestBase
{
    [Fact]
    public void Prestige_ResetsAllSkills()
    {
        // Arrange
        AddSkill("cash_flow", 3);
        AddSkill("street_smarts", 2);
        AddSkill("old_connections", 1);  // Should be applied before reset
        
        // Act
        var startingCash = SkillService.GetStartingCashAfterPrestige();
        SkillService.ResetForPrestige();
        
        // Assert
        Assert.Empty(GameState.Skills);
        Assert.Equal(75, startingCash);  // Old Connections level 1
    }
    
    [Fact]
    public void Prestige_AppliesOldConnectionsBeforeReset()
    {
        // Arrange
        AddSkill("old_connections", 5);  // $375 starting cash
        
        // Act
        var startingCash = SkillService.GetStartingCashAfterPrestige();
        SkillService.ResetForPrestige();
        
        // Assert
        Assert.Equal(375, startingCash);
    }
    
    [Fact]
    public void Prestige_AppliesReputationBeforeReset()
    {
        // Arrange
        AddSkill("reputation", 3);  // +12% to prestige bonus
        
        // Act
        var bonusRate = SkillService.GetPrestigeBonusRate();  // Should be 0.32
        SkillService.ResetForPrestige();
        
        // Assert
        Assert.Equal(0.32, bonusRate, precision: 2);
        Assert.Empty(GameState.Skills);  // Skills reset after calculation
    }
}
```

### Edge Case Tests

```csharp
public class SkillEdgeCaseTests : SkillTestBase
{
    [Fact]
    public void NoSkills_ReturnsBaseValues()
    {
        // No skills added
        
        // Act
        var incomeMultiplier = SkillService.GetTotalIncomeMultiplier();
        var upgradeCostMultiplier = SkillService.GetUpgradeCostMultiplier();
        var offlineEfficiency = SkillService.GetOfflineEfficiency();
        
        // Assert
        Assert.Equal(1.0, incomeMultiplier);
        Assert.Equal(1.0, upgradeCostMultiplier);
        Assert.Equal(0.4, offlineEfficiency);  // Base 40%
    }
    
    [Fact]
    public void SkillNotInList_ReturnsNoBonus()
    {
        // Act
        var multiplier = SkillService.GetOperationMultiplier("nonexistent");
        
        // Assert
        Assert.Equal(1.0, multiplier);
    }
    
    [Fact]
    public void ZeroLevel_TreatedAsNoSkill()
    {
        // Arrange
        GameState.Skills.Add(new SkillState { Id = "cash_flow", Level = 0 });
        
        // Act
        var multiplier = SkillService.GetTotalIncomeMultiplier();
        
        // Assert
        Assert.Equal(1.0, multiplier);
    }
    
    [Fact]
    public void OfflineEfficiency_CapsAtOneHundredPercent()
    {
        // Arrange - Even with max Night Owl, shouldn't exceed 100%
        AddSkill("night_owl", 5);  // +30%
        
        // If base is 40%, max is 70%
        var efficiency = SkillService.GetOfflineEfficiency();
        
        // Assert
        Assert.True(efficiency <= 1.0);
    }
}
```

---

## TEST EXECUTION INSTRUCTIONS

### For Claude Code

1. **Create test project:**
   ```bash
   dotnet new xunit -n MadeMan.IdleEmpire.Tests -o tests/MadeMan.IdleEmpire.Tests
   ```

2. **Add project reference:**
   ```bash
   dotnet add tests/MadeMan.IdleEmpire.Tests reference src/MadeMan.IdleEmpire
   ```

3. **Add required packages:**
   ```bash
   cd tests/MadeMan.IdleEmpire.Tests
   dotnet add package Moq
   ```

4. **Create test files per the examples above**

5. **Run tests:**
   ```bash
   dotnet test tests/MadeMan.IdleEmpire.Tests
   ```

6. **All tests must pass before balance changes are considered complete**

---

## VERIFICATION CHECKLIST

After implementing all changes:

### Balance Changes
- [ ] Operation income values updated
- [ ] Unlock costs updated
- [ ] Upgrade multiplier updated
- [ ] Prestige threshold and bonus updated
- [ ] Milestone thresholds updated
- [ ] Skill effects updated (all 18 skills)
- [ ] Offline efficiency base updated

### Tests
- [ ] Test project created
- [ ] All 18 skills have at least 2 test cases
- [ ] Stacking tests pass
- [ ] Edge case tests pass
- [ ] All tests green

### Manual Verification
- [ ] Game builds without errors
- [ ] Game runs on emulator
- [ ] First prestige takes ~30-45 min of active play
- [ ] Skills feel impactful but not overpowered

---

## NOTES FOR FUTURE SYSTEMS

### Crew System Impact
- Crew wages will create ongoing expenses
- Balance is designed with ~20-30% of income potentially going to wages
- High-tier crew should require careful economic management

### Casino Impact  
- Casino games will provide risk/reward dynamic
- Current cash flow leaves room for gambling losses
- Skill system doesn't directly affect casino (intentional)

### Mission System Impact
- Missions will provide burst rewards
- Current milestone thresholds account for mission income spikes
- Skills may eventually affect mission success rates

---

**Document Version:** 1.0
**Created:** 2024-12-27
**Target:** Claude Code Implementation
