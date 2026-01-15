using Lebenslauf;
using Shiny.Extensions.DependencyInjection;
using Windows.Storage;

namespace Lebenslauf.Features.Cv.Services;

/// <summary>
/// Service to manage the active CV profile selection with persistence.
/// </summary>
[Service(UnoService.Lifetime, TryAdd = UnoService.TryAdd)]
public class ProfileStateService : IProfileStateService
{
    private const string ProfileSlugKey = "ActiveProfileSlug";
    private string? _activeProfileSlug;
    private bool _initialized;

    public string? ActiveProfileSlug
    {
        get
        {
            EnsureInitialized();
            return _activeProfileSlug;
        }
    }

    public event EventHandler<ProfileChangedEventArgs>? ProfileChanged;

    public Task SetActiveProfileAsync(string? slug)
    {
        EnsureInitialized();

        var oldSlug = _activeProfileSlug;
        if (oldSlug == slug)
        {
            return Task.CompletedTask;
        }

        _activeProfileSlug = slug;

        // Persist the setting
        try
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (string.IsNullOrEmpty(slug))
            {
                settings.Values.Remove(ProfileSlugKey);
            }
            else
            {
                settings.Values[ProfileSlugKey] = slug;
            }
        }
        catch
        {
            // Ignore storage errors - value is still in memory
        }

        // Notify listeners
        ProfileChanged?.Invoke(this, new ProfileChangedEventArgs(oldSlug, slug));

        return Task.CompletedTask;
    }

    private void EnsureInitialized()
    {
        if (_initialized) return;

        try
        {
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.TryGetValue(ProfileSlugKey, out var value) && value is string slug)
            {
                _activeProfileSlug = slug;
            }
        }
        catch
        {
            // Ignore storage errors - use default (null)
        }

        _initialized = true;
    }
}
