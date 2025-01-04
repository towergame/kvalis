using System.Reflection;
using Kvalis.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;

namespace Kvalis.Services;

public class KvalisContext : DbContext
{
    public KvalisContext() {}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .AddUserSecrets<Program>()
            .Build();
        
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseSqlServer($"Server={config["Database:Hostname"]};"
                          + $"Database=ElectionThing;"
                          + $"User Id=sa;"
                          + $"Password={config["Database:Password"]};"
                          + "Trusted_Connection=false;"
                          + "MultipleActiveResultSets=true;"
                          + "trustServerCertificate=true;");
    }
    
    public DbSet<Election> Elections { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Option> Options { get; set; }
    public DbSet<Ballot> Ballots { get; set; }
    public DbSet<Vote> Votes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Election>()
            .HasMany(e => e.Questions)
            .WithOne(q => q.Election)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Question>()
            .HasMany(q => q.Options)
            .WithOne(o => o.Question)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Election>()
            .HasMany(e => e.Ballots)
            .WithOne(b => b.Election)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Election>()
            .HasMany(e => e.Votes)
            .WithOne(v => v.Election)
            .OnDelete(DeleteBehavior.Cascade);
        modelBuilder.Entity<Vote>()
            .HasOne(v => v.Question)
            .WithMany(q => q.Votes)
            .OnDelete(DeleteBehavior.NoAction);
        modelBuilder.Entity<Vote>()
            .HasOne(v => v.Option)
            .WithMany()
            .OnDelete(DeleteBehavior.NoAction);
    }
}