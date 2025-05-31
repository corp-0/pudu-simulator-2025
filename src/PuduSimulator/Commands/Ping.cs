using Discord.Interactions;

namespace PuduSimulator.Commands;

public class Ping: InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("ping", "Responds with pong, wow!")]
    public async Task HandlePing()
    {
        await RespondAsync("pong");
    }
}