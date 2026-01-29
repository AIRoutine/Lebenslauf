using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;

/// <summary>
/// Request to export CV as PDF.
/// </summary>
/// <param name="ProfileSlug">Optional profile slug. If null, uses default profile.</param>
public record ExportCvPdfRequest(string? ProfileSlug = null) : IRequest<ExportCvPdfResponse>;

/// <summary>
/// PDF export response containing the document bytes.
/// </summary>
/// <param name="FileBytes">PDF document as byte array.</param>
/// <param name="FileName">Suggested filename for download.</param>
/// <param name="ContentType">MIME type (application/pdf).</param>
public record ExportCvPdfResponse(
    byte[] FileBytes,
    string FileName,
    string ContentType
);

/// <summary>
/// Request to export CV as DOCX.
/// </summary>
/// <param name="ProfileSlug">Optional profile slug. If null, uses default profile.</param>
public record ExportCvDocxRequest(string? ProfileSlug = null) : IRequest<ExportCvDocxResponse>;

/// <summary>
/// DOCX export response containing the document bytes.
/// </summary>
/// <param name="FileBytes">DOCX document as byte array.</param>
/// <param name="FileName">Suggested filename for download.</param>
/// <param name="ContentType">MIME type (application/vnd.openxmlformats-officedocument.wordprocessingml.document).</param>
public record ExportCvDocxResponse(
    byte[] FileBytes,
    string FileName,
    string ContentType
);

/// <summary>
/// Request to export Projects overview as PDF.
/// </summary>
/// <param name="ProfileSlug">Optional profile slug. If null, uses default profile.</param>
public record ExportProjectsPdfRequest(string? ProfileSlug = null) : IRequest<ExportProjectsPdfResponse>;

/// <summary>
/// Projects PDF export response containing the document bytes.
/// </summary>
public record ExportProjectsPdfResponse(
    byte[] FileBytes,
    string FileName,
    string ContentType
);
