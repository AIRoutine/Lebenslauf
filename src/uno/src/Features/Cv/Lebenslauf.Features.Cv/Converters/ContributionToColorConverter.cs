using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace Lebenslauf.Features.Cv.Converters;

/// <summary>
/// Converts GitHub contribution count to a color based on activity level.
/// Uses GitHub's contribution graph color scheme.
/// </summary>
public class ContributionToColorConverter : IValueConverter
{
    // GitHub contribution colors (dark theme - matching official GitHub)
    private static readonly SolidColorBrush Level0 = new(ColorHelper.FromArgb(255, 22, 27, 34));   // #161B22 - No activity (dark gray)
    private static readonly SolidColorBrush Level1 = new(ColorHelper.FromArgb(255, 14, 68, 41));   // #0E4429 - Low (dark green)
    private static readonly SolidColorBrush Level2 = new(ColorHelper.FromArgb(255, 0, 109, 50));   // #006D32 - Medium
    private static readonly SolidColorBrush Level3 = new(ColorHelper.FromArgb(255, 38, 166, 65));  // #26A641 - High
    private static readonly SolidColorBrush Level4 = new(ColorHelper.FromArgb(255, 57, 211, 83));  // #39D353 - Very High (bright green)

    public object Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is not int count)
            return Level0;

        // Adjusted thresholds to match GitHub's distribution (more dark, fewer bright)
        return count switch
        {
            0 => Level0,
            >= 1 and <= 4 => Level1,   // Dark green - most common
            >= 5 and <= 9 => Level2,   // Medium green
            >= 10 and <= 16 => Level3, // Bright green
            _ => Level4                 // Brightest - rare
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotImplementedException();
    }
}
