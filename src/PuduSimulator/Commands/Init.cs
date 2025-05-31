using Discord.Interactions;
using Microsoft.EntityFrameworkCore;
using PuduSimulator.Data;
using PuduSimulator.Models;

namespace PuduSimulator.Commands;

public class Init: InteractionModuleBase<SocketInteractionContext>
{
    [SlashCommand("init",
        "Initializes the discord server and channel that invoked the command so they are ready to have pudus")]
    public async Task HandleInit()
    {
        await DeferAsync(ephemeral: true);
        ulong guildId = Context.Guild?.Id ?? throw new InvalidOperationException("Command must be run in a server.");
        ulong channelId = Context.Channel.Id;
        await using PuduDbContext db = new();
        bool alreadyExists = await db.DiscordServers
            .AnyAsync(s => s.DiscordId == guildId);
        
        if (alreadyExists)
        {
            await RespondAsync("This server is already initialized. If you want to change the channel for the Pudu lair, try <another command>");
            return;
        }
        
        DiscordServer newServer = new()
        {
            DiscordId = guildId,
            ChannelId = channelId,
            Records = new ServerRecords
            {
                ReleasedPudus = 0,
                DeadPudus = 0
            }
        };
        
        db.DiscordServers.Add(newServer);
        await db.SaveChangesAsync();

        await FollowupAsync("Pudu system initialized for this server.");
    }
}