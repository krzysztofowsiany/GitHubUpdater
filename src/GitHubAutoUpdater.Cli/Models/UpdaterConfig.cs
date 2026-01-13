namespace GitHubAutoUpdater.Cli.Models;

internal class UpdaterConfig
{
    public string Repository { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = string.Empty;
}
