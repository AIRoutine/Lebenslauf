#if __WASM__
using System.Runtime.InteropServices.JavaScript;
using System.Runtime.Versioning;

namespace Lebenslauf.Core.Styles.Controls;

/// <summary>
/// JavaScript interop methods for WebAssembly.
/// </summary>
[SupportedOSPlatform("browser")]
public static partial class JsInterop
{
    /// <summary>
    /// Triggers a file download in the browser.
    /// </summary>
    [JSImport("globalThis.triggerBrowserDownload")]
    public static partial void TriggerBrowserDownload(string base64Data, string mimeType, string fileName);
}
#endif
