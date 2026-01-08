using System.Text.Json;
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
        // Expertise Category
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
                new() { Id = Guid.NewGuid(), Name = "MVVM", SortOrder = 8 }
            ]
        };

        // Basic Knowledge Category
        var basicCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Grundlagen",
            SortOrder = 2,
            Skills =
            [
                new() { Id = Guid.NewGuid(), Name = "Xamarin.Android", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "Xamarin.iOS", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Java", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "Swift", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "C", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "C++", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "SQL", SortOrder = 7 }
            ]
        };

        // Frameworks Category
        var frameworksCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "Frameworks & Libraries",
            SortOrder = 3,
            Skills =
            [
                new() { Id = Guid.NewGuid(), Name = "ReactiveUI", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "Prism", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Shiny", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "Shiny Mediator", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "Syncfusion", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "SkiaSharp", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "Lottie", SortOrder = 7 },
                new() { Id = Guid.NewGuid(), Name = "SQLite-net", SortOrder = 8 },
                new() { Id = Guid.NewGuid(), Name = "Refit", SortOrder = 9 },
                new() { Id = Guid.NewGuid(), Name = "ZXing", SortOrder = 10 },
                new() { Id = Guid.NewGuid(), Name = "SignalR", SortOrder = 11 }
            ]
        };

        // DevOps Category
        var devOpsCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "DevOps & Tools",
            SortOrder = 4,
            Skills =
            [
                new() { Id = Guid.NewGuid(), Name = "Visual Studio", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "VS Code", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Rider", SortOrder = 3 },
                new() { Id = Guid.NewGuid(), Name = "Git", SortOrder = 4 },
                new() { Id = Guid.NewGuid(), Name = "Azure DevOps", SortOrder = 5 },
                new() { Id = Guid.NewGuid(), Name = "GitLab", SortOrder = 6 },
                new() { Id = Guid.NewGuid(), Name = "GitHub", SortOrder = 7 },
                new() { Id = Guid.NewGuid(), Name = "Jira", SortOrder = 8 },
                new() { Id = Guid.NewGuid(), Name = "Bitbucket", SortOrder = 9 },
                new() { Id = Guid.NewGuid(), Name = "Confluence", SortOrder = 10 }
            ]
        };

        // AI Tools Category
        var aiCategory = new SkillCategory
        {
            Id = Guid.NewGuid(),
            Name = "AI Tools",
            SortOrder = 5,
            Skills =
            [
                new() { Id = Guid.NewGuid(), Name = "Claude Code", SortOrder = 1 },
                new() { Id = Guid.NewGuid(), Name = "GitHub Copilot", SortOrder = 2 },
                new() { Id = Guid.NewGuid(), Name = "Codex", SortOrder = 3 }
            ]
        };

        await dbContext.Set<SkillCategory>().AddRangeAsync(
            [expertiseCategory, basicCategory, frameworksCategory, devOpsCategory, aiCategory],
            cancellationToken);
    }

    private async Task SeedProjectsAsync(CancellationToken cancellationToken)
    {
        var projects = new List<Project>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Orderlyze",
                Description = "Kassensystem App: Rechnungen erstellen, Tischplan mit Drag & Drop, Reservierungen, Berichte, Mitarbeiterverwaltung, Statistiken, Multi-Device Sync.",
                Technologies = JsonSerializer.Serialize(new[] { "Xamarin.Forms", "MAUI", "C#", "Prism", "ReactiveUI", "Syncfusion", "SQLite", "Shiny.BLE" }),
                AppStoreUrl = "https://apps.apple.com/at/app/orderlyze/id1495015799",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=orderlyze.com",
                WebsiteUrl = "https://www.orderlyze.com",
                SortOrder = 1
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Colop E-Mark",
                Description = "Stempel-Editor App: Eigener Editor fuer mehrfarbige Abdrucke, QR/Barcode Generator, WLAN-Druckerverbindung, mehrsprachig (7 Sprachen).",
                Technologies = JsonSerializer.Serialize(new[] { "Xamarin.Forms", "C#", "Prism", "SkiaSharp", "ZXing", "SQLite" }),
                AppStoreUrl = "https://apps.apple.com/at/app/colop-e-mark/id1397292575",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=com.colop.colopemark",
                WebsiteUrl = "https://www.colop.com/de/digital-produkte",
                SortOrder = 2
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Sybos",
                Description = "Feuerwehr-Verwaltung: Chat mit Push-Notifications, QR-Code Scanner, Kalender, Zusatzalarmierung, Materialverwaltung, Server-Synchronisation.",
                Technologies = JsonSerializer.Serialize(new[] { "Xamarin.Forms", "MAUI", "C#", "Prism", "ReactiveUI", "Refit", "ZXing", "Shiny" }),
                AppStoreUrl = "https://apps.apple.com/at/app/sybos/id1176062382",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=at.syPhone",
                WebsiteUrl = "https://www.sybos.net",
                SortOrder = 3
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "PracticeBird",
                Description = "Notenblatt App: Mitspielendes Notenblatt, MusicXML/MIDI Import, Playback mit Klavier/Schlagzeug, Stimmenhervorhebung, Social Login.",
                Technologies = JsonSerializer.Serialize(new[] { "Xamarin.Forms", "C#", "Prism", "ReactiveUI", "SkiaSharp", "Refit" }),
                AppStoreUrl = "https://apps.apple.com/us/app/practice-bird-interactive-sheet-music-and-scores/id1253492926",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=phonicscore.phonicscore_lite",
                WebsiteUrl = "https://www.practicebird.com",
                SortOrder = 4
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Ekey Bionyx",
                Description = "Fingerscanner App: Push-Nachrichten, hohe Sicherheit mit Verschluesselung, Bluetooth-Anbindung zu Tuerschloessern, umfangreiches Benutzermanagement.",
                Technologies = JsonSerializer.Serialize(new[] { "Xamarin.Forms", "C#", "Prism", "Shiny", "ZXing", "Azure Notification Hub", "Lottie" }),
                AppStoreUrl = "https://apps.apple.com/at/app/ekey-bionyx/id1484053054",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=net.ekey.bionyx",
                WebsiteUrl = "https://www.ekey.net/ekey-bionyx-app/",
                SortOrder = 5
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Miele",
                Description = "Smart Home App: Hausgeraete mobil steuern, Push-Nachrichten ueber Geraetestatus, verschiedene Assistenten als Nuget-Module, eigener Shop.",
                Technologies = JsonSerializer.Serialize(new[] { "MAUI", "C#", "MVVM", "Firebase", "Roslyn Code Generation", "Sentry", "Lottie" }),
                AppStoreUrl = "https://apps.apple.com/at/app/miele-app-smart-home/id930406907",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=de.miele.infocontrol",
                WebsiteUrl = "https://www.miele.at/c/miele-app-2594.htm",
                SortOrder = 6
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Asfinag",
                Description = "Verkehrsinfo App: Personalisierter Homescreen, 1800+ Webcams, Verkehrsinfos & Baustellen, E-Ladestationen, Digitale Vignette, Routenplaner, 12 Sprachen.",
                Technologies = JsonSerializer.Serialize(new[] { "MAUI", "C#", "Prism", "Prism Regions", "OpenAPI", "Polly", "Syncfusion", "SQLite" }),
                AppStoreUrl = "https://apps.apple.com/at/app/asfinag/id453459323",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=at.asfinag.unterwegs",
                WebsiteUrl = "https://www.asfinag.at/asfinag-app/",
                SortOrder = 7
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Lolyo",
                Description = "Mitarbeiter-Kommunikations App: Interne Kommunikationsplattform fuer Unternehmen.",
                Technologies = JsonSerializer.Serialize(new[] { "Xamarin.Android", "Xamarin.iOS", "C#", "GitLab" }),
                AppStoreUrl = null,
                PlayStoreUrl = null,
                WebsiteUrl = "https://www.lolyo.at",
                SortOrder = 8
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Protimer",
                Description = "Zeiterfassungs App: Migration von Xamarin.Forms auf MAUI.",
                Technologies = JsonSerializer.Serialize(new[] { "MAUI", "Xamarin.Forms", "C#" }),
                AppStoreUrl = "https://apps.apple.com/ch/app/protimer-easy/id1114715179",
                PlayStoreUrl = "https://play.google.com/store/apps/details?id=ch.protimereasy.terminal",
                WebsiteUrl = null,
                SortOrder = 9
            }
        };

        await dbContext.Set<Project>().AddRangeAsync(projects, cancellationToken);
    }
}
