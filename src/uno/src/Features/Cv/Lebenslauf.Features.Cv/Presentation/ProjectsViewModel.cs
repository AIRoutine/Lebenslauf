using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class ProjectsViewModel : PageViewModel, INavigationAware
{
    public ProjectsViewModel(BaseServices baseServices) : base(baseServices)
    {
        Title = "Projektuebersicht";
        _ = LoadDataAsync();
    }

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private IReadOnlyList<ProjectDetailModel> _projects = [];

    public void OnNavigatedTo(object? parameter)
    {
    }

    public void OnNavigatedFrom()
    {
    }

    private async Task LoadDataAsync()
    {
        using (BeginBusy("Lade Projekte..."))
        {
            try
            {
                var (_, result) = await Mediator.Request(new GetCvHttpRequest());

                if (result?.Projects is not null)
                {
                    Projects = result.Projects
                        .Select(p => new ProjectDetailModel(
                            Name: p.Name,
                            Framework: p.Framework ?? "N/A",
                            AppStoreUrl: p.AppStoreUrl,
                            PlayStoreUrl: p.PlayStoreUrl,
                            WebsiteUrl: p.WebsiteUrl,
                            Functions: p.Functions.ToList(),
                            TechnicalAspects: p.TechnicalAspects.ToList()))
                        .ToList();
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load projects from API: {ex.Message}");
            }

            // Fallback to mock data if API fails
            LoadMockData();
        }
    }

    private void LoadMockData()
    {
        Projects =
        [
            new ProjectDetailModel(
                Name: "Orderlyze",
                Framework: "Xamarin.Forms / MAUI",
                AppStoreUrl: "https://apps.apple.com/at/app/orderlyze/id1495015799",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=orderlyze.com",
                WebsiteUrl: "https://www.orderlyze.com",
                Functions:
                [
                    "Rechnungen erstellen und ausdrucken",
                    "Tischplan mit Drag und Drop",
                    "Reservierungen erstellen",
                    "Tages-, Monats- und Jahresberichte",
                    "Mitarbeiterverwaltung",
                    "Statistiken und Analysen",
                    "Synchronisation zwischen Geraeten"
                ],
                TechnicalAspects:
                [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "State Changes: ReactiveUI",
                    "Server: OpenApi Generierung",
                    "Bluetooth: Shiny.BLE",
                    "Datenbank: sqlite-net (Async)",
                    "UI: Syncfusion"
                ]),

            new ProjectDetailModel(
                Name: "Colop E-Mark",
                Framework: "Xamarin.Forms",
                AppStoreUrl: "https://apps.apple.com/at/app/colop-e-mark/id1397292575",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=com.colop.colopemark",
                WebsiteUrl: "https://www.colop.com/de/digital-produkte",
                Functions:
                [
                    "Editor fuer mehrfarbige Abdruecke",
                    "QR und Barcode Generierung",
                    "Eigenes Benutzersystem",
                    "Video-Wizard Einfuehrung",
                    "WLAN Drucker Verbindung",
                    "7 Sprachen"
                ],
                TechnicalAspects:
                [
                    "Framework: Xamarin.Forms",
                    "App Lifecycle: Prism",
                    "Design Pattern: Prism MVVM",
                    "Editor: SkiaSharp",
                    "QR-Code: ZXing",
                    "Datenbank: SQLite .NET"
                ]),

            new ProjectDetailModel(
                Name: "Sybos",
                Framework: "Xamarin.Forms / MAUI",
                AppStoreUrl: "https://apps.apple.com/at/app/sybos/id1176062382",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=at.syPhone",
                WebsiteUrl: "https://www.sybos.net",
                Functions:
                [
                    "Chat mit Push-Notifications",
                    "QR-Code Scanner fuer Material",
                    "Kalender fuer Veranstaltungen",
                    "Zusatzalarmierung mit Push",
                    "Dark Mode",
                    "Materialverwaltung",
                    "Server Synchronisation"
                ],
                TechnicalAspects:
                [
                    "Programmiersprachen: C#, XAML",
                    "App Lifecycle: Prism",
                    "State Changes: ReactiveUI",
                    "Server: Refit REST",
                    "QR-Code: ZXing",
                    "Push: Shiny Library"
                ])
        ];
    }

    [UnoCommand]
    private async Task GoBackAsync()
    {
        await Navigator.NavigateBackAsync(this);
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
}

public record ProjectDetailModel(
    string Name,
    string Framework,
    string? AppStoreUrl,
    string? PlayStoreUrl,
    string? WebsiteUrl,
    IReadOnlyList<string> Functions,
    IReadOnlyList<string> TechnicalAspects);
