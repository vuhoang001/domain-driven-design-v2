namespace Procurement.Domain.Order;

public interface IOrderRepository
{
    Task AddAsync(Order order);
}