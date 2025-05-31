using System.Runtime.Loader;
using DotNetEnv;
using PuduSimulator;
using PuduSimulator.Simulation;

Env.TraversePath().Load();

string token = Environment.GetEnvironmentVariable("DISCORD_TOKEN")
               ?? throw new InvalidOperationException("Missing environment variable 'DISCORD_TOKEN'");

string environment = Environment.GetEnvironmentVariable("BOT_ENV") ?? "dev";
string? devGuild = Environment.GetEnvironmentVariable("DEV_GUILD");
string[] cli = Environment.GetCommandLineArgs();
bool registerCommands = cli.Contains("register-commands");

CancellationTokenSource botStopCts = new();
BotStartup bot = new(environment, devGuild, botStopCts.Token);

if (registerCommands)
{
    await bot.RegisterGlobalCommands();
    return;
}

CancellationTokenSource simulationStopCts = new();
Simulation simulation = new(2, simulationStopCts);

Console.CancelKeyPress += async (_, e) =>
{
    Console.WriteLine("SIGINT received. Shutting down...");
    await bot.Stop();
    botStopCts.Cancel();
    simulation.Stop();
};

AssemblyLoadContext.Default.Unloading += async _ =>
{
    Console.WriteLine("SIGTERM received. Shutting down...");
    await bot.Stop();
    botStopCts.Cancel();
    simulation.Stop();
};

simulation.Start();
await bot.Run(token);
simulation.Stop();