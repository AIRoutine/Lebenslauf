namespace Lebenslauf.Features.Cv.Services;

/// <summary>
/// Service to manage the active CV profile selection.
/// </summary>
public interface IProfileStateService
{
    /// <summary>
    /// Gets the currently active profile slug.
    /// </summary>
    string? ActiveProfileSlug { get; }

    /// <summary>
    /// Sets the active profile and notifies listeners.
    /// </summary>
    /// <param name="slug">The profile slug to activate (null for default).</param>
    Task SetActiveProfileAsync(string? slug);

    /// <summary>
    /// Event raised when the active profile changes.
    /// </summary>
    event EventHandler<ProfileChangedEventArgs>? ProfileChanged;
}

/// <summary>
/// Event args for profile change notifications.
/// </summary>
public class ProfileChangedEventArgs : EventArgs
{
    public string? OldSlug { get; }
    public string? NewSlug { get; }

    public ProfileChangedEventArgs(string? oldSlug, string? newSlug)
    {
        OldSlug = oldSlug;
        NewSlug = newSlug;
    }
}
