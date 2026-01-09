using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Media.Imaging;
using SkiaSharp;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;

namespace Lebenslauf.Core.Styles.Controls;

public sealed partial class ExportHeaderControl : UserControl
{
    /// <summary>
    /// Static reference to the main window for file picker initialization.
    /// Set this from App.xaml.cs during startup.
    /// </summary>
    public static Window? MainWindow { get; set; }

    public static readonly DependencyProperty ExportTargetProperty =
        DependencyProperty.Register(
            nameof(ExportTarget),
            typeof(FrameworkElement),
            typeof(ExportHeaderControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty FileNameProperty =
        DependencyProperty.Register(
            nameof(FileName),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata("Export"));

    public FrameworkElement? ExportTarget
    {
        get => (FrameworkElement?)GetValue(ExportTargetProperty);
        set => SetValue(ExportTargetProperty, value);
    }

    public string FileName
    {
        get => (string)GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }

    public ExportHeaderControl()
    {
        this.InitializeComponent();
    }

    private async void OnExportClick(object sender, RoutedEventArgs e)
    {
        var target = ExportTarget;
        if (target is null)
        {
            return;
        }

        try
        {
            ExportButton.IsEnabled = false;

            // Find ScrollViewer to expand its content to full size
            var scrollViewer = FindScrollViewer(target);
            double originalWidth = double.NaN;
            double originalHeight = double.NaN;
            double originalTargetHeight = double.NaN;
            FrameworkElement? scrollContent = null;

            if (scrollViewer?.Content is FrameworkElement content)
            {
                scrollContent = content;

                // Save original sizes
                originalWidth = content.Width;
                originalHeight = content.Height;
                originalTargetHeight = target.Height;

                // Measure content at infinite size to get full desired size
                content.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                var desiredSize = content.DesiredSize;

                // Set explicit size to render full content
                var fullContentHeight = Math.Max(desiredSize.Height, scrollViewer.ExtentHeight);
                content.Width = Math.Max(desiredSize.Width, scrollViewer.ViewportWidth);
                content.Height = fullContentHeight;

                // Also expand the target if it's a wrapper (to include header + full content)
                if (target != scrollViewer && target != content)
                {
                    // Calculate extra height from header/other elements
                    var headerHeight = target.ActualHeight - scrollViewer.ActualHeight;
                    target.Height = headerHeight + fullContentHeight;
                }

                // Force layout update
                content.UpdateLayout();
                target.UpdateLayout();

                // Small delay to ensure layout is complete
                await Task.Delay(150);
            }

            // Render the ENTIRE target (including header wrapper)
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(target);

            // Restore original sizes
            if (scrollContent is not null)
            {
                scrollContent.Width = originalWidth;
                scrollContent.Height = originalHeight;
                scrollContent.UpdateLayout();
            }
            if (!double.IsNaN(originalTargetHeight))
            {
                target.Height = originalTargetHeight;
                target.UpdateLayout();
            }

            // Get pixel data
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();

            // Get dimensions
            var width = renderTargetBitmap.PixelWidth;
            var height = renderTargetBitmap.PixelHeight;

            // Show save file picker
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = FileName
            };
            savePicker.FileTypeChoices.Add("PDF Document", [".pdf"]);

            // Initialize picker with window handle (required for WinUI)
            if (MainWindow is not null)
            {
                var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
                WinRT.Interop.InitializeWithWindow.Initialize(savePicker, hwnd);
            }

            var file = await savePicker.PickSaveFileAsync();
            if (file is null)
            {
                return;
            }

            // Defer updates to prevent file access issues
            CachedFileManager.DeferUpdates(file);

            await SaveAsPdfAsync(file, pixels, width, height);

            // Complete the file updates
            await CachedFileManager.CompleteUpdatesAsync(file);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Export failed: {ex.Message}");
        }
        finally
        {
            ExportButton.IsEnabled = true;
        }
    }

    private static ScrollViewer? FindScrollViewer(DependencyObject element)
    {
        if (element is ScrollViewer sv)
        {
            return sv;
        }

        var childCount = VisualTreeHelper.GetChildrenCount(element);
        for (var i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(element, i);
            var result = FindScrollViewer(child);
            if (result is not null)
            {
                return result;
            }
        }

        return null;
    }

    private static async Task SaveAsPdfAsync(StorageFile file, byte[] pixels, int width, int height)
    {
        await Task.Run(async () =>
        {
            // Create SKBitmap from pixel data (BGRA format from RenderTargetBitmap)
            using var bitmap = new SKBitmap(width, height, SKColorType.Bgra8888, SKAlphaType.Premul);
            var handle = System.Runtime.InteropServices.GCHandle.Alloc(pixels, System.Runtime.InteropServices.GCHandleType.Pinned);
            try
            {
                bitmap.InstallPixels(
                    new SKImageInfo(width, height, SKColorType.Bgra8888, SKAlphaType.Premul),
                    handle.AddrOfPinnedObject(),
                    width * 4);

                // Create PDF document
                using var stream = await file.OpenStreamForWriteAsync();
                stream.SetLength(0); // Clear existing content

                using var document = SKDocument.CreatePdf(stream);
                using var canvas = document.BeginPage(width, height);

                // Draw the bitmap onto the PDF canvas
                canvas.DrawBitmap(bitmap, 0, 0);

                document.EndPage();
                document.Close();
            }
            finally
            {
                handle.Free();
            }
        });
    }
}
