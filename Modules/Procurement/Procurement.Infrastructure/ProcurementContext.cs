using Microsoft.EntityFrameworkCore;
using Procurement.Domain.Order;

namespace Procurement.Infrastructure;

public class ProcurementContext(DbContextOptions<ProcurementContext> options) : DbContext(options)
{
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);
        base.OnModelCreating(modelBuilder);
    }
}