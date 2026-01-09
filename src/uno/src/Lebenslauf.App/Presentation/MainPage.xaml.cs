using Microsoft.UI.Xaml.Controls;

namespace Lebenslauf.App.Presentation;

public sealed partial class MainPage : Page, IContentControlProvider
{
    public ContentControl ContentControl => NavigationContent;

    public MainPage()
    {
        this.InitializeComponent();
    }
}
