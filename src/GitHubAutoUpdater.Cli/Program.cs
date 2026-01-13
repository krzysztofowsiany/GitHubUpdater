using GitHubAutoUpdater.Cli.Commands;
using Spectre.Console.Cli;

var app = new CommandApp();

app.Configure(config =>
{
    config.SetApplicationName("githubautoupdater");

    config.AddCommand<InitCommand>("init")
        .WithDescription("Initialize GitHubAutoUpdater in this repository");

    config.AddCommand<PublishCommand>("publish")
        .WithDescription("Publish a new version for auto-update");

    config.AddCommand<DoctorCommand>("doctor")
        .WithDescription("Validate updater configuration");
});

return app.Run(args);