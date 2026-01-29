using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Net.Http;
#if !__WASM__
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

    public static readonly DependencyProperty ApiBaseUrlProperty =
        DependencyProperty.Register(
            nameof(ApiBaseUrl),
            typeof(string),
            typeof(ExportHeaderControl),
            new PropertyMetadata("https://lebenslauf-api.azurewebsites.net"));

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
            var script = $"(function(){{var a=document.createElement('a');a.href='data:{mimeType};base64,{base64}';a.download='{fileName}';document.body.appendChild(a);a.click();document.body.removeChild(a);}})();";

            await Uno.Foundation.WebAssemblyRuntime.InvokeAsync(script);
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

        if (!string.IsNullOrEmpty(ProfileSlug))
        {
            return $"{baseUrl}/api/cv/{ProfileSlug}/export/{format}";
        }

        return $"{baseUrl}/api/cv/export/{format}";
    }

    private void SetButtonsEnabled(bool enabled)
    {
        PdfButton.IsEnabled = enabled;
        DocxButton.IsEnabled = enabled;
    }
}
