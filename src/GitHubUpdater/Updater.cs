namespace GitHubUpdater;


public class Updater
{
public UpdaterOptions Options { get; private set; } = new UpdaterOptions();


public static Updater Configure(Action<UpdaterOptions> configure)
{
var updater = new Updater();
var options = new UpdaterOptions();
configure(options);
updater.Options = options;
return updater;
}


public async Task CheckAndUpdateAsync()
{
// placeholder dla logiki:
// 1. check GitHub Release
// 2. compare version
// 3. download zip
// 4. run UpdaterAgent
await Task.CompletedTask;
}
}


public class UpdaterOptions
{
public string Repository { get; set; } = string.Empty;
public string CurrentVersion { get; set; } = "0.0.0";
}