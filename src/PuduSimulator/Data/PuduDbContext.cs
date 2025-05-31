using Microsoft.EntityFrameworkCore;
using PuduSimulator.Models;

namespace PuduSimulator.Data;

public class PuduDbContext: DbContext
{
    public DbSet<DiscordServer> DiscordServers => Set<DiscordServer>();
    public DbSet<ServerRecords> ServerRecords => Set<ServerRecords>();
    public DbSet<Pudu> Pudus => Set<Pudu>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string dbPath = Environment.GetEnvironmentVariable("DB_PATH") 
                        ?? Path.Combine(AppContext.BaseDirectory, "pudu.db");
        
        Console.WriteLine($"[DB] Using database at: {dbPath}");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DiscordServer>()
            .HasKey(d => d.Id);
        
        modelBuilder.Entity<DiscordServer>()
            .HasIndex(d => d.DiscordId)
            .IsUnique();
        
        modelBuilder.Entity<DiscordServer>()
            .HasOne(d => d.Records)
            .WithOne()
            .HasForeignKey<ServerRecords>(r => r.DiscordServerId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<DiscordServer>()
            .HasOne(d => d.CurrentPudu)
            .WithOne()
            .HasForeignKey<Pudu>(p => p.DiscordServerId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}