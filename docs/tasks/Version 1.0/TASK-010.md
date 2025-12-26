# Task 010: Add Icons

## Metadata
- **Phase**: 2 - UI
- **Dependencies**: TASK-009
- **Estimated Time**: 30 min
- **Status**: COMPLETED
- **Design Reference**: docs/CLAUDE_CODE_IMPLEMENTATION_GUIDE.md (lines 858-948)
- **Requires Design Input**: NO

---

## Purpose

Add icons for all 5 operations.

**Why this is important:**
- Visual recognition of operations
- Improved UX
- Professional look

---

## Implementation Guide

### Step 1: Create PNG icons

Create simple placeholder icons as 64x64 PNG files in:
`src/MadeMan.IdleEmpire/Resources/Images/`

**Files:**
- icon_pickpocket.png
- icon_car.png
- icon_burglary.png
- icon_speakeasy.png
- icon_casino.png

### Step 2: Update OperationViewModel

In OperationViewModel, use Icon property from Operation model.

### Step 3: Update MainPage.xaml

Replace placeholder Label with Image:

```xml
<Image Source="{Binding Icon}"
       Aspect="AspectFit"
       HeightRequest="32"
       WidthRequest="32"/>
```

---

## Note

For MVP we can use simple emoji-based or text placeholders.
Real icons can be added in polish phase.

---

## Acceptance Criteria

- [x] 5 emoji icons via IconEmoji property (MVP approach)
- [x] Icons display in UI
- [x] Build succeeds with 0 errors

---

**Task Status**: COMPLETED
**Last Updated**: 2024-12-25
**Commit**: 8dd4516

## Implementation Note
Used emoji icons instead of PNG files for MVP simplicity:
- Pickpocket: 🤚 (raised hand)
- Car Theft: 🚗 (car)
- Burglary: 🏠 (house)
- Speakeasy: 🍺 (beer)
- Casino: 🎰 (slot machine)
