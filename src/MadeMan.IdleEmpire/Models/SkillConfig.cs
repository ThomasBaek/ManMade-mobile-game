namespace MadeMan.IdleEmpire.Models;

public static class SkillConfig
{
    // === MILESTONE THRESHOLDS ===
    public static readonly double[] MilestoneThresholds = new[]
    {
        1_000.0,
        5_000.0,
        15_000.0,
        50_000.0,
        150_000.0,
        500_000.0,
        1_500_000.0,
        5_000_000.0,
        15_000_000.0,
        50_000_000.0
    };

    public const int MaxSkills = 5;
    public const int MaxSkillLevel = 5;
    public const int SkillChoicesPerMilestone = 3;

    // === ALL SKILLS (18 total) ===
    public static readonly SkillDefinition[] Skills = new[]
    {
        // === INCOME SKILLS (6) ===
        new SkillDefinition
        {
            Id = "cash_flow",
            Name = "Cash Flow",
            Description = "Increases total income",
            Icon = "skill_cash_flow.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 4.0 // +4% per level
        },
        new SkillDefinition
        {
            Id = "street_smarts",
            Name = "Street Smarts",
            Description = "Boosts Tier 1 operations",
            Icon = "skill_street_smarts.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 10.0, // +10% per level
            TargetTier = 1
        },
        new SkillDefinition
        {
            Id = "business_acumen",
            Name = "Business Acumen",
            Description = "Boosts Tier 2 operations",
            Icon = "skill_business_acumen.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 12.0, // +12% per level
            TargetTier = 2
        },
        new SkillDefinition
        {
            Id = "lucky_break",
            Name = "Lucky Break",
            Description = "Chance for double income",
            Icon = "skill_lucky_break.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Chance,
            EffectPerLevel = 4.0 // +4% chance per level
        },
        new SkillDefinition
        {
            Id = "the_skim",
            Name = "The Skim",
            Description = "Cashback on all purchases",
            Icon = "skill_the_skim.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 5.0 // +5% cashback per level
        },
        new SkillDefinition
        {
            Id = "compound_interest",
            Name = "Compound Interest",
            Description = "Income grows over time",
            Icon = "skill_compound_interest.png",
            Category = SkillCategory.Income,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 3.0 // +3% per 5 min per level (max 6 intervals)
        },

        // === OPERATIONS SKILLS (4) ===
        new SkillDefinition
        {
            Id = "quick_hands",
            Name = "Quick Hands",
            Description = "Boosts Pickpocketing",
            Icon = "skill_quick_hands.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 25.0, // +25% per level
            TargetOperationId = "pickpocket"
        },
        new SkillDefinition
        {
            Id = "chop_shop",
            Name = "Chop Shop",
            Description = "Boosts Car Theft",
            Icon = "skill_chop_shop.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 20.0, // +20% per level
            TargetOperationId = "cartheft"
        },
        new SkillDefinition
        {
            Id = "inside_man",
            Name = "Inside Man",
            Description = "Boosts Burglary",
            Icon = "skill_inside_man.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 18.0, // +18% per level
            TargetOperationId = "burglary"
        },
        new SkillDefinition
        {
            Id = "happy_hour",
            Name = "Happy Hour",
            Description = "Boosts Speakeasy",
            Icon = "skill_happy_hour.png",
            Category = SkillCategory.Operations,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 15.0, // +15% per level
            TargetOperationId = "speakeasy"
        },

        // === OFFLINE SKILLS (4) ===
        new SkillDefinition
        {
            Id = "night_owl",
            Name = "Night Owl",
            Description = "Increases offline efficiency",
            Icon = "skill_night_owl.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 10.0 // +10% efficiency per level (50% -> 100%)
        },
        new SkillDefinition
        {
            Id = "extended_shift",
            Name = "Extended Shift",
            Description = "Extends max offline time",
            Icon = "skill_extended_shift.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.Duration,
            EffectPerLevel = 1.0 // +1 hour per level
        },
        new SkillDefinition
        {
            Id = "passive_income",
            Name = "Passive Income",
            Description = "Baseline income regardless of operations",
            Icon = "skill_passive_income.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.FlatBonus,
            EffectPerLevel = 1.0 // +$1/s per level
        },
        new SkillDefinition
        {
            Id = "godfathers_cut",
            Name = "Godfather's Cut",
            Description = "Bonus on offline earnings",
            Icon = "skill_godfathers_cut.png",
            Category = SkillCategory.Offline,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 8.0 // +8% per level
        },

        // === PRESTIGE SKILLS (4) ===
        new SkillDefinition
        {
            Id = "old_connections",
            Name = "Old Connections",
            Description = "Start with bonus cash after prestige",
            Icon = "skill_old_connections.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.FlatBonus,
            EffectPerLevel = 100.0 // +$100 per level
        },
        new SkillDefinition
        {
            Id = "reputation",
            Name = "Reputation",
            Description = "Increases prestige bonus",
            Icon = "skill_reputation.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.Multiplier,
            EffectPerLevel = 6.0 // +6% per level
        },
        new SkillDefinition
        {
            Id = "fast_learner",
            Name = "Fast Learner",
            Description = "Reduces upgrade costs",
            Icon = "skill_fast_learner.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.Reduction,
            EffectPerLevel = 4.0 // -4% per level
        },
        new SkillDefinition
        {
            Id = "early_bird",
            Name = "Early Bird",
            Description = "Reduces unlock costs",
            Icon = "skill_early_bird.png",
            Category = SkillCategory.Prestige,
            EffectType = SkillEffectType.Reduction,
            EffectPerLevel = 5.0 // -5% per level
        }
    };

    // === HELPER: Get operation tier ===
    public static int GetOperationTier(string operationId)
    {
        return operationId switch
        {
            "pickpocket" or "cartheft" or "burglary" => 1,
            "speakeasy" or "casino" => 2,
            _ => 0
        };
    }
}
