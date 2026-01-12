using System.Text.Json;

namespace GitHubUpdater.Providers;

public class GitHubReleaseProvider
{
    private readonly string _repo;
    private readonly string? _token;
    private readonly HttpClient _httpClient;

    public GitHubReleaseProvider(HttpClient httpClient, string repo, string? token = null)
    {
        _httpClient = httpClient;
        _repo = repo;
        _token = token;
    }

    public async Task<string> GetLatestReleaseVersionAsync()
    {
        var json = await GetLatestReleaseAsync();
        using var doc = JsonDocument.Parse(json);
        return doc.RootElement.GetProperty("tag_name").GetString() ?? "0.0.0";
    }

    public async Task<string> GetAssetUrlAsync(string assetName)
    {
        var json = await GetLatestReleaseAsync();
        using var doc = JsonDocument.Parse(json);
        var assets = doc.RootElement.GetProperty("assets").EnumerateArray();

        foreach (var asset in assets)
        {
            if (asset.GetProperty("name").GetString().Contains(assetName))
                return asset.GetProperty("browser_download_url").GetString()!;
        }

        throw new Exception($"Asset {assetName} not found in latest release");
    }


    private async Task<string> GetLatestReleaseAsync()
    {
        var url = $"https://api.github.com/repos/{_repo}/releases/latest";
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Add("User-Agent", "GitHubUpdater");
        request.Headers.Add("Accept", "application/vnd.github+json");
        request.Headers.Add("X-GitHub-Api-Version", "2022-11-28");
        if (!string.IsNullOrEmpty(_token))
            request.Headers.Add("Authorization", $"Bearer {_token}");

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadAsStringAsync();
    }
}
