using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Lebenslauf.Api.Features.Cv.Contracts.Mediator.Requests;

namespace Lebenslauf.Api.Features.Cv.Export.Docx;

/// <summary>
/// Generates CV as DOCX document using Open-XML-SDK.
/// </summary>
public static class CvDocxGenerator
{
    public static byte[] Generate(GetCvResponse data)
    {
        using var stream = new MemoryStream();
        using (var document = WordprocessingDocument.Create(stream, WordprocessingDocumentType.Document))
        {
            var mainPart = document.AddMainDocumentPart();
            mainPart.Document = new Document();
            var body = mainPart.Document.AppendChild(new Body());

            // Add styles
            AddStyles(mainPart);

            // Header - Name
            body.AppendChild(CreateHeading(data.PersonalData.Name, 1));

            // Subtitle
            body.AppendChild(CreateParagraph("SENIOR CROSS-PLATFORM DEVELOPER", "Subtitle"));

            // Contact info
            var age = CalculateAge(data.PersonalData.BirthDate);
            body.AppendChild(CreateParagraph($"Geb. {data.PersonalData.BirthDate:dd.MM.yyyy} · {age} Jahre"));
            body.AppendChild(CreateParagraph($"Email: {data.PersonalData.Email} | Telefon: {data.PersonalData.Phone}"));
            body.AppendChild(CreateParagraph($"Standort: {data.PersonalData.City}, {data.PersonalData.Country}"));

            // Divider
            body.AppendChild(CreateParagraph(""));

            // Skills section
            body.AppendChild(CreateHeading("FÄHIGKEITEN", 2));
            foreach (var category in data.SkillCategories)
            {
                var skills = string.Join(" · ", category.Skills.Select(s => s.Name));
                body.AppendChild(CreateParagraph($"{category.Name}: {skills}"));
            }

            // Work Experience
            body.AppendChild(CreateHeading("BERUFSERFAHRUNG", 2));
            foreach (var work in data.WorkExperience)
            {
                var endYear = work.EndDate?.Year.ToString() ?? "heute";
                body.AppendChild(CreateParagraph($"{work.StartDate.Year} – {endYear}", "DateRange"));
                body.AppendChild(CreateParagraph(work.Role, "JobTitle"));
                body.AppendChild(CreateParagraph(work.Company, "Company"));
                if (!string.IsNullOrEmpty(work.Description))
                {
                    body.AppendChild(CreateParagraph(work.Description, "Description"));
                }
                body.AppendChild(CreateParagraph(""));
            }

            // Education
            body.AppendChild(CreateHeading("AUSBILDUNG", 2));
            foreach (var edu in data.Education)
            {
                var endYear = edu.EndYear?.ToString() ?? "heute";
                body.AppendChild(CreateParagraph($"{edu.StartYear} – {endYear}", "DateRange"));
                body.AppendChild(CreateParagraph(edu.Institution, "JobTitle"));
                body.AppendChild(CreateParagraph(edu.Degree, "Company"));
                if (!string.IsNullOrEmpty(edu.Description))
                {
                    body.AppendChild(CreateParagraph(edu.Description, "Description"));
                }
                body.AppendChild(CreateParagraph(""));
            }

            // Projects
            if (data.Projects.Any())
            {
                body.AppendChild(CreateHeading("REFERENZPROJEKTE", 2));
                foreach (var project in data.Projects.Take(10))
                {
                    body.AppendChild(CreateParagraph(project.Name, "ProjectName"));
                    if (!string.IsNullOrEmpty(project.Description))
                    {
                        body.AppendChild(CreateParagraph(project.Description, "Description"));
                    }
                    if (project.Technologies.Any())
                    {
                        body.AppendChild(CreateParagraph($"Technologien: {string.Join(", ", project.Technologies)}", "Technologies"));
                    }
                    body.AppendChild(CreateParagraph(""));
                }
            }

            mainPart.Document.Save();
        }

        return stream.ToArray();
    }

    private static void AddStyles(MainDocumentPart mainPart)
    {
        var stylesPart = mainPart.AddNewPart<StyleDefinitionsPart>();
        var styles = new Styles();

        // Heading 1 style (Name)
        styles.AppendChild(CreateStyle("Heading1", "Heading 1", new RunProperties
        {
            Bold = new Bold(),
            FontSize = new FontSize { Val = "56" }, // 28pt
            Color = new Color { Val = "000000" }
        }));

        // Heading 2 style (Section headers)
        styles.AppendChild(CreateStyle("Heading2", "Heading 2", new RunProperties
        {
            Bold = new Bold(),
            FontSize = new FontSize { Val = "22" }, // 11pt
            Color = new Color { Val = "000000" }
        }));

        // Subtitle style
        styles.AppendChild(CreateStyle("Subtitle", "Subtitle", new RunProperties
        {
            FontSize = new FontSize { Val = "20" }, // 10pt
            Color = new Color { Val = "555555" }
        }));

        // DateRange style
        styles.AppendChild(CreateStyle("DateRange", "Date Range", new RunProperties
        {
            FontSize = new FontSize { Val = "18" }, // 9pt
            Color = new Color { Val = "888888" }
        }));

        // JobTitle style
        styles.AppendChild(CreateStyle("JobTitle", "Job Title", new RunProperties
        {
            Bold = new Bold(),
            FontSize = new FontSize { Val = "20" }, // 10pt
            Color = new Color { Val = "1A1A1A" }
        }));

        // Company style
        styles.AppendChild(CreateStyle("Company", "Company", new RunProperties
        {
            FontSize = new FontSize { Val = "18" }, // 9pt
            Color = new Color { Val = "555555" }
        }));

        // Description style
        styles.AppendChild(CreateStyle("Description", "Description", new RunProperties
        {
            FontSize = new FontSize { Val = "18" }, // 9pt
            Color = new Color { Val = "444444" }
        }));

        // ProjectName style
        styles.AppendChild(CreateStyle("ProjectName", "Project Name", new RunProperties
        {
            Bold = new Bold(),
            FontSize = new FontSize { Val = "18" }, // 9pt
            Color = new Color { Val = "000000" }
        }));

        // Technologies style
        styles.AppendChild(CreateStyle("Technologies", "Technologies", new RunProperties
        {
            Italic = new Italic(),
            FontSize = new FontSize { Val = "16" }, // 8pt
            Color = new Color { Val = "666666" }
        }));

        stylesPart.Styles = styles;
        stylesPart.Styles.Save();
    }

    private static Style CreateStyle(string styleId, string styleName, RunProperties runProperties)
    {
        return new Style(
            new StyleName { Val = styleName },
            new BasedOn { Val = "Normal" },
            new StyleRunProperties(runProperties.CloneNode(true))
        )
        {
            Type = StyleValues.Paragraph,
            StyleId = styleId
        };
    }

    private static Paragraph CreateHeading(string text, int level)
    {
        return new Paragraph(
            new ParagraphProperties(
                new ParagraphStyleId { Val = $"Heading{level}" },
                new SpacingBetweenLines { Before = "240", After = "120" }
            ),
            new Run(new Text(text))
        );
    }

    private static Paragraph CreateParagraph(string text, string? styleId = null)
    {
        var para = new Paragraph();

        if (!string.IsNullOrEmpty(styleId))
        {
            para.AppendChild(new ParagraphProperties(
                new ParagraphStyleId { Val = styleId }
            ));
        }

        para.AppendChild(new Run(new Text(text)));
        return para;
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
