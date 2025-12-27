using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Views;

public partial class SkillsGuidePage : ContentPage
{
    public SkillsGuidePage()
    {
        InitializeComponent();
        BuildSkillsList();
    }

    private void BuildSkillsList()
    {
        // Intro section
        SkillsList.Children.Add(CreateIntroSection());

        // Group skills by category
        var categories = new[]
        {
            (SkillCategory.Income, "💰 INCOME SKILLS", "Boost your cash flow and maximize earnings"),
            (SkillCategory.Operations, "⚡ OPERATION SKILLS", "Supercharge specific criminal enterprises"),
            (SkillCategory.Offline, "🌙 OFFLINE SKILLS", "Keep the money flowing while you're away"),
            (SkillCategory.Prestige, "⭐ PRESTIGE SKILLS", "Gain advantages for your next empire")
        };

        foreach (var (category, title, subtitle) in categories)
        {
            SkillsList.Children.Add(CreateCategoryHeader(title, subtitle));

            var skills = SkillConfig.Skills.Where(s => s.Category == category);
            foreach (var skill in skills)
            {
                SkillsList.Children.Add(CreateSkillCard(skill));
            }
        }

        // Build archetypes section
        SkillsList.Children.Add(CreateBuildArchetypes());
    }

    private static View CreateIntroSection()
    {
        return new Frame
        {
            BackgroundColor = GetColor("SurfaceLight"),
            CornerRadius = 12,
            Padding = new Thickness(16),
            BorderColor = Colors.Transparent,
            Margin = new Thickness(0, 0, 0, 8),
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Label
                    {
                        Text = "YOUR CREW AWAITS",
                        FontFamily = "BebasNeue",
                        FontSize = 18,
                        TextColor = GetColor("Gold"),
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = "As your reputation grows in New Porto, talented people seek you out. " +
                               "At each milestone, choose from 3 skills to build your unique empire. " +
                               "You can have up to 5 skills, each upgradeable to level 5.",
                        FontSize = 13,
                        TextColor = GetColor("TextSecondary"),
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    new Label
                    {
                        Text = "Skills reset on prestige — build a new strategy each run!",
                        FontSize = 12,
                        FontAttributes = FontAttributes.Italic,
                        TextColor = GetColor("Gold"),
                        HorizontalTextAlignment = TextAlignment.Center,
                        Margin = new Thickness(0, 4, 0, 0)
                    }
                }
            }
        };
    }

    private static View CreateCategoryHeader(string title, string subtitle)
    {
        return new VerticalStackLayout
        {
            Spacing = 4,
            Margin = new Thickness(0, 16, 0, 8),
            Children =
            {
                new Label
                {
                    Text = title,
                    FontFamily = "BebasNeue",
                    FontSize = 22,
                    TextColor = GetColor("Gold")
                },
                new Label
                {
                    Text = subtitle,
                    FontSize = 13,
                    TextColor = GetColor("TextSecondary")
                }
            }
        };
    }

    private static View CreateSkillCard(SkillDefinition skill)
    {
        var (flavorText, mechanicText) = GetSkillDescriptions(skill);

        var content = new VerticalStackLayout
        {
            Spacing = 10,
            Children =
            {
                // Skill name
                new Label
                {
                    Text = skill.Name.ToUpper(),
                    FontFamily = "BebasNeue",
                    FontSize = 20,
                    TextColor = GetColor("TextPrimary")
                },
                // Flavor text (narrative)
                new Label
                {
                    Text = $"\"{flavorText}\"",
                    FontSize = 13,
                    FontAttributes = FontAttributes.Italic,
                    TextColor = GetColor("TextSecondary")
                },
                // Mechanic description
                new Label
                {
                    Text = mechanicText,
                    FontSize = 14,
                    TextColor = GetColor("TextPrimary")
                },
                // Divider
                new BoxView
                {
                    HeightRequest = 1,
                    BackgroundColor = GetColor("SurfaceLight"),
                    Margin = new Thickness(0, 4)
                },
                // Level table
                CreateLevelTable(skill)
            }
        };

        return new Frame
        {
            BackgroundColor = GetColor("Surface"),
            CornerRadius = 12,
            Padding = new Thickness(16),
            BorderColor = Colors.Transparent,
            Margin = new Thickness(0, 4),
            Content = content
        };
    }

    private static (string flavor, string mechanic) GetSkillDescriptions(SkillDefinition skill)
    {
        return skill.Id switch
        {
            // INCOME SKILLS
            "cash_flow" => (
                "Money flows to those who know how to attract it.",
                "Increases ALL income from every operation. A foundational skill that scales with your entire empire."
            ),
            "street_smarts" => (
                "You didn't survive the streets without learning a few tricks.",
                "Boosts Tier 1 operations: Pickpocketing, Car Theft, and Burglary. Perfect for early-game dominance."
            ),
            "business_acumen" => (
                "Running a speakeasy isn't just about booze — it's about business.",
                "Boosts Tier 2 operations: Speakeasy and Casino. Maximize your late-game enterprises."
            ),
            "lucky_break" => (
                "Sometimes the dice roll your way. Make sure they roll often.",
                "Each income tick has a chance to pay DOUBLE. High risk, high reward — the gambler's choice."
            ),
            "the_skim" => (
                "Every transaction has a little extra for those who know where to look.",
                "Get cashback on every purchase. The more you spend on upgrades, the more you earn back."
            ),
            "compound_interest" => (
                "Time is money, and you've got plenty of both.",
                "Income grows the longer you play in a session. Rewards dedicated mobsters who stick around."
            ),

            // OPERATION SKILLS
            "quick_hands" => (
                "Faster than the eye can see, smoother than silk.",
                "Dramatically boosts Pickpocketing income. Turn petty theft into a serious enterprise."
            ),
            "chop_shop" => (
                "Every car that disappears puts money in your pocket.",
                "Supercharges Car Theft income. Your network of choppers works overtime."
            ),
            "inside_man" => (
                "It's not breaking in if someone leaves the door open.",
                "Massively increases Burglary profits. Your inside contacts tip you off to the best scores."
            ),
            "happy_hour" => (
                "The drinks flow, the money flows, everybody's happy.",
                "Boosts Speakeasy earnings. Your establishment becomes THE place to be in New Porto."
            ),

            // OFFLINE SKILLS
            "night_owl" => (
                "Your crew doesn't sleep. Neither should your income.",
                "Increases offline earning efficiency. Get more of your potential income while away."
            ),
            "extended_shift" => (
                "Your operations run like a well-oiled machine, day and night.",
                "Extends the maximum time you can earn offline. Stay away longer without losing out."
            ),
            "passive_income" => (
                "Some money just shows up. Don't ask questions.",
                "Provides a flat income bonus regardless of your operations. Guaranteed earnings, no matter what."
            ),
            "godfathers_cut" => (
                "The boss always gets his share. This time, YOU'RE the boss.",
                "Multiplies all offline earnings when you return. Big bonus for patient mobsters."
            ),

            // PRESTIGE SKILLS
            "old_connections" => (
                "Even after starting over, some friends remember what you did for them.",
                "Start with bonus cash after prestige. Get a head start on your next empire."
            ),
            "reputation" => (
                "Your name carries weight. Use it.",
                "Increases the prestige bonus you earn. Each reset becomes more powerful."
            ),
            "fast_learner" => (
                "You've done this before. You know the shortcuts.",
                "Reduces all upgrade costs. Level up your operations for less."
            ),
            "early_bird" => (
                "First to the opportunity, first to the profit.",
                "Reduces unlock costs for new operations. Expand your empire faster."
            ),

            _ => (skill.Description, "Improves your operations.")
        };
    }

    private static View CreateLevelTable(SkillDefinition skill)
    {
        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(60)), // Level label
                new ColumnDefinition(GridLength.Star)     // Value
            },
            RowSpacing = 6
        };

        // Header
        grid.Add(new Label
        {
            Text = "LEVEL",
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetColor("TextSecondary")
        }, 0, 0);

        grid.Add(new Label
        {
            Text = "BONUS",
            FontSize = 11,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetColor("TextSecondary")
        }, 1, 0);

        // Level rows
        for (int level = 1; level <= 5; level++)
        {
            var levelLabel = new Label
            {
                Text = $"★ Lvl {level}",
                FontSize = 13,
                TextColor = level == 5 ? GetColor("Gold") : GetColor("TextPrimary")
            };

            var valueLabel = new Label
            {
                Text = GetLevelValue(skill, level),
                FontSize = 13,
                FontAttributes = level == 5 ? FontAttributes.Bold : FontAttributes.None,
                TextColor = level == 5 ? GetColor("Gold") : GetColor("TextPrimary")
            };

            grid.Add(levelLabel, 0, level);
            grid.Add(valueLabel, 1, level);
        }

        return grid;
    }

    private static string GetLevelValue(SkillDefinition skill, int level)
    {
        var total = skill.EffectPerLevel * level;

        return skill.EffectType switch
        {
            SkillEffectType.Multiplier => skill.Id switch
            {
                "cash_flow" => $"+{total}% to ALL income",
                "street_smarts" => $"+{total}% to Tier 1 ops (Pickpocket, Car Theft, Burglary)",
                "business_acumen" => $"+{total}% to Tier 2 ops (Speakeasy, Casino)",
                "quick_hands" => $"+{total}% Pickpocket income",
                "chop_shop" => $"+{total}% Car Theft income",
                "inside_man" => $"+{total}% Burglary income",
                "happy_hour" => $"+{total}% Speakeasy income",
                "night_owl" => $"+{total}% offline efficiency",
                "godfathers_cut" => $"+{total}% offline earnings bonus",
                "reputation" => $"+{total}% prestige bonus",
                "compound_interest" => $"+{total}% per 5 min (max +{total * 6}% at 30 min)",
                _ => $"+{total}%"
            },
            SkillEffectType.Reduction => skill.Id switch
            {
                "fast_learner" => $"-{total}% upgrade costs",
                "early_bird" => $"-{total}% unlock costs",
                _ => $"-{total}%"
            },
            SkillEffectType.Chance => $"{total}% chance for 2x income",
            SkillEffectType.Duration => $"+{total} hours max offline time",
            SkillEffectType.FlatBonus => skill.Id switch
            {
                "passive_income" => $"+${total}/sec baseline income",
                "old_connections" => $"+${total} starting cash after prestige",
                _ => $"+${total}"
            },
            _ => ""
        };
    }

    private static View CreateBuildArchetypes()
    {
        var builds = new[]
        {
            ("🏃 SPEEDRUNNER", "Fast Learner, Early Bird, Cash Flow", "Rush to prestige and stack those bonuses!"),
            ("😴 AFK KING", "Night Owl, Extended Shift, Godfather's Cut", "Maximum gains while you sleep."),
            ("🔪 STREET HUSTLER", "Street Smarts, Quick Hands, Chop Shop", "Dominate the early game streets."),
            ("🎰 HIGH ROLLER", "Business Acumen, Happy Hour, Lucky Break", "Late-game power with a gambler's edge.")
        };

        var content = new VerticalStackLayout { Spacing = 12 };

        content.Add(new Label
        {
            Text = "🎯 BUILD ARCHETYPES",
            FontFamily = "BebasNeue",
            FontSize = 22,
            TextColor = GetColor("Gold"),
            Margin = new Thickness(0, 16, 0, 4)
        });

        content.Add(new Label
        {
            Text = "Combine skills for powerful strategies:",
            FontSize = 13,
            TextColor = GetColor("TextSecondary"),
            Margin = new Thickness(0, 0, 0, 8)
        });

        foreach (var (name, skills, desc) in builds)
        {
            content.Add(new Frame
            {
                BackgroundColor = GetColor("Surface"),
                CornerRadius = 8,
                Padding = new Thickness(12),
                BorderColor = Colors.Transparent,
                Content = new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        new Label
                        {
                            Text = name,
                            FontSize = 15,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = GetColor("TextPrimary")
                        },
                        new Label
                        {
                            Text = skills,
                            FontSize = 12,
                            TextColor = GetColor("Gold")
                        },
                        new Label
                        {
                            Text = desc,
                            FontSize = 12,
                            TextColor = GetColor("TextSecondary")
                        }
                    }
                }
            });
        }

        return content;
    }

    private static Color GetColor(string key)
    {
        if (Application.Current?.Resources.TryGetValue(key, out var value) == true && value is Color color)
            return color;
        return Colors.Gray;
    }

    private async void OnBackClicked(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("..");
    }
}
