# ? Sam's Lightning Lab: Compiled Bindings & VisualStateManager
## .NET MAUI Mortgage Calculator Enhancement

---

## ?? Lab Objectives

By the end of this lab, you will:
1. Understand why runtime bindings fail silently
2. Add `x:DataType` for compile-time binding safety
3. Use `VisualStateManager` to control UI states declaratively

**Time:** ~20 minutes

---



# Part 1: The Silent Binding Problem

## Step 1.1: Break a Binding (On Purpose!)

1. Open `Views/NewScenarioPage.xaml`

2. Find this line (~line 22):
```xml
<Label Text="{Binding MonthlyPayment, StringFormat='Monthly: {0:C}'}" />
```

3. **Introduce a typo** - change `MonthlyPayment` to `MonthlyPaymnt`:
```xml
<Label Text="{Binding MonthlyPaymnt, StringFormat='Monthly: {0:C}'}" />
```

4. **Build the solution** (Ctrl+Shift+B)

> ?? **Notice:** No compile errors! The build succeeds.

5. **Run the app** and enter loan values

> ? **Result:** The "Monthly" label shows nothing. No error. No warning. Just... silence.

**This is the problem with runtime bindings - they fail silently!**

---

## Step 1.2: Add Compiled Bindings to NewScenarioPage

1. In `Views/NewScenarioPage.xaml`, modify the `<ContentPage>` tag:

**BEFORE:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="RedoRepository_Sam_Paula.Views.NewScenarioPage"
             Title="New Scenario">
```

**AFTER:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:RedoRepository_Sam_Paula.ViewModels"
             x:DataType="vm:NewScenarioViewModel"
             x:Class="RedoRepository_Sam_Paula.Views.NewScenarioPage"
             Title="New Scenario">
```

2. **Build the solution** (Ctrl+Shift+B)

> ? **Now you get a COMPILE ERROR:**
> ```
> XFC0045: Binding: Property "MonthlyPaymnt" not found on "NewScenarioViewModel"
> ```

3. **Fix the typo** - change `MonthlyPaymnt` back to `MonthlyPayment`

4. Build again - Success! ?

---

## Step 1.3: Add Compiled Bindings to HomePage

1. Open `Views/HomePage.xaml`

2. Modify the `<ContentPage>` tag:

**BEFORE:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             x:Class="RedoRepository_Sam_Paula.Views.HomePage"
             Title="Mortgage Calculator">
```

**AFTER:**
```xml
<ContentPage xmlns="http://schemas.microsoft.com/dotnet/2021/maui"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:vm="clr-namespace:RedoRepository_Sam_Paula.ViewModels"
             xmlns:models="clr-namespace:RedoRepository_Sam_Paula.Models"
             x:DataType="vm:HomeViewModel"
             x:Class="RedoRepository_Sam_Paula.Views.HomePage"
             Title="Mortgage Calculator">
```

3. **Important!** The `DataTemplate` binds to `LoanScenario`, not `HomeViewModel`. Add `x:DataType` to the DataTemplate:

**BEFORE:**
```xml
<DataTemplate>
    <Frame Margin="0,5" Padding="10" BorderColor="#CCCCCC">
```

**AFTER:**
```xml
<DataTemplate x:DataType="models:LoanScenario">
    <Frame Margin="0,5" Padding="10" BorderColor="#CCCCCC">
```

4. Build and verify - no errors! ?

---

## ?? Part 1 Checkpoint

Test your understanding:
- [ ] Try adding a typo to a binding in HomePage.xaml - does it catch it at compile time?
- [ ] Notice you now get **IntelliSense** when typing binding paths!

---

# Part 2: VisualStateManager for UI States

## Step 2.1: Add IsBusy Property to ViewModel

1. Open `ViewModels/NewScenarioViewModel.cs`

2. Add a new field and property (after the other private fields, around line 21):

```csharp
private bool _isBusy;

public bool IsBusy
{
    get => _isBusy;
    set { if (_isBusy == value) return; _isBusy = value; OnPropertyChanged(); }
}
```

---

## Step 2.2: Add Loading Indicator

1. Open `Views/NewScenarioPage.xaml`

2. Add an `ActivityIndicator` after the error label:

```xml
<Label Text="{Binding ErrorMessage}"
       TextColor="Red"
       IsVisible="{Binding HasError}" />

<!-- ADD THIS -->
<ActivityIndicator IsRunning="{Binding IsBusy}"
                   IsVisible="{Binding IsBusy}"
                   Color="#512BD4"
                   HorizontalOptions="Center" />

<Label Text="{Binding MonthlyPayment, StringFormat='Monthly: {0:C}'}" />
```

---

## Step 2.3: Add VisualStateManager to Save Button

1. In `Views/NewScenarioPage.xaml`, replace the Save button:

**BEFORE:**
```xml
<Button Text="Save Scenario"
        Command="{Binding SaveCommand}"
        IsEnabled="{Binding CanSave}" />
```

**AFTER:**
```xml
<Button Text="Save Scenario"
        Command="{Binding SaveCommand}"
        IsEnabled="{Binding CanSave}">
    <VisualStateManager.VisualStateGroups>
        <VisualStateGroup x:Name="CommonStates">
            <VisualState x:Name="Normal">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="#512BD4" />
                    <Setter Property="TextColor" Value="White" />
                    <Setter Property="Scale" Value="1" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="Disabled">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="#CCCCCC" />
                    <Setter Property="TextColor" Value="#666666" />
                    <Setter Property="Scale" Value="0.95" />
                </VisualState.Setters>
            </VisualState>
            <VisualState x:Name="PointerOver">
                <VisualState.Setters>
                    <Setter Property="BackgroundColor" Value="#7B5CD6" />
                    <Setter Property="Scale" Value="1.02" />
                </VisualState.Setters>
            </VisualState>
        </VisualStateGroup>
    </VisualStateManager.VisualStateGroups>
</Button>
```

2. **Run the app** and observe:
   - Button appears grayed out and slightly smaller when disabled
   - Button changes color on hover (desktop)

---

## Step 2.4: (Bonus) Simulate Loading State

1. Open `ViewModels/NewScenarioViewModel.cs`

2. Modify the `Save` method to be async and show loading:

**BEFORE:**
```csharp
private void Save()
{
    decimal.TryParse(LoanAmountText, out var principal);
    // ... rest of method
}
```

**AFTER:**
```csharp
private async Task SaveAsync()
{
    IsBusy = true;
    
    // Simulate network delay
    await Task.Delay(1500);
    
    decimal.TryParse(LoanAmountText, out var principal);
    decimal.TryParse(InterestRateText, out var rate);
    int.TryParse(LoanTermYearsText, out var years);

    _store.Scenarios.Add(new LoanScenario
    {
        LoanAmount = principal,
        InterestRate = rate,
        TermYears = years,
        MonthlyPayment = MonthlyPayment,
        TotalPaid = TotalPaid,
        TotalInterestPaid = TotalInterestPaid
    });

    IsBusy = false;
    await Shell.Current.GoToAsync("..");
}
```

3. Update the constructor to use the async method:

**BEFORE:**
```csharp
SaveCommand = new Command(Save, () => CanSave);
```

**AFTER:**
```csharp
SaveCommand = new Command(async () => await SaveAsync(), () => CanSave);
```

4. **Run and test** - You'll see the spinner when saving!

---

## ?? Part 2 Checkpoint

Test your implementation:
- [ ] Open the app with no values entered - is the Save button grayed out?
- [ ] Enter valid values - does the button become purple?
- [ ] Click Save - does the spinner appear?
- [ ] (Desktop) Hover over the button - does it change color?

---

# ?? Quick Reference

## Compiled Bindings Syntax

```xml
<!-- Page-level binding -->
<ContentPage xmlns:vm="clr-namespace:YourNamespace.ViewModels"
             x:DataType="vm:YourViewModel">

<!-- DataTemplate binding (for collections) -->
<DataTemplate x:DataType="models:YourModel">
```

## VisualStateManager Syntax

```xml
<VisualStateManager.VisualStateGroups>
    <VisualStateGroup x:Name="CommonStates">
        <VisualState x:Name="Normal">
            <VisualState.Setters>
                <Setter Property="BackgroundColor" Value="Blue" />
            </VisualState.Setters>
        </VisualState>
        <VisualState x:Name="Disabled">
            <VisualState.Setters>
                <Setter Property="BackgroundColor" Value="Gray" />
            </VisualState.Setters>
        </VisualState>
    </VisualStateGroup>
</VisualStateManager.VisualStateGroups>
```

## Built-in Visual States

| State | Trigger |
|-------|---------|
| `Normal` | Default state |
| `Disabled` | `IsEnabled="False"` |
| `Focused` | Control has keyboard focus |
| `PointerOver` | Mouse hover (desktop/tablet) |
| `Pressed` | Touch/click active |

---

# ? Lab Complete!

## What You Learned

| Feature | Benefit |
|---------|---------|
| `x:DataType` | Compile-time binding errors instead of silent failures |
| `x:DataType` on DataTemplate | Type-safe bindings inside collections |
| `VisualStateManager` | Declarative UI state changes in XAML |
| `IsBusy` pattern | Loading indicators without code-behind |

## Key Takeaways

1. **Always use `x:DataType`** - Silent binding failures waste debugging time
2. **Remember DataTemplates** - They need their own `x:DataType` for the item type
3. **Use built-in states** - `Normal`, `Disabled`, `Focused`, `PointerOver` work automatically
4. **Keep UI logic in XAML** - VisualStateManager means less code-behind

---

## ?? Extra Credit

Try these on your own:
- [ ] Add VSM to the "Clear All" button on HomePage
- [ ] Add a "Pressed" visual state with a darker color
- [ ] Add error styling (red background) to Entry fields when `HasError` is true

---

*Created for Mobile & Cloud Development - Sam's Lightning Lab Series*
