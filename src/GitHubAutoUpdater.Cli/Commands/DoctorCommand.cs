using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.Json;

namespace GitHubAutoUpdater.Cli.Commands;
internal class DoctorCommand : AsyncCommand<CommandSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, CommandSettings settings, CancellationToken cancellationToken)
    {
        string configPath = Path.Combine(Directory.GetCurrentDirectory(), "updaterconfig.json");

        if (!File.Exists(configPath))
        {
            AnsiConsole.MarkupLine("[red]Updater config not found. Run 'githubupdater init' first.[/]");
            return Task.FromResult(1);
        }

        var config = JsonSerializer.Deserialize<Models.UpdaterConfig>(File.ReadAllText(configPath));
        if (config == null)
        {
            AnsiConsole.MarkupLine("[red]Updater config is invalid.[/]");
            return Task.FromResult(1);
        }

        string? token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrEmpty(token))
        {
            AnsiConsole.MarkupLine("[yellow]Warning: GITHUB_TOKEN is not set. Publish will fail.[/]");
        }
        else
        {
            AnsiConsole.MarkupLine("[green]GITHUB_TOKEN found.[/]");
        }

        AnsiConsole.MarkupLine($"[blue]Repository: {config.Repository}[/]");
        AnsiConsole.MarkupLine($"[blue]Current Version: {config.CurrentVersion}[/]");
        AnsiConsole.MarkupLine("[green]All basic checks passed![/]");

        return Task.FromResult(0);
    }
}
