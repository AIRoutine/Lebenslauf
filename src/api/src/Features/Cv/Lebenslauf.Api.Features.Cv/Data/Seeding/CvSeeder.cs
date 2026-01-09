using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Core.Data.Seeding;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Lebenslauf.Api.Features.Cv.Data.Seeding;

public class CvSeeder(AppDbContext dbContext) : ISeeder
{
    public int Order => 10;

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if already seeded
        if (await dbContext.Set<PersonalData>().AnyAsync(cancellationToken))
            return;

        await SeedPersonalDataAsync(cancellationToken);
        await SeedEducationAsync(cancellationToken);
        await SeedWorkExperienceAsync(cancellationToken);
        await SeedSkillsAsync(cancellationToken);
        await SeedProjectsAsync(cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task SeedPersonalDataAsync(CancellationToken cancellationToken)
    {
        var personalData = new PersonalData
        {
            Id = Guid.NewGuid(),
            Name = "Daniel Hufnagl",
            Title = "Senior Cross-Platform Developer",
            Email = "daniel.hufnagl@aon.at",
            Phone = "+43-664-73221804",
            Address = "Stockham 44",
            City = "Laakirchen",
            PostalCode = "4663",
            Country = "Oesterreich",
            BirthDate = new DateOnly(1998, 8, 1),
            Citizenship = "Oesterreich",
            ProfileImageUrl = null
        };

        await dbContext.Set<PersonalData>().AddAsync(personalData, cancellationToken);
    }

    private async Task SeedEducationAsync(CancellationToken cancellationToken)
    {
        var education = new List<Education>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Institution = "HTL Grieskirchen",
                Degree = "Reife- und Diplompruefung Informatik",
                StartYear = 2012,
                EndYear = 2017,
                Description = "Diplomarbeit: Programmierung eines Verwaltungssystems fuer Aerzte und Mitarbeiter inklusive Anzeige auf IP-TVS fuer das Kurheim Bad Schallerbach",
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

    private async Task SeedWorkExperienceAsync(CancellationToken cancellationToken)
    {
        var workExperience = new List<WorkExperience>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Company = "Selbstaendig",
                Role = "Vollzeit Einzelunternehmer",
                StartDate = new DateOnly(2019, 11, 30),
                EndDate = null,
                IsCurrent = true,
                Description = "Cross-Platform App-Entwicklung mit Xamarin Forms, .NET MAUI und Uno Platform. Kunden: Miele, Asfinag, Ekey, und weitere.",
                SortOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Company = "Skopek GmbH & CO KG",
                Role = "Xamarin Forms Entwickler bei Colop Stempelerzeugung",
                StartDate = new DateOnly(2018, 8, 20),
                EndDate = new DateOnly(2019, 11, 30),
                IsCurrent = false,
                Description = "Entwicklung der Colop E-Mark App mit eigenem Editor fuer mehrfarbige Abdrucke.",
                SortOrder = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                Company = "DDL GmbH",
                Role = "C# Entwickler & Wifi Trainer",
                StartDate = new DateOnly(2017, 11, 27),
                EndDate = new DateOnly(2018, 7, 15),
                IsCurrent = false,
                Description = "Datenverwaltung von Ver- und Entsorgungsnetzen, Programmieren von C# Anwendungen, Wifi Trainer fuer C# (Gruppen von 20-30 Personen).",
                SortOrder = 3
            }
        };

        await dbContext.Set<WorkExperience>().AddRangeAsync(workExperience, cancellationToken);
    }

    private async Task SeedSkillsAsync(CancellationToken cancellationToken)
    {
        // 1. Expertise Category
        var expertiseCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Expertise",
            SortOrder = 1,
            Skills =
            [
                new() { Id = Guid.NewGuid(), Name = "C#", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = ".NET", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "MAUI", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "Xamarin Forms", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "Uno Platform", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "ASP.NET", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "XAML", SortOrder = 7 },
                new() { Id = Guid.NewGuid(), Name = "MVVM Pattern", SortOrder = 8 }
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
                new() { Id = Guid.NewGuid(), Name = "Xamarin.Android", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "Xamarin.iOS", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Java", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "Swift", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "C", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "C++", SortOrder = 6 }
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
                new() { Id = Guid.NewGuid(), Name = "MAUI/Xamarin Forms als Hauptframework", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "XAML fuer das Design", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Prism als MVVM Framework", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "ReactiveUI fuer Statusveraenderungen", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "Shiny Framework", SortOrder = 5 }
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
                new() { Id = Guid.NewGuid(), Name = "Visual Studio", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "Visual Studio Code", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Rider", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "Git", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "Microsoft Azure DevOps", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "GitLab", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "GitHub", SortOrder = 7 },
                new() { Id = Guid.NewGuid(), Name = "Jira", SortOrder = 8 },
                new() { Id = Guid.NewGuid(), Name = "Bitbucket", SortOrder = 9 },
                new() { Id = Guid.NewGuid(), Name = "Confluence", SortOrder = 10 }
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
                new() { Id = Guid.NewGuid(), Name = "Claude Code", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "Codex", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "GitHub Copilot", SortOrder = 3 }
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
                new() { Id = Guid.NewGuid(), Name = "MAUI Essentials", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "MAUI Community Toolkit", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Syncfusion", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "SignalR", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "SQLite-net", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "ReactiveUI", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "Refit REST Library", SortOrder = 7 },
                new() { Id = Guid.NewGuid(), Name = "Prism", SortOrder = 8 },
                new() { Id = Guid.NewGuid(), Name = "Shiny", SortOrder = 9 },
                new() { Id = Guid.NewGuid(), Name = "Shiny Mediator", SortOrder = 10 },
                new() { Id = Guid.NewGuid(), Name = "SharpNado", SortOrder = 11 },
                new() { Id = Guid.NewGuid(), Name = "SkiaSharp", SortOrder = 12 },
                new() { Id = Guid.NewGuid(), Name = "Lottie", SortOrder = 13 },
                new() { Id = Guid.NewGuid(), Name = "ZXing", SortOrder = 14 },
                new() { Id = Guid.NewGuid(), Name = "BarcodeNative", SortOrder = 15 }
            ]
        };

        // 7. App Development Experience Category
        var appDevCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "App Entwicklungserfahrung",
            SortOrder = 7,
            Skills =
            [
                new() { Id = Guid.NewGuid(), Name = "Bluetooth Drucker Anbindung", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "QR-Code Scanner Anbindung", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Push Notifications (Cross Platform)", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "REST Service Kommunikation", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "Lokale Datenbank in App", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "Raspberry Pi Kommunikation", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "Eigener Editor", SortOrder = 7 },
                new() { Id = Guid.NewGuid(), Name = "Google Maps Einbindung", SortOrder = 8 },
                new() { Id = Guid.NewGuid(), Name = "Dark Mode Design", SortOrder = 9 },
                new() { Id = Guid.NewGuid(), Name = "Kontaktverwaltung", SortOrder = 10 },
                new() { Id = Guid.NewGuid(), Name = "WebView Integration", SortOrder = 11 },
                new() { Id = Guid.NewGuid(), Name = "Token Authentifizierung", SortOrder = 12 },
                new() { Id = Guid.NewGuid(), Name = "Google/Facebook Login", SortOrder = 13 },
                new() { Id = Guid.NewGuid(), Name = "Kalender Implementierung", SortOrder = 14 },
                new() { Id = Guid.NewGuid(), Name = "Caching mit Sync (Request Pattern)", SortOrder = 15 }
            ]
        };

        await dbContext.Set<SkillCategory>().AddRangeAsync(
            [expertiseCategory, basicCategory, appBaseCategory, devOpsCategory, aiCategory, frameworksCategory, appDevCategory],
            cancellationToken);
    }

    private async Task SeedProjectsAsync(CancellationToken cancellationToken)
    {
        var projects = new List<Project>
        {
            CreateProject(
                name: "Orderlyze",
                description: "Kassensystem App: Rechnungen erstellen, Tischplan mit Drag & Drop, Reservierungen, Berichte, Mitarbeiterverwaltung, Statistiken, Multi-Device Sync.",
                framework: "Xamarin.Forms / MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/orderlyze/id1495015799",
                playStoreUrl: "https://play.google.com/store/apps/details?id=orderlyze.com",
                websiteUrl: "https://www.orderlyze.com",
                sortOrder: 1,
                technologies: ["Xamarin.Forms", "MAUI", "C#", "Prism", "ReactiveUI", "Syncfusion", "SQLite", "Shiny.BLE"],
                functions: [
                    "Rechnungen erstellen und ausdrucken",
                    "Tischplan mit Drag und Drop",
                    "Reservierungen erstellen",
                    "Tages-, Monats- und Jahresberichte",
                    "Mitarbeiterverwaltung",
                    "Statistiken und Analysen",
                    "Synchronisation zwischen Geraeten"
                ],
                technicalAspects: [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "State Changes: ReactiveUI",
                    "Server: OpenApi Generierung",
                    "Bluetooth: Shiny.BLE",
                    "Datenbank: sqlite-net (Async)",
                    "UI: Syncfusion"
                ]),

            CreateProject(
                name: "Colop E-Mark",
                description: "Stempel-Editor App: Eigener Editor fuer mehrfarbige Abdrucke, QR/Barcode Generator, WLAN-Druckerverbindung, mehrsprachig (7 Sprachen).",
                framework: "Xamarin.Forms",
                appStoreUrl: "https://apps.apple.com/at/app/colop-e-mark/id1397292575",
                playStoreUrl: "https://play.google.com/store/apps/details?id=com.colop.colopemark",
                websiteUrl: "https://www.colop.com/de/digital-produkte",
                sortOrder: 2,
                technologies: ["Xamarin.Forms", "C#", "Prism", "SkiaSharp", "ZXing", "SQLite"],
                functions: [
                    "Editor fuer mehrfarbige Abdruecke",
                    "QR und Barcode Generierung",
                    "Eigenes Benutzersystem",
                    "Video-Wizard Einfuehrung",
                    "WLAN Drucker Verbindung",
                    "7 Sprachen"
                ],
                technicalAspects: [
                    "Framework: Xamarin.Forms",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "Editor: SkiaSharp",
                    "QR-Code: ZXing",
                    "Datenbank: SQLite .NET"
                ]),

            CreateProject(
                name: "Sybos",
                description: "Feuerwehr-Verwaltung: Chat mit Push-Notifications, QR-Code Scanner, Kalender, Zusatzalarmierung, Materialverwaltung, Server-Synchronisation.",
                framework: "Xamarin.Forms / MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/sybos/id1176062382",
                playStoreUrl: "https://play.google.com/store/apps/details?id=at.syPhone",
                websiteUrl: "https://www.sybos.net",
                sortOrder: 3,
                technologies: ["Xamarin.Forms", "MAUI", "C#", "Prism", "ReactiveUI", "Refit", "ZXing", "Shiny"],
                functions: [
                    "Chat mit Push-Notifications",
                    "QR-Code Scanner fuer Material",
                    "Kalender fuer Veranstaltungen",
                    "Zusatzalarmierung mit Push",
                    "Dark Mode",
                    "Materialverwaltung",
                    "Server Synchronisation"
                ],
                technicalAspects: [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "State Changes: ReactiveUI",
                    "Server: Refit REST",
                    "QR-Code: ZXing",
                    "Push: Shiny Library"
                ]),

            CreateProject(
                name: "Ekey Bionyx",
                description: "Fingerscanner App: Push-Nachrichten, hohe Sicherheit mit Verschluesselung, Bluetooth-Anbindung zu Tuerschloessern, umfangreiches Benutzermanagement.",
                framework: "Xamarin.Forms",
                appStoreUrl: "https://apps.apple.com/at/app/ekey-bionyx/id1484053054",
                playStoreUrl: "https://play.google.com/store/apps/details?id=net.ekey.bionyx",
                websiteUrl: "https://www.ekey.net/ekey-bionyx-app/",
                sortOrder: 4,
                technologies: ["Xamarin.Forms", "C#", "Prism", "Shiny", "Azure Notification Hub", "Lottie"],
                functions: [
                    "Automatische Push-Nachrichten",
                    "Hoechste Sicherheit/Verschluesselung",
                    "Bluetooth zu Fingerscanner",
                    "Benutzermanagement/Rollensystem"
                ],
                technicalAspects: [
                    "Framework: Xamarin.Forms",
                    "Repository: Azure DevOps",
                    "Push: Azure Notification Hub",
                    "Bluetooth: Shiny",
                    "CI/CD: Azure DevOps",
                    "Animationen: Lottie"
                ]),

            CreateProject(
                name: "Miele Smart Home",
                description: "Smart Home App: Hausgeraete mobil steuern, Push-Nachrichten ueber Geraetestatus, verschiedene Assistenten als Nuget-Module, eigener Shop.",
                framework: "MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/miele-app-smart-home/id930406907",
                playStoreUrl: "https://play.google.com/store/apps/details?id=de.miele.infocontrol",
                websiteUrl: "https://www.miele.at/c/miele-app-2594.htm",
                sortOrder: 5,
                technologies: ["MAUI", "C#", "MVVM", "Firebase", "Roslyn Code Generation", "Sentry", "Lottie"],
                functions: [
                    "Hausgeraete mobil steuern",
                    "Push ueber Geraetestatus",
                    "Assistenten-System (NuGet)",
                    "Eigener Shop"
                ],
                technicalAspects: [
                    "Framework: MAUI",
                    "Repository: Bitbucket",
                    "Notifications: Firebase",
                    "Codegenerierung: Roslyn",
                    "Logging: Sentry",
                    "Animationen: Lottie"
                ]),

            CreateProject(
                name: "Asfinag",
                description: "Verkehrsinfo App: Personalisierter Homescreen, 1800+ Webcams, Verkehrsinfos & Baustellen, E-Ladestationen, Digitale Vignette, Routenplaner, 12 Sprachen.",
                framework: "MAUI",
                appStoreUrl: "https://apps.apple.com/at/app/asfinag/id453459323",
                playStoreUrl: "https://play.google.com/store/apps/details?id=at.asfinag.unterwegs",
                websiteUrl: "https://www.asfinag.at/asfinag-app/",
                sortOrder: 6,
                technologies: ["MAUI", "C#", "Prism", "OpenAPI", "Polly", "Syncfusion", "SQLite"],
                functions: [
                    "Personalisierter Homescreen",
                    "1800+ Webcams",
                    "Verkehrsinfos und Baustellen",
                    "Raststationen und E-Ladestationen",
                    "Digitale Vignette",
                    "Routenplaner (Europa)",
                    "12 Sprachen"
                ],
                technicalAspects: [
                    "Framework: MAUI",
                    "App Lifecycle: Prism",
                    "Server: OpenApi, Polly, Scalar",
                    "UI: Syncfusion",
                    "Datenbank: SQLite .NET",
                    "Push: Cross Platform"
                ]),

            CreateProject(
                name: "PracticeBird",
                description: "Notenblatt App: Mitspielendes Notenblatt, MusicXML/MIDI Import, Playback mit Klavier/Schlagzeug, Stimmenhervorhebung, Social Login.",
                framework: "Xamarin.Forms",
                appStoreUrl: "https://apps.apple.com/us/app/practice-bird-interactive-sheet-music-and-scores/id1253492926",
                playStoreUrl: "https://play.google.com/store/apps/details?id=phonicscore.phonicscore_lite",
                websiteUrl: "https://www.practicebird.com",
                sortOrder: 7,
                technologies: ["Xamarin.Forms", "C#", "Prism", "ReactiveUI", "SkiaSharp", "Refit"],
                functions: [
                    "Mitspielendes Notenblatt",
                    "MusicXML/MIDI Dateien",
                    "Playback mit Klavier/Schlagzeug",
                    "Stimme hervorheben",
                    "Facebook/Google Login"
                ],
                technicalAspects: [
                    "Framework: Xamarin.Forms",
                    "App Lifecycle: Prism",
                    "State Changes: ReactiveUI",
                    "Server: Refit REST",
                    "Notenblatt: SkiaSharp"
                ])
        };

        await dbContext.Set<Project>().AddRangeAsync(projects, cancellationToken);
    }

    private static Project CreateProject(
        string name,
        string description,
        string framework,
        string? appStoreUrl,
        string? playStoreUrl,
        string? websiteUrl,
        int sortOrder,
        string[] technologies,
        string[] functions,
        string[] technicalAspects)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = name,
            Description = description,
            Framework = framework,
            AppStoreUrl = appStoreUrl,
            PlayStoreUrl = playStoreUrl,
            WebsiteUrl = websiteUrl,
            SortOrder = sortOrder
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

        return project;
    }
}
