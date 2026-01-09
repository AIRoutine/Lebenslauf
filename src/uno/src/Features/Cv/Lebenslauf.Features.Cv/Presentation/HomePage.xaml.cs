using Uno.Extensions.Navigation;

namespace Lebenslauf.Features.Cv.Presentation;

public sealed partial class HomePage : Page
{
    public HomePage()
    {
        this.InitializeComponent();
    }

    private async void OnCvClick(object sender, RoutedEventArgs e)
    {
        await this.Navigator()!.NavigateRouteAsync(this, "Cv");
    }

    private async void OnSkillsClick(object sender, RoutedEventArgs e)
    {
        await this.Navigator()!.NavigateRouteAsync(this, "Skills");
    }

    private async void OnProjectsClick(object sender, RoutedEventArgs e)
    {
        await this.Navigator()!.NavigateRouteAsync(this, "Projects");
    }
}
