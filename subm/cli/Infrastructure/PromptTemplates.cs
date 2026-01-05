using Automation.Cli.Contracts;

namespace Automation.Cli.Infrastructure;

/// <summary>
/// Alle Prompt-Templates fuer die Analyse-Steps.
/// </summary>
public static class PromptTemplates
{
    public static string GetDataAnalysisPrompt(StepContext context) => $"""
        {context.GetSharedContext()}

        Um das Ticket umzusetzten braucht man dazu Aenderungen im Data Bereich? Sprich gibt es Aenderungen bei Entities oder braucht es neue Entities?

        - Welche Entities muessen geloescht werden?
        - Welche Entities muessen bearbeitet werden?
        - Welche Entities muessen neu hinzugefuegt werden?

        Falls im Bereich Data was umgesetzt werden muss erstell ein SubTask beim Ticket dafuer.
        """;

    public static string GetApiAnalysisPrompt(StepContext context) => $"""
        {context.GetSharedContext()}

        Um das Ticket umzusetzten braucht man dazu Aenderungen im Bereich RestService?

        - Welchen Endpoints muessen geloescht werden?
        - Welchen Endpoints muessen bearbeitet werden?
        - Welchen Endpoints muessen neu hinzugefuegt werden?
        - Welche Aenderungen braucht man sonst noch im RestService?

        Falls im Bereich Restservice was umgesetzt werden muss erstell ein SubTask beim Ticket dafuer.
        """;

    public static string GetFrontendAnalysisPrompt(StepContext context) => $"""
        {context.GetSharedContext()}

        Um das Ticket umzusetzten braucht man dazu Aenderungen im Bereich Frontend?

        - Welche Pages/ViewModels muessen geloescht werden werden?
        - Welche Pages/ViewModels muessen bearbeitet werden?
        - Welche Pages/ViewModels muessen hinzugefuegt werden werden?
        - Welche Aenderungen braucht es sonst im Frontend Bereich?

        Falls im Bereich Frontend was umgesetzt werden muss erstell ein SubTask beim Ticket dafuer.
        """;

    public static string GetProjectStructurePrompt(StepContext context) => $"""
        {context.GetSharedContext()}

        Ueberlege dir anhand von den SubTasks von dem Ticket welche csprojs betroffen sind in der jetztigen Projektstruktur.
        Braucht es neue Feature csproj?
        Braucht es sonst neue csproj?

        Bearbeite jetzt alle Subtasks und schreib dazu in welche csprojs die Tasks implementiert werden muessen.
        """;

    public static string GetSkillMappingPrompt(StepContext context) => $"""
        {context.GetSharedContext()}

        Schau dir alle verfuegbaren Skills an und bearbeite alle Subtasks und schreib dazu welchen Skill du verwenden sollst beim dem Task falls einer passt.

        Verfuegbare Skills:
        - uno-dev:api-endpoint-authoring - API Endpoints mit Shiny.Mediator erstellen
        - uno-dev:api-library-authoring - Neue API Feature Libraries (Contracts, Handlers, Entities, Configurations) erstellen
        - uno-dev:mediator-authoring - Commands, Events oder Requests mit Shiny.Mediator erstellen
        - uno-dev:store-authoring - Persistenten State mit Shiny Stores erstellen
        - uno-dev:viewmodel-authoring - ViewModels fuer Uno Platform Apps erstellen
        - uno-dev:xaml-authoring - XAML Views, Pages, UserControls oder UI Elemente fuer Uno Platform erstellen
        """;

    public static string GetImplementTaskPrompt(StepContext context, string task) => $"""
        {context.GetSharedContext()}

        Implementiere folgenden Task:
        {task}

        Nutze die passenden Skills falls angegeben. Implementiere vollstaendig und teste ob der Code kompiliert.
        """;

    public static string GetSeedingPrompt(StepContext context) =>
        context.GetSharedContext() + """


        Fuer das gerade implementierte Data/Entity Feature muessen jetzt Mock-Daten erstellt werden.

        1. Erstelle einen Seeder im Feature-Projekt:
           - Datei: Features/{FeatureName}/Lebenslauf.Api.Features.{FeatureName}/Data/Seeding/{FeatureName}Seeder.cs
           - Implementiere ISeeder aus Lebenslauf.Api.Core.Data.Seeding
           - Pruefe ob Daten bereits existieren (idempotent!)
           - Fuege 5-10 realistische Testdaten pro Entity hinzu

        2. Registriere den Seeder in der Feature ServiceCollectionExtensions:
           services.AddSeeder<{FeatureName}Seeder>();

        Beispiel Seeder:
        ```csharp
        using Lebenslauf.Api.Core.Data;
        using Lebenslauf.Api.Core.Data.Seeding;
        using Microsoft.EntityFrameworkCore;

        public class MyFeatureSeeder(AppDbContext dbContext) : ISeeder
        {
            public int Order => 10;

            public async Task SeedAsync(CancellationToken cancellationToken = default)
            {
                if (await dbContext.Set<MyEntity>().AnyAsync(cancellationToken))
                    return;

                dbContext.Set<MyEntity>().AddRange(
                    new MyEntity { Name = "Test 1", ... },
                    new MyEntity { Name = "Test 2", ... }
                );

                await dbContext.SaveChangesAsync(cancellationToken);
            }
        }
        ```

        WICHTIG:
        - Seeder MUSS idempotent sein (pruefen ob Daten existieren)
        - Datenbank wird NICHT geloescht
        - Nach Implementierung: dotnet build ausfuehren um zu pruefen ob alles kompiliert
        """;

    /// <summary>
    /// Prueft ob ein Task Data/Entity-bezogen ist.
    /// </summary>
    public static bool IsDataTask(string task) =>
        task.Contains("Data", StringComparison.OrdinalIgnoreCase) ||
        task.Contains("Entity", StringComparison.OrdinalIgnoreCase) ||
        task.Contains("Entities", StringComparison.OrdinalIgnoreCase) ||
        task.Contains("Daten", StringComparison.OrdinalIgnoreCase);
}
