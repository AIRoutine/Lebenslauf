using CommunityToolkit.Mvvm.ComponentModel;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class ProjectsViewModel : PageViewModel, INavigationAware
{
    public ProjectsViewModel(BaseServices baseServices) : base(baseServices)
    {
        Title = "Projektuebersicht";
        LoadData();
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

    private void LoadData()
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
                ]),

            new ProjectDetailModel(
                Name: "Ekey Bionyx",
                Framework: "Xamarin.Forms",
                AppStoreUrl: "https://apps.apple.com/at/app/ekey-bionyx/id1484053054",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=net.ekey.bionyx",
                WebsiteUrl: "https://www.ekey.net/ekey-bionyx-app/",
                Functions:
                [
                    "Automatische Push-Nachrichten",
                    "Hoechste Sicherheit/Verschluesselung",
                    "Bluetooth zu Fingerscanner",
                    "Benutzermanagement/Rollensystem"
                ],
                TechnicalAspects:
                [
                    "Framework: Xamarin.Forms",
                    "Repository: Azure DevOps",
                    "Push: Azure Notification Hub",
                    "Bluetooth: Shiny",
                    "CI/CD: Azure DevOps",
                    "Animationen: Lottie"
                ]),

            new ProjectDetailModel(
                Name: "Miele Smart Home",
                Framework: "MAUI",
                AppStoreUrl: "https://apps.apple.com/at/app/miele-app-smart-home/id930406907",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=de.miele.infocontrol",
                WebsiteUrl: "https://www.miele.at/c/miele-app-2594.htm",
                Functions:
                [
                    "Hausgeraete mobil steuern",
                    "Push ueber Geraetestatus",
                    "Assistenten-System (NuGet)",
                    "Eigener Shop"
                ],
                TechnicalAspects:
                [
                    "Framework: MAUI",
                    "Repository: Bitbucket",
                    "Notifications: Firebase",
                    "Codegenerierung: Roslyn",
                    "Logging: Sentry",
                    "Animationen: Lottie"
                ]),

            new ProjectDetailModel(
                Name: "Asfinag",
                Framework: "MAUI",
                AppStoreUrl: "https://apps.apple.com/at/app/asfinag/id453459323",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=at.asfinag.unterwegs",
                WebsiteUrl: "https://www.asfinag.at/asfinag-app/",
                Functions:
                [
                    "Personalisierter Homescreen",
                    "1800+ Webcams",
                    "Verkehrsinfos und Baustellen",
                    "Raststationen und E-Ladestationen",
                    "Digitale Vignette",
                    "Routenplaner (Europa)",
                    "12 Sprachen"
                ],
                TechnicalAspects:
                [
                    "Framework: MAUI",
                    "App Lifecycle: Prism",
                    "Server: OpenApi, Polly, Scalar",
                    "UI: Syncfusion",
                    "Datenbank: SQLite .NET",
                    "Push: Cross Platform"
                ]),

            new ProjectDetailModel(
                Name: "PracticeBird",
                Framework: "Xamarin.Forms",
                AppStoreUrl: "https://apps.apple.com/us/app/practice-bird-interactive-sheet-music-and-scores/id1253492926",
                PlayStoreUrl: "https://play.google.com/store/apps/details?id=phonicscore.phonicscore_lite",
                WebsiteUrl: "https://www.practicebird.com",
                Functions:
                [
                    "Mitspielendes Notenblatt",
                    "MusicXML/MIDI Dateien",
                    "Playback mit Klavier/Schlagzeug",
                    "Stimme hervorheben",
                    "Facebook/Google Login"
                ],
                TechnicalAspects:
                [
                    "Framework: Xamarin.Forms",
                    "App Lifecycle: Prism",
                    "State Changes: ReactiveUI",
                    "Server: Refit REST",
                    "Notenblatt: SkiaSharp"
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
