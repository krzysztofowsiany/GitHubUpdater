using GitHubUpdater.Providers;
using System.Diagnostics;

namespace GitHubUpdater;

public class Updater
{
    public UpdaterOptions Options { get; private set; } = new();

    private GitHubReleaseProvider _provider = null!;
    private HttpClient _httpClient = null!;

    public static Updater Configure(Action<UpdaterOptions> configure, HttpClient? httpClient = null)
    {
        var updater = new Updater();
        var options = new UpdaterOptions();
        configure(options);
        updater.Options = options;
        updater._httpClient = httpClient ?? new HttpClient();
        updater._provider = new GitHubReleaseProvider(updater._httpClient, options.Repository, options.GitHubToken);
        return updater;
    }

    public async Task CheckAndUpdateAsync()
    {
        var latest = await _provider.GetLatestReleaseVersionAsync();

        if (latest == Options.CurrentVersion)
        {
            Console.WriteLine("Already up to date!");
            return;
        }

        var tempPath = await DownloadNewVersionToTemp(latest);

        var agentPath = Path.Combine(AppContext.BaseDirectory, "UpdaterAgent.exe");
        Console.WriteLine($"Start updater: {agentPath}");

        var startInfo = new ProcessStartInfo(agentPath)
        {
            Arguments = $"\"{tempPath}\" \"{AppContext.BaseDirectory}\"",
            UseShellExecute = false,
        };
        Process.Start(startInfo);

        Console.WriteLine("Updater agent started. Exiting app...");
        Environment.Exit(0);
    }

    private async Task<string> GetNewVersionUrl(string latest)
    {
        Console.WriteLine($"New version available: {latest}");
        var zipUrl = await _provider.GetAssetUrlAsync(Options.AssetName);
        Console.WriteLine($"ZipUrl: {zipUrl}");
        return zipUrl;
    }

    private async Task<string> DownloadNewVersionToTemp(string latest)
    {
        var zipUrl = await GetNewVersionUrl(latest);

        var updateDir = Path.Combine(Path.GetTempPath(), "GitHubUpdater", Guid.NewGuid().ToString());
        Directory.CreateDirectory(updateDir);
        var tempPath = Path.Combine(updateDir, $"update-{latest}.zip");
        Console.WriteLine($"Download path: {tempPath}");

        using (var response = await _httpClient.GetAsync(zipUrl))
        {
            response.EnsureSuccessStatusCode();
            using var fs = File.Create(tempPath);
            await response.Content.CopyToAsync(fs);
        }

        return tempPath;
    }
}
