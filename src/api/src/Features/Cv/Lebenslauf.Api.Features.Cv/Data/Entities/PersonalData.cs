using Lebenslauf.Api.Core.Data.Entities;

namespace Lebenslauf.Api.Features.Cv.Data.Entities;

public class PersonalData : BaseEntity
{
    public string? AcademicTitle { get; set; }
    public required string Name { get; set; }
    public required string Title { get; set; }
    public required string Email { get; set; }
    public required string Phone { get; set; }
    public required string Address { get; set; }
    public required string City { get; set; }
    public required string PostalCode { get; set; }
    public required string Country { get; set; }
    public required DateOnly BirthDate { get; set; }
    public required string Citizenship { get; set; }
    public string? ProfileImageUrl { get; set; }

    // Profile relationship - each PersonalData belongs to one Profile
    public Guid ProfileId { get; set; }
    public Profile Profile { get; set; } = null!;
}
