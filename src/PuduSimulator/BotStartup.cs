using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace PuduSimulator;

public class BotStartup
{
    private readonly DiscordSocketClient _client;
    private readonly InteractionService _interactionService;
    private readonly string _environment;
    private readonly ulong? _devGuildId;
    private readonly CancellationToken _token;
    
    public BotStartup(string environment, string? devGuildId, CancellationToken ct)
    {
        _token = ct;
        _client = new DiscordSocketClient(new DiscordSocketConfig
        {
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        });

        _interactionService = new InteractionService(_client.Rest);
        _environment = environment;
        _devGuildId = ulong.TryParse(devGuildId, out ulong id) ? id : null;

        _client.Log += msg =>
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        };

        _client.InteractionCreated += async interaction =>
        {
            SocketInteractionContext ctx = new(_client, interaction);
            await _interactionService.ExecuteCommandAsync(ctx, null);
        };

        _client.Ready += RegisterGuildCommands;
    }
    
    public async Task Run(string token)
    {
        await _interactionService.AddModulesAsync(typeof(Program).Assembly, null);
        await _client.LoginAsync(TokenType.Bot, token);
        await _client.StartAsync();
        await Task.Delay(-1, _token);
    }

    public async Task Stop()
    {
        await _client.StopAsync();
        _interactionService.Dispose();
        await _client.LogoutAsync();
        await _client.DisposeAsync();
    }
    
    public async Task RegisterGlobalCommands()
    {
        await _interactionService.AddModulesAsync(typeof(Program).Assembly, null);

        if (_environment == "prod")
        {
            await _interactionService.RegisterCommandsGloballyAsync();
            Console.WriteLine("Registered global commands.");
        }
        else if (_devGuildId.HasValue)
        {
            await _interactionService.RegisterCommandsToGuildAsync(_devGuildId.Value);
            Console.WriteLine($"Registered dev guild commands to {_devGuildId.Value}.");
        }
    }
    
    private async Task RegisterGuildCommands()
    {
        if (_environment == "dev" && _devGuildId.HasValue)
        {
            await _interactionService.RegisterCommandsToGuildAsync(_devGuildId.Value);
            Console.WriteLine($"[READY] Registered dev guild commands to {_devGuildId.Value}.");
        }
    }
}