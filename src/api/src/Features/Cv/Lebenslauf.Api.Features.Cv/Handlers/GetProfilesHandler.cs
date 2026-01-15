using Lebenslauf.Api;
using Lebenslauf.Api.Core.Data;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using Lebenslauf.Api.Features.Cv.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Shiny.Extensions.DependencyInjection;
using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Handlers;

[Service(ApiService.Lifetime, TryAdd = ApiService.TryAdd)]
public class GetProfilesHandler(AppDbContext dbContext) : IRequestHandler<GetProfilesRequest, GetProfilesResponse>
{
    [MediatorHttpGet("/api/profiles", OperationId = "GetProfiles")]
    public async Task<GetProfilesResponse> Handle(GetProfilesRequest request, IMediatorContext context, CancellationToken cancellationToken)
    {
        var profiles = await dbContext.Set<Profile>()
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return new GetProfilesResponse(
            Profiles: profiles.Select(MapProfile).ToList()
        );
    }

    private static ProfileDto MapProfile(Profile entity)
    {
        return new ProfileDto(
            Id: entity.Id,
            Slug: entity.Slug,
            Name: entity.Name,
            Description: entity.Description,
            IsDefault: entity.IsDefault
        );
    }
}
