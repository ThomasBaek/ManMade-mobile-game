# 🎮 MADE MAN: IDLE EMPIRE
## MVP Specifikation til .NET MAUI

---

# OVERSIGT

## Mål
En **minimal viable product** af idle mafia-spillet der kan:
- Køres på Android (primært) og iOS (sekundært)
- Implementeres via Claude Code
- Bruger simpel grafik genereret af Claude
- Gemmer data lokalt på enheden

## Tech Stack

| Komponent | Teknologi |
|-----------|-----------|
| **Framework** | .NET MAUI (.NET 8) |
| **Sprog** | C# |
| **UI** | XAML + MVVM (CommunityToolkit.Mvvm) |
| **Lokal Storage** | SQLite (sqlite-net-pcl) ELLER Preferences API |
| **Timer/Background** | IDispatcherTimer + App Lifecycle |
| **Grafik** | SVG ikoner + Solid colors + Simple shapes |
| **Target** | Android 8.0+ (API 26) primært |

## Hvorfor MAUI?
- Du er .NET udvikler = minimal learning curve
- MVVM pattern = clean separation
- Cross-platform fra dag 1
- God idle game support (timers, background, local storage)
- Claude Code kan generere C#/XAML uden problemer

---

# MVP SCOPE

## ✅ Inkluderet i MVP

### Core Gameplay
1. **3 Crime Types** (passiv indkomst)
   - Pickpocketing (starter-crime)
   - Car Theft (unlock ved 500$)
   - Burglary (unlock ved 2,500$)

2. **1 Business Type**
   - Speakeasy (unlock ved 10,000$)

3. **Upgrade System**
   - Hver crime/business har 10 levels
   - Hver level = +50% base income

4. **Simpel Prestige**
   - Manual reset-knap
   - Giver permanent multiplier baseret på total earnings

### Ressourcer
- **Cash ($)** - Primær valuta
- **Respect** - Unlock metric (forenklet)

### UI Screens
1. **Main Screen** - Crime overview + earnings ticker
2. **Upgrades Screen** - Liste over alle upgrades
3. **Stats Screen** - Total earnings, prestige info

## ❌ IKKE i MVP (Senere iterationer)
- Heat system
- Crew members
- Territories
- PvP/Raids
- Notifications
- Ads/IAP
- Cloud save
- Achievements
- Story/narrative
- Animations

---

# ARKITEKTUR

## Projekt Struktur

```
MadeMan.IdleEmpire/
├── MadeMan.IdleEmpire.sln
└── MadeMan.IdleEmpire/
    ├── App.xaml
    ├── App.xaml.cs
    ├── MauiProgram.cs
    ├── AppShell.xaml
    ├── AppShell.xaml.cs
    │
    ├── Models/
    │   ├── Crime.cs
    │   ├── Business.cs
    │   ├── GameState.cs
    │   └── PrestigeData.cs
    │
    ├── ViewModels/
    │   ├── BaseViewModel.cs
    │   ├── MainViewModel.cs
    │   ├── UpgradesViewModel.cs
    │   └── StatsViewModel.cs
    │
    ├── Views/
    │   ├── MainPage.xaml
    │   ├── MainPage.xaml.cs
    │   ├── UpgradesPage.xaml
    │   ├── UpgradesPage.xaml.cs
    │   ├── StatsPage.xaml
    │   └── StatsPage.xaml.cs
    │
    ├── Services/
    │   ├── IGameService.cs
    │   ├── GameService.cs
    │   ├── ISaveService.cs
    │   └── SaveService.cs
    │
    ├── Resources/
    │   ├── Fonts/
    │   ├── Images/
    │   │   ├── icon_pickpocket.svg
    │   │   ├── icon_car.svg
    │   │   ├── icon_burglary.svg
    │   │   ├── icon_speakeasy.svg
    │   │   └── icon_cash.svg
    │   ├── Styles/
    │   │   └── Colors.xaml
    │   └── Raw/
    │
    └── Platforms/
        ├── Android/
        └── iOS/
```

---

# DATA MODELLER

## GameState.cs
```csharp
public class GameState
{
    public double Cash { get; set; } = 0;
    public double TotalEarnings { get; set; } = 0;
    public int PrestigeLevel { get; set; } = 0;
    public double PrestigeMultiplier { get; set; } = 1.0;
    public DateTime LastSaveTime { get; set; } = DateTime.UtcNow;
    
    public List<Crime> Crimes { get; set; } = new();
    public List<Business> Businesses { get; set; } = new();
}
```

## Crime.cs
```csharp
public class Crime
{
    public string Id { get; set; }           // "pickpocket", "cartheft", "burglary"
    public string Name { get; set; }         // Display name
    public string Icon { get; set; }         // Resource path
    public int Level { get; set; } = 0;      // 0 = locked, 1-10 = upgrade levels
    public double BaseCashPerSecond { get; set; }
    public double UnlockCost { get; set; }
    public double UpgradeCost => UnlockCost * Math.Pow(2, Level);
    public bool IsUnlocked => Level > 0;
    
    public double CurrentCashPerSecond(double prestigeMultiplier)
    {
        if (!IsUnlocked) return 0;
        return BaseCashPerSecond * (1 + (Level - 1) * 0.5) * prestigeMultiplier;
    }
}
```

## Business.cs
```csharp
public class Business
{
    public string Id { get; set; }           // "speakeasy"
    public string Name { get; set; }
    public string Icon { get; set; }
    public int Level { get; set; } = 0;
    public double BaseCashPerSecond { get; set; }
    public double UnlockCost { get; set; }
    public double UpgradeCost => UnlockCost * Math.Pow(2.5, Level);
    public bool IsUnlocked => Level > 0;
    
    public double CurrentCashPerSecond(double prestigeMultiplier)
    {
        if (!IsUnlocked) return 0;
        return BaseCashPerSecond * (1 + (Level - 1) * 0.75) * prestigeMultiplier;
    }
}
```

---

# GAME CONFIGURATION

## Initial Crime/Business Data
```csharp
public static class GameConfig
{
    public static List<Crime> DefaultCrimes => new()
    {
        new Crime 
        { 
            Id = "pickpocket", 
            Name = "Pickpocketing", 
            Icon = "icon_pickpocket.svg",
            BaseCashPerSecond = 1.0,
            UnlockCost = 0  // Free starter
        },
        new Crime 
        { 
            Id = "cartheft", 
            Name = "Car Theft", 
            Icon = "icon_car.svg",
            BaseCashPerSecond = 5.0,
            UnlockCost = 500
        },
        new Crime 
        { 
            Id = "burglary", 
            Name = "Burglary", 
            Icon = "icon_burglary.svg",
            BaseCashPerSecond = 20.0,
            UnlockCost = 2500
        }
    };
    
    public static List<Business> DefaultBusinesses => new()
    {
        new Business 
        { 
            Id = "speakeasy", 
            Name = "Speakeasy", 
            Icon = "icon_speakeasy.svg",
            BaseCashPerSecond = 50.0,
            UnlockCost = 10000
        }
    };
    
    public const int MaxLevel = 10;
    public const double TickIntervalMs = 100; // 10 ticks per second
}
```

---

# SERVICES

## IGameService.cs
```csharp
public interface IGameService
{
    GameState State { get; }
    double TotalIncomePerSecond { get; }
    
    void Initialize();
    void Tick(double deltaSeconds);
    void UnlockCrime(string crimeId);
    void UpgradeCrime(string crimeId);
    void UnlockBusiness(string businessId);
    void UpgradeBusiness(string businessId);
    void Prestige();
    void CalculateOfflineEarnings();
}
```

## GameService.cs (Core Logic)
```csharp
public class GameService : IGameService
{
    private readonly ISaveService _saveService;
    
    public GameState State { get; private set; }
    
    public double TotalIncomePerSecond
    {
        get
        {
            double income = 0;
            foreach (var crime in State.Crimes)
                income += crime.CurrentCashPerSecond(State.PrestigeMultiplier);
            foreach (var biz in State.Businesses)
                income += biz.CurrentCashPerSecond(State.PrestigeMultiplier);
            return income;
        }
    }
    
    public GameService(ISaveService saveService)
    {
        _saveService = saveService;
    }
    
    public void Initialize()
    {
        State = _saveService.Load() ?? CreateNewGame();
        CalculateOfflineEarnings();
    }
    
    private GameState CreateNewGame()
    {
        var state = new GameState
        {
            Crimes = GameConfig.DefaultCrimes,
            Businesses = GameConfig.DefaultBusinesses
        };
        
        // Auto-unlock pickpocketing at level 1
        state.Crimes[0].Level = 1;
        
        return state;
    }
    
    public void Tick(double deltaSeconds)
    {
        double earned = TotalIncomePerSecond * deltaSeconds;
        State.Cash += earned;
        State.TotalEarnings += earned;
    }
    
    public void UnlockCrime(string crimeId)
    {
        var crime = State.Crimes.FirstOrDefault(c => c.Id == crimeId);
        if (crime == null || crime.IsUnlocked) return;
        if (State.Cash < crime.UnlockCost) return;
        
        State.Cash -= crime.UnlockCost;
        crime.Level = 1;
    }
    
    public void UpgradeCrime(string crimeId)
    {
        var crime = State.Crimes.FirstOrDefault(c => c.Id == crimeId);
        if (crime == null || !crime.IsUnlocked) return;
        if (crime.Level >= GameConfig.MaxLevel) return;
        if (State.Cash < crime.UpgradeCost) return;
        
        State.Cash -= crime.UpgradeCost;
        crime.Level++;
    }
    
    // Similar for Business...
    
    public void Prestige()
    {
        if (State.TotalEarnings < 100000) return; // Minimum threshold
        
        // Calculate bonus: +10% per 100k earned
        double bonus = Math.Floor(State.TotalEarnings / 100000) * 0.1;
        
        State.PrestigeLevel++;
        State.PrestigeMultiplier += bonus;
        State.Cash = 0;
        State.TotalEarnings = 0;
        
        // Reset crimes and businesses
        State.Crimes = GameConfig.DefaultCrimes;
        State.Businesses = GameConfig.DefaultBusinesses;
        State.Crimes[0].Level = 1; // Re-unlock pickpocketing
    }
    
    public void CalculateOfflineEarnings()
    {
        var now = DateTime.UtcNow;
        var offlineSeconds = (now - State.LastSaveTime).TotalSeconds;
        
        // Cap at 8 hours
        offlineSeconds = Math.Min(offlineSeconds, 8 * 60 * 60);
        
        if (offlineSeconds > 60) // Only if away > 1 minute
        {
            double offlineEarnings = TotalIncomePerSecond * offlineSeconds * 0.5; // 50% efficiency
            State.Cash += offlineEarnings;
            State.TotalEarnings += offlineEarnings;
        }
        
        State.LastSaveTime = now;
    }
}
```

## ISaveService.cs
```csharp
public interface ISaveService
{
    GameState? Load();
    void Save(GameState state);
    void Delete();
}
```

## SaveService.cs (Using Preferences for simplicity)
```csharp
public class SaveService : ISaveService
{
    private const string SaveKey = "gamestate";
    
    public GameState? Load()
    {
        try
        {
            var json = Preferences.Default.Get(SaveKey, string.Empty);
            if (string.IsNullOrEmpty(json)) return null;
            return JsonSerializer.Deserialize<GameState>(json);
        }
        catch
        {
            return null;
        }
    }
    
    public void Save(GameState state)
    {
        state.LastSaveTime = DateTime.UtcNow;
        var json = JsonSerializer.Serialize(state);
        Preferences.Default.Set(SaveKey, json);
    }
    
    public void Delete()
    {
        Preferences.Default.Remove(SaveKey);
    }
}
```

---

# VIEWMODELS

## MainViewModel.cs
```csharp
public partial class MainViewModel : ObservableObject
{
    private readonly IGameService _gameService;
    private readonly ISaveService _saveService;
    private IDispatcherTimer? _gameTimer;
    private DateTime _lastTick;
    
    [ObservableProperty]
    private string cashDisplay = "$0";
    
    [ObservableProperty]
    private string incomeDisplay = "$0/s";
    
    [ObservableProperty]
    private ObservableCollection<CrimeItemViewModel> crimes = new();
    
    [ObservableProperty]
    private ObservableCollection<BusinessItemViewModel> businesses = new();
    
    public MainViewModel(IGameService gameService, ISaveService saveService)
    {
        _gameService = gameService;
        _saveService = saveService;
    }
    
    public void OnAppearing()
    {
        _gameService.Initialize();
        RefreshUI();
        StartGameLoop();
    }
    
    public void OnDisappearing()
    {
        StopGameLoop();
        _saveService.Save(_gameService.State);
    }
    
    private void StartGameLoop()
    {
        _lastTick = DateTime.UtcNow;
        _gameTimer = Application.Current?.Dispatcher.CreateTimer();
        if (_gameTimer != null)
        {
            _gameTimer.Interval = TimeSpan.FromMilliseconds(GameConfig.TickIntervalMs);
            _gameTimer.Tick += OnGameTick;
            _gameTimer.Start();
        }
    }
    
    private void StopGameLoop()
    {
        _gameTimer?.Stop();
        _gameTimer = null;
    }
    
    private void OnGameTick(object? sender, EventArgs e)
    {
        var now = DateTime.UtcNow;
        var delta = (now - _lastTick).TotalSeconds;
        _lastTick = now;
        
        _gameService.Tick(delta);
        UpdateDisplays();
    }
    
    private void UpdateDisplays()
    {
        CashDisplay = FormatCurrency(_gameService.State.Cash);
        IncomeDisplay = $"{FormatCurrency(_gameService.TotalIncomePerSecond)}/s";
        
        // Update crime items
        foreach (var crimeVm in Crimes)
        {
            crimeVm.Refresh();
        }
    }
    
    private void RefreshUI()
    {
        Crimes.Clear();
        foreach (var crime in _gameService.State.Crimes)
        {
            Crimes.Add(new CrimeItemViewModel(crime, _gameService));
        }
        
        Businesses.Clear();
        foreach (var biz in _gameService.State.Businesses)
        {
            Businesses.Add(new BusinessItemViewModel(biz, _gameService));
        }
        
        UpdateDisplays();
    }
    
    private string FormatCurrency(double value)
    {
        if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
        if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
        if (value >= 1_000) return $"${value / 1_000:F2}K";
        return $"${value:F2}";
    }
}
```

## CrimeItemViewModel.cs
```csharp
public partial class CrimeItemViewModel : ObservableObject
{
    private readonly Crime _crime;
    private readonly IGameService _gameService;
    
    public string Name => _crime.Name;
    public string Icon => _crime.Icon;
    
    [ObservableProperty]
    private string levelDisplay = "";
    
    [ObservableProperty]
    private string incomeDisplay = "";
    
    [ObservableProperty]
    private string buttonText = "";
    
    [ObservableProperty]
    private bool canInteract;
    
    [ObservableProperty]
    private bool isUnlocked;
    
    public CrimeItemViewModel(Crime crime, IGameService gameService)
    {
        _crime = crime;
        _gameService = gameService;
        Refresh();
    }
    
    public void Refresh()
    {
        IsUnlocked = _crime.IsUnlocked;
        LevelDisplay = IsUnlocked ? $"Lvl {_crime.Level}/{GameConfig.MaxLevel}" : "LOCKED";
        IncomeDisplay = IsUnlocked 
            ? $"${_crime.CurrentCashPerSecond(_gameService.State.PrestigeMultiplier):F2}/s" 
            : "";
        
        if (!IsUnlocked)
        {
            ButtonText = $"Unlock ${_crime.UnlockCost:F0}";
            CanInteract = _gameService.State.Cash >= _crime.UnlockCost;
        }
        else if (_crime.Level < GameConfig.MaxLevel)
        {
            ButtonText = $"Upgrade ${_crime.UpgradeCost:F0}";
            CanInteract = _gameService.State.Cash >= _crime.UpgradeCost;
        }
        else
        {
            ButtonText = "MAX";
            CanInteract = false;
        }
    }
    
    [RelayCommand]
    private void Interact()
    {
        if (!IsUnlocked)
        {
            _gameService.UnlockCrime(_crime.Id);
        }
        else
        {
            _gameService.UpgradeCrime(_crime.Id);
        }
        Refresh();
    }
}
```

---

# UI/VIEWS

## MainPage.xaml
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:MadeMan.IdleEmpire.ViewModels"
             x:Class="MadeMan.IdleEmpire.Views.MainPage"
             x:DataType="vm:MainViewModel"
             BackgroundColor="{StaticResource BackgroundDark}">
    
    <Grid RowDefinitions="Auto,*,Auto">
        
        <!-- Header: Cash Display -->
        <Frame Grid.Row="0" 
               BackgroundColor="{StaticResource SurfaceDark}"
               Padding="20"
               Margin="10"
               CornerRadius="10">
            <VerticalStackLayout HorizontalOptions="Center">
                <Label Text="{Binding CashDisplay}"
                       FontSize="36"
                       FontAttributes="Bold"
                       TextColor="{StaticResource CashGold}"
                       HorizontalOptions="Center"/>
                <Label Text="{Binding IncomeDisplay}"
                       FontSize="18"
                       TextColor="{StaticResource TextSecondary}"
                       HorizontalOptions="Center"/>
            </VerticalStackLayout>
        </Frame>
        
        <!-- Main Content: Crime/Business List -->
        <ScrollView Grid.Row="1" Padding="10">
            <VerticalStackLayout Spacing="10">
                
                <!-- Crimes Section -->
                <Label Text="CRIMINAL ACTIVITIES" 
                       FontSize="14" 
                       TextColor="{StaticResource TextSecondary}"
                       Margin="5,10,0,5"/>
                
                <CollectionView ItemsSource="{Binding Crimes}">
                    <CollectionView.ItemTemplate>
                        <DataTemplate x:DataType="vm:CrimeItemViewModel">
                            <Frame BackgroundColor="{StaticResource SurfaceDark}"
                                   Padding="15"
                                   Margin="0,5"
                                   CornerRadius="8">
                                <Grid ColumnDefinitions="50,*,Auto">
                                    
                                    <!-- Icon -->
                                    <Image Grid.Column="0"
                                           Source="{Binding Icon}"
                                           HeightRequest="40"
                                           WidthRequest="40"/>
                                    
                                    <!-- Info -->
                                    <VerticalStackLayout Grid.Column="1" 
                                                         Margin="10,0"
                                                         VerticalOptions="Center">
                                        <Label Text="{Binding Name}"
                                               FontSize="16"
                                               FontAttributes="Bold"
                                               TextColor="{StaticResource TextPrimary}"/>
                                        <HorizontalStackLayout Spacing="10">
                                            <Label Text="{Binding LevelDisplay}"
                                                   FontSize="12"
                                                   TextColor="{StaticResource TextSecondary}"/>
                                            <Label Text="{Binding IncomeDisplay}"
                                                   FontSize="12"
                                                   TextColor="{StaticResource CashGold}"/>
                                        </HorizontalStackLayout>
                                    </VerticalStackLayout>
                                    
                                    <!-- Action Button -->
                                    <Button Grid.Column="2"
                                            Text="{Binding ButtonText}"
                                            Command="{Binding InteractCommand}"
                                            IsEnabled="{Binding CanInteract}"
                                            BackgroundColor="{StaticResource AccentRed}"
                                            TextColor="White"
                                            CornerRadius="5"
                                            Padding="15,8"
                                            FontSize="12"/>
                                </Grid>
                            </Frame>
                        </DataTemplate>
                    </CollectionView.ItemTemplate>
                </CollectionView>
                
                <!-- Businesses Section -->
                <Label Text="BUSINESSES" 
                       FontSize="14" 
                       TextColor="{StaticResource TextSecondary}"
                       Margin="5,20,0,5"/>
                
                <CollectionView ItemsSource="{Binding Businesses}">
                    <!-- Same template as crimes -->
                </CollectionView>
                
            </VerticalStackLayout>
        </ScrollView>
        
        <!-- Bottom Navigation -->
        <Grid Grid.Row="2" 
              ColumnDefinitions="*,*,*"
              BackgroundColor="{StaticResource SurfaceDark}"
              Padding="0,10">
            <Button Grid.Column="0" Text="🏠 Main" BackgroundColor="Transparent"/>
            <Button Grid.Column="1" Text="⬆️ Upgrades" BackgroundColor="Transparent"
                    Clicked="OnUpgradesClicked"/>
            <Button Grid.Column="2" Text="📊 Stats" BackgroundColor="Transparent"
                    Clicked="OnStatsClicked"/>
        </Grid>
        
    </Grid>
</ContentPage>
```

## Colors.xaml
```xml
<?xml version="1.0" encoding="utf-8" ?>
<ResourceDictionary xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
                    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml">
    
    <!-- Dark Theme Colors -->
    <Color x:Key="BackgroundDark">#1A1A2E</Color>
    <Color x:Key="SurfaceDark">#16213E</Color>
    <Color x:Key="AccentRed">#E94560</Color>
    <Color x:Key="CashGold">#FFD700</Color>
    <Color x:Key="TextPrimary">#FFFFFF</Color>
    <Color x:Key="TextSecondary">#A0A0A0</Color>
    <Color x:Key="Success">#4CAF50</Color>
    <Color x:Key="Locked">#555555</Color>
    
</ResourceDictionary>
```

---

# SIMPEL GRAFIK

## SVG Ikoner (Claude kan generere disse)

### icon_pickpocket.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="28" fill="#2D3748"/>
  <path d="M24 20 L40 20 L42 40 L22 40 Z" fill="#E94560"/>
  <circle cx="32" cy="26" r="4" fill="#FFD700"/>
  <path d="M28 44 L32 52 L36 44" stroke="#FFD700" stroke-width="2" fill="none"/>
</svg>
```

### icon_car.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="28" fill="#2D3748"/>
  <rect x="14" y="28" width="36" height="14" rx="3" fill="#E94560"/>
  <rect x="18" y="22" width="28" height="10" rx="2" fill="#E94560"/>
  <circle cx="22" cy="42" r="4" fill="#1A1A2E"/>
  <circle cx="42" cy="42" r="4" fill="#1A1A2E"/>
  <rect x="20" y="24" width="10" height="6" fill="#87CEEB"/>
  <rect x="34" y="24" width="10" height="6" fill="#87CEEB"/>
</svg>
```

### icon_burglary.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="28" fill="#2D3748"/>
  <rect x="20" y="24" width="24" height="20" fill="#E94560"/>
  <rect x="18" y="20" width="28" height="6" fill="#C53030"/>
  <rect x="28" y="30" width="8" height="10" fill="#1A1A2E"/>
  <circle cx="34" cy="35" r="1.5" fill="#FFD700"/>
</svg>
```

### icon_speakeasy.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="28" fill="#2D3748"/>
  <rect x="18" y="26" width="28" height="22" fill="#8B4513"/>
  <rect x="28" y="34" width="8" height="14" fill="#654321"/>
  <rect x="20" y="28" width="6" height="8" fill="#FFD700"/>
  <rect x="38" y="28" width="6" height="8" fill="#FFD700"/>
  <path d="M18 26 L32 16 L46 26" fill="#C53030"/>
</svg>
```

### icon_cash.svg
```svg
<svg viewBox="0 0 64 64" xmlns="http://www.w3.org/2000/svg">
  <circle cx="32" cy="32" r="28" fill="#2D3748"/>
  <circle cx="32" cy="32" r="20" fill="#FFD700"/>
  <text x="32" y="40" text-anchor="middle" font-size="24" font-weight="bold" fill="#1A1A2E">$</text>
</svg>
```

---

# DEPENDENCY INJECTION

## MauiProgram.cs
```csharp
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });
        
        // Services
        builder.Services.AddSingleton<ISaveService, SaveService>();
        builder.Services.AddSingleton<IGameService, GameService>();
        
        // ViewModels
        builder.Services.AddTransient<MainViewModel>();
        builder.Services.AddTransient<UpgradesViewModel>();
        builder.Services.AddTransient<StatsViewModel>();
        
        // Pages
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<UpgradesPage>();
        builder.Services.AddTransient<StatsPage>();
        
        return builder.Build();
    }
}
```

---

# APP LIFECYCLE

## App.xaml.cs
```csharp
public partial class App : Application
{
    private readonly IGameService _gameService;
    private readonly ISaveService _saveService;
    
    public App(IGameService gameService, ISaveService saveService)
    {
        InitializeComponent();
        _gameService = gameService;
        _saveService = saveService;
        MainPage = new AppShell();
    }
    
    protected override void OnSleep()
    {
        // Save when app goes to background
        _saveService.Save(_gameService.State);
        base.OnSleep();
    }
    
    protected override void OnResume()
    {
        // Calculate offline earnings when app resumes
        _gameService.CalculateOfflineEarnings();
        base.OnResume();
    }
}
```

---

# IMPLEMENTATION PLAN FOR CLAUDE CODE

## Fase 1: Projekt Setup (Dag 1)
```bash
# Commands til Claude Code
dotnet new maui -n MadeMan.IdleEmpire
cd MadeMan.IdleEmpire
dotnet add package CommunityToolkit.Mvvm
dotnet add package System.Text.Json
```

**Tasks:**
1. ✅ Opret MAUI projekt
2. ✅ Tilføj NuGet packages
3. ✅ Setup folder struktur
4. ✅ Tilføj Colors.xaml

## Fase 2: Core Logic (Dag 1-2)
**Tasks:**
1. ✅ Implementer Models (Crime, Business, GameState)
2. ✅ Implementer GameConfig
3. ✅ Implementer ISaveService + SaveService
4. ✅ Implementer IGameService + GameService
5. ✅ Unit tests for GameService (optional)

## Fase 3: ViewModels (Dag 2)
**Tasks:**
1. ✅ Implementer BaseViewModel
2. ✅ Implementer MainViewModel
3. ✅ Implementer CrimeItemViewModel
4. ✅ Implementer BusinessItemViewModel
5. ✅ Setup DI i MauiProgram.cs

## Fase 4: UI (Dag 2-3)
**Tasks:**
1. ✅ Opret MainPage.xaml
2. ✅ Opret StatsPage.xaml (simpel visning af prestige)
3. ✅ Setup AppShell navigation
4. ✅ Tilføj SVG ikoner

## Fase 5: Polish & Test (Dag 3)
**Tasks:**
1. ✅ Test på Android emulator
2. ✅ Fix eventuelle bugs
3. ✅ Verify save/load fungerer
4. ✅ Verify offline earnings

---

# NUGET PACKAGES

```xml
<ItemGroup>
    <PackageReference Include="CommunityToolkit.Mvvm" Version="8.2.2" />
    <PackageReference Include="System.Text.Json" Version="8.0.0" />
</ItemGroup>
```

---

# BUILD & RUN

## Android
```bash
# Build for Android
dotnet build -f net8.0-android

# Run on emulator
dotnet build -f net8.0-android -t:Run

# Create APK
dotnet publish -f net8.0-android -c Release
```

## iOS (kræver Mac)
```bash
# Build for iOS
dotnet build -f net8.0-ios

# Run on simulator
dotnet build -f net8.0-ios -t:Run
```

---

# NÆSTE ITERATION (Post-MVP)

Når MVP virker, kan disse features tilføjes:

1. **Heat System** - Tilføj Heat ressource + razzia mekanik
2. **Flere Crimes/Businesses** - Expand content
3. **Notifications** - Brug MAUI Local Notifications plugin
4. **Animations** - Tilføj simple fade/scale animationer
5. **Sound Effects** - Cha-ching ved køb
6. **Stats Page** - Udvidet statistik
7. **Cloud Save** - Firebase integration

---

# ESTIMERET TIDSFORBRUG

| Fase | Timer |
|------|-------|
| Setup + Config | 1-2 |
| Models + Services | 2-3 |
| ViewModels | 2-3 |
| UI/XAML | 3-4 |
| Testing + Debug | 2-3 |
| **Total** | **10-15 timer** |

Med Claude Code assistance kan dette reduceres betydeligt, da store dele af koden kan genereres automatisk.

---

*MVP Specifikation Version 1.0*
*Target: .NET 8 MAUI - Android First*
