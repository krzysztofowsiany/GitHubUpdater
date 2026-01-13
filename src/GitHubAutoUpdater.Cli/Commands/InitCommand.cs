using Spectre.Console;
using Spectre.Console.Cli;
using System.Text.Json;

namespace GitHubAutoUpdater.Cli.Commands;

public class InitCommand : AsyncCommand<CommandSettings>
{
    public override Task<int> ExecuteAsync(CommandContext context, CommandSettings settings, CancellationToken cancellationToken)
    {
        {
            string configPath = Path.Combine(Directory.GetCurrentDirectory(), "updaterconfig.json");

            if (File.Exists(configPath))
            {
                AnsiConsole.MarkupLine("[yellow]Updater config already exists.[/]");
                return Task.FromResult(0);
            }

            string? repo = Environment.GetEnvironmentVariable("UPDATER_REPOSITORY");
            string? version = Environment.GetEnvironmentVariable("UPDATER_VERSION");

            // Jeśli interaktywnie i brak env, pytamy użytkownika
            if (string.IsNullOrEmpty(repo) && Environment.UserInteractive)
            {
                repo = AnsiConsole.Ask<string>("Enter GitHub repository for updater [owner/repo]:");
            }
            repo ??= "your-org/your-repo"; // fallback

            if (string.IsNullOrEmpty(version) && Environment.UserInteractive)
            {
                version = AnsiConsole.Ask<string>("Enter current version [0.0.0]:");
            }
            version ??= "0.0.0"; // fallback

            var config = new Models.UpdaterConfig
            {
                Repository = repo,
                CurrentVersion = version
            };

            File.WriteAllText(configPath, JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true }));
            AnsiConsole.MarkupLine($"[green]Created updater config at {configPath}[/]");

            return Task.FromResult(0);
        }
    }
}
