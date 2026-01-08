# Lebenslauf App - Implementierungsplan

## Ziel

Reprasentations-App fuer deine CV-Daten mit einfacher Aenderbarkeit durch zentrale Backend-Datenhaltung.

---

## Architektur-Entscheidung: Multi-Entity Ansatz

**Gruende:**
1. Erweiterbarkeit - Einzelne Sektionen spaeter editierbar
2. Normalisierung - Skills koennen Projekten zugeordnet werden
3. Query-Flexibilitaet - Nur bestimmte Teile laden
4. Konsistenz - Folgt dem bestehenden Feature-Pattern

---

## Datenmodell

```
+------------------+       +------------------+
|   PersonalData   |       |    Education     |
+------------------+       +------------------+
| Id (Guid)        |       | Id (Guid)        |
| Name             |       | Institution      |
| Email            |       | Degree           |
| Phone            |       | StartYear        |
| Address          |       | EndYear          |
| City             |       | Description      |
| PostalCode       |       | SortOrder        |
| Country          |       +------------------+
| BirthDate        |
| Citizenship      |       +------------------+
| ProfileImageUrl  |       | WorkExperience   |
+------------------+       +------------------+
                           | Id (Guid)        |
+------------------+       | Company          |
|  SkillCategory   |       | Role             |
+------------------+       | StartDate        |
| Id (Guid)        |       | EndDate          |
| Name             |       | Description      |
| SortOrder        |       | IsCurrent        |
+------------------+       | SortOrder        |
        |                  +------------------+
        | 1:n
        v                  +------------------+
+------------------+       |     Project      |
|      Skill       |       +------------------+
+------------------+       | Id (Guid)        |
| Id (Guid)        |       | Name             |
| Name             |       | Description      |
| CategoryId (FK)  |       | Technologies     |
| SortOrder        |       | AppStoreUrl      |
+------------------+       | PlayStoreUrl     |
                           | WebsiteUrl       |
                           | ImageUrl         |
                           | SortOrder        |
                           +------------------+
```

---

## Projektstruktur

### Backend (API)

```
src/api/src/Features/Cv/
|
+-- Lebenslauf.Api.Features.Cv.Contracts/
|   +-- README.md
|   +-- Lebenslauf.Api.Features.Cv.Contracts.csproj
|   +-- Mediator/
|       +-- Requests/
|           +-- GetCvRequest.cs
|           +-- GetCvResponse.cs (embedded DTOs)
|
+-- Lebenslauf.Api.Features.Cv/
    +-- README.md
    +-- Lebenslauf.Api.Features.Cv.csproj
    +-- Configuration/
    |   +-- ServiceCollectionExtensions.cs
    +-- Data/
    |   +-- Entities/
    |   |   +-- PersonalData.cs
    |   |   +-- Education.cs
    |   |   +-- WorkExperience.cs
    |   |   +-- SkillCategory.cs
    |   |   +-- Skill.cs
    |   |   +-- Project.cs
    |   +-- Configurations/
    |   |   +-- PersonalDataConfiguration.cs
    |   |   +-- EducationConfiguration.cs
    |   |   +-- WorkExperienceConfiguration.cs
    |   |   +-- SkillCategoryConfiguration.cs
    |   |   +-- SkillConfiguration.cs
    |   |   +-- ProjectConfiguration.cs
    |   +-- Seeding/
    |       +-- CvSeeder.cs
    +-- Handlers/
        +-- GetCvHandler.cs
```

### Frontend (Uno)

```
src/uno/src/Features/Cv/
|
+-- Lebenslauf.Features.Cv.Contracts/
|   +-- README.md
|   +-- Lebenslauf.Features.Cv.Contracts.csproj
|   +-- (DTOs via OpenAPI generiert)
|
+-- Lebenslauf.Features.Cv/
    +-- README.md
    +-- Lebenslauf.Features.Cv.csproj
    +-- Configuration/
    |   +-- ServiceCollectionExtensions.cs
    +-- Presentation/
        +-- CvPage.xaml
        +-- CvPage.xaml.cs
        +-- CvViewModel.cs
        +-- Sections/
            +-- HeaderSection.xaml(.cs)
            +-- EducationSection.xaml(.cs)
            +-- ExperienceSection.xaml(.cs)
            +-- SkillsSection.xaml(.cs)
            +-- ProjectsSection.xaml(.cs)
```

---

## UI Layout

### Mobile (Narrow)

```
+-------------------------+
|       HEADER            |
| +-----+                 |
| |Foto | Daniel Hufnagl  |
| +-----+ Laakirchen, AT  |
|         +43-664-...     |
|         daniel@...      |
+-------------------------+
|     AUSBILDUNG          |
| o HTL Grieskirchen      |
| |   2012-2017           |
| o VS/HS Laakirchen      |
|     2004-2012           |
+-------------------------+
|     ERFAHRUNG           |
| o Selbstaendig          |
| |   seit 2019           |
| o Skopek/Colop          |
|     2018-2019           |
+-------------------------+
|     SKILLS              |
| [C#] [.NET] [Maui]      |
| [Xamarin] [MVVM] ...    |
+-------------------------+
|     PROJEKTE            |
| +-------+  +-------+    |
| |Orderl.|  | Colop |    |
| +-------+  +-------+    |
+-------------------------+
```

### Desktop (Wide)

```
+----------------------------------------------------------+
|                        HEADER                             |
| +-------+  Daniel Hufnagl                                 |
| | Foto  |  Stockham 44, 4663 Laakirchen                  |
| +-------+  +43-664-73221804 | daniel.hufnagl@aon.at      |
+----------------------------------------------------------+
|     AUSBILDUNG           |        ERFAHRUNG              |
| o HTL Grieskirchen       | o Selbstaendig                |
| |   Informatik 2012-17   | |   seit Nov 2019             |
| o VS/HS Laakirchen       | o Skopek GmbH - Colop         |
|     2004-2012            |     Aug 2018 - Nov 2019       |
+----------------------------------------------------------+
|                        SKILLS                             |
| Expertise: [C#] [.NET] [Maui] [Xamarin] [MVVM] [ASP.NET] |
| Tools:     [VS] [Rider] [Git] [Azure DevOps] [Jira]      |
| AI:        [Claude Code] [Copilot] [Codex]               |
+----------------------------------------------------------+
|                       PROJEKTE                            |
| +----------+  +----------+  +----------+  +----------+   |
| | Orderlyze|  |  Colop   |  |  Sybos   |  |  Miele   |   |
| +----------+  +----------+  +----------+  +----------+   |
+----------------------------------------------------------+
```

---

## Implementierungsschritte

### Phase 1: Backend Setup

```
[1] API Contracts Projekt erstellen
     |
     v
[2] API Feature Projekt erstellen
     |
     v
[3] Entities definieren (6 Stueck)
     |
     v
[4] Entity Configurations erstellen
     |
     v
[5] CvSeeder mit deinen Daten
     |
     v
[6] GetCvHandler implementieren
     |
     v
[7] API Endpoint registrieren
     |
     v
[8] Core.Startup erweitern
```

### Phase 2: Database

```
[9] dotnet ef migrations add InitialCv
     |
     v
[10] dotnet ef database update
     |
     v
[11] API starten und testen
```

### Phase 3: Frontend Setup

```
[12] Uno Contracts Projekt erstellen
     |
     v
[13] Uno Feature Projekt erstellen
     |
     v
[14] CvViewModel implementieren
     |
     v
[15] CvPage.xaml erstellen
```

### Phase 4: UI Sections

```
[16] HeaderSection
     |
     v
[17] EducationSection
     |
     v
[18] ExperienceSection
     |
     v
[19] SkillsSection
     |
     v
[20] ProjectsSection
```

### Phase 5: Integration

```
[21] Navigation Route konfigurieren
     |
     v
[22] Core.Startup erweitern
     |
     v
[23] App.csproj References
     |
     v
[24] Testen und Styling
```

---

## Deine CV-Daten (fuer Seeder)

### PersonalData
- Name: Daniel Hufnagl
- Email: daniel.hufnagl@aon.at
- Phone: +43-664-73221804
- Address: Stockham 44
- City: Laakirchen
- PostalCode: 4663
- Country: Oesterreich
- BirthDate: 1998-08-01
- Citizenship: Oesterreich

### Education
1. HTL Grieskirchen - Informatik (2012-2017)
2. VS/HS Laakirchen (2004-2012)

### WorkExperience
1. Vollzeit Einzelunternehmer (2019-heute)
2. Skopek GmbH - Xamarin Forms bei Colop (2018-2019)
3. DDL GmbH - C# Entwicklung, Wifi Trainer (2017-2018)

### SkillCategories + Skills
- **Expertise:** C#, .NET, Maui, Xamarin Forms, UnoPlatform, ASP.Net, XAML, MVVM
- **Grundlagen:** Xamarin.Android, Xamarin.iOS, Java, Swift, C, C++
- **Frameworks:** ReactiveUI, Prism, Shiny, Syncfusion, SkiaSharp, Lottie, SQLite-net, Refit, ZXing
- **DevOps:** Visual Studio, VS Code, Rider, Git, Azure DevOps, Gitlab, Github, Jira, Bitbucket, Confluence
- **AI Tools:** Claude Code, Codex, Github Copilot

### Projects
1. Orderlyze - Kassensystem App
2. Colop E-Mark - Stempel-Editor
3. Sybos - Feuerwehr-Verwaltung
4. PracticeBird - Notenblatt App
5. Ekey - Fingerscanner/Tuersicherheit
6. Miele - Smart Home Steuerung
7. Asfinag - Verkehrsinfo App
8. Lolyo - Mitarbeiter-Kommunikation
9. Protimer - Zeiterfassung (Migration)

---

## Aenderbarkeit

Die Daten koennen einfach geaendert werden:

1. **Seeder bearbeiten:** `CvSeeder.cs` enthaelt alle Daten
2. **Datenbank loeschen:** `app.db` loeschen, neu starten
3. **Oder:** Spaeter Admin-Panel hinzufuegen

---

## Offene Fragen

Vor der Implementierung zu klaeren:

1. Soll ein Profilbild angezeigt werden? (Asset hinzufuegen?)
2. Sollen Store-Links klickbar sein (Browser oeffnen)?
3. Farbschema: Material Standard oder custom?
4. Mehrsprachig (DE/EN) erforderlich?
