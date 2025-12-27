# 👊 RIVAL FAMILY EVENTS SPECIFICATION
## Made Man: Idle Empire - Feature Design Document

**Version:** 1.0  
**Date:** December 2024  
**Purpose:** Design document for Claude Code implementation  
**Priority:** Phase 5.5 (After Skills System, Before Casino)

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

## What Is The Rival Family System?

New Porto isn't empty - three powerful mafia families compete with you for control of the city. They will periodically challenge your operations through **random events** that demand response.

## Why This Feature?

The original Omerta browser game had intense PvP where players could permanently kill each other's accounts. This created constant tension and drama. Since Made Man is single-player, we simulate this experience through:

- **Named rival families** with distinct personalities
- **Random events** that threaten your operations
- **Choice-based responses** with risk/reward trade-offs
- **Persistent rivalry** that evolves over time

## Core Design Philosophy

Rivals should feel like **real threats**, not just random obstacles. Each family has a personality and predictable patterns. The player must decide: pay tribute, fight back, or accept losses. This transforms idle gameplay into a world that feels alive and dangerous.

---

# THE THREE FAMILIES

## Family Overview

```
                    NEW PORTO POWER STRUCTURE
                    
        ┌─────────────────────────────────────────┐
        │              THE PLAYER                 │
        │         (Rising Power)                  │
        └─────────────────────────────────────────┘
                          │
            ┌─────────────┼─────────────┐
            ▼             ▼             ▼
    ┌───────────┐  ┌───────────┐  ┌───────────┐
    │  BIANCHI  │  │  O'MALLEY │  │  KOWALSKI │
    │  Family   │  │  Family   │  │  Family   │
    │ (Italian) │  │  (Irish)  │  │  (Polish) │
    └───────────┘  └───────────┘  └───────────┘
```

---

## The Bianchi Family

| Aspect | Details |
|--------|---------|
| **Ethnicity** | Italian-American |
| **Boss** | Don Salvatore Bianchi |
| **Territory** | Little Italy, Uptown |
| **Style** | Traditional, honor-bound, calculated |
| **Primary Threat** | Business takeovers, protection rackets |
| **Personality** | "Nothing personal. Just business." |

### Bianchi Event Types
- Demand tribute payments (regular "tax")
- Attempt to take over your operations
- Offer "partnerships" (share profits or face consequences)
- Target your high-value operations specifically

### Visual Identity
- Colors: Black and gold
- Symbol: Lion head
- Aesthetic: Classic, elegant, old-world

---

## The O'Malley Family

| Aspect | Details |
|--------|---------|
| **Ethnicity** | Irish-American |
| **Boss** | Patrick "Red" O'Malley |
| **Territory** | The Docks, Warehouse District |
| **Style** | Aggressive, unpredictable, violent |
| **Primary Threat** | Crew attacks, hijacking shipments |
| **Personality** | "We hit fast and we hit hard." |

### O'Malley Event Types
- Attack your crew members (send to hospital/jail)
- Hijack mission rewards (steal percentage of payout)
- Start street wars (ongoing damage until resolved)
- Raid your cash reserves directly

### Visual Identity
- Colors: Green and black
- Symbol: Celtic knot or shamrock
- Aesthetic: Rough, working-class, pub brawler

---

## The Kowalski Family

| Aspect | Details |
|--------|---------|
| **Ethnicity** | Polish-American |
| **Boss** | Viktor Kowalski |
| **Territory** | Chinatown, East Side |
| **Style** | Cunning, infiltration, sabotage |
| **Primary Threat** | Sabotage, information warfare |
| **Personality** | "You won't see us coming." |

### Kowalski Event Types
- Sabotage operations (reduce income temporarily)
- Plant informants (increase police heat)
- Steal trade secrets (temporarily boost their own power)
- Corrupt your crew members (loyalty events)

### Visual Identity
- Colors: Blue and silver
- Symbol: Eagle or wolf
- Aesthetic: Cold, calculating, Eastern European

---

# EVENT SYSTEM

## Event Trigger Mechanics

Events trigger based on time and player status:

| Factor | Effect |
|--------|--------|
| **Time** | Base chance every X minutes of active play |
| **Prestige Level** | Higher prestige = more frequent events |
| **Total Income** | Higher income = larger targets |
| **Owned Territories** | More territory = more attack surface |
| **Recent Events** | Cooldown prevents event spam |

### Suggested Timing (Tunable)

| Prestige | Base Event Interval | Notes |
|----------|---------------------|-------|
| 1-2 | Every 30-60 minutes | Gentle introduction |
| 3-4 | Every 20-40 minutes | Regular challenges |
| 5+ | Every 15-30 minutes | Constant pressure |

**Important:** Events should only trigger during active play or shortly after returning (welcome back event). Not during pure offline time.

---

## Event Structure

Each event follows this structure:

```
┌─────────────────────────────────────────────────┐
│              EVENT NOTIFICATION                 │
│                                                 │
│  [Family Icon]                                  │
│                                                 │
│  "THE BIANCHI FAMILY"                          │
│  has made a move against you!                  │
│                                                 │
│  [TAP TO RESPOND]                              │
└─────────────────────────────────────────────────┘
                    │
                    ▼
┌─────────────────────────────────────────────────┐
│              EVENT DETAIL MODAL                 │
│                                                 │
│  PROTECTION DEMAND                             │
│                                                 │
│  Don Bianchi sends his regards—and his         │
│  collectors. Your speakeasy needs              │
│  "protection" this month.                      │
│                                                 │
│  ─────────────────────────────────             │
│                                                 │
│  OPTION A: Pay Tribute                         │
│  Cost: $15,000                                 │
│  Result: Event ends peacefully                 │
│                                                 │
│  OPTION B: Refuse                              │
│  Cost: None                                    │
│  Risk: 40% chance of retaliation               │
│  Retaliation: -25% speakeasy income (4 hours)  │
│                                                 │
│  OPTION C: Send Crew                           │
│  Requires: 2 Enforcers                         │
│  Risk: 30% mission failure                     │
│  Success: No tribute + Bianchi weakened        │
│  Failure: Crew arrested + tribute doubled      │
│                                                 │
│  [PAY $15,000]  [REFUSE]  [SEND CREW]         │
└─────────────────────────────────────────────────┘
```

---

# EVENT CATALOG

## Bianchi Family Events

### Event: Protection Demand

| Aspect | Details |
|--------|---------|
| **Trigger** | Random, more frequent with higher income |
| **Description** | Bianchi family demands monthly "protection" payment |
| **Scale** | Payment scales with total income (roughly 10-20% of hourly income) |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Pay Tribute | Cash (scaled) | 0% | Event ends | N/A |
| Refuse | None | 50% | Nothing happens | Operation income -25% for 4h |
| Send Crew | 2 Enforcers | 35% | No pay + relations improve | Crew jailed + double tribute |

---

### Event: Business Takeover Attempt

| Aspect | Details |
|--------|---------|
| **Trigger** | After player reaches certain income threshold |
| **Description** | Bianchi attempts hostile takeover of one of your operations |
| **Target** | Highest-level operation |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Buy Them Out | Large cash payment | 0% | Keep operation | N/A |
| Legal Defense | Medium payment | 30% | Keep operation | Lose 2 operation levels |
| Fight Back | 3+ Crew | 40% | Keep + bonus income | Lose operation entirely |

---

### Event: Partnership Offer

| Aspect | Details |
|--------|---------|
| **Trigger** | Mid-game, when player has multiple operations |
| **Description** | Bianchi offers to "partner" on operations (they take 20% forever) |
| **Tone** | Presented as generous offer |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Accept Partnership | -20% income (permanent) | 0% | Protection from Bianchi | N/A |
| Politely Decline | None | 40% | Nothing | Bianchi events more frequent |
| Counter-offer | Large cash | 60% | One-time payment, no partnership | Bianchi declares war |

---

## O'Malley Family Events

### Event: Crew Ambush

| Aspect | Details |
|--------|---------|
| **Trigger** | When player has active crew members |
| **Description** | O'Malley thugs attack your crew on the street |
| **Target** | Random crew member |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Pay Medical Bills | $5,000-20,000 | 0% | Crew recovers immediately | N/A |
| Let Them Heal | None | 0% | Crew unavailable 2-8 hours | N/A |
| Retaliate | 2 Muscle | 45% | Crew back + O'Malley weakened | Another crew member hurt |

---

### Event: Shipment Hijack

| Aspect | Details |
|--------|---------|
| **Trigger** | After completing a mission |
| **Description** | O'Malley intercepts your mission payout |
| **Scale** | Threatens 50% of reward |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Let Them Take It | -50% reward | 0% | Keep remainder | N/A |
| Chase Them Down | 2 Wheelman | 35% | Keep full reward | Lose 75% + crew hurt |
| Set a Trap | 3+ Crew, wait 1h | 25% | Double reward | Lose all + crew jailed |

---

### Event: Street War Declaration

| Aspect | Details |
|--------|---------|
| **Trigger** | If player has refused/fought O'Malley multiple times |
| **Description** | Full street war - ongoing damage until resolved |
| **Severity** | -15% all income until resolved |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Sue for Peace | Major cash payment | 0% | War ends | N/A |
| Fight to Win | All available crew | 50% | War ends + bonus | War continues + losses |
| Outlast Them | Wait 24h | 0% | War ends eventually | Continued income loss |

---

## Kowalski Family Events

### Event: Sabotage

| Aspect | Details |
|--------|---------|
| **Trigger** | Random, targets operations |
| **Description** | Your operation has been sabotaged - equipment damaged |
| **Effect** | One operation produces 0 income until fixed |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Repair Immediately | Cash (scaled to operation) | 0% | Operation restored | N/A |
| Hunt Saboteur | 2 Enforcers | 40% | Free repair + catch spy | Repair at full cost |
| Wait for Repairs | None | 0% | Operation restored in 4h | N/A |

---

### Event: Police Informant

| Aspect | Details |
|--------|---------|
| **Trigger** | When player has crew |
| **Description** | Kowalski planted an informant - police heat increasing |
| **Effect** | +50% heat gain until resolved |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Root Out Informant | Investigation time (2h) | 30% | Find and remove | Informant continues |
| Bribe Police | Large cash | 0% | Negates informant | N/A |
| Accept and Adapt | None | 0% | Learn to work with heat | Permanent vulnerability |

---

### Event: Crew Turned

| Aspect | Details |
|--------|---------|
| **Trigger** | When player has 3+ crew |
| **Description** | One of your crew has been turned by Kowalski |
| **Effect** | Crew member leaves and joins rival |

**Options:**

| Option | Cost | Risk | Success Outcome | Failure Outcome |
|--------|------|------|-----------------|-----------------|
| Let Them Go | Lose 1 crew member | 0% | Clean break | N/A |
| Make Counter-offer | 2x crew salary | 50% | Keep crew member | Lose money + crew |
| Example Must Be Made | 1 Hitman | 20% | Crew loyalty +, fear rivals | Failed hit + crew gone |

---

# RIVALRY MECHANICS

## Rivalry Level

Each family has a **Rivalry Level** from 0-100:

| Level | Name | Event Frequency | Severity |
|-------|------|-----------------|----------|
| 0-20 | Cold Peace | Rare | Low stakes |
| 21-40 | Tension | Occasional | Medium stakes |
| 41-60 | Rivalry | Frequent | Significant |
| 61-80 | Hostile | Very frequent | Major threats |
| 81-100 | War | Constant | Existential threats |

### What Affects Rivalry

| Action | Effect |
|--------|--------|
| Pay tribute | -5 rivalry |
| Accept partnership | -20 rivalry |
| Refuse demand | +10 rivalry |
| Fight back successfully | -15 rivalry |
| Fight back and fail | +20 rivalry |
| Kill their crew | +25 rivalry |
| Time passing | -1 per day (slow decay) |

## Family Strength

Each family also has a **Strength** meter:

- **Strong Family:** More dangerous events, higher stakes
- **Weakened Family:** Easier events, lower demands
- **Defeated Family:** No more events (temporary or permanent?)

Player actions can weaken rival families:
- Successful retaliations
- Completing anti-family missions
- Taking their territory (if Territory System exists)

---

# DATA MODEL

## Rival Family Entity

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Unique identifier (bianchi, omalley, kowalski) |
| Name | string | Display name |
| BossName | string | Name of family boss |
| Description | string | Flavor text about family |
| Style | string | Personality/threat style |
| RivalryLevel | int | 0-100, current hostility |
| Strength | int | 0-100, family power level |
| PrimaryThreatType | enum | What kind of events they generate |
| LastEventTime | DateTime | When they last attacked |
| EventCooldown | TimeSpan | Minimum time between events |

## Event Entity

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Unique event identifier |
| FamilyId | string | Which family triggers this |
| Title | string | Event title |
| Description | string | Narrative text |
| MinPrestige | int | Minimum prestige to trigger |
| MinIncome | double | Minimum income to trigger |
| Options | List | Available response options |

## Event Option Entity

| Property | Type | Description |
|----------|------|-------------|
| Id | string | Option identifier |
| Label | string | Button text |
| Description | string | Explanation of choice |
| CashCost | double | Immediate cash cost |
| CrewRequirement | string | Required crew types |
| RiskPercent | int | Chance of failure |
| SuccessEffect | Effect | What happens on success |
| FailureEffect | Effect | What happens on failure |

## Effect Entity

| Property | Type | Description |
|----------|------|-------------|
| Type | enum | Income, Cash, Heat, Crew, Rivalry |
| Target | string | What's affected |
| Value | double | Modifier value |
| Duration | TimeSpan? | How long (null = permanent) |
| Message | string | Feedback text |

---

# USER INTERFACE

## Event Notification

When event triggers:
1. **Alert indicator** appears (exclamation mark or similar)
2. Tapping opens event modal
3. Event demands attention but doesn't pause game

### Notification Badge

Show on:
- Bottom navigation (main area)
- Wherever makes sense in UI hierarchy

### Event Queue

If multiple events trigger close together:
- Queue them
- Show one at a time
- Indicate "1 more event pending"

## Event Modal

Full-screen or large modal with:
- Family identity (name, icon, colors)
- Event narrative text
- Clear presentation of options
- Risk/reward clearly shown
- Crew requirements visible
- Confirmation for dangerous choices

## Family Status Screen

New screen accessible from settings or empire tab:

```
┌─────────────────────────────────────────────────┐
│               RIVAL FAMILIES                    │
├─────────────────────────────────────────────────┤
│                                                 │
│  [Lion Icon] BIANCHI FAMILY                    │
│  ████████░░░░░░░░░░░░ Rivalry: 35%             │
│  Status: Tense                                  │
│  "Don Bianchi is watching your rise..."        │
│                                                 │
│  [Clover Icon] O'MALLEY FAMILY                 │
│  ██░░░░░░░░░░░░░░░░░░ Rivalry: 10%             │
│  Status: Cold Peace                            │
│  "The Irish keep their distance—for now."      │
│                                                 │
│  [Eagle Icon] KOWALSKI FAMILY                  │
│  ████████████░░░░░░░░ Rivalry: 55%             │
│  Status: Hostile                               │
│  "Viktor Kowalski has marked you."             │
│                                                 │
└─────────────────────────────────────────────────┘
```

---

# PERSISTENCE

## Save Data Required

- Three family states (rivalry level, strength, last event time)
- Active effects from events (income modifiers with duration)
- Event history (optional, for narrative continuity)

## Prestige Behavior

**Rivalry levels reset on prestige** - You start fresh with all families.

Rationale: This prevents permanent difficulty escalation and lets players try different strategies each run.

**Exception:** Consider a small carryover bonus:
- "You have a reputation. Families start with +10 rivalry."
- Or: "Past successes echo. Families start weakened."

---

# BALANCE CONSIDERATIONS

## Event Frequency

Events should feel like:
- **Occasional challenges**, not constant harassment
- **Meaningful interruptions**, not annoying spam
- **Real threats**, not trivial obstacles

Suggested baseline: One event every 20-30 minutes of active play at mid-game.

## Cost Scaling

Event costs should scale with player progress:
- Early game tribute: $5,000-20,000
- Mid game tribute: $50,000-200,000
- Late game tribute: $500,000+

Use percentage of hourly income as guide (e.g., "Pay 15% of your hourly income").

## Risk Balance

| Choice Type | Risk | Reward |
|-------------|------|--------|
| Safe (pay) | 0% | No bonus, lose cash |
| Medium (refuse) | 30-50% | Possible clean escape |
| Risky (fight) | 40-60% | Big rewards if successful |

Players should feel:
- Safe options are always available (money solves problems)
- Risky options are tempting but genuinely dangerous
- Crew deployment matters and has real stakes

---

# IMPLEMENTATION PHASES

## Phase 1: Core Data & Event Engine
1. Create RivalFamily model
2. Create Event and EventOption models
3. Create Effect model for outcomes
4. Build event triggering logic (time + conditions)
5. Build random event selection system

## Phase 2: Event Content
1. Create 3-5 events per family (9-15 total)
2. Define options for each event
3. Balance costs and risks
4. Write narrative text for all events

## Phase 3: UI Components
1. Create event notification indicator
2. Build event detail modal
3. Create family status screen
4. Handle event queue

## Phase 4: Effect Integration
1. Integrate with income calculation
2. Integrate with crew system
3. Integrate with police/heat system
4. Handle temporary vs. permanent effects

## Phase 5: Polish
1. Visual identity for each family
2. Animation/feedback for event outcomes
3. Sound effects if applicable
4. Tutorial for first event

---

# TESTING CHECKLIST

## Event Trigger Tests
- [ ] Events trigger at appropriate intervals
- [ ] Events don't trigger during offline time
- [ ] Higher prestige increases event frequency
- [ ] Cooldown prevents event spam
- [ ] Events respect minimum requirements

## Response Tests
- [ ] Pay option deducts correct cash
- [ ] Refuse option has correct risk percentage
- [ ] Fight option checks crew requirements
- [ ] Success outcomes apply correctly
- [ ] Failure outcomes apply correctly
- [ ] Crew-based options use/release crew properly

## Effect Tests
- [ ] Income modifiers apply to correct operations
- [ ] Cash changes reflect in balance
- [ ] Heat changes affect police system
- [ ] Timed effects expire correctly
- [ ] Effects persist through app restart (while active)

## Rivalry Tests
- [ ] Rivalry levels change based on actions
- [ ] Higher rivalry increases event severity
- [ ] Rivalry decays over time
- [ ] Rivalry resets on prestige
- [ ] Family strength affects event difficulty

## UI Tests
- [ ] Event notification appears correctly
- [ ] Event modal displays all information
- [ ] Options are clearly presented
- [ ] Family status screen shows accurate data
- [ ] Event queue handles multiple events

## Edge Cases
- [ ] No crew available for crew-based options
- [ ] Not enough cash for pay options
- [ ] Event triggers at prestige boundary
- [ ] Multiple effects from same family stack correctly

---

# FUTURE EXPANSION POSSIBILITIES

These are NOT part of MVP but could extend the system:

1. **Family Missions** - Special missions to directly attack rival operations
2. **Alliances** - Form temporary alliance with one family against another
3. **Family Elimination** - Permanently remove a family from the game
4. **Story Integration** - Named characters from families appear in story chapters
5. **Territory Conflict** - Rivals contest your owned districts (links to Territory System)
6. **Peace Treaties** - Expensive but permanent peace options
7. **Family Favors** - Call in favors from weakened families

---

# INTEGRATION WITH OTHER SYSTEMS

## Territory System

If Territory System is implemented:
- Families can contest your districts
- Losing territory reduces bonuses
- Some events only trigger in certain districts
- "Taking" family territory becomes an option

## Crew System

Events integrate closely with crew:
- Crew required for fight options
- Crew can be casualties of events
- Crew loyalty can be tested by Kowalski events

## Police System

Events affect heat:
- Some events increase heat
- Bribery options can decrease heat
- Informant events directly manipulate heat system

## Mission System

Events can interact with missions:
- Hijack events target mission rewards
- Some events trigger after mission completion
- Family-targeted missions could be added

---

# NARRATIVE EXAMPLES

## Sample Event: Bianchi Protection Demand

**Notification:**
> "The Bianchi Family demands an audience."

**Event Text:**
> Don Salvatore Bianchi's men arrive at your speakeasy just before opening. They're polite, well-dressed, and absolutely not here for the jazz.
>
> "The Don appreciates your success," the lead man says, straightening his cuffs. "He thinks it's time you showed some... appreciation in return. $15,000 should cover this month's protection."
>
> His smile doesn't reach his eyes.

**Options:**
- **[Pay the $15,000]** - "Business is business. Pay the man."
- **[Refuse]** - "Tell Don Bianchi I don't need his protection."
- **[Send Your Own Message]** - Deploy 2 Enforcers to make your position clear.

---

## Sample Event: O'Malley Ambush

**Notification:**
> "ALERT: O'Malley attack in progress!"

**Event Text:**
> Your wheelman Tommy didn't show up for his shift. You find him in an alley off Dock Street, beaten bloody. A shamrock is painted on the wall beside him.
>
> Red O'Malley's calling card.
>
> Tommy's alive, but he won't be driving for a while. Unless you want to speed up his recovery... or send a message back.

**Options:**
- **[Pay for Treatment - $8,000]** - "Get Tommy to Dr. Moretti. Whatever it costs."
- **[Let Him Heal]** - "Tommy's tough. He'll recover on his own." (Unavailable 6 hours)
- **[Retaliate]** - Deploy 2 Muscle to find who did this. And return the favor.

---

# SUMMARY

The Rival Family Events system adds:
- **Living world** - New Porto isn't empty; powerful enemies exist
- **Tension** - Constant awareness that threats can emerge
- **Meaningful choices** - Pay, refuse, or fight - each has consequences
- **Character** - Named families with personalities create story moments
- **Risk/reward** - Fighting back is tempting but dangerous

This transforms Made Man from pure idle clicking into an experience where the player feels like they're navigating a dangerous criminal underworld, not just watching numbers increase.

---

*Design Specification v1.0*  
*Made Man: Idle Empire*  
*Phase 5.5 Feature*
