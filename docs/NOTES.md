# Made Man: Idle Empire - Notes & Scratchpad

## Hurtige Noter
- Projekt bruger net10.0 - skal ændres til net8.0 i TASK-001
- CommunityToolkit.Mvvm skal installeres

---

## Design Beslutninger Log

### 2024-12-25: Projekt Analyse
- Fandt at projektet var oprettet med net10.0 template
- Mapper struktur er klar men tom
- Skal følge CLAUDE_CODE_IMPLEMENTATION_GUIDE.md specifikation

---

## Kode Snippets (til reference)

### FormatCash Helper
```csharp
private string FormatCash(double value)
{
    if (value >= 1_000_000_000) return $"${value / 1_000_000_000:F2}B";
    if (value >= 1_000_000) return $"${value / 1_000_000:F2}M";
    if (value >= 1_000) return $"${value / 1_000:F2}K";
    return $"${value:F2}";
}
```

### Operation Upgrade Formula
```csharp
// Cost increases by multiplier per level
public double GetUpgradeCost(int currentLevel)
    => UnlockCost * Math.Pow(UpgradeMultiplier, currentLevel);

// Income = base * level * prestige bonus
public double GetIncome(int level, double prestigeBonus)
    => level > 0 ? BaseIncome * level * prestigeBonus : 0;
```

---

## Kommende Overvejelser
- [ ] Skal der tilføjes animations senere?
- [ ] Skal der tilføjes lyd effekter?
- [ ] Hvordan håndterer vi meget store tal? (scientific notation?)

---

## Links & Referencer
- [CommunityToolkit.Mvvm Docs](https://learn.microsoft.com/en-us/dotnet/communitytoolkit/mvvm/)
- [MAUI Preferences API](https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/preferences)

---

## Fejlfinding Log
*Tilføj fejl og løsninger her efterhånden*

---

## Performance Noter
*Tilføj performance observationer her*
