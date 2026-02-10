using BuildingBlocks.Domain;

namespace Procurement.Domain.Order;

public class OrderItemId(Guid value) : TypeIdValueBase(value);