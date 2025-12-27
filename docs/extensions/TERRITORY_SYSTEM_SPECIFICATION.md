# 🏙️ TERRITORY SYSTEM SPECIFICATION
## Made Man: Idle Empire - Feature Design Document

**Version:** 1.0  
**Date:** December 2024  
**Purpose:** Design document for Claude Code implementation  
**Priority:** Phase 4.5 (After Police System, Before Skills)

---

# HOW TO USE THIS DOCUMENT

This document describes WHAT should be built, not HOW. Claude Code should:

1. Read the feature description
2. Analyze the existing project structure
3. Evaluate how this feature integrates with existing systems
4. Form its own implementation plan
5. Implement in accordance with the project's architecture

**Important:** The project structure may have changed. Claude Code should always start by analyzing the current codebase before implementation.

---

# CONCEPT OVERVIEW

## What Is The Territory System?

New Porto is divided into distinct **districts**, each with its own criminal character and economic bonuses. The player starts controlling one district and can expand their empire by **purchasing control** of additional districts.

## Why This Feature?

The original Omerta browser game had territory and city-based gameplay that created a sense of **world ownership**. This system brings that feeling to a single-player idle game by:

- Adding geographic progression beyond just "numbers go up"
- Creating meaningful strategic choices (which district to expand into next)
- Providing visual feedback of empire growth (map view)
- Unlocking district-specific bonuses that affect gameplay style

## Core Design Philosophy

Territories should feel like **real expansion** of criminal influence. Each district has a distinct identity and provides bonuses that encourage different playstyles. Controlling more territory = more income but also more complexity to manage.

---

# THE CITY OF NEW PORTO

## Map Layout

```
┌─────────────────────────────────────────────────────┐
│                    NEW PORTO                        │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐          │
│  │  UPTOWN  │  │ DOWNTOWN │  │ EAST     │          │
│  │  $$$     │  │  Center  │  │ SIDE     │          │
│  └──────────┘  └──────────┘  └──────────┘          │
│  ┌──────────┐  ┌──────────┐  ┌──────────┐          │
│  │ LITTLE   │  │WAREHOUSE │  │  THE     │          │
│  │ ITALY    │  │ DISTRICT │  │  DOCKS   │          │
│  └──────────┘  └──────────┘  └──────────┘          │
│  ┌──────────┐  ┌──────────┐                        │
│  │CHINATOWN │  │ HARBOR   │                        │
│  │          │  │ POINT    │                        │
│  └──────────┘  └──────────┘                        │
└─────────────────────────────────────────────────────┘
```

---

# DISTRICT DEFINITIONS

## Overview Table

| District | Theme | Primary Bonus | Purchase Price | Unlock Requirement |
|----------|-------|---------------|----------------|-------------------|
| **Downtown** | Commerce hub | Base district (no bonus) | FREE | Start |
| **The Docks** | Smuggling, imports | +25% Bootlegging income | $100,000 | Prestige 2 |
| **Little Italy** | Old families | -15% Crew wages | $150,000 | Prestige 2 |
| **Warehouse District** | Operations HQ | +20% Mission rewards | $200,000 | Prestige 3 |
| **Chinatown** | Underground gambling | +30% Casino winnings | $300,000 | Prestige 3 |
| **East Side** | Street crime | +25% Pickpocket/Car Theft | $250,000 | Prestige 3 |
| **Uptown** | Political connections | -20% Police heat gain | $500,000 | Prestige 4 |
| **Harbor Point** | Import/export | +15% ALL operation income | $1,000,000 | Prestige 5 |

---

## Detailed District Descriptions

### Downtown (Starting District)

| Aspect | Details |
|--------|---------|
| **Theme** | The beating heart of New Porto's commercial district |
| **Visual Style** | Art deco buildings, busy streets, neon signs |
| **Narrative** | "Where you started. A cramped apartment above a deli. Not much, but it's yours." |
| **Bonus** | None (baseline) |
| **Cost** | FREE (starting territory) |
| **Unlock** | Game start |

**Special:** All players begin here. This establishes the baseline for all bonuses.

---

### The Docks

| Aspect | Details |
|--------|---------|
| **Theme** | Shipping, smuggling, union corruption |
| **Visual Style** | Foggy piers, cargo ships, warehouses |
| **Narrative** | "Control the docks, control what comes in. And what doesn't get reported to customs." |
| **Bonus** | +25% income from Bootlegging operations |
| **Cost** | $100,000 |
| **Unlock** | Prestige 2 |

**Gameplay Impact:** Essential for players focusing on bootlegging/alcohol operations. Makes speakeasy and distillery more profitable.

---

### Little Italy

| Aspect | Details |
|--------|---------|
| **Theme** | Traditional mafia families, respect, loyalty |
| **Visual Style** | Narrow streets, trattorias, old brownstones |
| **Narrative** | "The old neighborhood. Where omertà is still the law. Where loyalty means something." |
| **Bonus** | -15% Crew wages |
| **Cost** | $150,000 |
| **Unlock** | Prestige 2 |

**Gameplay Impact:** Reduces ongoing expenses significantly. Essential for players running large crews. Stacks well with mission-focused playstyle.

---

### Warehouse District

| Aspect | Details |
|--------|---------|
| **Theme** | Operations, planning, organized heists |
| **Visual Style** | Industrial, brick warehouses, loading bays |
| **Narrative** | "Empty warehouses make excellent planning rooms. And even better places to... store things." |
| **Bonus** | +20% Mission cash rewards |
| **Cost** | $200,000 |
| **Unlock** | Prestige 3 |

**Gameplay Impact:** Directly boosts active gameplay loop (missions). Synergizes with crew system and risk-taking playstyle.

---

### Chinatown

| Aspect | Details |
|--------|---------|
| **Theme** | Underground gambling, exotic goods, secrets |
| **Visual Style** | Red lanterns, narrow alleys, hidden doors |
| **Narrative** | "Behind every noodle shop, a gambling den. Behind every herbalist, an opium dream." |
| **Bonus** | +30% Casino winnings (applies to net profit) |
| **Cost** | $300,000 |
| **Unlock** | Prestige 3 |

**Gameplay Impact:** Makes casino feature more profitable. High-risk high-reward synergy for gambling-focused players.

---

### East Side

| Aspect | Details |
|--------|---------|
| **Theme** | Street crime, hustling, survival |
| **Visual Style** | Tenements, street corners, pawn shops |
| **Narrative** | "The hungry streets. Every kid here is either a mark or a mugger. Usually both." |
| **Bonus** | +25% income from Pickpocket and Car Theft operations |
| **Cost** | $250,000 |
| **Unlock** | Prestige 3 |

**Gameplay Impact:** Boosts early-game operations. Useful for speedrun strategies or early prestige optimization.

---

### Uptown

| Aspect | Details |
|--------|---------|
| **Theme** | Wealth, politics, corruption |
| **Visual Style** | Mansions, manicured lawns, fancy cars |
| **Narrative** | "Where the real power lives. Judges, politicians, police chiefs. Everyone has a price." |
| **Bonus** | -20% Police heat gain rate |
| **Cost** | $500,000 |
| **Unlock** | Prestige 4 |

**Gameplay Impact:** Reduces risk from police system. Allows more aggressive mission strategies. Synergizes with high-risk operations.

---

### Harbor Point

| Aspect | Details |
|--------|---------|
| **Theme** | Elite territory, international connections |
| **Visual Style** | Luxury yachts, exclusive clubs, ocean views |
| **Narrative** | "The pinnacle. From here, you don't just control New Porto. You own it." |
| **Bonus** | +15% ALL operation income |
| **Cost** | $1,000,000 |
| **Unlock** | Prestige 5 |

**Gameplay Impact:** The ultimate territory. Universal bonus affects all income sources. End-game goal that provides satisfying power spike.

---

# USER INTERFACE

## Territory Tab/Page

A new tab or sub-section accessible from the main navigation. Suggests placement under "Empire" tab as a sub-view.

### Map View

Display a stylized map of New Porto showing all districts:

- **Owned districts:** Full color, active appearance
- **Available districts:** Visible but grayed/dimmed with price tag
- **Locked districts:** Darker, with lock icon and unlock requirement text

### Interaction Flow

```
Player taps district on map
         │
         ▼
┌─────────────────────────┐
│   DISTRICT DETAIL MODAL │
│                         │
│   [District Name]       │
│   [Visual/Icon]         │
│   [Description]         │
│                         │
│   Bonus: +25% Bootleg   │
│   Status: Available     │
│   Price: $100,000       │
│                         │
│   [PURCHASE BUTTON]     │
│   or                    │
│   [LOCKED - Prestige 3] │
└─────────────────────────┘
```

### Owned Districts Summary

Below the map (or in a scrollable list):
- Show all owned districts with their active bonuses
- Total bonus summary: "Your Empire Bonuses: +25% Bootleg, -15% Wages..."

---

# DATA MODEL

## District Entity

Each district should have these properties:

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Unique identifier (e.g., "docks", "little_italy") |
| Name | string | Display name |
| Description | string | Narrative flavor text |
| Theme | string | Short theme description |
| BonusType | enum | Type of bonus (Income, Cost, Heat, Casino, All) |
| BonusTarget | string | What the bonus applies to (e.g., "bootleg", "crew_wages") |
| BonusValue | double | Bonus multiplier (e.g., 0.25 for +25%, -0.15 for -15%) |
| PurchasePrice | double | Cash cost to acquire |
| UnlockPrestige | int | Minimum prestige level required |
| IsOwned | bool | Whether player controls this district |

## GameState Extension

The existing GameState needs to track:

- List of owned district IDs
- Starting district is always owned (Downtown)

## District Definitions

Store all district definitions in a configuration file or static class, similar to how Operations are defined. This keeps game balance data in one place.

---

# BONUS APPLICATION

## How Bonuses Stack

District bonuses apply **multiplicatively** with other bonuses (prestige, skills):

```
Final Income = Base Income × Prestige Bonus × Skill Bonuses × District Bonus
```

## Bonus Categories

| Category | Affected By | Example Districts |
|----------|-------------|-------------------|
| Operation Income | Specific operation types | Docks (+bootleg), East Side (+street crime) |
| All Income | Every income source | Harbor Point (+15% all) |
| Cost Reduction | Crew wages, upgrade costs | Little Italy (-15% wages) |
| Heat Reduction | Police heat gain rate | Uptown (-20% heat) |
| Casino Modifier | Casino net winnings | Chinatown (+30% winnings) |
| Mission Rewards | Mission completion payouts | Warehouse District (+20%) |

## Integration Points

Districts affect these existing systems:

1. **Income Calculation Service** - Must check owned districts for income bonuses
2. **Crew Wage Calculation** - Must apply wage reduction if Little Italy owned
3. **Police/Heat System** - Must apply heat reduction if Uptown owned
4. **Casino System** - Must apply winnings modifier if Chinatown owned
5. **Mission Reward Calculation** - Must apply bonus if Warehouse District owned

---

# PURCHASE FLOW

## Requirements Check

Before allowing purchase:
1. Player has required prestige level
2. Player has enough cash
3. District is not already owned

## Purchase Action

When player confirms purchase:
1. Deduct cash from player balance
2. Add district ID to owned list
3. Immediately apply district bonuses to all calculations
4. Show purchase confirmation with bonus summary
5. Persist state

## UI Feedback

- Purchase button shows price and affordability
- Locked districts show unlock requirement clearly
- Owned districts show "CONTROLLED" status
- Consider celebratory animation/feedback on purchase

---

# PERSISTENCE

## Save Data

District ownership must persist between sessions. Add to existing save system:

- Array of owned district IDs
- Downtown is always included (default)

## Prestige Behavior

**Districts persist through prestige.** Once purchased, a district remains owned forever. This makes territory a form of permanent progression alongside prestige bonuses.

---

# BALANCE CONSIDERATIONS

## Pricing Philosophy

- Early districts (Prestige 2-3) should be achievable within first few prestiges
- Mid districts require deliberate saving
- Harbor Point ($1M) is a long-term goal

## Bonus Stacking

Be mindful of bonus stacking power:
- Player with ALL districts would have significant bonuses
- This is intentional - territory represents permanent power growth
- Balance new features assuming veteran players may have all territories

## Prestige Gates

Prestige requirements prevent:
- New players from being overwhelmed with choices
- Speed-running past content
- Making early game trivially easy with late-game bonuses

---

# IMPLEMENTATION PHASES

## Phase 1: Core Data
1. Create District model/entity
2. Define all 8 districts in configuration
3. Extend GameState with owned districts list
4. Add persistence for district ownership

## Phase 2: Bonus System
1. Create DistrictBonusService or extend existing bonus calculations
2. Integrate district bonuses into income calculations
3. Integrate with crew wages
4. Integrate with other affected systems (as they exist)

## Phase 3: User Interface
1. Create Territory page/view
2. Design and implement map visualization
3. Create district detail modal
4. Implement purchase flow
5. Show owned districts and active bonuses

## Phase 4: Polish
1. Visual feedback for owned vs available vs locked
2. Purchase animations/celebrations
3. Integration into existing Empire tab navigation
4. Tutorial/guidance for new feature

---

# TESTING CHECKLIST

## Functional Tests
- [ ] Can view all districts on map
- [ ] Owned district (Downtown) shows as controlled from start
- [ ] Locked districts show prestige requirement
- [ ] Available districts show purchase price
- [ ] Can purchase district when requirements met
- [ ] Cannot purchase when lacking cash
- [ ] Cannot purchase locked districts
- [ ] District bonuses apply immediately after purchase
- [ ] District ownership persists after app restart
- [ ] District ownership persists after prestige

## Bonus Integration Tests
- [ ] Docks bonus increases bootleg operation income
- [ ] Little Italy reduces crew wages
- [ ] Warehouse District increases mission rewards
- [ ] Chinatown increases casino winnings
- [ ] East Side boosts pickpocket and car theft
- [ ] Uptown reduces heat gain rate
- [ ] Harbor Point applies to ALL income sources
- [ ] Multiple district bonuses stack correctly

## Edge Cases
- [ ] Player with all districts doesn't break calculations
- [ ] Bonus calculations handle zero/negative values properly
- [ ] UI handles long district names
- [ ] Map displays correctly on different screen sizes

---

# VISUAL STYLE NOTES

## Map Aesthetic

The map should evoke 1930s style:
- Vintage/aged paper texture
- Art deco typography for district names
- Sepia or muted color palette
- District boundaries drawn like old city planning maps

## District Icons

Each district should have a recognizable icon:
- Downtown: City skyline
- Docks: Anchor or ship
- Little Italy: Wine bottle or Italian flag colors
- Warehouse District: Crate or warehouse building
- Chinatown: Lantern or dragon
- East Side: Street corner or tenement
- Uptown: Mansion or top hat
- Harbor Point: Yacht or lighthouse

---

# FUTURE EXPANSION POSSIBILITIES

These are NOT part of MVP but could extend the system later:

1. **District Upgrades** - Invest more cash to increase district bonus level
2. **District Events** - Random events specific to owned districts
3. **District Heat** - Each district has separate heat level
4. **District Operations** - Unique operations only available in certain districts
5. **Rival Presence** - Rival families can contest your districts (see Rival Family Events spec)

---

# SUMMARY

The Territory System adds:
- **Geographic progression** - Visual representation of empire growth
- **Strategic choices** - Which bonus do you want next?
- **Permanent progression** - Districts persist through prestige
- **Thematic depth** - Each district tells part of New Porto's story

This transforms "Made Man" from pure numbers game into an empire-building experience with a tangible world to conquer.

---

*Design Specification v1.0*  
*Made Man: Idle Empire*  
*Phase 4.5 Feature*
