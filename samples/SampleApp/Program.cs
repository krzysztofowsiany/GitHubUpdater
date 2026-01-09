using GitHubUpdater;


class Program
{
static async Task Main()
{
await Updater.Configure(o => {
o.Repository = "your-org/your-app";
o.CurrentVersion = "1.0.0";
}).CheckAndUpdateAsync();


Console.WriteLine("App started...");
}
}