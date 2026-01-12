using GitHubUpdater;
using System.Text.Json;

class Program
{
    static async Task Main()
    {
        var config = LoadConfig();

        await Updater.Configure(o =>
        {
            o.Repository = "krzysztofowsiany/GitHubUpdater";
            o.CurrentVersion = "0.0.1";
            o.GitHubToken = config.GitHubToken;
            o.AssetName = "app-win-x64.zip.zip";
        }, new HttpClient()).CheckAndUpdateAsync();

        Console.WriteLine("App started...");
    }

    static AppConfig LoadConfig()
    {
        var configPath = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
        if (!File.Exists(configPath))
            throw new FileNotFoundException($"Config file not found: {configPath}");

        var json = File.ReadAllText(configPath);
        return JsonSerializer.Deserialize<AppConfig>(json) ?? new AppConfig();
    }
}
