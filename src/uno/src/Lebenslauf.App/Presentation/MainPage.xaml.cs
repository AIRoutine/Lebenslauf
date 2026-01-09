using Lebenslauf.Features.Cv.Presentation;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Lebenslauf.App.Presentation;

public sealed partial class MainPage : Page, IContentControlProvider
{
    public ContentControl ContentControl => NavigationContent;

    private static readonly Dictionary<Type, string> PageToContentMap = new()
    {
        { typeof(HomePage), "Home" },
        { typeof(CvPage), "Lebenslauf" },
        { typeof(SkillsPage), "Programmierkenntnisse" },
        { typeof(ProjectsPage), "Projektuebersicht" }
    };

    private DispatcherQueueTimer? _pollTimer;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += OnLoaded;
        this.Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Start polling to check for page changes
        _pollTimer = DispatcherQueue.CreateTimer();
        _pollTimer.Interval = TimeSpan.FromMilliseconds(200);
        _pollTimer.IsRepeating = true;
        _pollTimer.Tick += OnPollTick;
        _pollTimer.Start();

        // Initial update
        UpdateNavigationSelection();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _pollTimer?.Stop();
        _pollTimer = null;
    }

    private Type? _lastPageType;

    private void OnPollTick(DispatcherQueueTimer sender, object args)
    {
        var page = FindCurrentPage();
        var pageType = page?.GetType();

        // Only update if page changed
        if (pageType != _lastPageType)
        {
            _lastPageType = pageType;
            UpdateNavigationSelection();
        }
    }

    private void UpdateNavigationSelection()
    {
        var page = FindCurrentPage();
        if (page == null)
            return;

        var pageType = page.GetType();

        // Find the matching NavigationViewItem and select it
        if (PageToContentMap.TryGetValue(pageType, out var navItemContent))
        {
            foreach (var item in NavView.MenuItems.OfType<NavigationViewItem>())
            {
                if (item.Content?.ToString() == navItemContent)
                {
                    if (NavView.SelectedItem != item)
                    {
                        NavView.SelectedItem = item;
                    }
                    return;
                }
            }
        }
    }

    private Page? FindCurrentPage()
    {
        // Search the visual tree for any Page that's not MainPage
        return FindPageInVisualTree(NavigationContent);
    }

    private Page? FindPageInVisualTree(DependencyObject parent)
    {
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            // Check if this child is a Page (but not MainPage)
            if (child is Page page && page is not MainPage)
            {
                return page;
            }

            // Recurse into children
            var result = FindPageInVisualTree(child);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}
