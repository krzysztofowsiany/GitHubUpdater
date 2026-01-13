namespace GitHubAutoUpdater.Cli.Services;
public class LicenseService
{
    public void EnsurePro(string repoVisibility)
    {
        if (repoVisibility == "private")
            throw new Exception("Private repositories require GitHubUpdater Pro");
    }
}
