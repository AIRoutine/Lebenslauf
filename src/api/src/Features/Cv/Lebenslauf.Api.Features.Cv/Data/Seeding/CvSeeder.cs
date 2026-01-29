using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Core.Data.Seeding;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lebenslauf.Api.Features.Cv.Data.Seeding;

public class CvSeeder(AppDbContext dbContext) : ISeeder
{
    public int Order => 10;

    // Store skill IDs for profile linking
    private readonly Dictionary<string, Guid> _skillIds = new();
    // Store project IDs for profile linking
    private readonly Dictionary<string, Guid> _projectIds = new();
    // Store work experience IDs for profile linking
    private readonly Dictionary<string, Guid> _workExperienceIds = new();

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if already seeded
        if (await dbContext.Set<PersonalData>().AnyAsync(cancellationToken))
            return;

        await SeedPersonalDataAsync(cancellationToken);
        await SeedFamilyMembersAsync(cancellationToken);
        await SeedEducationAsync(cancellationToken);
        await SeedInternshipsAsync(cancellationToken);
        await SeedWorkExperienceAsync(cancellationToken);
        await SeedSkillsAsync(cancellationToken);
        await SeedProjectsAsync(cancellationToken);

        // Save first to get IDs, then add profile links
        await dbContext.SaveChangesAsync(cancellationToken);

        await SeedProfileSkillsAsync(cancellationToken);
        await SeedProfileProjectsAsync(cancellationToken);
        await SeedProfileWorkExperiencesAsync(cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPersonalDataAsync(CancellationToken cancellationToken)
    {
        // Create PersonalData for each profile with customized titles
        var personalDataList = new List<PersonalData>
        {
            new()
            {
                Id = Guid.NewGuid(),
                ProfileId = ProfileSeeder.DefaultProfileId,
                AcademicTitle = "Ing.",
                Name = "Daniel Hufnagl",
                Title = "Senior Cross-Platform Developer",
                Email = "d.hufnagl@codelisk.com",
                Phone = "+43-664-73221804",
                Address = "Stockham 44",
                City = "Laakirchen",
                PostalCode = "4663",
                Country = "Oesterreich",
                BirthDate = new DateOnly(1998, 8, 1),
                Citizenship = "Oesterreich",
                ProfileImageUrl = null
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProfileId = ProfileSeeder.BackendProfileId,
                AcademicTitle = "Ing.",
                Name = "Daniel Hufnagl",
                Title = "Senior .NET Backend Developer",
                Email = "d.hufnagl@codelisk.com",
                Phone = "+43-664-73221804",
                Address = "Stockham 44",
                City = "Laakirchen",
                PostalCode = "4663",
                Country = "Oesterreich",
                BirthDate = new DateOnly(1998, 8, 1),
                Citizenship = "Oesterreich",
                ProfileImageUrl = null
            },
            new()
            {
                Id = Guid.NewGuid(),
                ProfileId = ProfileSeeder.MobileProfileId,
                AcademicTitle = "Ing.",
                Name = "Daniel Hufnagl",
                Title = "Senior Mobile App Developer",
                Email = "d.hufnagl@codelisk.com",
                Phone = "+43-664-73221804",
                Address = "Stockham 44",
                City = "Laakirchen",
                PostalCode = "4663",
                Country = "Oesterreich",
                BirthDate = new DateOnly(1998, 8, 1),
                Citizenship = "Oesterreich",
                ProfileImageUrl = null
            }
        };

        await dbContext.Set<PersonalData>().AddRangeAsync(personalDataList, cancellationToken);
    }

    private async Task SeedFamilyMembersAsync(CancellationToken cancellationToken)
    {
        var familyMembers = new List<FamilyMember>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Relationship = "Vater",
                Profession = "Elektroinstallateur",
                BirthYear = null,
                SortOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Relationship = "Mutter",
                Profession = "Vertriebsassistenz",
                BirthYear = null,
                SortOrder = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                Relationship = "Bruder",
                Profession = "Mechatroniker",
                BirthYear = 2000,
                SortOrder = 3
            },
            new()
            {
                Id = Guid.NewGuid(),
                Relationship = "Bruder",
                Profession = "NMS Schueler",
                BirthYear = 2007,
                SortOrder = 4
            }
        };

        await dbContext.Set<FamilyMember>().AddRangeAsync(familyMembers, cancellationToken);
    }

    private async Task SeedEducationAsync(CancellationToken cancellationToken)
    {
        var education = new List<Education>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Institution = "HTL Grieskirchen",
                Degree = "Informatik",
                StartYear = 2012,
                EndYear = 2017,
                Description = "Diplomarbeit: Verwaltungssystem fuer Aerzte und Mitarbeiter inkl. IP-TV Anzeige fuer Kurheim Bad Schallerbach.",
                SortOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Institution = "VS, HS Laakirchen",
                Degree = "Volksschule und Hauptschule",
                StartYear = 2004,
                EndYear = 2012,
                Description = null,
                SortOrder = 2
            }
        };

        await dbContext.Set<Education>().AddRangeAsync(education, cancellationToken);
    }

    private async Task SeedInternshipsAsync(CancellationToken cancellationToken)
    {
        var internships = new List<Internship>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Company = "Firma Stoeber",
                Role = "Programmierpraktikum",
                Year = 2014,
                Month = 7,
                EndMonth = 7,
                Description = "Programmierarbeiten",
                SortOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Company = "Elektro Steinschaden",
                Role = "Elektrotechnik Praktikum",
                Year = 2015,
                Month = 7,
                EndMonth = 8,
                Description = "Elektrotechnische Arbeiten",
                SortOrder = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                Company = "HTL Grieskirchen",
                Role = "Reife- und Diplompruefung",
                Year = 2017,
                Month = 6,
                EndMonth = 6,
                Description = "Abschluss Informatik",
                SortOrder = 3
            }
        };

        await dbContext.Set<Internship>().AddRangeAsync(internships, cancellationToken);
    }

    private async Task SeedWorkExperienceAsync(CancellationToken cancellationToken)
    {
        // Helper to create work experience and store its ID
        WorkExperience CreateWorkExp(string company, string role, DateOnly startDate, DateOnly? endDate,
            bool isCurrent, string description, int sortOrder)
        {
            var id = Guid.NewGuid();
            _workExperienceIds[company] = id;
            return new WorkExperience
            {
                Id = id,
                Company = company,
                Role = role,
                StartDate = startDate,
                EndDate = endDate,
                IsCurrent = isCurrent,
                Description = description,
                SortOrder = sortOrder
            };
        }

        var workExperience = new List<WorkExperience>
        {
            CreateWorkExp(
                company: "Selbstaendig",
                role: "Vollzeit Einzelunternehmer",
                startDate: new DateOnly(2019, 11, 30),
                endDate: null,
                isCurrent: true,
                description: "Cross-Platform App-Entwicklung mit Xamarin Forms, .NET MAUI und Uno Platform. Kunden: Miele, Asfinag, Ekey, und weitere.",
                sortOrder: 1),
            CreateWorkExp(
                company: "Skopek GmbH & CO KG",
                role: "Xamarin Forms Entwickler bei Colop Stempelerzeugung",
                startDate: new DateOnly(2018, 8, 20),
                endDate: new DateOnly(2019, 11, 30),
                isCurrent: false,
                description: "Entwicklung der Colop E-Mark App mit eigenem Editor fuer mehrfarbige Abdrucke.",
                sortOrder: 2),
            CreateWorkExp(
                company: "DDL GmbH",
                role: "C# Entwickler & Wifi Trainer",
                startDate: new DateOnly(2017, 11, 27),
                endDate: new DateOnly(2018, 7, 15),
                isCurrent: false,
                description: "Datenverwaltung von Ver- und Entsorgungsnetzen, Programmieren von C# Anwendungen, Wifi Trainer fuer C# (Gruppen von 20-30 Personen).",
                sortOrder: 3)
        };

        await dbContext.Set<WorkExperience>().AddRangeAsync(workExperience, cancellationToken);
    }

    private async Task SeedSkillsAsync(CancellationToken cancellationToken)
    {
        // Helper to create skill and store its ID
        Skill CreateSkill(string name, int sortOrder)
        {
            var id = Guid.NewGuid();
            _skillIds[name] = id;
            return new Skill { Id = id, Name = name, SortOrder = sortOrder };
        }

        // 1. Expertise Category
        var expertiseCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Expertise",
            SortOrder = 1,
            Skills =
            [
                CreateSkill("C#", 1),
                CreateSkill(".NET", 2),
                CreateSkill("MAUI", 3),
                CreateSkill("Xamarin Forms", 4),
                CreateSkill("Uno Platform", 5),
                CreateSkill("ASP.NET Core", 6),
                CreateSkill("Entity Framework Core", 7),
                CreateSkill("XAML", 8),
                CreateSkill("MVVM Pattern", 9),
                CreateSkill("REST API Design", 10)
            ]
        };

        // 2. Basic Knowledge Category
        var basicCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Grundkenntnisse",
            SortOrder = 2,
            Skills =
            [
                CreateSkill("Xamarin.Android", 1),
                CreateSkill("Xamarin.iOS", 2),
                CreateSkill("Java", 3),
                CreateSkill("Swift", 4),
                CreateSkill("C", 5),
                CreateSkill("C++", 6)
            ]
        };

        // 3. App Base Technologies Category
        var appBaseCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "App Basistechnologien",
            SortOrder = 3,
            Skills =
            [
                CreateSkill("MAUI/Uno Platform als Hauptframework", 1),
                CreateSkill("XAML fuer das Design", 2),
                CreateSkill("Prism als MVVM Framework", 3),
                CreateSkill("ReactiveUI fuer Statusveraenderungen", 4),
                CreateSkill("Shiny Framework", 5),
                CreateSkill("Shiny Mediator Pattern", 6)
            ]
        };

        // 4. DevOps & Tools Category
        var devOpsCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "DevOps & Tools",
            SortOrder = 4,
            Skills =
            [
                CreateSkill("Visual Studio", 1),
                CreateSkill("Visual Studio Code", 2),
                CreateSkill("Rider", 3),
                CreateSkill("Git", 4),
                CreateSkill("Microsoft Azure DevOps", 5),
                CreateSkill("GitLab", 6),
                CreateSkill("GitHub", 7),
                CreateSkill("Jira", 8),
                CreateSkill("Bitbucket", 9),
                CreateSkill("Confluence", 10)
            ]
        };

        // 5. AI Tools Category
        var aiCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "AI Tools",
            SortOrder = 5,
            Skills =
            [
                CreateSkill("Claude Code", 1),
                CreateSkill("Codex", 2),
                CreateSkill("GitHub Copilot", 3)
            ]
        };

        // 6. Frameworks & Libraries Category
        var frameworksCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Frameworks & Libraries",
            SortOrder = 6,
            Skills =
            [
                CreateSkill("MAUI Essentials", 1),
                CreateSkill("MAUI Community Toolkit", 2),
                CreateSkill("Syncfusion", 3),
                CreateSkill("SignalR", 4),
                CreateSkill("SQLite-net", 5),
                CreateSkill("ReactiveUI", 6),
                CreateSkill("Refit REST Library", 7),
                CreateSkill("Prism", 8),
                CreateSkill("Shiny", 9),
                CreateSkill("Shiny Mediator", 10),
                CreateSkill("SharpNado", 11),
                CreateSkill("SkiaSharp", 12),
                CreateSkill("Lottie", 13),
                CreateSkill("ZXing", 14),
                CreateSkill("BarcodeNative", 15),
                CreateSkill("Cake Build", 16),
                CreateSkill("Polly (Resilience)", 17),
                CreateSkill("Swagger/OpenAPI", 18),
                CreateSkill("ClosedXML", 19),
                CreateSkill("Stimulsoft Reports", 20)
            ]
        };

        // 7. Database & Backend Category
        var databaseCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Datenbanken & Backend",
            SortOrder = 7,
            Skills =
            [
                CreateSkill("PostgreSQL", 1),
                CreateSkill("SQLite", 2),
                CreateSkill("SQL Server", 3),
                CreateSkill("JWT Authentication", 4),
                CreateSkill("Multi-Tenant Architektur", 5),
                CreateSkill("Repository Pattern", 6),
                CreateSkill("Code Generation", 7)
            ]
        };

        // 8. App Development Experience Category
        var appDevCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "App Entwicklungserfahrung",
            SortOrder = 8,
            Skills =
            [
                CreateSkill("Bluetooth Drucker Anbindung", 1),
                CreateSkill("QR-Code Scanner Anbindung", 2),
                CreateSkill("Push Notifications (Cross Platform)", 3),
                CreateSkill("REST Service Kommunikation", 4),
                CreateSkill("Lokale Datenbank in App", 5),
                CreateSkill("Raspberry Pi Kommunikation", 6),
                CreateSkill("Eigener Editor", 7),
                CreateSkill("Google Maps Einbindung", 8),
                CreateSkill("Dark Mode Design", 9),
                CreateSkill("Kontaktverwaltung", 10),
                CreateSkill("WebView Integration", 11),
                CreateSkill("Token Authentifizierung", 12),
                CreateSkill("Google/Facebook Login", 13),
                CreateSkill("Kalender Implementierung", 14),
                CreateSkill("Caching mit Sync (Request Pattern)", 15),
                CreateSkill("App Store Verwaltung (Play Store, App Store, Huawei AppGallery)", 16)
            ]
        };

        await dbContext.Set<SkillCategory>().AddRangeAsync(
            [expertiseCategory, basicCategory, appBaseCategory, devOpsCategory, aiCategory, frameworksCategory, databaseCategory, appDevCategory],
            cancellationToken);
    }

    private async Task SeedProjectsAsync(CancellationToken cancellationToken)
    {
        var projects = new List<Project>
        {
            CreateProjectWithId(
                name: "Orderlyze",
                description: "Kassensystem App: Rechnungen erstellen, Tischplan mit Drag & Drop, Reservierungen, Berichte, Mitarbeiterverwaltung, Statistiken, Multi-Device Sync.",
                framework: "C# / MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/orderlyze/id1495015799",
                playStoreUrl: "https://play.google.com/store/apps/details?id=orderlyze.com",
                appGalleryUrl: "https://appgallery.huawei.com/app/C103291527",
                websiteUrl: "https://www.orderlyze.com",
                sortOrder: 1,
                startDate: new DateOnly(2018, 1, 1),
                endDate: null,
                isCurrent: true,
                technologies: ["Xamarin.Forms", "MAUI", "C#", "Prism", "ReactiveUI", "Syncfusion", "SQLite", "Shiny.BLE"],
                functions: [
                    "Rechnungen erstellen und ausdrucken",
                    "Tischplan mit Drag und Drop selber gestalten",
                    "Reservierungen erstellen",
                    "Tages-, Monats- und Jahresberichte",
                    "Mitarbeiterverwaltung",
                    "Rechnungen verwalten",
                    "Statistiken und Analysen",
                    "Produkte und vieles mehr verwalten",
                    "Infrastruktur bearbeiten",
                    "Stammkunden",
                    "Synchronisation zwischen mehreren Geraeten"
                ],
                technicalAspects: [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "State Changes: ReactiveUI",
                    "Kommunikation mit dem Server: OpenApi Generierung",
                    "Bluetooth Module: Shiny.BLE",
                    "Datenbank: sqlite-net (Async Pattern)",
                    "UI Komponenten: Syncfusion"
                ],
                subProjects: [
                    new SubProjectData(
                        Name: "REST API",
                        Description: "Backend-Service fuer Datensynchronisation und Cloud-Funktionen",
                        Framework: ".NET 10",
                        Technologies: ["ASP.NET Core", "Entity Framework", "OpenAPI", "SQLite"]),
                    new SubProjectData(
                        Name: "Printing Service",
                        Description: "Druckserver fuer Bon- und Etikettendrucker",
                        Framework: ".NET 10",
                        Technologies: ["ZPL/EPL", "ESC/POS", "USB/Network"]),
                    new SubProjectData(
                        Name: "MCP Server",
                        Description: "AI Support Chatbot mit Dokumentationssuche",
                        Framework: ".NET 10",
                        Technologies: ["MCP Protocol", "Docker", "Semantic Search"]),
                    new SubProjectData(
                        Name: "Documentation",
                        Description: "Benutzerhandbuch und technische Dokumentation",
                        Framework: "MkDocs",
                        Technologies: ["Material Theme", "Markdown", "GitHub Pages"])
                ]),

            CreateProjectWithId(
                name: "Colop E-Mark",
                description: "Stempel-Editor App: Eigener Editor fuer mehrfarbige Abdrucke, QR/Barcode Generator, WLAN-Druckerverbindung, mehrsprachig (7 Sprachen).",
                framework: "C# / Xamarin.Forms",
                appStoreUrl: "https://apps.apple.com/at/app/colop-e-mark/id1397292575",
                playStoreUrl: "https://play.google.com/store/apps/details?id=com.colop.colopemark",
                appGalleryUrl: null,
                websiteUrl: "https://www.colop.com/de/digital-produkte",
                sortOrder: 2,
                startDate: new DateOnly(2019, 1, 1),
                endDate: new DateOnly(2020, 3, 1),
                isCurrent: false,
                technologies: ["Xamarin.Forms", "C#", "Prism", "SkiaSharp", "ZXing", "SQLite"],
                functions: [
                    "Eigener Editor um vielseitige mehrfarbige Abdruecke selbst zu gestalten",
                    "Generieren von QR und Barcodes in der App",
                    "Eigenes Benutzersystem",
                    "Wizard mit Video-Einfuehrung",
                    "Verbindung mit mobilen Drucker ueber WLAN",
                    "Mehrsprachig (DE, EN, ES, FR, IT, SE, PT)"
                ],
                technicalAspects: [
                    "Programmiersprachen: C#, XAML",
                    "Haupt-Framework: Xamarin.Forms",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "Implementierung Editor: SkiaSharp",
                    "Implementierung QR-Code Scanner: ZXing",
                    "Datenbank: SQLite .NET (Async Pattern)"
                ],
                subProjects: [
                    new SubProjectData(
                        Name: "WPF Desktop Designer",
                        Description: "Desktop-Anwendung mit eigenem Stempel-Editor und WLAN-Verbindung zum E-Mark",
                        Framework: "WPF",
                        Technologies: ["SkiaSharp", "MVVM", "WLAN/TCP"])
                ]),

            CreateProjectWithId(
                name: "Sybos",
                description: "Feuerwehr-Verwaltung: Chat mit Push-Notifications, QR-Code Scanner, Kalender, Zusatzalarmierung, Materialverwaltung, Server-Synchronisation.",
                framework: "C# / MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/sybos/id1176062382",
                playStoreUrl: "https://play.google.com/store/apps/details?id=at.syPhone",
                appGalleryUrl: null,
                websiteUrl: "https://www.sybos.net",
                sortOrder: 3,
                startDate: new DateOnly(2020, 9, 1),
                endDate: new DateOnly(2023, 6, 1),
                isCurrent: false,
                technologies: ["Xamarin.Forms", "MAUI", "C#", "Prism", "ReactiveUI", "Refit", "ZXing", "Shiny"],
                functions: [
                    "Chat fuer Feuerwehrmitglieder mit Push-Notifications",
                    "QR-Code Scanner um QR-Code von Material zu scannen",
                    "Eigener Kalender fuer Veranstaltungen",
                    "Zusatzalarmierung mit Push Notifications",
                    "Dark Mode",
                    "Ansicht der Materialverwaltung",
                    "Implementierung der Synchronisation mit dem Server",
                    "Implementierung einer lokalen Datenbank"
                ],
                technicalAspects: [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "State Changes: ReactiveUI",
                    "Kommunikation mit dem Server: Refit REST library for .NET",
                    "Implementierung QR-Code Scanner: ZXing",
                    "Datenbank: SQLite .NET (Async Pattern)",
                    "Push Notifications: Cross Platform mit Shiny Library"
                ]),

            CreateProjectWithId(
                name: "Ekey Bionyx",
                description: "Fingerscanner App: Push-Nachrichten, hohe Sicherheit mit Verschluesselung, Bluetooth-Anbindung zu Tuerschloessern, umfangreiches Benutzermanagement.",
                framework: "C# / Xamarin.Forms",
                appStoreUrl: "https://apps.apple.com/at/app/ekey-bionyx/id1484053054",
                playStoreUrl: "https://play.google.com/store/apps/details?id=net.ekey.bionyx",
                appGalleryUrl: null,
                websiteUrl: "https://www.ekey.net/ekey-bionyx-app/",
                sortOrder: 4,
                startDate: new DateOnly(2021, 8, 1),
                endDate: new DateOnly(2022, 2, 1),
                isCurrent: false,
                technologies: ["Xamarin.Forms", "C#", "Prism", "Shiny", "Azure Notification Hub", "Lottie"],
                functions: [
                    "Automatische Push-Nachrichten",
                    "Hoechste Sicherheit ueber Verschluesselung",
                    "Bluetooth Anbindung zu Fingerscanner an Tuer",
                    "Eigenes umfangreiches Benutzermanagement und Rollensystem"
                ],
                technicalAspects: [
                    "Programmiersprache: C#, .NET, XAML",
                    "Framework: Xamarin.Forms",
                    "Repository Verwaltung: Azure DevOps",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "Push Nachrichten: Azure Notification Hub, Apple, Firebase, Shiny",
                    "CI/CD: Azure DevOps",
                    "Animationen: Lottie"
                ]),

            CreateProjectWithId(
                name: "Miele Smart Home",
                description: "Smart Home App: Hausgeraete mobil steuern, Push-Nachrichten ueber Geraetestatus, verschiedene Assistenten als Nuget-Module, eigener Shop.",
                framework: "C# / MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/miele-app-smart-home/id930406907",
                playStoreUrl: "https://play.google.com/store/apps/details?id=de.miele.infocontrol",
                appGalleryUrl: null,
                websiteUrl: "https://www.miele.at/c/miele-app-2594.htm",
                sortOrder: 5,
                startDate: new DateOnly(2022, 2, 1),
                endDate: new DateOnly(2024, 12, 1),
                isCurrent: false,
                technologies: ["MAUI", "C#", "MVVM", "Azure Notification Hub", "Lottie"],
                functions: [
                    "Hausgeraete mobil steuern: Bedienen Sie Ihre Hausgeraete bequem per App",
                    "Push Nachrichten ueber Status der Geraete",
                    "Unterschiedliche Assistenten in Form von eigenem Nugetpaket-System",
                    "Eigener Shop"
                ],
                technicalAspects: [
                    "Programmiersprache: C#, .NET, XAML",
                    "Framework: MAUI",
                    "Repository Verwaltung: Bitbucket",
                    "App Lifecycle: Eigenentwicklung",
                    "Design Pattern: MVVM",
                    "Push Nachrichten: Azure Notification Hub",
                    "CI/CD: Bitbucket",
                    "Animationen: Lottie"
                ]),

            CreateProjectWithId(
                name: "Asfinag",
                description: "Verkehrsinfo App: Personalisierter Homescreen, 1800+ Webcams, Verkehrsinfos & Baustellen, E-Ladestationen, Digitale Vignette, Routenplaner, 12 Sprachen.",
                framework: "C# / MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/asfinag/id453459323",
                playStoreUrl: "https://play.google.com/store/apps/details?id=at.asfinag.unterwegs",
                appGalleryUrl: null,
                websiteUrl: "https://www.asfinag.at/asfinag-app/",
                sortOrder: 6,
                startDate: new DateOnly(2023, 1, 1),
                endDate: new DateOnly(2024, 12, 1),
                isCurrent: false,
                technologies: ["MAUI", "C#", "Prism", "OpenAPI", "Polly", "Scalar", "Syncfusion", "SQLite"],
                functions: [
                    "Personalisierter Homescreen",
                    "Ueber 1800 Webcams + Nachbarlaender",
                    "Verkehrsinfos und Baustellen",
                    "Raststationen und Rastplaetze",
                    "E-Ladestationen (E-Control)",
                    "Digitale Vignette und Streckenmaut, GO-Selfcare",
                    "Routenplaner (Europarouting)",
                    "ASFINAG News",
                    "12 Sprachen",
                    "24/7 Service-Center"
                ],
                technicalAspects: [
                    "Framework: MAUI",
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "Kommunikation mit dem Server: OpenApi, Polly, Scalar",
                    "Syncfusion Bibliothek",
                    "Datenbank: SQLite .NET (Async Pattern)",
                    "Push Notifications: Cross Platform"
                ]),

            CreateProjectWithId(
                name: "Communalaudit",
                description: "Enterprise REST API fuer oesterreichisches Kommunalaudit-System: Audit-Management fuer Gemeinden, Integration mit Austrian Statistics (Statcube), hierarchische Report-Generierung, Multi-Tenant Architektur mit Mandantentrennung.",
                framework: "C# / ASP.NET Core",
                appStoreUrl: null,
                playStoreUrl: null,
                appGalleryUrl: null,
                websiteUrl: "https://www.communalaudit.at/",
                sortOrder: 7,
                startDate: new DateOnly(2024, 7, 1),
                endDate: new DateOnly(2025, 5, 1),
                isCurrent: false,
                technologies: [".NET 8.0", "ASP.NET Core", "Entity Framework Core", "PostgreSQL", "Shiny.Mediator", "JWT Authentication", "Swagger/OpenAPI", "Stimulsoft Reports", "Refit", "Polly", "ClosedXML", "CsvHelper"],
                functions: [
                    "REST API fuer Audit-Verwaltung und -Durchfuehrung",
                    "Integration mit Austrian Statistics (Statcube API) fuer statistische Daten",
                    "Multi-Tenant Architektur mit Mandantentrennung pro Gemeinde",
                    "Hierarchische Audit-Reports mit konfigurierbaren Formeln",
                    "Dashboard-Visualisierungen mit verschiedenen Chart-Typen",
                    "Excel- und CSV-Import/Export fuer Massendaten",
                    "Report-Generierung mit Stimulsoft Reports Engine",
                    "JWT-basierte Authentifizierung mit Rollensystem",
                    "Automatische Code-Generierung fuer CRUD-Operationen"
                ],
                technicalAspects: [
                    "Framework: ASP.NET Core 8.0.5 mit .NET 8.0 LTS",
                    "Datenbank: PostgreSQL mit Entity Framework Core 8.0",
                    "Architektur: Repository Pattern + Manager Pattern (Business Logic Layer)",
                    "Mediator: Shiny.Mediator fuer Request/Response Handling",
                    "Authentifizierung: JWT Bearer Token + ASP.NET Identity",
                    "API-Dokumentation: Swagger/OpenAPI mit Swashbuckle",
                    "Resilience: Polly fuer Retry-Policies und Circuit Breaker",
                    "Code Generation: Codelisk Framework fuer Controller, Manager, Repositories",
                    "Testing: XUnit mit AutoFixture und In-Memory Database"
                ],
                subProjects: [
                    new SubProjectData(
                        Name: "Communalaudit Report Library",
                        Description: "Domain Model Library fuer hierarchische Audit-Reports mit Questionnaire-Support und Chart-Konfiguration",
                        Framework: ".NET 8.0",
                        Technologies: ["Domain-Driven Design", "Nullable Reference Types", "Hierarchical Data Model"]),
                    new SubProjectData(
                        Name: "Statcube API Client",
                        Description: "Type-safe REST Client fuer Austrian Statistics API mit Resilience Patterns",
                        Framework: ".NET 8.0",
                        Technologies: ["Refit", "Polly", "Dependency Injection"]),
                    new SubProjectData(
                        Name: "Code Generation Framework",
                        Description: "Source Generators fuer automatische Erstellung von Controllern, Managern und Repositories",
                        Framework: ".NET 8.0",
                        Technologies: ["Roslyn Source Generators", "T4 Templates", "Reflection"])
                ]),

            CreateProjectWithId(
                name: "Red Bull ReMa",
                description: "iPad-basierte Lagerverwaltungs-App für Red Bull Event-Logistik mit RFID-Integration, Packlisten-Management, Retourenerfassung und Schadensmeldung.",
                framework: "C# / .NET MAUI",
                appStoreUrl: null,
                playStoreUrl: null,
                appGalleryUrl: null,
                websiteUrl: "https://sofa1.at/en/project/redbull-en/",
                sortOrder: 8,
                startDate: new DateOnly(2025, 6, 1),
                endDate: null,
                isCurrent: true,
                technologies: [".NET MAUI", "C#", "Azure Application Insights", "SignalR", "SQLite", "Zebra SDK", "RFID"],
                functions: [
                    "Packlisten-Management fuer Auftragskonfektionierung",
                    "Retourenerfassung und Ruecklieferungsverfolgung",
                    "Schadensmeldung mit Dokumentation",
                    "RFID-Scanning fuer automatische Artikelerfassung",
                    "Echtzeit-Bestandsuebersicht",
                    "Seriennummernverwaltung mit Etikettendruck und RFID-Codierung",
                    "Integration mit Web-Portal (Back Office)"
                ],
                technicalAspects: [
                    "Framework: .NET MAUI",
                    "Programmiersprachen: C#, XAML",
                    "Logging: Azure Application Insights",
                    "Barcode Scanner: Zebra SDK",
                    "RFID Integration: 7iD Technologies",
                    "Echtzeit-Updates: SignalR",
                    "Datenbank: SQLite .NET (Async Pattern)",
                    "DI: Constructor Injection Pattern"
                ],
                subProjects: [
                    new SubProjectData(
                        Name: "cs.m.core.ui",
                        Description: "Wiederverwendbare UI-Komponenten Bibliothek fuer SOFA1 MAUI Anwendungen",
                        Framework: ".NET MAUI",
                        Technologies: ["Custom Controls", "NuGet Packaging", "Azure DevOps CI/CD"])
                ]),

            CreateProjectWithId(
                name: "PracticeBird",
                description: "Notenblatt App: Mitspielendes Notenblatt, MusicXML/MIDI Import, Playback mit Klavier/Schlagzeug, Stimmenhervorhebung, Social Login.",
                framework: "C# / Xamarin.Forms",
                appStoreUrl: "https://apps.apple.com/us/app/practice-bird-interactive-sheet-music-and-scores/id1253492926",
                playStoreUrl: "https://play.google.com/store/apps/details?id=phonicscore.phonicscore_lite",
                appGalleryUrl: null,
                websiteUrl: null,
                sortOrder: 9,
                startDate: new DateOnly(2020, 3, 1),
                endDate: new DateOnly(2020, 12, 1),
                isCurrent: false,
                technologies: ["Xamarin.Forms", "C#", "Prism", "ReactiveUI", "SkiaSharp", "Refit"],
                functions: [
                    "Eigenes mitspielendes Notenblatt",
                    "Hinzufuegen und Verwalten von eigenen MusicXML oder MIDI Dateien",
                    "Playback Notenblatt abspielen mit Klavier und Schlagzeug Klaengen",
                    "Hervorheben von eigener Stimme auf Notenblatt",
                    "Login mit Facebook oder Google-Konto"
                ],
                technicalAspects: [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "State Changes: ReactiveUI",
                    "Kommunikation mit dem Server: Refit REST library for .NET",
                    "Notenblatt: SkiaSharp"
                ])
        };

        await dbContext.Set<Project>().AddRangeAsync(projects, cancellationToken);
    }

    private Project CreateProjectWithId(
        string name,
        string description,
        string framework,
        string? appStoreUrl,
        string? playStoreUrl,
        string? appGalleryUrl,
        string? websiteUrl,
        int sortOrder,
        DateOnly startDate,
        DateOnly? endDate,
        bool isCurrent,
        string[] technologies,
        string[] functions,
        string[] technicalAspects,
        SubProjectData[]? subProjects = null)
    {
        var id = Guid.NewGuid();
        _projectIds[name] = id;

        var project = new Project
        {
            Id = id,
            Name = name,
            Description = description,
            Framework = framework,
            AppStoreUrl = appStoreUrl,
            PlayStoreUrl = playStoreUrl,
            AppGalleryUrl = appGalleryUrl,
            WebsiteUrl = websiteUrl,
            SortOrder = sortOrder,
            StartDate = startDate,
            EndDate = endDate,
            IsCurrent = isCurrent
        };

        for (var i = 0; i < technologies.Length; i++)
        {
            project.Technologies.Add(new ProjectTechnology
            {
                Id = Guid.NewGuid(),
                Name = technologies[i],
                SortOrder = i + 1
            });
        }

        for (var i = 0; i < functions.Length; i++)
        {
            project.Functions.Add(new ProjectFunction
            {
                Id = Guid.NewGuid(),
                Description = functions[i],
                SortOrder = i + 1
            });
        }

        for (var i = 0; i < technicalAspects.Length; i++)
        {
            project.TechnicalAspects.Add(new ProjectTechnicalAspect
            {
                Id = Guid.NewGuid(),
                Description = technicalAspects[i],
                SortOrder = i + 1
            });
        }

        if (subProjects is not null)
        {
            for (var i = 0; i < subProjects.Length; i++)
            {
                var sp = subProjects[i];
                var subProject = new ProjectSubProject
                {
                    Id = Guid.NewGuid(),
                    Name = sp.Name,
                    Description = sp.Description,
                    Framework = sp.Framework,
                    SortOrder = i + 1
                };

                for (var j = 0; j < sp.Technologies.Length; j++)
                {
                    subProject.Technologies.Add(new ProjectSubProjectTechnology
                    {
                        Id = Guid.NewGuid(),
                        Name = sp.Technologies[j],
                        SortOrder = j + 1
                    });
                }

                project.SubProjects.Add(subProject);
            }
        }

        return project;
    }

    private record SubProjectData(string Name, string? Description, string? Framework, string[] Technologies);

    private async Task SeedProfileSkillsAsync(CancellationToken cancellationToken)
    {
        var profileSkills = new List<ProfileSkill>();

        // Default profile: All skills
        var sortOrder = 1;
        foreach (var skillId in _skillIds.Values)
        {
            profileSkills.Add(new ProfileSkill
            {
                ProfileId = ProfileSeeder.DefaultProfileId,
                SkillId = skillId,
                SortOrder = sortOrder++,
                IsHighlighted = false
            });
        }

        // Backend profile: Backend-focused skills
        var backendSkills = new[]
        {
            "C#", ".NET", "ASP.NET Core", "Entity Framework Core", "MVVM Pattern", "REST API Design",
            "Visual Studio", "Visual Studio Code", "Rider", "Git",
            "Microsoft Azure DevOps", "GitLab", "GitHub",
            "Claude Code", "Codex", "GitHub Copilot",
            "SignalR", "SQLite-net", "Refit REST Library", "Shiny Mediator",
            "Polly (Resilience)", "Swagger/OpenAPI", "ClosedXML", "Stimulsoft Reports",
            "PostgreSQL", "SQLite", "SQL Server", "JWT Authentication",
            "Multi-Tenant Architektur", "Repository Pattern", "Code Generation",
            "REST Service Kommunikation", "Token Authentifizierung"
        };
        sortOrder = 1;
        foreach (var skillName in backendSkills)
        {
            if (_skillIds.TryGetValue(skillName, out var skillId))
            {
                profileSkills.Add(new ProfileSkill
                {
                    ProfileId = ProfileSeeder.BackendProfileId,
                    SkillId = skillId,
                    SortOrder = sortOrder++,
                    IsHighlighted = skillName is "C#" or ".NET" or "ASP.NET Core" or "Entity Framework Core" or "PostgreSQL"
                });
            }
        }

        // Mobile profile: Mobile-focused skills
        var mobileSkills = new[]
        {
            "C#", ".NET", "MAUI", "Xamarin Forms", "Uno Platform", "XAML", "MVVM Pattern",
            "Xamarin.Android", "Xamarin.iOS",
            "MAUI/Uno Platform als Hauptframework", "XAML fuer das Design",
            "Prism als MVVM Framework", "ReactiveUI fuer Statusveraenderungen", "Shiny Framework", "Shiny Mediator Pattern",
            "Visual Studio", "Rider", "Git",
            "MAUI Essentials", "MAUI Community Toolkit", "Syncfusion",
            "ReactiveUI", "Prism", "Shiny", "SkiaSharp", "Lottie", "ZXing", "BarcodeNative", "Cake Build",
            "Bluetooth Drucker Anbindung", "QR-Code Scanner Anbindung",
            "Push Notifications (Cross Platform)", "Lokale Datenbank in App",
            "Dark Mode Design", "Google Maps Einbindung",
            "App Store Verwaltung (Play Store, App Store, Huawei AppGallery)"
        };
        sortOrder = 1;
        foreach (var skillName in mobileSkills)
        {
            if (_skillIds.TryGetValue(skillName, out var skillId))
            {
                profileSkills.Add(new ProfileSkill
                {
                    ProfileId = ProfileSeeder.MobileProfileId,
                    SkillId = skillId,
                    SortOrder = sortOrder++,
                    IsHighlighted = skillName is "MAUI" or "Xamarin Forms" or "Uno Platform"
                });
            }
        }

        await dbContext.Set<ProfileSkill>().AddRangeAsync(profileSkills, cancellationToken);
    }

    private async Task SeedProfileProjectsAsync(CancellationToken cancellationToken)
    {
        var profileProjects = new List<ProfileProject>();

        // Default profile: All projects (no description overrides)
        var sortOrder = 1;
        foreach (var projectId in _projectIds.Values)
        {
            profileProjects.Add(new ProfileProject
            {
                ProfileId = ProfileSeeder.DefaultProfileId,
                ProjectId = projectId,
                SortOrder = sortOrder++,
                IsHighlighted = false,
                DescriptionOverride = null
            });
        }

        // Backend profile: Projects with backend focus and description overrides
        var backendProjects = new (string Name, bool IsHighlighted, string? DescriptionOverride)[]
        {
            ("Communalaudit", true, "Enterprise REST API fuer oesterreichisches Kommunalaudit-System: PostgreSQL mit Entity Framework Core, Multi-Tenant Architektur, Shiny.Mediator Pattern, JWT Authentication, Integration mit Austrian Statistics API (Statcube), Stimulsoft Reports, Code Generation Framework fuer Boilerplate-Reduktion."),
            ("Red Bull ReMa", true, "Enterprise Lagerverwaltung mit SignalR Echtzeit-Updates, RFID-Integration, Azure Application Insights Logging, REST API Anbindung."),
            ("Orderlyze", true, "Backend-Architektur mit REST API, Multi-Device Synchronisation, SQLite Datenbank und Cloud-Integration. Implementierung von Real-Time Updates und Caching-Strategien."),
            ("Sybos", false, "Server-Synchronisation mit RESTful API, Push-Notification Backend mit Firebase/APNs, Chat-Backend mit SignalR fuer Echtzeit-Kommunikation."),
            ("Ekey Bionyx", false, "Azure Notification Hub Integration, Token-basierte Authentifizierung, verschluesselte API-Kommunikation, Backend-Services fuer Geraeteverwaltung."),
            ("Miele Smart Home", false, "Smart Home Backend-Integration, IoT-Geraetekommunikation, Cloud-Services Anbindung, RESTful API Design."),
            ("Asfinag", true, "Umfangreiche API-Integration mit OpenAPI, Polly fuer Resilience Patterns, Caching-Strategien, Multi-Source Datenaggregation."),
            ("Colop E-Mark", false, "WPF Desktop Designer mit Backend-Kommunikation, WLAN/TCP Druckersteuerung, Datenpersistierung mit SQLite."),
            ("PracticeBird", false, "REST API Integration mit Refit, Cloud-Synchronisation von MusicXML/MIDI Dateien, Social Login Backend-Anbindung (Facebook, Google OAuth).")
        };
        sortOrder = 1;
        foreach (var (projectName, isHighlighted, descOverride) in backendProjects)
        {
            if (_projectIds.TryGetValue(projectName, out var projectId))
            {
                profileProjects.Add(new ProfileProject
                {
                    ProfileId = ProfileSeeder.BackendProfileId,
                    ProjectId = projectId,
                    SortOrder = sortOrder++,
                    IsHighlighted = isHighlighted,
                    DescriptionOverride = descOverride
                });
            }
        }

        // Mobile profile: All mobile apps with mobile-focused descriptions
        var mobileProjects = new (string Name, bool IsHighlighted, string? DescriptionOverride)[]
        {
            ("Red Bull ReMa", true, "iPad-Lagerverwaltung mit RFID/Barcode-Scanning via Zebra SDK, Echtzeit-Bestandsuebersicht, Offline-Faehigkeit mit SQLite."),
            ("Orderlyze", true, "Cross-Platform Kassensystem mit Drag & Drop UI, Bluetooth-Druckeranbindung, Offline-First Architektur und intuitivem Touch-Interface."),
            ("Colop E-Mark", true, "Custom Editor mit SkiaSharp, Multi-Touch Gestensteuerung, WLAN-Druckerintegration, mehrsprachige App (7 Sprachen)."),
            ("Sybos", false, "Native Push-Notifications, QR-Code Scanner Integration, Custom Kalender-Komponente, Dark Mode Support."),
            ("Ekey Bionyx", false, "Bluetooth Low Energy Anbindung, Biometrische Sicherheit, Lottie Animationen, Benutzerfreundliche Rollenverwaltung."),
            ("Miele Smart Home", true, "IoT-Geraetesteuerung, Push-Benachrichtigungen, modulare Architektur mit NuGet-Paketen, Rich Animations."),
            ("Asfinag", false, "Responsive UI fuer alle Geraetegroessen, Offline-Karten, 12 Sprachen, barrierefreies Design."),
            ("PracticeBird", false, "SkiaSharp Notenblatt-Rendering, Audio-Playback Synchronisation, Social Login Integration.")
        };
        sortOrder = 1;
        foreach (var (projectName, isHighlighted, descOverride) in mobileProjects)
        {
            if (_projectIds.TryGetValue(projectName, out var projectId))
            {
                profileProjects.Add(new ProfileProject
                {
                    ProfileId = ProfileSeeder.MobileProfileId,
                    ProjectId = projectId,
                    SortOrder = sortOrder++,
                    IsHighlighted = isHighlighted,
                    DescriptionOverride = descOverride
                });
            }
        }

        await dbContext.Set<ProfileProject>().AddRangeAsync(profileProjects, cancellationToken);
    }

    private async Task SeedProfileWorkExperiencesAsync(CancellationToken cancellationToken)
    {
        var profileWorkExperiences = new List<ProfileWorkExperience>();

        // Default profile: All work experiences
        var sortOrder = 1;
        foreach (var workExpId in _workExperienceIds.Values)
        {
            profileWorkExperiences.Add(new ProfileWorkExperience
            {
                ProfileId = ProfileSeeder.DefaultProfileId,
                WorkExperienceId = workExpId,
                SortOrder = sortOrder++,
                IsHighlighted = false,
                DescriptionOverride = null
            });
        }

        // Backend profile: Emphasize backend aspects
        var backendWorkExp = new (string Company, bool IsHighlighted, string? DescriptionOverride)[]
        {
            ("Selbstaendig", true, "Backend-Entwicklung mit ASP.NET Core, API Design, Datenbankarchitektur mit Entity Framework Core. Cloud-Services Integration (Azure, AWS). CI/CD Pipelines."),
            ("Skopek GmbH & CO KG", false, "Backend-Entwicklung fuer die Colop E-Mark Infrastruktur, REST API Design, Datenbankmodellierung."),
            ("DDL GmbH", true, "C# Backend-Entwicklung fuer Datenverwaltungssysteme, SQL Server Datenbanken, Trainer fuer C# Grundlagen und fortgeschrittene Konzepte.")
        };
        sortOrder = 1;
        foreach (var (company, isHighlighted, descOverride) in backendWorkExp)
        {
            if (_workExperienceIds.TryGetValue(company, out var workExpId))
            {
                profileWorkExperiences.Add(new ProfileWorkExperience
                {
                    ProfileId = ProfileSeeder.BackendProfileId,
                    WorkExperienceId = workExpId,
                    SortOrder = sortOrder++,
                    IsHighlighted = isHighlighted,
                    DescriptionOverride = descOverride
                });
            }
        }

        // Mobile profile: Emphasize mobile aspects
        var mobileWorkExp = new (string Company, bool IsHighlighted, string? DescriptionOverride)[]
        {
            ("Selbstaendig", true, "Cross-Platform Mobile Entwicklung mit Xamarin Forms, .NET MAUI und Uno Platform. UI/UX Design, Native Platform Features, App Store Deployments. Kunden: Miele, Asfinag, Ekey."),
            ("Skopek GmbH & CO KG", true, "Entwicklung der Colop E-Mark App mit Custom Editor (SkiaSharp), Multi-Touch Gestures, Bluetooth-Druckeranbindung."),
            ("DDL GmbH", false, "C# Entwicklung, erste Erfahrungen mit mobilen Anwendungen, Trainer fuer Programmiergrundlagen.")
        };
        sortOrder = 1;
        foreach (var (company, isHighlighted, descOverride) in mobileWorkExp)
        {
            if (_workExperienceIds.TryGetValue(company, out var workExpId))
            {
                profileWorkExperiences.Add(new ProfileWorkExperience
                {
                    ProfileId = ProfileSeeder.MobileProfileId,
                    WorkExperienceId = workExpId,
                    SortOrder = sortOrder++,
                    IsHighlighted = isHighlighted,
                    DescriptionOverride = descOverride
                });
            }
        }

        await dbContext.Set<ProfileWorkExperience>().AddRangeAsync(profileWorkExperiences, cancellationToken);
    }
}
