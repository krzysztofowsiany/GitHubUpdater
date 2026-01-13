using GitHubAutoUpdater.Cli.Services;
using Spectre.Console;
using Spectre.Console.Cli;

public partial class PublishCommand : AsyncCommand<PublishCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandOption("--version <VERSION>")]
        public string Version { get; set; } = default!;

        [CommandOption("--app <PATH>")]
        public string AppPath { get; set; } = default!;
    }

    private readonly ZipService _zipService;
    private readonly GitHubReleaseService _releaseService;

    public PublishCommand()
    {
        _zipService = new ZipService();
        _releaseService = new GitHubReleaseService();
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings, CancellationToken cancellationToken)
    {
        AnsiConsole.MarkupLine($"[green]Publishing update {settings.Version} for {settings.AppPath}...[/]");

        // 1️⃣ Zip build
        string zipPath = _zipService.CreateZip(settings.AppPath, System.IO.Path.GetTempPath());
        AnsiConsole.MarkupLine($"[blue]Zipped build to {zipPath}[/]");

        // 2️⃣ Generate manifest.json
        string manifestPath = _zipService.GenerateManifest(zipPath, settings.Version);
        AnsiConsole.MarkupLine($"[blue]Generated manifest.json at {manifestPath}[/]");

        // 3️⃣ Upload GitHub Release
        await _releaseService.UploadReleaseAsync(settings.Version, zipPath, manifestPath, cancellationToken);
        AnsiConsole.MarkupLine("[green]Release uploaded successfully![/]");

        return 0;
    }
}