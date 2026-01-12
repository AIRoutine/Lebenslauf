using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using Lebenslauf.Features.Cv.Contracts.Models;
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
    private IReadOnlyList<ProjectModel> _projects = [];

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
                    // Use the same ProjectModel as CvViewModel - single source of truth
                    Projects = result.Projects
                        .Select(p => new ProjectModel(
                            p.Id,
                            p.Name,
                            p.Description,
                            p.Framework,
                            (p.Technologies ?? []).ToList(),
                            (p.Functions ?? []).ToList(),
                            (p.TechnicalAspects ?? []).ToList(),
                            p.AppStoreUrl,
                            p.PlayStoreUrl,
                            p.WebsiteUrl,
                            p.ImageUrl))
                        .ToList();
                    return;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Failed to load projects from API: {ex.Message}");
            }

            // API is single source of truth - no mock data fallback
            Projects = [];
        }
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
