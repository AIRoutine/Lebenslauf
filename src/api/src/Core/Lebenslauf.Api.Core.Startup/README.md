# Lebenslauf.Api.Core.Startup

Zentrales DI-Setup für die API. Orchestriert die Registrierung aller Services und Features.

## Zweck

- Zentrale Anlaufstelle für die Dependency Injection Konfiguration
- Registrierung von Shiny Mediator
- Orchestrierung der Feature-Registrierungen
- Einbindung des Datenzugriffs

## Öffentliche APIs

### ServiceCollectionExtensions

```csharp
services.AddApiServices(configuration);
```

Registriert:
- Shiny Mediator für CQRS-Pattern
- AppDbContext via `AddAppData()`
- Alle Features (Auth, etc.)

## Abhängigkeiten

- `Lebenslauf.Api.Core.Data` - Datenzugriff
- `Lebenslauf.Api.Features.Auth` - Auth Feature
- Shiny.Mediator - Mediator Pattern

## Verwendung

In `Program.cs`:

```csharp
builder.Services.AddApiServices(builder.Configuration);
```

## Neue Features hinzufügen

Bei neuen Features die Registrierung ergänzen:

```csharp
public static IServiceCollection AddApiServices(this IServiceCollection services, IConfiguration configuration)
{
    services.AddShinyMediator();
    services.AddAppData(configuration);

    // Features
    services.AddAuthFeature();
    services.Add{NeuesFeature}Feature();  // Hier hinzufügen

    return services;
}
```
