using CommunityToolkit.Mvvm.ComponentModel;
using Lebenslauf.Core.ApiClient.Generated;
using Lebenslauf.Features.Cv.Contracts.Models;
using Lebenslauf.Features.Cv.Services;
using Microsoft.Extensions.Logging;
using UnoFramework.Contracts.Navigation;
using UnoFramework.Generators;
using UnoFramework.ViewModels;

namespace Lebenslauf.Features.Cv.Presentation;

public partial class AdminViewModel : PageViewModel, INavigationAware
{
    private const string AdminPassword = "Admin!";
    private readonly IProfileStateService _profileStateService;

    public AdminViewModel(BaseServices baseServices, IProfileStateService profileStateService) : base(baseServices)
    {
        _profileStateService = profileStateService;
    }

    [ObservableProperty]
    private IReadOnlyList<ProfileModel> _profiles = [];

    [ObservableProperty]
    private ProfileModel? _selectedProfile;

    [ObservableProperty]
    private string? _activeProfileSlug;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotAuthenticated))]
    private bool _isAuthenticated;

    public bool IsNotAuthenticated => !IsAuthenticated;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPasswordError))]
    private string _passwordInput = string.Empty;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HasPasswordError))]
    private string? _passwordError;

    public bool HasPasswordError => !string.IsNullOrEmpty(PasswordError);

    public void OnNavigatedTo(object? parameter)
    {
        OnNavigatingTo();
        IsAuthenticated = false;
        PasswordInput = string.Empty;
        PasswordError = null;
        ActiveProfileSlug = _profileStateService.ActiveProfileSlug;
    }

    public void OnNavigatedFrom()
    {
        OnNavigatingFrom();
    }

    private async Task LoadProfilesAsync(CancellationToken ct)
    {
        using (BeginBusy("Lade Profile..."))
        {
            try
            {
                var (_, result) = await Mediator.Request(new GetProfilesHttpRequest(), ct);
                ct.ThrowIfCancellationRequested();

                if (result?.Profiles is not null)
                {
                    Profiles = result.Profiles.Select(p => new ProfileModel(
                        p.Id,
                        p.Slug,
                        p.Name,
                        p.Description,
                        p.IsDefault
                    )).ToList();

                    // Select the currently active profile
                    SelectedProfile = Profiles.FirstOrDefault(p => p.Slug == ActiveProfileSlug)
                        ?? Profiles.FirstOrDefault(p => p.IsDefault);
                    return;
                }
            }
            catch (OperationCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to load profiles from API");
            }

            // Fallback mock data
            Profiles =
            [
                new ProfileModel(Guid.Empty, "default", "Vollstaendiges Profil", "Zeigt alle Skills und Projekte", true),
                new ProfileModel(Guid.Empty, "backend", "Backend Developer", "Fokus auf .NET, ASP.NET, APIs", false),
                new ProfileModel(Guid.Empty, "mobile", "Mobile Developer", "Fokus auf MAUI und Uno Platform", false)
            ];
            SelectedProfile = Profiles.FirstOrDefault(p => p.IsDefault);
        }
    }

    [UnoCommand]
    private async Task ActivateProfileAsync(ProfileModel? profile)
    {
        if (profile is null) return;

        await _profileStateService.SetActiveProfileAsync(profile.IsDefault ? null : profile.Slug);
        ActiveProfileSlug = _profileStateService.ActiveProfileSlug;
        SelectedProfile = profile;

        Logger.LogInformation("Activated profile: {ProfileSlug}", profile.Slug);
    }

    [UnoCommand]
    private async Task GoBackAsync()
    {
        await Navigator.NavigateBackAsync(this);
    }

    [UnoCommand]
    private async Task LoginAsync()
    {
        if (PasswordInput == AdminPassword)
        {
            IsAuthenticated = true;
            PasswordError = null;
            _ = LoadProfilesAsync(NavigationToken);
        }
        else
        {
            PasswordError = "Falsches Passwort";
            PasswordInput = string.Empty;
        }

        await Task.CompletedTask;
    }
}
