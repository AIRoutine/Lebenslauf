using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class SkillsViewModel : PageViewModel, INavigationAware
{
    public SkillsViewModel(BaseServices baseServices) : base(baseServices)
    {
        Title = "Programmierkenntnisse";
        _ = LoadDataAsync();
    }

    [ObservableProperty]
    private string _title;

    // Expertise
    [ObservableProperty]
    private IReadOnlyList<string> _expertiseSkills = [];

    // Basic Knowledge
    [ObservableProperty]
    private IReadOnlyList<string> _basicSkills = [];

    // App Base Technologies
    [ObservableProperty]
    private IReadOnlyList<string> _appBaseTechnologies = [];

    // Dev Tools
    [ObservableProperty]
    private IReadOnlyList<string> _devTools = [];

    // AI Tools
    [ObservableProperty]
    private IReadOnlyList<string> _aiTools = [];

    // Frameworks
    [ObservableProperty]
    private IReadOnlyList<string> _frameworks = [];

    // App Development Experience
    [ObservableProperty]
    private IReadOnlyList<string> _appDevExperience = [];

    public void OnNavigatedTo(object? parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task LoadDataAsync()
    {
        using (BeginBusy("Lade Skills..."))
        {
            try
            {
                var (_, result) = await Mediator.Request(new GetCvHttpRequest());

                if (result?.SkillCategories is not null)
                {
                    foreach (var category in result.SkillCategories)
                    {
                        var skills = category.Skills.Select(s => s.Name).ToList();

                        switch (category.Name)
                        {
                            case "Expertise":
                                ExpertiseSkills = skills;
                                break;
                            case "Grundkenntnisse":
                                BasicSkills = skills;
                                break;
                            case "App Basistechnologien":
                                AppBaseTechnologies = skills;
                                break;
                            case "DevOps & Tools":
                                DevTools = skills;
                                break;
                            case "AI Tools":
                                AiTools = skills;
                                break;
                            case "Frameworks & Libraries":
                                Frameworks = skills;
                                break;
                            case "App Entwicklungserfahrung":
                                AppDevExperience = skills;
                                break;
                        }
                    }
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load skills from API: {ex.Message}");
            }

            // Fallback to mock data if API fails
            LoadMockData();
        }
    }

    private void LoadMockData()
    {
        ExpertiseSkills =
        [
            "C#", ".NET", "MAUI", "Xamarin Forms", "Uno Platform",
            "ASP.NET", "XAML", "MVVM Pattern"
        ];

        BasicSkills =
        [
            "Xamarin.Android", "Xamarin.iOS", "Java", "Swift", "C", "C++"
        ];

        AppBaseTechnologies =
        [
            "MAUI/Xamarin Forms als Hauptframework",
            "XAML fuer das Design",
            "Prism als MVVM Framework",
            "ReactiveUI fuer Statusveraenderungen",
            "Shiny Framework"
        ];

        DevTools =
        [
            "Visual Studio", "Visual Studio Code", "Rider",
            "Git", "Microsoft Azure DevOps", "GitLab", "GitHub",
            "Jira", "Bitbucket", "Confluence"
        ];

        AiTools =
        [
            "Claude Code", "Codex", "GitHub Copilot"
        ];

        Frameworks =
        [
            "MAUI Essentials", "MAUI Community Toolkit", "Syncfusion",
            "SignalR", "SQLite-net", "ReactiveUI", "Refit REST Library",
            "Prism", "Shiny", "Shiny Mediator", "SharpNado",
            "SkiaSharp", "Lottie", "ZXing", "BarcodeNative"
        ];

        AppDevExperience =
        [
            "Bluetooth Drucker Anbindung",
            "QR-Code Scanner Anbindung",
            "Push Notifications (Cross Platform)",
            "REST Service Kommunikation",
            "Lokale Datenbank in App",
            "Raspberry Pi Kommunikation",
            "Eigener Editor",
            "Google Maps Einbindung",
            "Dark Mode Design",
            "Kontaktverwaltung",
            "WebView Integration",
            "Token Authentifizierung",
            "Google/Facebook Login",
            "Kalender Implementierung",
            "Caching mit Sync (Request Pattern)"
        ];
    }

    [UnoCommand]
    private async Task GoBackAsync()
    {
        await Navigator.NavigateBackAsync(this);
    }
}
