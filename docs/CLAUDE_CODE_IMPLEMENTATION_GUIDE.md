# 🎮 MADE MAN: IDLE EMPIRE
## Claude Code Implementation Guide v2.0

---

# DOKUMENT FORMÅL

Dette dokument er designet til at blive brugt direkte med **Claude Code** til fuld implementering af spillet. Alt er specificeret så Claude Code kan:

1. Oprette projekt fra scratch
2. Implementere al kode
3. Generere grafik assets
4. Bygge og teste appen

**Workflow:**
- **Claude.ai** → Brainstorming, design, grafik, UX beslutninger
- **Claude Code** → Al kode, implementering, build, test

---

# KRITISK MVP RE-EVALUERING

## ❌ Problemer med Original Spec

| Problem | Konsekvens |
|---------|------------|
| 3 separate screens | For fragmenteret UX, unødvendig navigation |
| Prestige ved $100,000 | Alt for langt væk for nye spillere (30+ min) |
| Ingen "hook" i første 60 sek | Spillere forlader før de forstår spillet |
| Ingen visuel feedback | "Numbers go up" uden satisfying visuals |
| For mange unlock costs | Forvirrende progression |

## ✅ Ny Fokuseret MVP

### Princip: "One Screen Wonder"
Alt gameplay på ÉN skærm. Ingen navigation. Instant forståelse.

### De Første 60 Sekunder (KRITISK)
```
0-5 sek:   Se cash ticker starte fra $0
5-15 sek:  Første $10 tjent → Celebration animation
15-30 sek: Kan købe første upgrade ($10) → Instant gratification
30-60 sek: Income doubled → "Aha!" moment
```

### Ny Progression Curve
| Milestone | Tid | Handling |
|-----------|-----|----------|
| Første upgrade | 15 sek | $10 - køb Pickpocket Lvl 2 |
| Unlock Car Theft | 2 min | $50 |
| Unlock Burglary | 5 min | $250 |
| Unlock Speakeasy | 10 min | $1,000 |
| Første Prestige MULIGHED | 15 min | $10,000 |

**Kritisk ændring:** Alle costs reduceret 10x for hurtigere dopamin-hits.

---

# TECH STACK (Uændret)

```yaml
Framework: .NET MAUI (.NET 8)
Language: C#
UI: XAML + MVVM
State: CommunityToolkit.Mvvm
Storage: Preferences API (JSON)
Target: Android 8.0+ (API 26)
```

### NuGet Packages
```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
```

---

# PROJEKT STRUKTUR

```
MadeMan.IdleEmpire/
├── MadeMan.IdleEmpire.sln
└── MadeMan.IdleEmpire/
    ├── App.xaml                    # App resources
    ├── App.xaml.cs                 # Lifecycle (save/load)
    ├── MauiProgram.cs              # DI setup
    │
    ├── Models/
    │   ├── GameState.cs            # Complete game state
    │   ├── Operation.cs            # Crime/Business unified model
    │   └── GameConfig.cs           # All balance constants
    │
    ├── ViewModels/
    │   ├── MainViewModel.cs        # Single ViewModel for entire game
    │   └── OperationViewModel.cs   # Individual operation display
    │
    ├── Views/
    │   └── MainPage.xaml           # SINGLE PAGE - alt gameplay
    │
    ├── Services/
    │   ├── IGameEngine.cs          # Game loop interface
    │   ├── GameEngine.cs           # Core tick logic
    │   └── SaveManager.cs          # Persistence
    │
    └── Resources/
        ├── Styles/
        │   └── Theme.xaml          # Colors + styles
        └── Images/
            └── *.svg               # Simple icons
```

**Bemærk:** Kun 1 Page, 1 ViewModel for gameplay. Maksimal simplicitet.

---

# DATA MODELLER

## GameState.cs
```csharp
public class GameState
{
    // Core resources
    public double Cash { get; set; } = 0;
    public double TotalEarned { get; set; } = 0;
    
    // Prestige
    public int PrestigeCount { get; set; } = 0;
    public double PrestigeBonus { get; set; } = 1.0; // Multiplier
    
    // Operations (crimes + businesses combined)
    public List<OperationState> Operations { get; set; } = new();
    
    // Meta
    public DateTime LastPlayedUtc { get; set; } = DateTime.UtcNow;
}

public class OperationState
{
    public string Id { get; set; }
    public int Level { get; set; } = 0; // 0 = locked
}
```

## Operation.cs (Unified Crime/Business)
```csharp
public class Operation
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Icon { get; set; }
    public string Description { get; set; }
    
    // Economics
    public double BaseIncome { get; set; }      // $/sec at level 1
    public double UnlockCost { get; set; }      // Cost to unlock (level 0→1)
    public double UpgradeMultiplier { get; set; } = 1.5; // Cost increase per level
    
    // Calculated
    public double GetUpgradeCost(int currentLevel) 
        => UnlockCost * Math.Pow(UpgradeMultiplier, currentLevel);
    
    public double GetIncome(int level, double prestigeBonus)
        => level > 0 ? BaseIncome * level * prestigeBonus : 0;
}
```

## GameConfig.cs (ALLE Balance Konstanter)
```csharp
public static class GameConfig
{
    // === OPERATIONS ===
    public static readonly Operation[] Operations = new[]
    {
        // TIER 1: Street Crimes
        new Operation
        {
            Id = "pickpocket",
            Name = "Pickpocketing",
            Icon = "icon_pickpocket.png",
            Description = "Snup lommerne på turisterne",
            BaseIncome = 1.0,
            UnlockCost = 0, // FREE STARTER
            UpgradeMultiplier = 1.4
        },
        new Operation
        {
            Id = "cartheft",
            Name = "Car Theft",
            Icon = "icon_car.png",
            Description = "Stjæl biler, sælg dele",
            BaseIncome = 4.0,
            UnlockCost = 50,
            UpgradeMultiplier = 1.5
        },
        new Operation
        {
            Id = "burglary",
            Name = "Burglary",
            Icon = "icon_burglary.png",
            Description = "Bryd ind i de riges hjem",
            BaseIncome = 15.0,
            UnlockCost = 250,
            UpgradeMultiplier = 1.6
        },
        
        // TIER 2: Business
        new Operation
        {
            Id = "speakeasy",
            Name = "Speakeasy",
            Icon = "icon_speakeasy.png",
            Description = "Din første illegale bar",
            BaseIncome = 50.0,
            UnlockCost = 1000,
            UpgradeMultiplier = 1.8
        },
        new Operation
        {
            Id = "casino",
            Name = "Underground Casino",
            Icon = "icon_casino.png",
            Description = "Huset vinder altid",
            BaseIncome = 200.0,
            UnlockCost = 5000,
            UpgradeMultiplier = 2.0
        }
    };
    
    // === PRESTIGE ===
    public const double PrestigeThreshold = 10_000;     // Minimum to prestige
    public const double PrestigeBonusPerReset = 0.25;   // +25% per prestige
    
    // === OFFLINE ===
    public const double MaxOfflineHours = 4;
    public const double OfflineEfficiency = 0.5;        // 50% of normal income
    
    // === TIMING ===
    public const int TicksPerSecond = 10;
    public const int AutoSaveIntervalSeconds = 30;
}
```

---

# GAME ENGINE

## IGameEngine.cs
```csharp
public interface IGameEngine
{
    GameState State { get; }
    double IncomePerSecond { get; }
    
    void Initialize();
    void Tick(double deltaSeconds);
    bool CanUnlock(string operationId);
    bool CanUpgrade(string operationId);
    void UnlockOrUpgrade(string operationId);
    bool CanPrestige();
    void DoPrestige();
}
```

## GameEngine.cs (Core Implementation)
```csharp
public class GameEngine : IGameEngine
{
    private readonly SaveManager _saveManager;
    
    public GameState State { get; private set; }
    
    public double IncomePerSecond
    {
        get
        {
            double total = 0;
            foreach (var op in GameConfig.Operations)
            {
                var state = GetOperationState(op.Id);
                total += op.GetIncome(state.Level, State.PrestigeBonus);
            }
            return total;
        }
    }
    
    public void Initialize()
    {
        State = _saveManager.Load() ?? CreateNewGame();
        CalculateOfflineEarnings();
    }
    
    private GameState CreateNewGame()
    {
        var state = new GameState();
        
        // Initialize all operations as locked
        foreach (var op in GameConfig.Operations)
        {
            state.Operations.Add(new OperationState 
            { 
                Id = op.Id, 
                Level = op.UnlockCost == 0 ? 1 : 0 // Auto-unlock free operations
            });
        }
        
        return state;
    }
    
    public void Tick(double deltaSeconds)
    {
        double earned = IncomePerSecond * deltaSeconds;
        State.Cash += earned;
        State.TotalEarned += earned;
    }
    
    public bool CanUnlock(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        return state.Level == 0 && State.Cash >= op.UnlockCost;
    }
    
    public bool CanUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        if (state.Level == 0) return false;
        return State.Cash >= op.GetUpgradeCost(state.Level);
    }
    
    public void UnlockOrUpgrade(string operationId)
    {
        var op = GetOperation(operationId);
        var state = GetOperationState(operationId);
        
        double cost = state.Level == 0 
            ? op.UnlockCost 
            : op.GetUpgradeCost(state.Level);
        
        if (State.Cash >= cost)
        {
            State.Cash -= cost;
            state.Level++;
        }
    }
    
    public bool CanPrestige() 
        => State.TotalEarned >= GameConfig.PrestigeThreshold;
    
    public void DoPrestige()
    {
        if (!CanPrestige()) return;
        
        // Calculate bonus
        State.PrestigeCount++;
        State.PrestigeBonus += GameConfig.PrestigeBonusPerReset;
        
        // Reset progress
        State.Cash = 0;
        State.TotalEarned = 0;
        
        // Reset operations
        foreach (var opState in State.Operations)
        {
            var op = GetOperation(opState.Id);
            opState.Level = op.UnlockCost == 0 ? 1 : 0;
        }
    }
    
    private void CalculateOfflineEarnings()
    {
        var offlineTime = DateTime.UtcNow - State.LastPlayedUtc;
        var hours = Math.Min(offlineTime.TotalHours, GameConfig.MaxOfflineHours);
        
        if (hours > 0.01) // More than 36 seconds
        {
            double earnings = IncomePerSecond * hours * 3600 * GameConfig.OfflineEfficiency;
            State.Cash += earnings;
            State.TotalEarned += earnings;
            // TODO: Show offline earnings popup
        }
        
        State.LastPlayedUtc = DateTime.UtcNow;
    }
    
    // Helpers
    private Operation GetOperation(string id) 
        => GameConfig.Operations.First(o => o.Id == id);
    
    private OperationState GetOperationState(string id)
        => State.Operations.First(o => o.Id == id);
}
```

---

# UI DESIGN

## Design Principper
1. **Én skærm** - Alt synligt uden scroll hvis muligt
2. **Thumb-zone** - Vigtige knapper i bunden (nemt at nå)
3. **Konstant feedback** - Cash ticker altid synlig og animeret
4. **Progression clarity** - Næste unlock altid synlig

## Color Palette
```xaml
<!-- Theme.xaml -->
<Color x:Key="Background">#1A1A2E</Color>
<Color x:Key="Surface">#16213E</Color>
<Color x:Key="SurfaceLight">#1F2B47</Color>
<Color x:Key="Primary">#E94560</Color>
<Color x:Key="PrimaryDark">#C73E54</Color>
<Color x:Key="Gold">#FFD700</Color>
<Color x:Key="GoldDark">#B8960B</Color>
<Color x:Key="TextPrimary">#FFFFFF</Color>
<Color x:Key="TextSecondary">#8892A0</Color>
<Color x:Key="Success">#4ADE80</Color>
<Color x:Key="Locked">#4A5568</Color>
```

## Screen Layout (ASCII Mockup)
```
┌─────────────────────────────────────┐
│  💰 $1,234.56          +$45.20/s   │ ← Cash header (always visible)
├─────────────────────────────────────┤
│                                     │
│  ┌─────────────────────────────┐   │
│  │ 🤚 Pickpocketing    Lvl 5   │   │ ← Operation card
│  │     $5.00/s                 │   │
│  │     [████████░░] $45 →Lvl 6 │   │ ← Progress + upgrade button
│  └─────────────────────────────┘   │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ 🚗 Car Theft        Lvl 2   │   │
│  │     $8.00/s                 │   │
│  │     [██████░░░░] $75 →Lvl 3 │   │
│  └─────────────────────────────┘   │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ 🔒 Burglary         LOCKED  │   │ ← Locked state
│  │     Unlock: $250            │   │
│  │     [░░░░░░░░░░] TAP TO BUY │   │
│  └─────────────────────────────┘   │
│                                     │
├─────────────────────────────────────┤
│  ⭐ PRESTIGE AVAILABLE              │ ← Shows when available
│  Reset for +25% permanent bonus     │
│  [        BECOME A MADE MAN        ]│
└─────────────────────────────────────┘
```

---

# XAML IMPLEMENTATION

## MainPage.xaml
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MadeMan.IdleEmpire.ViewModels"
             x:Class="MadeMan.IdleEmpire.Views.MainPage"
             x:DataType="vm:MainViewModel"
             BackgroundColor="{StaticResource Background}"
             Shell.NavBarIsVisible="False">
    
    <Grid RowDefinitions="Auto,*,Auto" Padding="16">
        
        <!-- === CASH HEADER === -->
        <Frame Grid.Row="0" 
               BackgroundColor="{StaticResource Surface}"
               CornerRadius="16"
               Padding="20"
               Margin="0,0,0,16">
            <Grid ColumnDefinitions="*,Auto">
                <VerticalStackLayout>
                    <Label Text="CASH"
                           FontSize="12"
                           TextColor="{StaticResource TextSecondary}"/>
                    <Label Text="{Binding CashDisplay}"
                           FontSize="36"
                           FontAttributes="Bold"
                           TextColor="{StaticResource Gold}"/>
                </VerticalStackLayout>
                
                <Frame Grid.Column="1"
                       BackgroundColor="{StaticResource SurfaceLight}"
                       CornerRadius="8"
                       Padding="12,8"
                       VerticalOptions="Center">
                    <Label Text="{Binding IncomeDisplay}"
                           FontSize="16"
                           TextColor="{StaticResource Success}"/>
                </Frame>
            </Grid>
        </Frame>
        
        <!-- === OPERATIONS LIST === -->
        <ScrollView Grid.Row="1">
            <VerticalStackLayout Spacing="12"
                                 BindableLayout.ItemsSource="{Binding Operations}">
                <BindableLayout.ItemTemplate>
                    <DataTemplate x:DataType="vm:OperationViewModel">
                        <Frame BackgroundColor="{StaticResource Surface}"
                               CornerRadius="12"
                               Padding="16">
                            <Frame.GestureRecognizers>
                                <TapGestureRecognizer Command="{Binding TapCommand}"/>
                            </Frame.GestureRecognizers>
                            
                            <Grid ColumnDefinitions="50,*,Auto" RowDefinitions="Auto,Auto">
                                
                                <!-- Icon -->
                                <Frame Grid.RowSpan="2"
                                       BackgroundColor="{StaticResource SurfaceLight}"
                                       CornerRadius="8"
                                       Padding="8"
                                       HeightRequest="50"
                                       WidthRequest="50">
                                    <Image Source="{Binding Icon}"
                                           Aspect="AspectFit"/>
                                </Frame>
                                
                                <!-- Name + Level -->
                                <HorizontalStackLayout Grid.Column="1" Spacing="8">
                                    <Label Text="{Binding Name}"
                                           FontSize="16"
                                           FontAttributes="Bold"
                                           TextColor="{StaticResource TextPrimary}"/>
                                    <Label Text="{Binding LevelDisplay}"
                                           FontSize="14"
                                           TextColor="{StaticResource TextSecondary}"/>
                                </HorizontalStackLayout>
                                
                                <!-- Income -->
                                <Label Grid.Column="1" Grid.Row="1"
                                       Text="{Binding IncomeDisplay}"
                                       FontSize="14"
                                       TextColor="{StaticResource Gold}"
                                       IsVisible="{Binding IsUnlocked}"/>
                                
                                <!-- Action Button -->
                                <Button Grid.Column="2" Grid.RowSpan="2"
                                        Text="{Binding ButtonText}"
                                        Command="{Binding TapCommand}"
                                        BackgroundColor="{Binding ButtonColor}"
                                        TextColor="White"
                                        CornerRadius="8"
                                        Padding="16,8"
                                        FontSize="14"
                                        FontAttributes="Bold"/>
                            </Grid>
                        </Frame>
                    </DataTemplate>
                </BindableLayout.ItemTemplate>
            </VerticalStackLayout>
        </ScrollView>
        
        <!-- === PRESTIGE PANEL === -->
        <Frame Grid.Row="2"
               BackgroundColor="{StaticResource Primary}"
               CornerRadius="16"
               Padding="20"
               Margin="0,16,0,0"
               IsVisible="{Binding CanPrestige}">
            <Grid RowDefinitions="Auto,Auto">
                <Label Text="⭐ PRESTIGE AVAILABLE"
                       FontSize="16"
                       FontAttributes="Bold"
                       TextColor="White"
                       HorizontalOptions="Center"/>
                <Button Grid.Row="1"
                        Text="{Binding PrestigeButtonText}"
                        Command="{Binding PrestigeCommand}"
                        BackgroundColor="White"
                        TextColor="{StaticResource Primary}"
                        CornerRadius="8"
                        Margin="0,12,0,0"
                        FontAttributes="Bold"/>
            </Grid>
        </Frame>
        
    </Grid>
</ContentPage>
```

---

# VIEWMODEL

## MainViewModel.cs
```csharp
public partial class MainViewModel : ObservableObject
{
    private readonly IGameEngine _engine;
    private readonly SaveManager _saveManager;
    private IDispatcherTimer _gameTimer;
    private IDispatcherTimer _saveTimer;
    private DateTime _lastTick;
    
    [ObservableProperty]
    private string cashDisplay = "$0";
    
    [ObservableProperty]
    private string incomeDisplay = "+$0/s";
    
    [ObservableProperty]
    private bool canPrestige;
    
    [ObservableProperty]
    private string prestigeButtonText = "BECOME A MADE MAN";
    
    public ObservableCollection<OperationViewModel> Operations { get; } = new();
    
    public MainViewModel(IGameEngine engine, SaveManager saveManager)
    {
        _engine = engine;
        _saveManager = saveManager;
    }
    
    public void OnAppearing()
    {
        _engine.Initialize();
        BuildOperationViewModels();
        StartGameLoop();
        StartAutoSave();
    }
    
    public void OnDisappearing()
    {
        StopTimers();
        _saveManager.Save(_engine.State);
    }
    
    private void BuildOperationViewModels()
    {
        Operations.Clear();
        foreach (var op in GameConfig.Operations)
        {
            Operations.Add(new OperationViewModel(op, _engine));
        }
    }
    
    private void StartGameLoop()
    {
        _lastTick = DateTime.UtcNow;
        _gameTimer = Application.Current.Dispatcher.CreateTimer();
        _gameTimer.Interval = TimeSpan.FromMilliseconds(1000 / GameConfig.TicksPerSecond);
        _gameTimer.Tick += OnGameTick;
        _gameTimer.Start();
    }
    
    private void OnGameTick(object sender, EventArgs e)
    {
        var now = DateTime.UtcNow;
        var delta = (now - _lastTick).TotalSeconds;
        _lastTick = now;
        
        _engine.Tick(delta);
        UpdateDisplay();
    }
    
    private void UpdateDisplay()
    {
        CashDisplay = FormatCash(_engine.State.Cash);
        IncomeDisplay = $"+{FormatCash(_engine.IncomePerSecond)}/s";
        CanPrestige = _engine.CanPrestige();
        
        if (CanPrestige)
        {
            var bonus = GameConfig.PrestigeBonusPerReset * 100;
            PrestigeButtonText = $"RESET FOR +{bonus:0}% BONUS";
        }
        
        foreach (var op in Operations)
        {
            op.Refresh();
        }
    }
    
    [RelayCommand]
    private void Prestige()
    {
        if (!_engine.CanPrestige()) return;
        
        _engine.DoPrestige();
        BuildOperationViewModels(); // Rebuild UI
        UpdateDisplay();
    }
    
    private string FormatCash(double value)
    {
        if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
        if (value >= 1_000) return $"${value / 1_000:F2}K";
        return $"${value:F2}";
    }
    
    private void StartAutoSave()
    {
        _saveTimer = Application.Current.Dispatcher.CreateTimer();
        _saveTimer.Interval = TimeSpan.FromSeconds(GameConfig.AutoSaveIntervalSeconds);
        _saveTimer.Tick += (s, e) => _saveManager.Save(_engine.State);
        _saveTimer.Start();
    }
    
    private void StopTimers()
    {
        _gameTimer?.Stop();
        _saveTimer?.Stop();
    }
}
```

## OperationViewModel.cs
```csharp
public partial class OperationViewModel : ObservableObject
{
    private readonly Operation _operation;
    private readonly IGameEngine _engine;
    
    public string Name => _operation.Name;
    public string Icon => _operation.Icon;
    
    [ObservableProperty]
    private string levelDisplay;
    
    [ObservableProperty]
    private string incomeDisplay;
    
    [ObservableProperty]
    private string buttonText;
    
    [ObservableProperty]
    private Color buttonColor;
    
    [ObservableProperty]
    private bool isUnlocked;
    
    public OperationViewModel(Operation operation, IGameEngine engine)
    {
        _operation = operation;
        _engine = engine;
        Refresh();
    }
    
    public void Refresh()
    {
        var state = _engine.State.Operations.First(o => o.Id == _operation.Id);
        IsUnlocked = state.Level > 0;
        
        if (!IsUnlocked)
        {
            // Locked state
            LevelDisplay = "LOCKED";
            IncomeDisplay = "";
            ButtonText = $"${_operation.UnlockCost:F0}";
            ButtonColor = _engine.CanUnlock(_operation.Id) 
                ? Color.FromArgb("#4ADE80")  // Green - can afford
                : Color.FromArgb("#4A5568"); // Gray - can't afford
        }
        else
        {
            // Unlocked state
            LevelDisplay = $"Lvl {state.Level}";
            var income = _operation.GetIncome(state.Level, _engine.State.PrestigeBonus);
            IncomeDisplay = $"${income:F2}/s";
            
            var upgradeCost = _operation.GetUpgradeCost(state.Level);
            ButtonText = $"${upgradeCost:F0}";
            ButtonColor = _engine.CanUpgrade(_operation.Id)
                ? Color.FromArgb("#E94560")  // Red/Primary - can afford
                : Color.FromArgb("#4A5568"); // Gray - can't afford
        }
    }
    
    [RelayCommand]
    private void Tap()
    {
        _engine.UnlockOrUpgrade(_operation.Id);
        Refresh();
    }
}
```

---

# USER FLOWS

## Flow 1: Første Gang Spiller (0-60 sek)
```
┌─────────────────────────────────────────────────────────┐
│ STEP 1: App åbner                                       │
│ → Cash: $0, Income: $1/s (Pickpocket auto-unlocked)    │
│ → Spilleren ser tallene stige                           │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ STEP 2: Efter ~10 sek                                   │
│ → Cash: $10, "Upgrade" knap lyser grøn                 │
│ → Spilleren tapper → Pickpocket Lvl 2                  │
│ → Income: $2/s (DOUBLED!)                              │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ STEP 3: Efter ~45 sek                                   │
│ → Cash: $50, "Car Theft" unlock knap lyser grøn        │
│ → Spilleren tapper → Ny indtægtskilde!                 │
│ → Income: $6/s (Car Theft + Pickpocket)                │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ HOOK COMPLETE                                           │
│ Spilleren forstår: Tjene → Upgrade → Tjene mere        │
└─────────────────────────────────────────────────────────┘
```

## Flow 2: Returnerende Spiller
```
┌─────────────────────────────────────────────────────────┐
│ STEP 1: App åbner efter 4 timer væk                    │
│ → Offline earnings beregnes (4h × income × 50%)        │
│ → POPUP: "Du tjente $12,345 mens du var væk!"          │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ STEP 2: Spilleren har nu råd til upgrades              │
│ → Tapper på flere upgrades                             │
│ → Income stiger markant                                │
└─────────────────────────────────────────────────────────┘
```

## Flow 3: Prestige
```
┌─────────────────────────────────────────────────────────┐
│ TRIGGER: TotalEarned >= $10,000                        │
│ → Rød "PRESTIGE" panel vises i bunden                  │
│ → Tekst: "Reset for +25% permanent bonus"              │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ SPILLERENS VALG:                                        │
│ A) Fortsæt og spar mere → Større fremtidig indkomst   │
│ B) Prestige nu → Start forfra med bonus                │
└─────────────────────────────────────────────────────────┘
                          ↓
┌─────────────────────────────────────────────────────────┐
│ EFTER PRESTIGE:                                         │
│ → Cash: $0                                             │
│ → PrestigeBonus: 1.25x (alt income +25%)              │
│ → Kun Pickpocket unlocked igen                         │
│ → Men progression er HURTIGERE denne gang!             │
└─────────────────────────────────────────────────────────┘
```

---

# SVG ICONS (Claude Code skal generere)

## Design Retningslinjer
- Størrelse: 64x64 viewBox
- Style: Flat, simpel, bold farver
- Baggrund: Cirkel med #2D3748
- Accent: #E94560 (rød) eller #FFD700 (guld)

## Icon Specifications

### icon_pickpocket.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="30" fill="#2D3748"/>
  <!-- Hand reaching into pocket -->
  <path d="M20 25 C20 20, 28 18, 32 22 L36 30 L28 34 Z" fill="#E8D5B7"/>
  <!-- Wallet -->
  <rect x="30" y="32" width="16" height="12" rx="2" fill="#8B4513"/>
  <rect x="32" y="34" width="12" height="3" fill="#654321"/>
  <!-- Money sticking out -->
  <rect x="38" y="28" width="8" height="6" fill="#85BB65"/>
</svg>
```

### icon_car.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="30" fill="#2D3748"/>
  <!-- Car body -->
  <path d="M12 38 L16 28 L24 24 L40 24 L48 28 L52 38 L52 42 L12 42 Z" fill="#E94560"/>
  <!-- Windows -->
  <path d="M18 28 L24 24 L40 24 L46 28 L42 34 L22 34 Z" fill="#87CEEB"/>
  <!-- Wheels -->
  <circle cx="20" cy="42" r="5" fill="#1A1A2E"/>
  <circle cx="44" cy="42" r="5" fill="#1A1A2E"/>
  <circle cx="20" cy="42" r="2" fill="#4A5568"/>
  <circle cx="44" cy="42" r="2" fill="#4A5568"/>
</svg>
```

### icon_burglary.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="30" fill="#2D3748"/>
  <!-- House -->
  <path d="M32 14 L50 28 L50 50 L14 50 L14 28 Z" fill="#4A5568"/>
  <!-- Roof -->
  <path d="M32 10 L54 28 L50 28 L32 14 L14 28 L10 28 Z" fill="#E94560"/>
  <!-- Door (open) -->
  <rect x="26" y="34" width="12" height="16" fill="#1A1A2E"/>
  <!-- Mask figure in door -->
  <circle cx="32" cy="40" r="4" fill="#2D3748"/>
  <rect x="28" y="38" width="8" height="2" fill="#1A1A2E"/>
</svg>
```

### icon_speakeasy.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="30" fill="#2D3748"/>
  <!-- Building -->
  <rect x="16" y="24" width="32" height="26" fill="#8B4513"/>
  <!-- Secret door -->
  <rect x="26" y="34" width="12" height="16" fill="#654321"/>
  <circle cx="36" cy="42" r="1.5" fill="#FFD700"/>
  <!-- "CLOSED" sign (wink wink) -->
  <rect x="20" y="26" width="24" height="6" fill="#1A1A2E"/>
  <text x="32" y="31" text-anchor="middle" font-size="5" fill="#E94560">CLOSED</text>
  <!-- Cocktail glass hint -->
  <path d="M44 18 L48 18 L46 24 L46 28 L44 28 L44 24 Z" fill="#FFD700" opacity="0.7"/>
</svg>
```

### icon_casino.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="30" fill="#2D3748"/>
  <!-- Chips stack -->
  <ellipse cx="24" cy="40" rx="10" ry="4" fill="#E94560"/>
  <ellipse cx="24" cy="38" rx="10" ry="4" fill="#C73E54"/>
  <ellipse cx="24" cy="36" rx="10" ry="4" fill="#E94560"/>
  <!-- Cards -->
  <rect x="36" y="22" width="14" height="20" rx="2" fill="white" transform="rotate(15 43 32)"/>
  <rect x="34" y="24" width="14" height="20" rx="2" fill="white" transform="rotate(-10 41 34)"/>
  <!-- Card symbols -->
  <text x="40" y="36" font-size="10" fill="#E94560">♠</text>
  <!-- Dice -->
  <rect x="42" y="40" width="10" height="10" rx="2" fill="white"/>
  <circle cx="45" cy="43" r="1" fill="#1A1A2E"/>
  <circle cx="49" cy="47" r="1" fill="#1A1A2E"/>
</svg>
```

---

# CLAUDE CODE IMPLEMENTERINGS-KOMMANDOER

## Fase 1: Projekt Oprettelse
```bash
# 1. Opret MAUI projekt
dotnet new maui -n MadeMan.IdleEmpire -o MadeMan.IdleEmpire

# 2. Naviger til projekt
cd MadeMan.IdleEmpire

# 3. Tilføj NuGet packages
dotnet add package CommunityToolkit.Mvvm --version 8.2.2

# 4. Verificer projekt struktur
ls -la
```

## Fase 2: Mappestruktur
```bash
# Opret mapper
mkdir -p Models ViewModels Views Services Resources/Styles Resources/Images

# Verificer
find . -type d -name "*.cs" -o -type d
```

## Fase 3: Fil Oprettelse (Rækkefølge)
```
1. Models/GameState.cs
2. Models/Operation.cs  
3. Models/GameConfig.cs
4. Services/SaveManager.cs
5. Services/IGameEngine.cs
6. Services/GameEngine.cs
7. ViewModels/OperationViewModel.cs
8. ViewModels/MainViewModel.cs
9. Resources/Styles/Theme.xaml
10. Views/MainPage.xaml + .cs
11. MauiProgram.cs (update DI)
12. App.xaml.cs (update lifecycle)
13. Resources/Images/*.svg (alle ikoner)
```

## Fase 4: Build & Test
```bash
# Build for Android
dotnet build -f net8.0-android

# Kør på emulator (kræver at emulator kører)
dotnet build -f net8.0-android -t:Run

# Eller lav APK
dotnet publish -f net8.0-android -c Release
```

---

# TEST CHECKLISTE

## Funktionel Test
- [ ] App starter uden crash
- [ ] Cash ticker opdaterer hvert 100ms
- [ ] Pickpocket er auto-unlocked ved start
- [ ] Kan købe upgrades når man har råd
- [ ] Knapper er disabled når man ikke har råd
- [ ] Nye operations unlocker ved korrekt beløb
- [ ] Prestige knap vises ved $10,000 total earned
- [ ] Prestige nulstiller korrekt men beholder bonus
- [ ] Data gemmes ved app close
- [ ] Data loader ved app open
- [ ] Offline earnings beregnes korrekt

## UX Test
- [ ] Første upgrade mulig inden 15 sekunder
- [ ] Første unlock (Car Theft) mulig inden 2 minutter
- [ ] Tal formateres korrekt (K, M, B)
- [ ] Knap farver indikerer affordability tydeligt
- [ ] Ingen UI jank eller stuttering

---

# POST-MVP ROADMAP

## Version 1.1: Polish
- Offline earnings popup
- Simple tap animation (scale bounce)
- Sound effect ved køb (cha-ching)

## Version 1.2: Mere Content
- 5 flere operations (total 10)
- Achievement system (simple)

## Version 1.3: Heat System
- Heat resource der stiger med aktivitet
- Razzia event ved max heat
- Influence upgrade der reducerer heat

## Version 2.0: Multiplayer-lite
- Leaderboards
- Asynkrone raids

---

# OPSUMMERING

## Hvad Claude Code Skal Gøre

1. **Opret projekt** med korrekt struktur
2. **Implementer models** (GameState, Operation, GameConfig)
3. **Implementer services** (SaveManager, GameEngine)
4. **Implementer ViewModels** (MainViewModel, OperationViewModel)
5. **Opret UI** (Theme.xaml, MainPage.xaml)
6. **Generer SVG ikoner** (5 stk som specificeret)
7. **Konfigurer DI** i MauiProgram.cs
8. **Test** at alt virker

## Kritiske Beslutninger Taget

| Original Spec | Ny Spec | Hvorfor |
|---------------|---------|---------|
| 3 pages | 1 page | Simplicitet, ingen navigation |
| $100K prestige | $10K prestige | Hurtigere dopamin loop |
| $500 Car Theft | $50 Car Theft | Unlock inden 2 min |
| Crime + Business models | Unified Operation | Mindre kode, samme funktionalitet |
| Separate ViewModel per item | OperationViewModel | Cleaner binding |

---

*Implementation Guide v2.0*
*Optimeret til Claude Code execution*
*Alle tal, farver, og specs er endelige*
