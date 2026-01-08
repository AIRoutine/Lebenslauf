# Lebenslauf.Core.ApiClient

API-Client-Bibliothek für die Kommunikation mit dem Backend via Shiny.Mediator HTTP Extension.

## Zweck

Dieses Projekt generiert automatisch HTTP-Contracts aus der OpenAPI-Spezifikation des Backends und stellt diese für alle Uno-Features bereit.

## Funktionsweise

### OpenAPI Contract Generation

Die Contracts werden zur Build-Zeit aus der API-Spezifikation generiert:

```xml
<MediatorHttp Include="ApiContracts"
              Uri="http://localhost:5292/openapi/v1.json"
              Namespace="Lebenslauf.Core.ApiClient.Generated"
              ContractPostfix="HttpRequest"
              GenerateJsonConverters="true"
              Visible="false" />
```

### Verwendung in Features

```csharp
// Inject IMediator
public class MyViewModel(IMediator mediator)
{
    public async Task LoadData()
    {
        var response = await mediator.Request(new GetDataHttpRequest());
    }
}
```

### Konfiguration

Base-URL in `appsettings.json`:

```json
{
  "Mediator": {
    "Http": {
      "Lebenslauf.Core.ApiClient.Generated.*": "http://localhost:5292"
    }
  }
}
```

## Abhängigkeiten

- `Shiny.Mediator` - Mediator Pattern Implementation
- `Shiny.Mediator.Http` - HTTP Extension für automatische Request-Handling

## Referenzen

- [Shiny.Mediator HTTP Extension](https://shinylib.net/mediator/extensions/http/)
- [Shiny.Mediator GitHub](https://github.com/shinyorg/mediator)
