using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using Lebenslauf.Features.Cv.Contracts.Models;
using Lebenslauf.Features.Cv.Services;
using Microsoft.Extensions.Logging;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class ProjectsViewModel : PageViewModel, INavigationAware
{
    private readonly IProfileStateService _profileStateService;

    public ProjectsViewModel(BaseServices baseServices, IProfileStateService profileStateService) : base(baseServices)
    {
        _profileStateService = profileStateService;
        _profileStateService.ProfileChanged += OnProfileChanged;
        Title = "Projektuebersicht";
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

    private void OnProfileChanged(object? sender, ProfileChangedEventArgs e)
    {
        // Reload data when profile changes
        _ = LoadDataAsync(CancellationToken.None);
    }

    private async Task LoadDataAsync(CancellationToken ct)
    {
        using (BeginBusy("Lade Projekte..."))
        {
            try
            {
                var (_, result) = await Mediator.Request(new GetCvHttpRequest { ProfileSlug = _profileStateService.ActiveProfileSlug ?? "" }, ct);
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
                            (p.SubProjects ?? []).Select(sp => new SubProjectModel(
                                sp.Id,
                                sp.Name,
                                sp.Description,
                                sp.Framework,
                                (sp.Technologies ?? []).ToList()
                            )).ToList(),
                            p.AppStoreUrl,
                            p.PlayStoreUrl,
                            p.AppGalleryUrl,
                            p.WebsiteUrl,
                            p.ImageUrl,
                            p.StartDate,
                            p.EndDate,
                            p.IsCurrent))
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
