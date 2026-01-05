using System.Diagnostics;
using Shiny.Extensions.DependencyInjection;

namespace Automation.Cli.Infrastructure;

/// <summary>
/// Implementierung fuer das Ausfuehren von CLI-Prozessen.
/// </summary>
[Service(CliService.Lifetime, TryAdd = CliService.TryAdd, Type = typeof(IProcessRunner))]
public class ProcessRunner : IProcessRunner
{
    public async Task<ProcessResult> RunClaudeAsync(string prompt, CancellationToken ct = default)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "claude",
            Arguments = $"--dangerously-skip-permissions -p \"{EscapeArgument(prompt)}\"",
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = false
        };

        return await RunProcessAsync(startInfo, ct);
    }

    public async Task<ProcessResult> RunGitHubAsync(string arguments, CancellationToken ct = default)
    {
        var startInfo = new ProcessStartInfo
        {
            FileName = "gh",
            Arguments = arguments,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        return await RunProcessAsync(startInfo, ct);
    }

    private static async Task<ProcessResult> RunProcessAsync(ProcessStartInfo startInfo, CancellationToken ct)
    {
        using var process = new Process { StartInfo = startInfo };

        try
        {
            process.Start();

            var outputTask = process.StandardOutput.ReadToEndAsync(ct);
            var errorTask = process.StandardError.ReadToEndAsync(ct);

            await process.WaitForExitAsync(ct);

            var output = await outputTask;
            var error = await errorTask;

            return new ProcessResult(
                Success: process.ExitCode == 0,
                Output: output,
                Error: string.IsNullOrWhiteSpace(error) ? null : error);
        }
        catch (Exception ex)
        {
            return new ProcessResult(Success: false, Output: string.Empty, Error: ex.Message);
        }
    }

    private static string EscapeArgument(string arg)
    {
        // Escape double quotes and backslashes for command line
        return arg.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }
}
