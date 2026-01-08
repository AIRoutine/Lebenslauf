using Lebenslauf.Features.Cv.Presentation;
using Microsoft.UI.Xaml.Controls;

namespace Lebenslauf.App.Presentation;

public sealed partial class Shell : UserControl, IContentControlProvider
{
    public ContentControl ContentControl => Splash;

    public Shell()
    {
        this.InitializeComponent();
        Loaded += Shell_Loaded;
    }

    private void Shell_Loaded(object sender, RoutedEventArgs e)
    {
        // Select first item by default
        if (NavView.MenuItems.FirstOrDefault() is NavigationViewItem firstItem)
        {
            NavView.SelectedItem = firstItem;
        }
    }

    private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        if (args.InvokedItemContainer is not NavigationViewItem item)
            return;

        var tag = item.Tag?.ToString();
        if (string.IsNullOrEmpty(tag))
            return;

        // Navigate using Frame directly
        Type? pageType = tag switch
        {
            "Cv" => typeof(CvPage),
            "Skills" => typeof(SkillsPage),
            "Projects" => typeof(ProjectsPage),
            _ => null
        };

        if (pageType is not null && RootFrame.CurrentSourcePageType != pageType)
        {
            RootFrame.Navigate(pageType);
        }
    }
}
