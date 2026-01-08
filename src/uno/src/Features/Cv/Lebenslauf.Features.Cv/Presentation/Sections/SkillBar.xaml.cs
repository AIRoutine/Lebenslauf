using Windows.Foundation;

namespace Lebenslauf.Features.Cv.Presentation.Sections;

public sealed partial class SkillBar : UserControl
{
    public static readonly DependencyProperty SkillNameProperty =
        DependencyProperty.Register(
            nameof(SkillName),
            typeof(string),
            typeof(SkillBar),
            new PropertyMetadata(string.Empty, OnPropertyChanged));

    public static readonly DependencyProperty PercentageProperty =
        DependencyProperty.Register(
            nameof(Percentage),
            typeof(int),
            typeof(SkillBar),
            new PropertyMetadata(0, OnPropertyChanged));

    public string SkillName
    {
        get => (string)GetValue(SkillNameProperty);
        set => SetValue(SkillNameProperty, value);
    }

    public int Percentage
    {
        get => (int)GetValue(PercentageProperty);
        set => SetValue(PercentageProperty, value);
    }

    public SkillBar()
    {
        this.InitializeComponent();
        this.Loaded += OnLoaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        UpdateDisplay();
    }

    private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is SkillBar skillBar)
        {
            skillBar.UpdateDisplay();
        }
    }

    private void UpdateDisplay()
    {
        SkillNameText.Text = SkillName;
        PercentageText.Text = $"{Percentage}%";

        // Update fill bar width based on percentage
        if (ActualWidth > 0)
        {
            FillBar.Width = ActualWidth * Percentage / 100.0;
        }
        else
        {
            // Use a relative width approach
            FillBar.Width = 200 * Percentage / 100.0; // Default width assumption
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var result = base.MeasureOverride(availableSize);
        UpdateFillBarWidth(availableSize.Width);
        return result;
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var result = base.ArrangeOverride(finalSize);
        UpdateFillBarWidth(finalSize.Width);
        return result;
    }

    private void UpdateFillBarWidth(double totalWidth)
    {
        if (totalWidth > 0 && !double.IsInfinity(totalWidth))
        {
            FillBar.Width = totalWidth * Percentage / 100.0;
        }
    }
}
