using MadeMan.IdleEmpire.Models;

namespace MadeMan.IdleEmpire.Views;

public partial class HelpPage : ContentPage
{
    public HelpPage()
    {
        InitializeComponent();
        BuildContent();
    }

    private void BuildContent()
    {
        // Welcome section
        ContentList.Children.Add(CreateWelcomeSection());

        // The Basics
        ContentList.Children.Add(CreateBasicsSection());

        // Operations Guide
        ContentList.Children.Add(CreateSectionHeader("OPERATIONS", "Your criminal enterprises"));
        foreach (var op in GameConfig.Operations)
        {
            ContentList.Children.Add(CreateOperationCard(op));
        }

        // Prestige Guide
        ContentList.Children.Add(CreatePrestigeSection());

        // Titles Guide
        ContentList.Children.Add(CreateTitlesSection());

        // Skills Reference
        ContentList.Children.Add(CreateSkillsReferenceSection());

        // Tips Section
        ContentList.Children.Add(CreateTipsSection());
    }

    private static View CreateWelcomeSection()
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
                        Text = "WELCOME TO NEW PORTO",
                        FontFamily = "BebasNeue",
                        FontSize = 18,
                        TextColor = GetColor("Gold"),
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = "The year is 1932. Prohibition is in full swing, and opportunity " +
                               "awaits those bold enough to seize it. Start from nothing and build " +
                               "your criminal empire, one operation at a time.",
                        FontSize = 13,
                        TextColor = GetColor("TextSecondary"),
                        HorizontalTextAlignment = TextAlignment.Center
                    }
                }
            }
        };
    }

    private static View CreateBasicsSection()
    {
        return new Frame
        {
            BackgroundColor = GetColor("Surface"),
            CornerRadius = 12,
            Padding = new Thickness(16),
            BorderColor = Colors.Transparent,
            Margin = new Thickness(0, 8),
            Content = new VerticalStackLayout
            {
                Spacing = 12,
                Children =
                {
                    new Label
                    {
                        Text = "THE BASICS",
                        FontFamily = "BebasNeue",
                        FontSize = 20,
                        TextColor = GetColor("Gold")
                    },
                    CreateBulletPoint("Operations generate cash automatically over time"),
                    CreateBulletPoint("Upgrade operations to multiply their earnings"),
                    CreateBulletPoint("Unlock new operations as you earn more cash"),
                    CreateBulletPoint("Reach milestones to unlock powerful skills"),
                    CreateBulletPoint("Prestige to reset with permanent bonuses")
                }
            }
        };
    }

    private static View CreateBulletPoint(string text)
    {
        return new HorizontalStackLayout
        {
            Spacing = 8,
            Children =
            {
                new Label
                {
                    Text = "•",
                    FontSize = 14,
                    TextColor = GetColor("Gold"),
                    VerticalOptions = LayoutOptions.Start
                },
                new Label
                {
                    Text = text,
                    FontSize = 14,
                    TextColor = GetColor("TextPrimary"),
                    VerticalOptions = LayoutOptions.Start
                }
            }
        };
    }

    private static View CreateSectionHeader(string title, string subtitle)
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

    private static View CreateOperationCard(Operation op)
    {
        var effectiveIncome = op.BaseYield / op.Interval;
        var unlockText = op.UnlockCost == 0 ? "FREE (Starter)" : $"${op.UnlockCost:N0}";

        return new Frame
        {
            BackgroundColor = GetColor("Surface"),
            CornerRadius = 12,
            Padding = new Thickness(16),
            BorderColor = Colors.Transparent,
            Margin = new Thickness(0, 4),
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    // Name
                    new Label
                    {
                        Text = op.Name.ToUpper(),
                        FontFamily = "BebasNeue",
                        FontSize = 18,
                        TextColor = GetColor("TextPrimary")
                    },
                    // Description
                    new Label
                    {
                        Text = $"\"{op.Description}\"",
                        FontSize = 13,
                        FontAttributes = FontAttributes.Italic,
                        TextColor = GetColor("TextSecondary")
                    },
                    // Stats grid
                    new Grid
                    {
                        ColumnDefinitions = new ColumnDefinitionCollection
                        {
                            new ColumnDefinition(GridLength.Star),
                            new ColumnDefinition(GridLength.Star)
                        },
                        RowDefinitions = new RowDefinitionCollection
                        {
                            new RowDefinition(GridLength.Auto),
                            new RowDefinition(GridLength.Auto)
                        },
                        RowSpacing = 4,
                        ColumnSpacing = 16,
                        Margin = new Thickness(0, 4, 0, 0),
                        Children =
                        {
                            CreateStatLabel("Base Income", 0, 0),
                            CreateStatValue($"${effectiveIncome:N1}/sec", 1, 0),
                            CreateStatLabel("Unlock Cost", 0, 1),
                            CreateStatValue(unlockText, 1, 1)
                        }
                    },
                    // Cycle info
                    new Label
                    {
                        Text = $"Pays ${op.BaseYield:N0} every {op.Interval:N0}s",
                        FontSize = 11,
                        TextColor = GetColor("TextSecondary"),
                        Margin = new Thickness(0, 4, 0, 0)
                    }
                }
            }
        };
    }

    private static Label CreateStatLabel(string text, int column, int row)
    {
        var label = new Label
        {
            Text = text,
            FontSize = 12,
            TextColor = GetColor("TextSecondary")
        };
        Grid.SetColumn(label, column);
        Grid.SetRow(label, row);
        return label;
    }

    private static Label CreateStatValue(string text, int column, int row)
    {
        var label = new Label
        {
            Text = text,
            FontSize = 14,
            FontAttributes = FontAttributes.Bold,
            TextColor = GetColor("Gold")
        };
        Grid.SetColumn(label, column);
        Grid.SetRow(label, row);
        return label;
    }

    private static View CreatePrestigeSection()
    {
        var threshold = GameConfig.PrestigeThreshold;
        var bonus = GameConfig.PrestigeBonusPerReset * 100;

        return new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                CreateSectionHeader("PRESTIGE", "Reset for permanent power"),
                new Frame
                {
                    BackgroundColor = GetColor("Surface"),
                    CornerRadius = 12,
                    Padding = new Thickness(16),
                    BorderColor = Colors.Transparent,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 12,
                        Children =
                        {
                            new Label
                            {
                                Text = "When you reach a certain level of wealth, you can choose to " +
                                       "\"go legit\" and start fresh with a permanent income bonus.",
                                FontSize = 14,
                                TextColor = GetColor("TextPrimary")
                            },
                            new BoxView
                            {
                                HeightRequest = 1,
                                BackgroundColor = GetColor("SurfaceLight")
                            },
                            CreatePrestigeInfoRow("Requirement", $"${threshold:N0} total earned"),
                            CreatePrestigeInfoRow("Bonus", $"+{bonus:N0}% income per prestige"),
                            CreatePrestigeInfoRow("Stacks", "Bonuses multiply together!"),
                            new BoxView
                            {
                                HeightRequest = 1,
                                BackgroundColor = GetColor("SurfaceLight")
                            },
                            new Label
                            {
                                Text = "WHAT RESETS:",
                                FontSize = 12,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = GetColor("Primary")
                            },
                            new Label
                            {
                                Text = "• Cash and operations\n• Operation levels\n• Active skills",
                                FontSize = 13,
                                TextColor = GetColor("TextSecondary")
                            },
                            new Label
                            {
                                Text = "WHAT STAYS:",
                                FontSize = 12,
                                FontAttributes = FontAttributes.Bold,
                                TextColor = GetColor("Success")
                            },
                            new Label
                            {
                                Text = "• Prestige multiplier bonus\n• Your title rank\n• Lifetime statistics",
                                FontSize = 13,
                                TextColor = GetColor("TextSecondary")
                            }
                        }
                    }
                }
            }
        };
    }

    private static View CreatePrestigeInfoRow(string label, string value)
    {
        return new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(100)),
                new ColumnDefinition(GridLength.Star)
            },
            Children =
            {
                new Label
                {
                    Text = label,
                    FontSize = 13,
                    TextColor = GetColor("TextSecondary")
                },
                new Label
                {
                    Text = value,
                    FontSize = 13,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = GetColor("Gold")
                }.Apply(l => Grid.SetColumn(l, 1))
            }
        };
    }

    private static View CreateTitlesSection()
    {
        var content = new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                CreateSectionHeader("TITLES", "Rise through the ranks"),
                new Frame
                {
                    BackgroundColor = GetColor("Surface"),
                    CornerRadius = 12,
                    Padding = new Thickness(16),
                    BorderColor = Colors.Transparent,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 8,
                        Children =
                        {
                            new Label
                            {
                                Text = "Each prestige increases your rank. Earn titles to show your power.",
                                FontSize = 14,
                                TextColor = GetColor("TextPrimary"),
                                Margin = new Thickness(0, 0, 0, 8)
                            }
                        }
                    }
                }
            }
        };

        // Get the frame's content stack
        var frame = (Frame)content.Children[1];
        var stack = (VerticalStackLayout)frame.Content;

        // Add each title
        foreach (var title in TitleConfig.Titles)
        {
            stack.Children.Add(CreateTitleRow(title));
        }

        return content;
    }

    private static View CreateTitleRow(TitleLevel title)
    {
        var prestigeText = title.RequiredPrestige == 0
            ? "Start"
            : $"{title.RequiredPrestige}x Prestige";

        return new Grid
        {
            ColumnDefinitions = new ColumnDefinitionCollection
            {
                new ColumnDefinition(new GridLength(90)),
                new ColumnDefinition(GridLength.Star)
            },
            Padding = new Thickness(0, 4),
            Children =
            {
                new Label
                {
                    Text = prestigeText,
                    FontSize = 12,
                    TextColor = GetColor("TextSecondary"),
                    VerticalOptions = LayoutOptions.Center
                },
                new VerticalStackLayout
                {
                    Children =
                    {
                        new Label
                        {
                            Text = title.Name,
                            FontSize = 14,
                            FontAttributes = FontAttributes.Bold,
                            TextColor = title.RequiredPrestige >= 20 ? GetColor("Gold") : GetColor("TextPrimary")
                        },
                        new Label
                        {
                            Text = title.Description,
                            FontSize = 11,
                            TextColor = GetColor("TextSecondary")
                        }
                    }
                }.Apply(v => Grid.SetColumn(v, 1))
            }
        };
    }

    private static View CreateSkillsReferenceSection()
    {
        return new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                CreateSectionHeader("SKILLS", "Choose your path"),
                new Frame
                {
                    BackgroundColor = GetColor("Surface"),
                    CornerRadius = 12,
                    Padding = new Thickness(16),
                    BorderColor = Colors.Transparent,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 12,
                        Children =
                        {
                            new Label
                            {
                                Text = "As you earn money, you'll hit milestones that unlock skill choices. " +
                                       "Pick from 3 random skills to customize your playstyle.",
                                FontSize = 14,
                                TextColor = GetColor("TextPrimary")
                            },
                            new BoxView
                            {
                                HeightRequest = 1,
                                BackgroundColor = GetColor("SurfaceLight")
                            },
                            CreateBulletPoint("Max 5 skills at once"),
                            CreateBulletPoint("Each skill upgrades to level 5"),
                            CreateBulletPoint("Skills reset on prestige"),
                            CreateBulletPoint("4 categories: Income, Operations, Offline, Prestige"),
                            new BoxView
                            {
                                HeightRequest = 1,
                                BackgroundColor = GetColor("SurfaceLight")
                            },
                            new Label
                            {
                                Text = "For detailed skill information, check the Skills Guide in the Info menu.",
                                FontSize = 12,
                                FontAttributes = FontAttributes.Italic,
                                TextColor = GetColor("Gold"),
                                HorizontalTextAlignment = TextAlignment.Center
                            }
                        }
                    }
                }
            }
        };
    }

    private static View CreateTipsSection()
    {
        return new VerticalStackLayout
        {
            Spacing = 8,
            Margin = new Thickness(0, 0, 0, 32),
            Children =
            {
                CreateSectionHeader("PRO TIPS", "Advice from the streets"),
                new Frame
                {
                    BackgroundColor = GetColor("SurfaceLight"),
                    CornerRadius = 12,
                    Padding = new Thickness(16),
                    BorderColor = Colors.Transparent,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 12,
                        Children =
                        {
                            CreateTip("Early Game", "Focus on upgrading Pickpocketing first. Small gains add up fast."),
                            CreateTip("Mid Game", "Unlock operations as soon as you can afford them - they pay off quickly."),
                            CreateTip("Late Game", "The Casino pays big but takes time. Balance upgrades across operations."),
                            CreateTip("Prestige", "Don't wait too long to prestige. The bonus compounds over time!"),
                            CreateTip("Skills", "Offline skills are great if you play in short sessions."),
                            CreateTip("Offline", $"You earn {GameConfig.OfflineEfficiency * 100:N0}% of your income while away (max {GameConfig.MaxOfflineHours:N0} hours).")
                        }
                    }
                }
            }
        };
    }

    private static View CreateTip(string title, string text)
    {
        return new VerticalStackLayout
        {
            Spacing = 2,
            Children =
            {
                new Label
                {
                    Text = title.ToUpper(),
                    FontSize = 11,
                    FontAttributes = FontAttributes.Bold,
                    TextColor = GetColor("Gold")
                },
                new Label
                {
                    Text = text,
                    FontSize = 13,
                    TextColor = GetColor("TextPrimary")
                }
            }
        };
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

// Extension for cleaner Grid.SetColumn syntax
internal static class ViewExtensions
{
    public static T Apply<T>(this T view, Action<T> action) where T : View
    {
        action(view);
        return view;
    }
}
