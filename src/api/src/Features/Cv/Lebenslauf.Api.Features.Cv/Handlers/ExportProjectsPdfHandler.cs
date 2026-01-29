using Lebenslauf.Api;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Lebenslauf.Api.Features.Cv.Export.Pdf;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Handlers;

[Service(ApiService.Lifetime, TryAdd = ApiService.TryAdd)]
public class ExportProjectsPdfHandler(IRequestHandler<GetCvRequest, GetCvResponse> getCvHandler)
    : IRequestHandler<ExportProjectsPdfRequest, ExportProjectsPdfResponse>
{
    static ExportProjectsPdfHandler()
    {
        QuestPDF.Settings.License = LicenseType.Community;
    }

    public async Task<ExportProjectsPdfResponse> Handle(ExportProjectsPdfRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        // Get CV data (which includes projects)
        var cvData = await getCvHandler.Handle(
            new GetCvRequest(request.ProfileSlug),
            context,
            cancellationToken);

        // Generate Projects PDF document
        var document = new ProjectsPdfDocument(cvData);
        var pdfBytes = document.GeneratePdf();

        // Generate filename
        var safeName = cvData.PersonalData.Name.Replace(" ", "_");
        var fileName = $"Projekte_{safeName}.pdf";

        return new ExportProjectsPdfResponse(
            FileBytes: pdfBytes,
            FileName: fileName,
            ContentType: "application/pdf"
        );
    }
}
