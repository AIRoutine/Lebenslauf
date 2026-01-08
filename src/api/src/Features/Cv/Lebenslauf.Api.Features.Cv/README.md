# Lebenslauf.Api.Features.Cv

CV/Lebenslauf Feature mit Entities, Seeding und Handlers.

## Zweck

- Speicherung und Bereitstellung von CV-Daten
- Seeding mit vordefinierten Lebenslauf-Daten
- API Handler fuer CV-Abfragen

## Oeffentliche APIs

### ServiceCollectionExtensions

```csharp
services.AddCvFeature();
```

Registriert:
- CvSeeder fuer initiale Datenbefuellung

## Entities

| Entity | Beschreibung |
|--------|--------------|
| PersonalData | Persoenliche Daten (Name, Kontakt, etc.) |
| Education | Ausbildungseintraege |
| WorkExperience | Berufserfahrung |
| SkillCategory | Skill-Kategorien (Expertise, Tools, etc.) |
| Skill | Einzelne Skills |
| Project | Referenzprojekte |

## Abhaengigkeiten

- Lebenslauf.Api.Core.Data - DbContext, BaseEntity
- Lebenslauf.Api.Core.Data.Seeding - ISeeder Interface
- Lebenslauf.Api.Features.Cv.Contracts - Request/Response DTOs

## Daten aendern

Die CV-Daten werden beim Start via `CvSeeder` geladen.
Um Daten zu aendern:

1. `Data/Seeding/CvSeeder.cs` bearbeiten
2. `app.db` Datei loeschen
3. API neu starten
