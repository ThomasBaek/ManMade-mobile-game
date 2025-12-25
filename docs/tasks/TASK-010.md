# Task 010: Add Icons

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-009
- **Estimated Time**: 30 min
- **Status**: BLOCKED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (linje 858-948)
- **Requires Design Input**: NO

---

## Formål

Tilføj ikoner for alle 5 operations.

**Hvorfor dette er vigtigt:**
- Visuel genkendelse af operations
- Forbedret UX
- Professionelt look

---

## Implementation Guide

### Step 1: Opret PNG ikoner

Opret simple placeholder ikoner som 64x64 PNG filer i:
`src/MadeMan.IdleEmpire/Resources/Images/`

**Filer:**
- icon_pickpocket.png
- icon_car.png
- icon_burglary.png
- icon_speakeasy.png
- icon_casino.png

### Step 2: Opdater OperationViewModel

I OperationViewModel, brug Icon property fra Operation model.

### Step 3: Opdater MainPage.xaml

Erstat placeholder Label med Image:

```xml
<Image Source="{Binding Icon}"
       Aspect="AspectFit"
       HeightRequest="32"
       WidthRequest="32"/>
```

---

## Note

For MVP kan vi bruge simple emoji-baserede eller text placeholders.
Rigtige ikoner kan tilføjes i polish fase.

---

## Acceptance Criteria

- [ ] 5 ikon-filer i Resources/Images
- [ ] Ikoner vises i UI
- [ ] Build succeeds med 0 errors

---

**Task Status**: BLOCKED (venter på TASK-009)
**Last Updated**: 2024-12-25
