using CavalryGirls.Inspector.Commands;

using Spectre.Console;
using Spectre.Console.Cli;

var cultureInfo = CultureInfo.CreateSpecificCulture("en-US");
CultureInfo.CurrentCulture = cultureInfo;
CultureInfo.CurrentUICulture = cultureInfo;

var app = new CommandApp();
app.Configure(config =>
{
    config
        .AddCommand<EnemiesCommand>("enemies")
        .WithDescription("Generate enemies data and images");

    config
        .AddCommand<WeaponsCommand>("weapons")
        .WithDescription("Generate weapons data and images");

    config.PropagateExceptions();
});

try
{
    AnsiConsole.MarkupLine("[green]Cavalry Girls Inspector[/]\n");
    return await app.RunAsync(args);
}
catch (Exception ex)
{
    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
    return -1;
}