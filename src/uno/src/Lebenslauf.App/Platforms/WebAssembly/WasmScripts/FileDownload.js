// File download interop for WASM
globalThis.triggerBrowserDownload = function(base64Data, mimeType, fileName) {
    const link = document.createElement('a');
    link.href = 'data:' + mimeType + ';base64,' + base64Data;
    link.download = fileName;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
};
