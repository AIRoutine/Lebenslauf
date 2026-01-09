using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Imaging;
using Windows.Graphics.Imaging;
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

    public static readonly DependencyProperty PageTitleProperty =
        DependencyProperty.Register(
            nameof(PageTitle),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata(string.Empty));

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

    public string PageTitle
    {
        get => (string)GetValue(PageTitleProperty);
        set => SetValue(PageTitleProperty, value);
    }

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

            // Render the target element to bitmap
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(target);

            // Get pixel data
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var pixels = pixelBuffer.ToArray();

            // Get dimensions
            var width = (uint)renderTargetBitmap.PixelWidth;
            var height = (uint)renderTargetBitmap.PixelHeight;

            // Show save file picker
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = FileName
            };
            savePicker.FileTypeChoices.Add("PNG Image", [".png"]);

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

            await SaveAsPngAsync(file, pixels, width, height);

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

    private static async Task SaveAsPngAsync(StorageFile file, byte[] pixels, uint width, uint height)
    {
        using var stream = await file.OpenAsync(FileAccessMode.ReadWrite);
        var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, stream);

        encoder.SetPixelData(
            BitmapPixelFormat.Bgra8,
            BitmapAlphaMode.Premultiplied,
            width,
            height,
            96, // DPI X
            96, // DPI Y
            pixels);

        await encoder.FlushAsync();
    }
}
