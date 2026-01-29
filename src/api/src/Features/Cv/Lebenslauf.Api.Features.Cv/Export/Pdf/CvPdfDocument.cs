using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace Lebenslauf.Api.Features.Cv.Export.Pdf;

/// <summary>
/// QuestPDF document definition for CV export.
/// Generates a clean, professional vector PDF.
/// </summary>
public class CvPdfDocument : IDocument
{
    private readonly GetCvResponse _data;
    private readonly byte[]? _profileImage;

    // Design constants
    private const string ColorPrimary = "#000000";
    private const string ColorSecondary = "#555555";
    private const string ColorMuted = "#888888";
    private const string ColorBackground = "#F8F8F8";
    private const string ColorBorder = "#E0E0E0";

    public CvPdfDocument(GetCvResponse data, byte[]? profileImage = null)
    {
        _data = data;
        _profileImage = profileImage;
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

            page.Content().Column(column =>
            {
                column.Spacing(20);

                // Header with name and contact
                column.Item().Element(ComposeHeader);

                // Divider
                column.Item().LineHorizontal(2).LineColor(ColorPrimary);

                // Skills section
                column.Item().Element(ComposeSkills);

                // Work experience
                column.Item().Element(ComposeWorkExperience);

                // Education
                column.Item().Element(ComposeEducation);

                // Projects
                column.Item().Element(ComposeProjects);
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
        container.Row(row =>
        {
            // Profile image (if available)
            if (_profileImage != null)
            {
                row.ConstantItem(80).Height(100).Image(_profileImage).FitArea();
                row.ConstantItem(20); // Spacing
            }

            // Name and contact info
            row.RelativeItem().Column(col =>
            {
                col.Item().Text(_data.PersonalData.Name)
                    .FontSize(28)
                    .Bold()
                    .LetterSpacing(-0.02f);

                col.Item().PaddingTop(4).Text("SENIOR CROSS-PLATFORM DEVELOPER")
                    .FontSize(10)
                    .FontColor(ColorSecondary)
                    .LetterSpacing(0.08f);

                col.Item().PaddingTop(8).Row(contactRow =>
                {
                    var age = CalculateAge(_data.PersonalData.BirthDate);
                    contactRow.AutoItem().Text($"Geb. {_data.PersonalData.BirthDate:dd.MM.yyyy} · {age} Jahre")
                        .FontSize(9)
                        .FontColor(ColorMuted);
                });

                col.Item().PaddingTop(12).Row(contactRow =>
                {
                    contactRow.AutoItem().Column(c =>
                    {
                        c.Item().Text("EMAIL").FontSize(8).FontColor(ColorMuted).LetterSpacing(0.1f);
                        c.Item().Text(_data.PersonalData.Email).FontSize(9);
                    });

                    contactRow.ConstantItem(30);

                    contactRow.AutoItem().Column(c =>
                    {
                        c.Item().Text("TELEFON").FontSize(8).FontColor(ColorMuted).LetterSpacing(0.1f);
                        c.Item().Text(_data.PersonalData.Phone).FontSize(9);
                    });

                    contactRow.ConstantItem(30);

                    contactRow.AutoItem().Column(c =>
                    {
                        c.Item().Text("STANDORT").FontSize(8).FontColor(ColorMuted).LetterSpacing(0.1f);
                        c.Item().Text($"{_data.PersonalData.City}, {_data.PersonalData.Country}").FontSize(9);
                    });
                });
            });
        });
    }

    private void ComposeSkills(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("FÄHIGKEITEN")
                .FontSize(10)
                .Bold()
                .LetterSpacing(0.1f);

            column.Item().PaddingTop(10).Row(row =>
            {
                foreach (var category in _data.SkillCategories.Take(3))
                {
                    row.RelativeItem().Column(col =>
                    {
                        col.Item().Text(category.Name.ToUpperInvariant())
                            .FontSize(8)
                            .FontColor(ColorMuted)
                            .LetterSpacing(0.1f);

                        col.Item().PaddingTop(4).Text(string.Join(" · ", category.Skills.Select(s => s.Name)))
                            .FontSize(9)
                            .SemiBold();
                    });
                }
            });
        });
    }

    private void ComposeWorkExperience(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("BERUFSERFAHRUNG")
                .FontSize(10)
                .Bold()
                .LetterSpacing(0.1f);

            column.Item().PaddingTop(12).Row(row =>
            {
                var workItems = _data.WorkExperience.Take(3).ToList();
                foreach (var work in workItems)
                {
                    row.RelativeItem().Background(ColorBackground).Padding(12).Column(col =>
                    {
                        var endYear = work.EndDate?.Year.ToString() ?? "heute";
                        col.Item().Text($"{work.StartDate.Year} – {endYear}")
                            .FontSize(9)
                            .FontColor(ColorMuted);

                        col.Item().PaddingTop(6).Text(work.Role)
                            .FontSize(10)
                            .SemiBold();

                        col.Item().PaddingTop(2).Text(work.Company)
                            .FontSize(9)
                            .FontColor(ColorSecondary);

                        if (!string.IsNullOrEmpty(work.Description))
                        {
                            col.Item().PaddingTop(6).Text(work.Description)
                                .FontSize(8)
                                .FontColor(ColorSecondary);
                        }
                    });

                    // Add spacing between items (except last)
                    if (work != workItems.Last())
                    {
                        row.ConstantItem(12);
                    }
                }
            });
        });
    }

    private void ComposeEducation(IContainer container)
    {
        container.Column(column =>
        {
            column.Item().Text("AUSBILDUNG")
                .FontSize(10)
                .Bold()
                .LetterSpacing(0.1f);

            column.Item().PaddingTop(12).Row(row =>
            {
                var eduItems = _data.Education.Take(2).ToList();
                foreach (var edu in eduItems)
                {
                    row.RelativeItem().Background(ColorBackground).Padding(12).Column(col =>
                    {
                        var endYear = edu.EndYear?.ToString() ?? "heute";
                        col.Item().Text($"{edu.StartYear} – {endYear}")
                            .FontSize(9)
                            .FontColor(ColorMuted);

                        col.Item().PaddingTop(6).Text(edu.Institution)
                            .FontSize(10)
                            .SemiBold();

                        col.Item().PaddingTop(2).Text(edu.Degree)
                            .FontSize(9)
                            .FontColor(ColorSecondary);

                        if (!string.IsNullOrEmpty(edu.Description))
                        {
                            col.Item().PaddingTop(6).Text(edu.Description)
                                .FontSize(8)
                                .FontColor(ColorSecondary);
                        }
                    });

                    if (edu != eduItems.Last())
                    {
                        row.ConstantItem(12);
                    }
                }
            });
        });
    }

    private void ComposeProjects(IContainer container)
    {
        if (!_data.Projects.Any())
            return;

        container.Column(column =>
        {
            column.Item().Text("REFERENZPROJEKTE")
                .FontSize(10)
                .Bold()
                .LetterSpacing(0.1f);

            column.Item().PaddingTop(12).Table(table =>
            {
                // 3 columns
                table.ColumnsDefinition(columns =>
                {
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                    columns.RelativeColumn();
                });

                var projectList = _data.Projects.Take(6).ToList();
                foreach (var project in projectList)
                {
                    table.Cell().Padding(6).Background(ColorBackground).Column(col =>
                    {
                        col.Item().Text(project.Name)
                            .FontSize(9)
                            .Bold();

                        if (!string.IsNullOrEmpty(project.Description))
                        {
                            col.Item().PaddingTop(4).Text(project.Description)
                                .FontSize(8)
                                .FontColor(ColorSecondary)
                                .ClampLines(2);
                        }
                    });
                }
            });
        });
    }

    private static int CalculateAge(DateOnly birthDate)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age))
            age--;
        return age;
    }
}
