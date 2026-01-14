using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using Lebenslauf.Features.Cv.Contracts.Models;
using Microsoft.Extensions.Logging;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class ProjectsViewModel : PageViewModel, INavigationAware
{
    public ProjectsViewModel(BaseServices baseServices) : base(baseServices)
    {
        Title = "Projektuebersicht";

        // Trigger initial load - OnNavigatedTo may not be called by navigation system
        OnNavigatingTo();
        _ = LoadDataAsync(NavigationToken);
    }

    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private IReadOnlyList<ProjectModel> _projects = [];

    public void OnNavigatedTo(object? parameter)
    {
        OnNavigatingTo();
        _ = LoadDataAsync(NavigationToken);
    }

    public void OnNavigatedFrom()
    {
        OnNavigatingFrom();
    }

    private async Task LoadDataAsync(CancellationToken ct)
    {
        using (BeginBusy("Lade Projekte..."))
        {
            try
            {
                var (_, result) = await Mediator.Request(new GetCvHttpRequest(), ct);
                ct.ThrowIfCancellationRequested();

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
                            p.AppGalleryUrl,
                            p.WebsiteUrl,
                            p.ImageUrl))
                        .ToList();
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                // Navigation cancelled - don't update UI
                return;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load projects from API");
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
