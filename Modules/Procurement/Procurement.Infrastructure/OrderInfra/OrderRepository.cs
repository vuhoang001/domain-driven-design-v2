using Procurement.Domain.Order;

namespace Procurement.Infrastructure.OrderInfra;

public class OrderRepository(ProcurementContext context) : IOrderRepository
{
    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
    }
}