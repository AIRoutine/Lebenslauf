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
    // GitHub contribution colors (light to dark green)
    private static readonly SolidColorBrush Level0 = new(ColorHelper.FromArgb(255, 235, 237, 240)); // #EBEDF0 - No activity
    private static readonly SolidColorBrush Level1 = new(ColorHelper.FromArgb(255, 155, 233, 168)); // #9BE9A8 - Low
    private static readonly SolidColorBrush Level2 = new(ColorHelper.FromArgb(255, 64, 196, 99));   // #40C463 - Medium
    private static readonly SolidColorBrush Level3 = new(ColorHelper.FromArgb(255, 48, 161, 78));   // #30A14E - High
    private static readonly SolidColorBrush Level4 = new(ColorHelper.FromArgb(255, 33, 110, 57));   // #216E39 - Very High

    public object Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is not int count)
            return Level0;

        return count switch
        {
            0 => Level0,
            >= 1 and <= 5 => Level1,
            >= 6 and <= 12 => Level2,
            >= 13 and <= 20 => Level3,
            _ => Level4
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotImplementedException();
    }
}
