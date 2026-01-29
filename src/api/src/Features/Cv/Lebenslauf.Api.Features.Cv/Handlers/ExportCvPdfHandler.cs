using Lebenslauf.Api;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Lebenslauf.Api.Features.Cv.Export.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Handlers;

[Service(ApiService.Lifetime, TryAdd = ApiService.TryAdd)]
public class ExportCvPdfHandler(IRequestHandler<GetCvRequest, GetCvResponse> getCvHandler)
    : IRequestHandler<ExportCvPdfRequest, ExportCvPdfResponse>
{
    static ExportCvPdfHandler()
    {
        // Configure QuestPDF license (Community license is free for revenue < $1M)
        QuestPDF.Settings.License = LicenseType.Community;
    }

    // Note: Endpoint is registered manually in ServiceCollectionExtensions for proper file download handling
    public async Task<ExportCvPdfResponse> Handle(ExportCvPdfRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        // Get CV data using existing handler
        var cvData = await getCvHandler.Handle(
            new GetCvRequest(request.ProfileSlug),
            context,
            cancellationToken);

        // Generate PDF document
        var document = new CvPdfDocument(cvData);
        var pdfBytes = document.GeneratePdf();

        // Generate filename
        var safeName = cvData.PersonalData.Name.Replace(" ", "_");
        var fileName = $"Lebenslauf_{safeName}.pdf";

        return new ExportCvPdfResponse(
            FileBytes: pdfBytes,
            FileName: fileName,
            ContentType: "application/pdf"
        );
    }
}
