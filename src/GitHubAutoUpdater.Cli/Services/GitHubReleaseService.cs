using Octokit;
using System.Text.Json;

namespace GitHubAutoUpdater.Cli.Services;

public class GitHubReleaseService
{
    private readonly GitHubClient _client;
    private readonly string _owner = "your-org";
    private readonly string _repo = "your-repo";

    public GitHubReleaseService()
    {
        string? token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
        if (string.IsNullOrEmpty(token))
            throw new Exception("GITHUB_TOKEN environment variable is required.");

        _client = new GitHubClient(new ProductHeaderValue("GitHubUpdater"))
        {
            Credentials = new Credentials(token)
        };

        // Wczytanie configu z updaterconfig.json
        string configPath = Path.Combine(Directory.GetCurrentDirectory(), "updaterconfig.json");
        if (!File.Exists(configPath))
            throw new Exception("updaterconfig.json not found. Run 'githubupdater init' first.");

        var config = JsonSerializer.Deserialize<Models.UpdaterConfig>(File.ReadAllText(configPath));
        if (config == null || string.IsNullOrEmpty(config.Repository))
            throw new Exception("Updater config is invalid.");

        var parts = config.Repository.Split('/');
        if (parts.Length != 2)
            throw new Exception("Repository format must be 'owner/repo'.");

        _owner = parts[0];
        _repo = parts[1];
    }

    public async Task UploadReleaseAsync(string version, string zipPath, string manifestPath, CancellationToken cancellationToken)
    {
        Release release;

        try
        {
            release = await _client.Repository.Release.Get(_owner, _repo, version);
        }
        catch (NotFoundException)
        {
            release = await _client.Repository.Release.Create(_owner, _repo, new NewRelease(version)
            {
                Name = version,
                Draft = false,
                Prerelease = false
            });
        }

        // Upload zip
        using var zipStream = File.OpenRead(zipPath);
        var zipAsset = new ReleaseAssetUpload
        {
            FileName = Path.GetFileName(zipPath),
            ContentType = "application/zip",
            RawData = zipStream
        };
        await _client.Repository.Release.UploadAsset(release, zipAsset);

        // Upload manifest
        using var manifestStream = File.OpenRead(manifestPath);
        var manifestAsset = new ReleaseAssetUpload
        {
            FileName = Path.GetFileName(manifestPath),
            ContentType = "application/json",
            RawData = manifestStream
        };
        await _client.Repository.Release.UploadAsset(release, manifestAsset);
    }
}