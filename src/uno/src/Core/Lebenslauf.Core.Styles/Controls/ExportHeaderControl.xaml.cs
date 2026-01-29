using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Net.Http;
#if __WASM__
using System.Runtime.InteropServices.JavaScript;
#else
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
#endif

namespace Lebenslauf.Core.Styles.Controls;

/// <summary>
/// Export control that downloads PDF/DOCX from the backend API.
/// </summary>
public sealed partial class ExportHeaderControl : UserControl
{
#if !__WASM__
    /// <summary>
    /// Static reference to the main window for file picker initialization.
    /// Set this from App.xaml.cs during startup.
    /// </summary>
    public static Window? MainWindow { get; set; }
#endif

    private static readonly HttpClient HttpClient = new();

    private static readonly string DefaultApiBaseUrl =
#if DEBUG
        "http://localhost:5292";
#else
        "https://lebenslauf-api.azurewebsites.net";
#endif

    public static readonly DependencyProperty ApiBaseUrlProperty =
        DependencyProperty.Register(
            nameof(ApiBaseUrl),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata(DefaultApiBaseUrl));

    public static readonly DependencyProperty ProfileSlugProperty =
        DependencyProperty.Register(
            nameof(ProfileSlug),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata(null));

    public static readonly DependencyProperty FileNameProperty =
        DependencyProperty.Register(
            nameof(FileName),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata("Lebenslauf"));

    public static readonly DependencyProperty ExportPathProperty =
        DependencyProperty.Register(
            nameof(ExportPath),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata("/api/cv/export"));

    /// <summary>
    /// Base URL of the API (e.g., https://lebenslauf-api.azurewebsites.net).
    /// </summary>
    public string ApiBaseUrl
    {
        get => (string)GetValue(ApiBaseUrlProperty);
        set => SetValue(ApiBaseUrlProperty, value);
    }

    /// <summary>
    /// Optional profile slug for profile-specific exports.
    /// </summary>
    public string? ProfileSlug
    {
        get => (string?)GetValue(ProfileSlugProperty);
        set => SetValue(ProfileSlugProperty, value);
    }

    /// <summary>
    /// Base filename for downloads (without extension).
    /// </summary>
    public string FileName
    {
        get => (string)GetValue(FileNameProperty);
        set => SetValue(FileNameProperty, value);
    }

    /// <summary>
    /// API export path (e.g., /api/cv/export or /api/cv/projects/export).
    /// </summary>
    public string ExportPath
    {
        get => (string)GetValue(ExportPathProperty);
        set => SetValue(ExportPathProperty, value);
    }

    public ExportHeaderControl()
    {
        this.InitializeComponent();
    }

#if __WASM__
    private async void OnPdfClick(object sender, RoutedEventArgs e)
    {
        await DownloadViaJavaScriptAsync("pdf", "application/pdf");
    }

    private async void OnDocxClick(object sender, RoutedEventArgs e)
    {
        await DownloadViaJavaScriptAsync("docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document");
    }

    private async Task DownloadViaJavaScriptAsync(string format, string mimeType)
    {
        try
        {
            SetButtonsEnabled(false);

            var url = BuildExportUrl(format);
            var fileName = $"{FileName}.{format}";

            // Download file bytes from API
            var bytes = await HttpClient.GetByteArrayAsync(url);

            // Trigger browser download via JavaScript interop
            var base64 = Convert.ToBase64String(bytes);
#pragma warning disable CA1416 // Platform compatibility - only called when __WASM__ is defined
            JsInterop.TriggerBrowserDownload(base64, mimeType, fileName);
#pragma warning restore CA1416
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Download failed: {ex.Message}");
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }
#else
    private async void OnPdfClick(object sender, RoutedEventArgs e)
    {
        await DownloadAndSaveAsync("pdf", "PDF Document", ".pdf");
    }

    private async void OnDocxClick(object sender, RoutedEventArgs e)
    {
        await DownloadAndSaveAsync("docx", "Word Document", ".docx");
    }

    private async Task DownloadAndSaveAsync(string format, string fileTypeDescription, string extension)
    {
        try
        {
            SetButtonsEnabled(false);

            // Show save file picker
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.DocumentsLibrary,
                SuggestedFileName = FileName
            };
            savePicker.FileTypeChoices.Add(fileTypeDescription, [extension]);

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

            // Download from API
            var url = BuildExportUrl(format);
            var bytes = await HttpClient.GetByteArrayAsync(url);

            // Save to file
            CachedFileManager.DeferUpdates(file);
            await FileIO.WriteBytesAsync(file, bytes);
            await CachedFileManager.CompleteUpdatesAsync(file);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Export failed: {ex.Message}");
        }
        finally
        {
            SetButtonsEnabled(true);
        }
    }
#endif

    private string BuildExportUrl(string format)
    {
        var baseUrl = ApiBaseUrl.TrimEnd('/');
        var exportPath = ExportPath.TrimStart('/').TrimEnd('/');

        return $"{baseUrl}/{exportPath}/{format}";
    }

    private void SetButtonsEnabled(bool enabled)
    {
        PdfButton.IsEnabled = enabled;
        DocxButton.IsEnabled = enabled;
    }
}
