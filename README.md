# Lebenslauf

Interaktiver Online-Lebenslauf von Daniel Hufnagl, entwickelt mit Uno Platform (WebAssembly) und ASP.NET Core Backend.

## Live Demo

**Website:** [www.dotnetmaui.de](https://www.dotnetmaui.de)

## Architektur

Das Projekt verwendet eine Frontend-Backend Split-Architektur:

| Komponente | Technologie | Deployment |
|------------|-------------|------------|
| Frontend | Uno Platform (WebAssembly) | Azure Static Web Apps |
| Backend | ASP.NET Core Minimal API | Azure App Service |
| Datenbank | SQLite | App Service (lokal) |

## Tech Stack

### Frontend (Uno Platform)
- .NET 10 / C#
- Uno Platform WebAssembly
- XAML UI
- Shiny.Mediator (HTTP Client)
- Material Design

### Backend (ASP.NET Core)
- .NET 10 / C#
- Minimal APIs
- Entity Framework Core
- SQLite Datenbank
- OpenAPI / Swagger
- Shiny.Mediator

## Deployment

### Frontend (WASM)
- **Platform:** Azure Static Web Apps
- **Domain:** www.dotnetmaui.de
- **CI/CD:** GitHub Actions (`.github/workflows/deploy-azure-swa.yml`)
- **Trigger:** Push auf `main` Branch

### Backend (API)
- **Platform:** Azure App Service
- **URL:** lebenslauf-api.azurewebsites.net
- **OpenAPI:** `/openapi/v1.json`

## Lokale Entwicklung

### Voraussetzungen
- .NET 10 SDK
- Visual Studio 2022+ oder Rider

### API starten
```bash
cd src/api/src/Lebenslauf.Api
dotnet run
```
API laeuft auf `http://localhost:5292`

### Frontend starten (mit .NET Aspire)
```bash
cd src/aspire/src/Lebenslauf.AppHost
dotnet run
```

### Datenbank zuruecksetzen
Die SQLite-Datenbank (`app.db`) wird beim API-Start automatisch erstellt und mit Seed-Daten befuellt.
Um die Datenbank zurueckzusetzen:
1. API stoppen
2. `app.db` loeschen
3. API neu starten

## Projektstruktur

```
Lebenslauf/
├── src/
│   ├── api/                    # Backend (ASP.NET Core)
│   │   └── src/
│   │       ├── Lebenslauf.Api/           # Host
│   │       ├── Core/                      # Shared Infrastructure
│   │       └── Features/Cv/               # CV Feature
│   │
│   ├── aspire/                 # .NET Aspire Orchestrierung
│   │
│   └── uno/                    # Frontend (Uno Platform)
│       └── src/
│           ├── Lebenslauf.App/           # Hauptprojekt
│           ├── Core/                      # Shared Infrastructure
│           └── Features/Cv/               # CV Feature
│
└── subm/                       # Git Submodules
```

## Features

- Multi-Profil Support (Default, Backend, Mobile)
- Responsive Design
- Dark/Light Mode
- Projekt-Portfolio mit Details
- Skills nach Kategorien
- Ausbildung und Berufserfahrung
- Kontaktinformationen

## Lizenz

Privates Projekt - Alle Rechte vorbehalten.
