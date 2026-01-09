public class GitHubReleaseProvider
{
private readonly string _repo;


public GitHubReleaseProvider(string repo)
{
_repo = repo;
}


public async Task<string> GetLatestReleaseAsync()
{
// placeholder: fetch latest release tag from GitHub API
await Task.Delay(1);
return "1.0.0";
}
}