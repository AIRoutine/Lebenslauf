# Lebenslauf.Features.Cv

CV/Lebenslauf Feature fuer das Uno Frontend.

## Zweck

- Anzeige des Lebenslaufs mit allen Sektionen
- ViewModel fuer Datenbindung
- UI Sections als wiederverwendbare UserControls

## Oeffentliche APIs

### ServiceCollectionExtensions

```csharp
services.AddCvFeature();
```

## Presentation

| Komponente | Beschreibung |
|------------|--------------|
| CvPage | Hauptseite mit ScrollView |
| CvViewModel | Laedt CV-Daten via API |
| HeaderSection | Name, Foto, Kontaktdaten |
| EducationSection | Ausbildung (Timeline) |
| ExperienceSection | Berufserfahrung (Timeline) |
| SkillsSection | Skills als Chips |
| ProjectsSection | Projekte als Cards |

## Abhaengigkeiten

- UnoFramework - BaseServices, PageViewModel
- Lebenslauf.Shared - UnoService Konstanten
- Lebenslauf.Features.Cv.Contracts - DTOs
