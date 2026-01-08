namespace Lebenslauf.Features.Cv.Presentation.Sections;

public sealed partial class ProjectsSection : UserControl
{
    public static readonly DependencyProperty OpenUrlCommandProperty =
        DependencyProperty.Register(
            nameof(OpenUrlCommand),
            typeof(System.Windows.Input.ICommand),
            typeof(ProjectsSection),
            new PropertyMetadata(null));

    public System.Windows.Input.ICommand? OpenUrlCommand
    {
        get => (System.Windows.Input.ICommand?)GetValue(OpenUrlCommandProperty);
        set => SetValue(OpenUrlCommandProperty, value);
    }

    public ProjectsSection()
    {
        this.InitializeComponent();
    }
}
