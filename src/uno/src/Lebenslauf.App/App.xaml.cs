using Lebenslauf.Core.Startup;
using Lebenslauf.Core.Styles.Controls;
using Lebenslauf.Features.Cv.Presentation;
using Uno.Resizetizer;

namespace Lebenslauf.App;

public partial class App : Application
{
    public App()
    {
        this.InitializeComponent();
    }

    protected Window? MainWindow { get; private set; }

    public IHost? Host { get; private set; }

    protected async override void OnLaunched(LaunchActivatedEventArgs args)
    {
        var builder = this.CreateBuilder(args)
            .UseToolkitNavigation()
            .Configure(host => host
#if DEBUG
                .UseEnvironment(Environments.Development)
#endif
                .UseLogging(configure: (context, logBuilder) =>
                {
                    logBuilder
                        .SetMinimumLevel(
                            context.HostingEnvironment.IsDevelopment() ?
                                LogLevel.Information :
                                LogLevel.Warning)
                        .CoreLogLevel(LogLevel.Warning);
                }, enableUnoLogging: true)
                .UseConfiguration(configure: configBuilder =>
                    configBuilder
                        .EmbeddedSource<App>()
                        .Section<AppConfig>()
                )
                .UseLocalization()
                .ConfigureServices((context, services) =>
                {
                    services.AddAppServices();
                    services.AddTransient<Shell>();
                })
                .UseNavigation(RegisterRoutes)
            );
        MainWindow = builder.Window;

        // Set MainWindow reference for ExportHeaderControl
        ExportHeaderControl.MainWindow = MainWindow;

#if DEBUG
        MainWindow.UseStudio();
#endif
        MainWindow.SetWindowIcon();

        Host = await builder.NavigateAsync<Shell>();
    }

    private static void RegisterRoutes(IViewRegistry views, IRouteRegistry routes)
    {
        views.Register(
            new ViewMap(ViewModel: typeof(ShellViewModel)),
            new ViewMap<MainPage, MainViewModel>(),
            new ViewMap<CvPage, CvViewModel>(),
            new ViewMap<SkillsPage, SkillsViewModel>(),
            new ViewMap<ProjectsPage, ProjectsViewModel>()
        );

        routes.Register(
            new RouteMap("", View: views.FindByViewModel<ShellViewModel>(),
                Nested:
                [
                    new RouteMap("Main", View: views.FindByViewModel<MainViewModel>(),
                        Nested:
                        [
                            new ("Cv", View: views.FindByViewModel<CvViewModel>(), IsDefault: true),
                            new ("Skills", View: views.FindByViewModel<SkillsViewModel>()),
                            new ("Projects", View: views.FindByViewModel<ProjectsViewModel>())
                        ]
                    )
                ]
            )
        );
    }
}
