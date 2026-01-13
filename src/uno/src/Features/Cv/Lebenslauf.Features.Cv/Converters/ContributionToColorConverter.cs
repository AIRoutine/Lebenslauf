using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Lebenslauf.Features.Cv.Converters;

/// <summary>
/// Converts GitHub contribution count to a color based on activity level.
/// Uses GitHub's exact contribution graph color scheme (dark theme).
/// </summary>
public class ContributionToColorConverter : IValueConverter
{
    // GitHub contribution colors (dark theme - CSS variables from github.com)
    // --color-calendar-graph-day-bg: #161b22
    // --color-calendar-graph-day-L1-bg: #0e4429
    // --color-calendar-graph-day-L2-bg: #006d32
    // --color-calendar-graph-day-L3-bg: #26a641
    // --color-calendar-graph-day-L4-bg: #39d353
    private static readonly SolidColorBrush Level0 = new(Color.FromArgb(255, 0x16, 0x1B, 0x22));  // #161B22
    private static readonly SolidColorBrush Level1 = new(Color.FromArgb(255, 0x0E, 0x44, 0x29));  // #0E4429
    private static readonly SolidColorBrush Level2 = new(Color.FromArgb(255, 0x00, 0x6D, 0x32));  // #006D32
    private static readonly SolidColorBrush Level3 = new(Color.FromArgb(255, 0x26, 0xA6, 0x41));  // #26A641
    private static readonly SolidColorBrush Level4 = new(Color.FromArgb(255, 0x39, 0xD3, 0x53));  // #39D353

    public object Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is not int count)
            return Level0;

        // GitHub uses quartile-based thresholds. Higher thresholds for active contributors.
        return count switch
        {
            0 => Level0,
            >= 1 and <= 9 => Level1,     // Low activity
            >= 10 and <= 19 => Level2,   // Medium activity
            >= 20 and <= 29 => Level3,   // High activity
            _ => Level4                   // Very high activity (30+)
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotImplementedException();
    }
}
