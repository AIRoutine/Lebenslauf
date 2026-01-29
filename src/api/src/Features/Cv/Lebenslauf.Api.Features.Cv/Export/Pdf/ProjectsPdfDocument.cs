using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Lebenslauf.Api.Features.Cv.Export.Pdf;

/// <summary>
/// QuestPDF document definition for Projects overview export.
/// </summary>
public class ProjectsPdfDocument : IDocument
{
    private readonly GetCvResponse _data;

    private const string ColorPrimary = "#000000";
    private const string ColorSecondary = "#555555";
    private const string ColorMuted = "#888888";
    private const string ColorBackground = "#F5F5F5";
    private const string ColorAccent = "#2563EB";

    public ProjectsPdfDocument(GetCvResponse data)
    {
        _data = data;
    }

    public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

    public void Compose(IDocumentContainer container)
    {
        container.Page(page =>
        {
            page.Size(PageSizes.A4);
            page.MarginVertical(1.5f, Unit.Centimetre);
            page.MarginHorizontal(2f, Unit.Centimetre);
            page.DefaultTextStyle(x => x.FontSize(10).FontColor(ColorPrimary));

            page.Header().Element(ComposeHeader);

            page.Content().PaddingTop(20).Column(column =>
            {
                column.Spacing(16);

                foreach (var project in _data.Projects)
                {
                    column.Item().Element(c => ComposeProject(c, project));
                }
            });

            page.Footer().AlignCenter().Text(text =>
            {
                text.CurrentPageNumber();
                text.Span(" / ");
                text.TotalPages();
            });
        });
    }

    private void ComposeHeader(IContainer container)
    {
        container.Column(col =>
        {
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text("PROJEKTÜBERSICHT")
                        .FontSize(24)
                        .Bold()
                        .LetterSpacing(-0.02f);

                    c.Item().PaddingTop(4).Text(_data.PersonalData.Name)
                        .FontSize(12)
                        .FontColor(ColorSecondary);
                });

                row.ConstantItem(120).AlignRight().Column(c =>
                {
                    c.Item().Text($"{_data.Projects.Count} Projekte")
                        .FontSize(10)
                        .FontColor(ColorMuted);
                });
            });

            col.Item().PaddingTop(12).LineHorizontal(2).LineColor(ColorPrimary);
        });
    }

    private void ComposeProject(IContainer container, ProjectDto project)
    {
        container.Background(ColorBackground).Padding(16).Column(col =>
        {
            // Project header
            col.Item().Row(row =>
            {
                row.RelativeItem().Column(c =>
                {
                    c.Item().Text(project.Name)
                        .FontSize(14)
                        .Bold();

                    if (!string.IsNullOrEmpty(project.Framework))
                    {
                        c.Item().PaddingTop(2).Text(project.Framework)
                            .FontSize(9)
                            .FontColor(ColorAccent);
                    }
                });

                // Time period
                if (project.StartDate.HasValue)
                {
                    row.ConstantItem(100).AlignRight().Text(text =>
                    {
                        var start = project.StartDate.Value.ToString("MMM yyyy");
                        var end = project.EndDate?.ToString("MMM yyyy") ?? "heute";
                        text.Span($"{start} – {end}")
                            .FontSize(9)
                            .FontColor(ColorMuted);
                    });
                }
            });

            // Description
            if (!string.IsNullOrEmpty(project.Description))
            {
                col.Item().PaddingTop(8).Text(project.Description)
                    .FontSize(9)
                    .FontColor(ColorSecondary);
            }

            // Technologies
            if (project.Technologies.Any())
            {
                col.Item().PaddingTop(10).Row(row =>
                {
                    row.AutoItem().Text("TECHNOLOGIEN")
                        .FontSize(8)
                        .FontColor(ColorMuted)
                        .LetterSpacing(0.05f);
                });

                col.Item().PaddingTop(4).Text(string.Join(" · ", project.Technologies))
                    .FontSize(9)
                    .SemiBold();
            }

            // Functions
            if (project.Functions.Any())
            {
                col.Item().PaddingTop(10).Row(row =>
                {
                    row.AutoItem().Text("FUNKTIONEN")
                        .FontSize(8)
                        .FontColor(ColorMuted)
                        .LetterSpacing(0.05f);
                });

                col.Item().PaddingTop(4).Column(funcCol =>
                {
                    foreach (var func in project.Functions)
                    {
                        funcCol.Item().Text($"• {func}")
                            .FontSize(9);
                    }
                });
            }

            // Technical Aspects
            if (project.TechnicalAspects.Any())
            {
                col.Item().PaddingTop(10).Row(row =>
                {
                    row.AutoItem().Text("TECHNISCHE ASPEKTE")
                        .FontSize(8)
                        .FontColor(ColorMuted)
                        .LetterSpacing(0.05f);
                });

                col.Item().PaddingTop(4).Column(techCol =>
                {
                    foreach (var aspect in project.TechnicalAspects)
                    {
                        techCol.Item().Text($"• {aspect}")
                            .FontSize(9);
                    }
                });
            }

            // SubProjects
            if (project.SubProjects.Any())
            {
                col.Item().PaddingTop(10).Row(row =>
                {
                    row.AutoItem().Text("TEILPROJEKTE")
                        .FontSize(8)
                        .FontColor(ColorMuted)
                        .LetterSpacing(0.05f);
                });

                col.Item().PaddingTop(4).Column(subCol =>
                {
                    foreach (var sub in project.SubProjects)
                    {
                        subCol.Item().PaddingTop(4).Row(subRow =>
                        {
                            subRow.AutoItem().Text($"▸ {sub.Name}")
                                .FontSize(9)
                                .SemiBold();

                            if (!string.IsNullOrEmpty(sub.Framework))
                            {
                                subRow.AutoItem().PaddingLeft(8).Text($"({sub.Framework})")
                                    .FontSize(8)
                                    .FontColor(ColorAccent);
                            }
                        });

                        if (!string.IsNullOrEmpty(sub.Description))
                        {
                            subCol.Item().PaddingLeft(12).Text(sub.Description)
                                .FontSize(8)
                                .FontColor(ColorSecondary);
                        }
                    }
                });
            }
        });
    }
}
