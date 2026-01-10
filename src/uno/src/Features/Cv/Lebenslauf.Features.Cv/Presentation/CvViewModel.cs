using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
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
        Title: "",
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
    private IReadOnlyList<FamilyMemberModel> _familyMembers = [];

    [ObservableProperty]
    private IReadOnlyList<EducationModel> _education = [];

    [ObservableProperty]
    private IReadOnlyList<InternshipModel> _internships = [];

    [ObservableProperty]
    private IReadOnlyList<WorkExperienceModel> _workExperience = [];

    /// <summary>
    /// Calculated age based on birth date.
    /// </summary>
    public int Age
    {
        get
        {
            if (PersonalData.BirthDate == default)
                return 0;

            var today = DateOnly.FromDateTime(DateTime.Today);
            var age = today.Year - PersonalData.BirthDate.Year;
            if (PersonalData.BirthDate > today.AddYears(-age))
                age--;
            return age;
        }
    }

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
            try
            {
                // Call API via Shiny Mediator's generated HTTP contract
                var (_, result) = await Mediator.Request(new GetCvHttpRequest());

                if (result is not null)
                {
                    // Map API response to local models
                    PersonalData = new PersonalDataModel(
                        Name: result.PersonalData.Name,
                        Title: result.PersonalData.Title,
                        Email: result.PersonalData.Email,
                        Phone: result.PersonalData.Phone,
                        Address: result.PersonalData.Address,
                        City: result.PersonalData.City,
                        PostalCode: result.PersonalData.PostalCode,
                        Country: result.PersonalData.Country,
                        BirthDate: result.PersonalData.BirthDate,
                        Citizenship: result.PersonalData.Citizenship,
                        ProfileImageUrl: result.PersonalData.ProfileImageUrl
                    );

                    // Notify that Age property changed after PersonalData is set
                    OnPropertyChanged(nameof(Age));

                    FamilyMembers = result.FamilyMembers
                        .Select(f => new FamilyMemberModel(f.Id, f.Relationship, f.Profession, f.BirthYear))
                        .ToList();

                    Education = result.Education
                        .Select(e => new EducationModel(e.Id, e.Institution, e.Degree, e.StartYear, e.EndYear, e.Description))
                        .ToList();

                    Internships = result.Internships
                        .Select(i => new InternshipModel(i.Id, i.Company, i.Role, i.Year, i.Month, i.EndMonth, i.Description))
                        .ToList();

                    WorkExperience = result.WorkExperience
                        .Select(w => new WorkExperienceModel(w.Id, w.Company, w.Role, w.StartDate, w.EndDate, w.Description, w.IsCurrent))
                        .ToList();

                    SkillCategories = result.SkillCategories
                        .Select(sc => new SkillCategoryModel(
                            sc.Id,
                            sc.Name,
                            sc.Skills.Select(s => new SkillModel(s.Id, s.Name)).ToList()))
                        .ToList();

                    Projects = result.Projects
                        .Select(p => new ProjectModel(p.Id, p.Name, p.Description, p.Technologies.ToList(), p.AppStoreUrl, p.PlayStoreUrl, p.WebsiteUrl, p.ImageUrl))
                        .ToList();

                    // Extract frameworks from categories
                    var frameworkCategory = SkillCategories.FirstOrDefault(c => c.Name.Contains("Framework", StringComparison.OrdinalIgnoreCase));
                    Frameworks = frameworkCategory?.Skills.Select(s => s.Name).ToList() ?? [];

                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load CV from API: {ex.Message}");
            }

            // Fallback to mock data if API fails
            LoadMockData();
        }
    }

    private void LoadMockData()
    {
        PersonalData = new PersonalDataModel(
            Name: "Daniel Hufnagl",
            Title: "Senior Cross-Platform Developer",
            Email: "d.hufnagl@codelisk.com",
            Phone: "+43-664-73221804",
            Address: "Stockham 44",
            City: "Laakirchen",
            PostalCode: "4663",
            Country: "Oesterreich",
            BirthDate: new DateOnly(1998, 8, 1),
            Citizenship: "Oesterreich",
            ProfileImageUrl: null
        );

        OnPropertyChanged(nameof(Age));

        FamilyMembers =
        [
            new(Guid.NewGuid(), "Vater", "Elektroinstallateur", null),
            new(Guid.NewGuid(), "Mutter", "Vertriebsassistenz", null),
            new(Guid.NewGuid(), "Bruder", "Mechatroniker", 2000),
            new(Guid.NewGuid(), "Bruder", "NMS Schueler", 2007)
        ];

        Education =
        [
            new(Guid.NewGuid(), "HTL Grieskirchen", "Reife- und Diplompruefung Informatik", 2012, 2017,
                "Diplomarbeit: Programmierung eines Verwaltungssystems fuer Aerzte und Mitarbeiter"),
            new(Guid.NewGuid(), "VS, HS Laakirchen", "Volksschule und Hauptschule", 2004, 2012, null)
        ];

        Internships =
        [
            new(Guid.NewGuid(), "Firma Stoeber", "Programmierpraktikum", 2014, 7, 7, "Programmierarbeiten"),
            new(Guid.NewGuid(), "Elektro Steinschaden", "Elektrotechnik Praktikum", 2015, 7, 8, "Elektrotechnische Arbeiten")
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
