using Shiny.Mediator;

namespace Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;

/// <summary>
/// Request to get all available CV profiles.
/// </summary>
public record GetProfilesRequest : IRequest<GetProfilesResponse>;

/// <summary>
/// Response containing all available profiles.
/// </summary>
public record GetProfilesResponse(IReadOnlyList<ProfileDto> Profiles);

/// <summary>
/// Profile information.
/// </summary>
public record ProfileDto(
    Guid Id,
    string Slug,
    string Name,
    string? Description,
    bool IsDefault
);
