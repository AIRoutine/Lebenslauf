using Lebenslauf.Features.Cv.Presentation;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;

namespace Lebenslauf.App.Presentation;

public sealed partial class MainPage : Page, IContentControlProvider
{
    public ContentControl ContentControl => NavigationContent;

    // Map page types to NavigationViewItem Content
    private static readonly Dictionary<Type, string> PageToNavItemMap = new()
    {
        { typeof(HomePage), "Home" },
        { typeof(CvPage), "Lebenslauf" },
        { typeof(SkillsPage), "Programmierkenntnisse" },
        { typeof(ProjectsPage), "Projektuebersicht" }
    };

    private Frame? _navigationFrame;
    private DispatcherQueueTimer? _initTimer;
    private int _initAttempts;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += OnLoaded;
        this.Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Use a short timer to find and subscribe to the inner Frame
        // This ensures the visual tree is fully built
        _initAttempts = 0;
        _initTimer = DispatcherQueue.CreateTimer();
        _initTimer.Interval = TimeSpan.FromMilliseconds(50);
        _initTimer.Tick += OnInitTimerTick;
        _initTimer.Start();
    }

    private void OnInitTimerTick(DispatcherQueueTimer sender, object args)
    {
        _initAttempts++;

        // Try to find and subscribe to Frame
        if (_navigationFrame == null)
        {
            _navigationFrame = FindNavigationFrame(NavigationContent);
            if (_navigationFrame != null)
            {
                _navigationFrame.Navigated += OnFrameNavigated;
            }
        }

        // Update selection from current page
        UpdateSelectionFromVisualTree();

        // Stop after successful subscription or max attempts (500ms)
        if (_navigationFrame != null || _initAttempts >= 10)
        {
            _initTimer?.Stop();
            _initTimer = null;
        }
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        _initTimer?.Stop();
        _initTimer = null;

        if (_navigationFrame != null)
        {
            _navigationFrame.Navigated -= OnFrameNavigated;
            _navigationFrame = null;
        }
    }

    private void OnFrameNavigated(object sender, NavigationEventArgs e)
    {
        // Use dispatcher to ensure UI updates properly
        DispatcherQueue.TryEnqueue(() => UpdateSelectionFromVisualTree());
    }

    private void UpdateSelectionFromVisualTree()
    {
        var page = FindCurrentPage(NavigationContent);
        if (page == null)
            return;

        var pageType = page.GetType();
        if (PageToNavItemMap.TryGetValue(pageType, out var navItemContent))
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

    private Page? FindCurrentPage(DependencyObject parent)
    {
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is Page page && page is not MainPage)
            {
                return page;
            }

            var result = FindCurrentPage(child);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }

    private Frame? FindNavigationFrame(DependencyObject parent)
    {
        var count = VisualTreeHelper.GetChildrenCount(parent);
        for (var i = 0; i < count; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);

            if (child is Frame frame && frame.Name == "NavigationFrame")
            {
                return frame;
            }

            var result = FindNavigationFrame(child);
            if (result != null)
            {
                return result;
            }
        }
        return null;
    }
}
