# Task 009: MainPage UI

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-008
- **Estimated Time**: 1.5 time
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 453-584)
- **Requires Design Input**: NO

---

## Formål

Implementer MainPage.xaml - den eneste skærm i spillet.

**Hvorfor dette er vigtigt:**
- Alt gameplay sker her
- Cash header altid synlig
- Operations liste med upgrade knapper
- Prestige panel når tilgængeligt

---

## Risici

### Potentielle Problemer
1. **XAML Binding Errors**:
   - Edge case: Forkert DataType
   - Impact: UI viser ikke data

2. **Layout Issues**:
   - Edge case: Forskellige skærmstørrelser
   - Impact: UI ser forkert ud

### Mitigering
- Brug x:DataType for compile-time checking
- Test på forskellige emulator størrelser

---

## Implementation Guide

### Step 1: Flyt MainPage.xaml til Views folder

Flyt filen fra root til Views/ og opdater namespace.

### Step 2: Opdater MainPage.xaml

**Sti**: `src/MadeMan.IdleEmpire/Views/MainPage.xaml`

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
               Margin="0,0,0,16"
               BorderColor="Transparent">
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
                       VerticalOptions="Center"
                       BorderColor="Transparent">
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
                               Padding="16"
                               BorderColor="Transparent">
                            <Frame.GestureRecognizers>
                                <TapGestureRecognizer Command="{Binding TapCommand}"/>
                            </Frame.GestureRecognizers>

                            <Grid ColumnDefinitions="50,*,Auto" RowDefinitions="Auto,Auto">

                                <!-- Icon placeholder -->
                                <Frame Grid.RowSpan="2"
                                       BackgroundColor="{StaticResource SurfaceLight}"
                                       CornerRadius="8"
                                       Padding="8"
                                       HeightRequest="50"
                                       WidthRequest="50"
                                       BorderColor="Transparent">
                                    <Label Text="$"
                                           FontSize="24"
                                           HorizontalOptions="Center"
                                           VerticalOptions="Center"
                                           TextColor="{StaticResource Gold}"/>
                                </Frame>

                                <!-- Name + Level -->
                                <HorizontalStackLayout Grid.Column="1" Spacing="8" Margin="12,0,0,0">
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
                                       IsVisible="{Binding IsUnlocked}"
                                       Margin="12,0,0,0"/>

                                <!-- Action Button -->
                                <Button Grid.Column="2" Grid.RowSpan="2"
                                        Text="{Binding ButtonText}"
                                        Command="{Binding TapCommand}"
                                        BackgroundColor="{Binding ButtonColor}"
                                        TextColor="White"
                                        CornerRadius="8"
                                        Padding="16,8"
                                        FontSize="14"
                                        FontAttributes="Bold"
                                        VerticalOptions="Center"/>
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
               IsVisible="{Binding CanPrestige}"
               BorderColor="Transparent">
            <VerticalStackLayout Spacing="12">
                <Label Text="PRESTIGE AVAILABLE"
                       FontSize="16"
                       FontAttributes="Bold"
                       TextColor="White"
                       HorizontalOptions="Center"/>
                <Button Text="{Binding PrestigeButtonText}"
                        Command="{Binding PrestigeCommand}"
                        BackgroundColor="White"
                        TextColor="{StaticResource Primary}"
                        CornerRadius="8"
                        FontAttributes="Bold"/>
            </VerticalStackLayout>
        </Frame>

    </Grid>
</ContentPage>
```

### Step 3: Opdater MainPage.xaml.cs

**Sti**: `src/MadeMan.IdleEmpire/Views/MainPage.xaml.cs`

```csharp
using MadeMan.IdleEmpire.ViewModels;

namespace MadeMan.IdleEmpire.Views;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnAppearing();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnDisappearing();
    }
}
```

### Step 4: Opdater AppShell.xaml

Opdater route til ny lokation:

```xml
<?xml version="1.0" encoding="UTF-8" ?>
<Shell
    x:Class="MadeMan.IdleEmpire.AppShell"
    xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
    xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
    xmlns:views="clr-namespace:MadeMan.IdleEmpire.Views"
    Shell.FlyoutBehavior="Disabled"
    Shell.NavBarIsVisible="False">

    <ShellContent
        ContentTemplate="{DataTemplate views:MainPage}"
        Route="MainPage" />

</Shell>
```

### Step 5: Registrer i MauiProgram.cs

Tilføj:
```csharp
builder.Services.AddTransient<Views.MainPage>();
```

### Step 6: Slet gamle MainPage filer fra root

Slet:
- `MainPage.xaml` (i root)
- `MainPage.xaml.cs` (i root)

---

## Acceptance Criteria

- [ ] MainPage.xaml i Views folder
- [ ] Cash header vises korrekt
- [ ] Operations liste med alle 5 operations
- [ ] Buttons viser priser
- [ ] Prestige panel vises når tilgængeligt
- [ ] Build succeeds med 0 errors

---

**Task Status**: BLOCKED (venter på TASK-008)
**Last Updated**: 2024-12-25
