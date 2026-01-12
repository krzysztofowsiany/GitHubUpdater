namespace GitHubUpdater;

public class UpdaterOptions
{
    public string Repository { get; set; } = string.Empty;
    public string CurrentVersion { get; set; } = "0.0.0";
    public string? GitHubToken { get; set; }
    public string AssetName { get; set; } = string.Empty;
}
