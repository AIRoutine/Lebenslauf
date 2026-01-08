using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Features.Cv.Contracts.Models;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class CvViewModel : PageViewModel, INavigationAware
{
    public CvViewModel(BaseServices baseServices) : base(baseServices)
    {
        Title = "Lebenslauf";

        // Load data immediately (OnNavigatedTo may not be called with current navigation setup)
        _ = LoadCvDataAsync();
    }

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private CvData? _cvData;

    [ObservableProperty]
    private PersonalDataModel _personalData = new(
        Name: "",
        Email: "",
        Phone: "",
        Address: "",
        City: "",
        PostalCode: "",
        Country: "",
        BirthDate: default,
        Citizenship: "",
        ProfileImageUrl: null
    );

    [ObservableProperty]
    private IReadOnlyList<EducationModel> _education = [];

    [ObservableProperty]
    private IReadOnlyList<WorkExperienceModel> _workExperience = [];

    [ObservableProperty]
    private IReadOnlyList<SkillCategoryModel> _skillCategories = [];

    [ObservableProperty]
    private IReadOnlyList<ProjectModel> _projects = [];

    [ObservableProperty]
    private IReadOnlyList<string> _frameworks = [];

    public void OnNavigatedTo(object? parameter)
    {
        _ = LoadCvDataAsync();
    }

    public void OnNavigatedFrom()
    {
        // Nothing to cleanup
    }

    private async Task LoadCvDataAsync()
    {
        using (BeginBusy("Lade Lebenslauf..."))
        {
            // TODO: Replace with API call via Mediator when API is running
            // var response = await Mediator.Request(new GetCvHttpRequest());

            await Task.Delay(500); // Simulate network delay

            // Load mock data
            LoadMockData();
        }
    }

    private void LoadMockData()
    {
        PersonalData = new PersonalDataModel(
            Name: "Daniel Hufnagl",
            Email: "daniel.hufnagl@aon.at",
            Phone: "+43-664-73221804",
            Address: "Stockham 44",
            City: "Laakirchen",
            PostalCode: "4663",
            Country: "Oesterreich",
            BirthDate: new DateOnly(1998, 8, 1),
            Citizenship: "Oesterreich",
            ProfileImageUrl: null
        );

        Education =
        [
            new(Guid.NewGuid(), "HTL Grieskirchen", "Reife- und Diplompruefung Informatik", 2012, 2017,
                "Diplomarbeit: Programmierung eines Verwaltungssystems fuer Aerzte und Mitarbeiter"),
            new(Guid.NewGuid(), "VS, HS Laakirchen", "Volksschule und Hauptschule", 2004, 2012, null)
        ];

        WorkExperience =
        [
            new(Guid.NewGuid(), "Selbstaendig", "Vollzeit Einzelunternehmer",
                new DateOnly(2019, 11, 30), null,
                "Cross-Platform App-Entwicklung mit Xamarin Forms, .NET MAUI und Uno Platform", true),
            new(Guid.NewGuid(), "Skopek GmbH & CO KG", "Xamarin Forms Entwickler bei Colop",
                new DateOnly(2018, 8, 20), new DateOnly(2019, 11, 30),
                "Entwicklung der Colop E-Mark App", false),
            new(Guid.NewGuid(), "DDL GmbH", "C# Entwickler & Wifi Trainer",
                new DateOnly(2017, 11, 27), new DateOnly(2018, 7, 15),
                "Datenverwaltung, C# Anwendungen, Wifi Trainer", false)
        ];

        SkillCategories =
        [
            new(Guid.NewGuid(), "Expertise",
            [
                new(Guid.NewGuid(), "C#"),
                new(Guid.NewGuid(), ".NET"),
                new(Guid.NewGuid(), "MAUI"),
                new(Guid.NewGuid(), "Xamarin Forms"),
                new(Guid.NewGuid(), "Uno Platform"),
                new(Guid.NewGuid(), "ASP.NET"),
                new(Guid.NewGuid(), "XAML"),
                new(Guid.NewGuid(), "MVVM")
            ]),
            new(Guid.NewGuid(), "Frameworks",
            [
                new(Guid.NewGuid(), "ReactiveUI"),
                new(Guid.NewGuid(), "Prism"),
                new(Guid.NewGuid(), "Shiny"),
                new(Guid.NewGuid(), "Syncfusion"),
                new(Guid.NewGuid(), "SkiaSharp"),
                new(Guid.NewGuid(), "Lottie")
            ]),
            new(Guid.NewGuid(), "DevOps & Tools",
            [
                new(Guid.NewGuid(), "Visual Studio"),
                new(Guid.NewGuid(), "Rider"),
                new(Guid.NewGuid(), "Git"),
                new(Guid.NewGuid(), "Azure DevOps"),
                new(Guid.NewGuid(), "GitHub")
            ]),
            new(Guid.NewGuid(), "AI Tools",
            [
                new(Guid.NewGuid(), "Claude Code"),
                new(Guid.NewGuid(), "GitHub Copilot"),
                new(Guid.NewGuid(), "Codex")
            ])
        ];

        Frameworks = ["ReactiveUI", "Prism", "Shiny", "Git", "Azure DevOps", "Visual Studio"];

        Projects =
        [
            new(Guid.NewGuid(), "Orderlyze", "Kassensystem App mit Tischplan, Reservierungen, Berichten",
                ["Xamarin.Forms", "MAUI", "Prism", "ReactiveUI", "Syncfusion"],
                "https://apps.apple.com/at/app/orderlyze/id1495015799",
                "https://play.google.com/store/apps/details?id=orderlyze.com",
                "https://www.orderlyze.com", null),
            new(Guid.NewGuid(), "Colop E-Mark", "Stempel-Editor mit mehrfarbigen Abdrucken",
                ["Xamarin.Forms", "SkiaSharp", "Prism"],
                "https://apps.apple.com/at/app/colop-e-mark/id1397292575",
                "https://play.google.com/store/apps/details?id=com.colop.colopemark",
                "https://www.colop.com/de/digital-produkte", null),
            new(Guid.NewGuid(), "Sybos", "Feuerwehr-Verwaltung mit Chat und Push",
                ["Xamarin.Forms", "MAUI", "Prism", "ReactiveUI", "Shiny"],
                "https://apps.apple.com/at/app/sybos/id1176062382",
                "https://play.google.com/store/apps/details?id=at.syPhone",
                "https://www.sybos.net", null),
            new(Guid.NewGuid(), "Miele", "Smart Home App fuer Hausgeraete",
                ["MAUI", "Firebase", "Roslyn", "Lottie"],
                "https://apps.apple.com/at/app/miele-app-smart-home/id930406907",
                "https://play.google.com/store/apps/details?id=de.miele.infocontrol",
                "https://www.miele.at/c/miele-app-2594.htm", null),
            new(Guid.NewGuid(), "Asfinag", "Verkehrsinfo mit 1800+ Webcams",
                ["MAUI", "Prism", "OpenAPI", "Syncfusion"],
                "https://apps.apple.com/at/app/asfinag/id453459323",
                "https://play.google.com/store/apps/details?id=at.asfinag.unterwegs",
                "https://www.asfinag.at/asfinag-app/", null),
            new(Guid.NewGuid(), "Ekey Bionyx", "Fingerscanner App mit Bluetooth",
                ["Xamarin.Forms", "Shiny", "Azure Notification Hub"],
                "https://apps.apple.com/at/app/ekey-bionyx/id1484053054",
                "https://play.google.com/store/apps/details?id=net.ekey.bionyx",
                "https://www.ekey.net/ekey-bionyx-app/", null)
        ];
    }

    [UnoCommand]
    private async Task OpenUrlAsync(string? url)
    {
        if (string.IsNullOrEmpty(url))
            return;

        try
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(url));
        }
        catch
        {
            // Ignore errors
        }
    }

    [UnoCommand]
    private async Task NavigateToSkillsAsync()
    {
        await Navigator.NavigateViewModelAsync<SkillsViewModel>(this);
    }

    [UnoCommand]
    private async Task NavigateToProjectsAsync()
    {
        await Navigator.NavigateViewModelAsync<ProjectsViewModel>(this);
    }
}
