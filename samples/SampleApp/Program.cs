using GitHubUpdater;


class Program
{
    static async Task Main()
    {
        await Updater.Configure(o =>
        {
            o.Repository = "krzysztofowsiany/GitHubUpdater";
            o.CurrentVersion = "0.0.1";
            o.GitHubToken = Environment.GetEnvironmentVariable("GITHUB_TOKEN");
            o.AssetName = "app-win-x64.zip.zip";
        }, new HttpClient()).CheckAndUpdateAsync();


        Console.WriteLine("App started...");
    }
}