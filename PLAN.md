# Plan: OnNavigatedTo Lifecycle Verbesserung für UnoFramework Submodule

## Problem-Analyse

Das aktuelle UnoFramework Submodule hat ein `INavigationAware` Interface mit `OnNavigatedTo` und `OnNavigatedFrom` Methoden, aber **niemand ruft diese Methoden auf**. Die ViewModels implementieren das Interface, aber die Navigation triggert es nicht.

### Aktueller Workaround
ViewModels laden Daten im Constructor:
```csharp
public HomeViewModel(BaseServices baseServices) : base(baseServices)
{
    // Trigger initial load - OnNavigatedTo may not be called by navigation system
    OnNavigatingTo();
    _ = LoadPersonalDataAsync(NavigationToken);
}
```

Das ist problematisch weil:
1. Daten werden bei jeder ViewModel-Erstellung geladen, nicht nur bei Navigation
2. Parameter können nicht empfangen werden
3. Bei gecachten Pages wird OnNavigatedTo nicht aufgerufen

## Lösungsoptionen

### Option A: Page Code-Behind mit OnNavigatedTo Override (Empfohlen)

**Uno Platform/WinUI unterstützt `Page.OnNavigatedTo` nativ** (laut Docs: WASM, Skia, Mobile).

Die Page-Klasse überschreibt `OnNavigatedTo` und leitet zum ViewModel weiter:

```csharp
// HomePage.xaml.cs
public sealed partial class HomePage : Page
{
    public HomePage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (DataContext is INavigationAware vm)
        {
            vm.OnNavigatedTo(e.Parameter);
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        if (DataContext is INavigationAware vm)
        {
            vm.OnNavigatedFrom();
        }
    }
}
```

**Vorteile:**
- Nutzt natives WinUI/Uno Platform Lifecycle
- Funktioniert mit Frame.Navigate und Region Navigation
- Kein Custom Navigation Framework nötig
- Parameter werden korrekt übergeben

**Nachteile:**
- Erfordert Code in jeder Page (kann mit Basisklasse vereinfacht werden)

### Option B: NavigationAwarePage Basisklasse (Empfohlen)

Erstelle eine Basisklasse die das automatisch macht:

```csharp
// In UnoFramework
public class NavigationAwarePage : Page
{
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);
        if (DataContext is INavigationAware vm)
        {
            vm.OnNavigatedTo(e.Parameter);
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);
        if (DataContext is INavigationAware vm)
        {
            vm.OnNavigatedFrom();
        }
    }
}
```

Dann erben alle Pages davon:
```xml
<fw:NavigationAwarePage x:Class="MyApp.HomePage" ...>
```

### Option C: Uno Extensions Navigation mit DataContext Injection (Komplexer)

Uno Extensions Navigation injiziert automatisch Daten via Constructor:

```csharp
public class SecondViewModel
{
    public SecondViewModel(Widget widget) // Parameter via DI
    {
        Name = widget.Name;
    }
}
```

Dies erfordert:
- DataViewMap Registrierung
- Parameter als Constructor-Dependency

**Nachteil:** Passt nicht gut zum bestehenden INavigationAware Pattern.

## Empfohlene Implementierung

### Schritt 1: NavigationAwarePage in UnoFramework erstellen

**Datei:** `subm/uno/src/UnoFramework/Pages/NavigationAwarePage.cs`

```csharp
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using UnoFramework.Contracts.Navigation;

namespace UnoFramework.Pages;

/// <summary>
/// Base page class that automatically triggers INavigationAware lifecycle on ViewModel.
/// </summary>
public class NavigationAwarePage : Page
{
    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        if (DataContext is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedTo(e.Parameter);
        }
    }

    protected override void OnNavigatedFrom(NavigationEventArgs e)
    {
        base.OnNavigatedFrom(e);

        if (DataContext is INavigationAware navigationAware)
        {
            navigationAware.OnNavigatedFrom();
        }
    }
}
```

### Schritt 2: Pages auf NavigationAwarePage umstellen

**Beispiel HomePage.xaml:**
```xml
<fw:NavigationAwarePage
    x:Class="Lebenslauf.Features.Cv.Presentation.HomePage"
    xmlns:fw="using:UnoFramework.Pages"
    ...>
```

### Schritt 3: Constructor-Load aus ViewModels entfernen

```csharp
public partial class HomeViewModel : PageViewModel, INavigationAware
{
    public HomeViewModel(BaseServices baseServices) : base(baseServices)
    {
        // Keine Daten mehr im Constructor laden
    }

    public void OnNavigatedTo(object? parameter)
    {
        OnNavigatingTo(); // CancellationToken Setup
        _ = LoadPersonalDataAsync(NavigationToken);
    }

    public void OnNavigatedFrom()
    {
        OnNavigatingFrom(); // Cleanup
    }
}
```

## Dateien zu ändern

### UnoFramework Submodule
1. **Neu:** `subm/uno/src/UnoFramework/Pages/NavigationAwarePage.cs`

### Lebenslauf Uno App
1. `src/uno/src/Features/Cv/Presentation/HomePage.xaml` - Base class ändern
2. `src/uno/src/Features/Cv/Presentation/CvPage.xaml` - Base class ändern
3. `src/uno/src/Features/Cv/Presentation/SkillsPage.xaml` - Base class ändern
4. `src/uno/src/Features/Cv/Presentation/ProjectsPage.xaml` - Base class ändern
5. `src/uno/src/Features/Cv/Presentation/HomeViewModel.cs` - Constructor-Load entfernen
6. `src/uno/src/Features/Cv/Presentation/CvViewModel.cs` - Constructor-Load entfernen
7. `src/uno/src/Features/Cv/Presentation/SkillsViewModel.cs` - Constructor-Load entfernen
8. `src/uno/src/Features/Cv/Presentation/ProjectsViewModel.cs` - Constructor-Load entfernen

## Risiken & Überlegungen

1. **DataContext Timing:** `OnNavigatedTo` wird VOR dem Visual Tree Load aufgerufen. Der DataContext sollte aber bereits gesetzt sein wenn DI korrekt konfiguriert ist.

2. **Initial Page:** Die erste Seite (HomePage) wird eventuell anders behandelt. Testen erforderlich.

3. **Region Navigation:** Bei Uno Extensions Region Navigation (nicht Frame.Navigate) könnte das Verhalten anders sein. Die App nutzt jedoch `NavigateViewModelAsync` was intern Frame.Navigate verwendet.
