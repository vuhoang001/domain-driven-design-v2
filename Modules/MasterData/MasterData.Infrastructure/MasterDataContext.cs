using BuildingBlocks.Application.Outbox;
using MasterData.Domain.Inventory;
using MasterData.Domain.Item;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MasterData.Infrastructure;

public class MasterDataContext(DbContextOptions options, ILoggerFactory loggerFactory) : DbContext(options)
{
    public DbSet<OutboxMessage> OutboxMessages { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Inventory> Inventories { get; set; }

    private readonly ILoggerFactory _loggerFactory = loggerFactory;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}