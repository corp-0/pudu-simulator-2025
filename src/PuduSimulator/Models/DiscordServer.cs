namespace PuduSimulator.Models;

public class ServerRecords
{
    public int Id { get; set; }
    public int ReleasedPudus { get; set; }
    public int DeadPudus { get; set; }
    
    public int DiscordServerId { get; set; }
}

public class DiscordServer
{
    public int Id { get; set; }
    public ulong DiscordId { get; set; }
    public ulong ChannelId { get; set; }
    public ServerRecords? Records { get; set; }
    public Pudu? CurrentPudu { get; set; }
}

public class Pudu
{
    public int Id { get; set; }
    
    public int DiscordServerId { get; set; }
}