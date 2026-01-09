using Lebenslauf.Features.Cv.Presentation;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Uno.Extensions.Navigation;

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

    private IRouteNotifier? _routeNotifier;
    private DispatcherQueueTimer? _pollTimer;
    private Type? _lastPageType;

    public MainPage()
    {
        this.InitializeComponent();
        this.Loaded += OnLoaded;
        this.Unloaded += OnUnloaded;
    }

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        // Subscribe to IRouteNotifier for NavigationView-initiated navigation
        if (Application.Current is App app && app.Host != null)
        {
            _routeNotifier = app.Host.Services.GetService<IRouteNotifier>();
            if (_routeNotifier != null)
            {
                _routeNotifier.RouteChanged += OnRouteChanged;
            }
        }

        // Also use polling as fallback for programmatic navigation
        // that doesn't trigger RouteChanged (e.g., Navigator.NavigateViewModelAsync)
        _pollTimer = DispatcherQueue.CreateTimer();
        _pollTimer.Interval = TimeSpan.FromMilliseconds(150);
        _pollTimer.Tick += OnPollTimerTick;
        _pollTimer.Start();

        // Initial update
        UpdateSelectionFromVisualTree();
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        if (_routeNotifier != null)
        {
            _routeNotifier.RouteChanged -= OnRouteChanged;
            _routeNotifier = null;
        }

        _pollTimer?.Stop();
        _pollTimer = null;
    }

    private void OnRouteChanged(object? sender, RouteChangedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(UpdateSelectionFromVisualTree);
    }

    private void OnPollTimerTick(DispatcherQueueTimer sender, object args)
    {
        var page = FindCurrentPage(NavigationContent);
        var currentType = page?.GetType();

        if (currentType != null && currentType != _lastPageType)
        {
            _lastPageType = currentType;
            UpdateNavigationSelection(currentType);
        }
    }

    private void UpdateSelectionFromVisualTree()
    {
        var page = FindCurrentPage(NavigationContent);
        if (page != null)
        {
            _lastPageType = page.GetType();
            UpdateNavigationSelection(_lastPageType);
        }
    }

    private void UpdateNavigationSelection(Type pageType)
    {
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
}
