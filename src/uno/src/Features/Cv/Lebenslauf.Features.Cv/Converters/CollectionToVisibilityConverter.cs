using System.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace Lebenslauf.Features.Cv.Converters;

/// <summary>
/// Converts empty collections to Visibility.Collapsed, and non-empty collections to Visibility.Visible.
/// </summary>
public class CollectionToVisibilityConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, string? language)
    {
        if (value is IEnumerable enumerable)
        {
            // Check if the enumerable has any items
            var enumerator = enumerable.GetEnumerator();
            try
            {
                if (enumerator.MoveNext())
                {
                    return Visibility.Visible;
                }
            }
            finally
            {
                (enumerator as IDisposable)?.Dispose();
            }
        }

        return Visibility.Collapsed;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, string? language)
    {
        throw new NotImplementedException();
    }
}
