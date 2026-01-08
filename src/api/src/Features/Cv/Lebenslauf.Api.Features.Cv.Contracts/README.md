# Lebenslauf.Api.Features.Cv.Contracts

Request/Response DTOs fuer das CV Feature.

## Zweck

- Definition der Mediator Request/Response Contracts
- Shared DTOs fuer API und potentielle Clients

## Oeffentliche APIs

### GetCvRequest

Laedt den vollstaendigen Lebenslauf:

```csharp
var response = await mediator.Request(new GetCvRequest());
```

### GetCvResponse

Enthaelt alle CV-Daten:
- PersonalData (Name, Kontakt, etc.)
- Education (Schulbildung)
- WorkExperience (Berufserfahrung)
- SkillCategories mit Skills
- Projects (Referenzprojekte)

## Abhaengigkeiten

- Shiny.Mediator.Contracts
