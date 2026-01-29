using Lebenslauf.Api;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Lebenslauf.Api.Features.Cv.Export.Docx;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Handlers;

[Service(ApiService.Lifetime, TryAdd = ApiService.TryAdd)]
public class ExportCvDocxHandler(IRequestHandler<GetCvRequest, GetCvResponse> getCvHandler)
    : IRequestHandler<ExportCvDocxRequest, ExportCvDocxResponse>
{
    // Note: Endpoint is registered manually in ServiceCollectionExtensions for proper file download handling
    public async Task<ExportCvDocxResponse> Handle(ExportCvDocxRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        // Get CV data using existing handler
        var cvData = await getCvHandler.Handle(
            new GetCvRequest(request.ProfileSlug),
            context,
            cancellationToken);

        // Generate DOCX document
        var docxBytes = CvDocxGenerator.Generate(cvData);

        // Generate filename
        var safeName = cvData.PersonalData.Name.Replace(" ", "_");
        var fileName = $"Lebenslauf_{safeName}.docx";

        return new ExportCvDocxResponse(
            FileBytes: docxBytes,
            FileName: fileName,
            ContentType: "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
        );
    }
}
