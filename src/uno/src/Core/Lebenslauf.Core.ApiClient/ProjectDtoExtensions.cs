using System.Text.Json.Serialization;

namespace Lebenslauf.Core.ApiClient.Generated;

/// <summary>
/// Partial class extension for ProjectDto to add AppGalleryUrl property.
/// This will be generated automatically once the API is deployed with the new field.
/// TODO: Remove this file after deployment.
/// </summary>
public partial class ProjectDto
{
    [JsonPropertyName("appGalleryUrl")]
    public string? AppGalleryUrl { get; set; }
}
