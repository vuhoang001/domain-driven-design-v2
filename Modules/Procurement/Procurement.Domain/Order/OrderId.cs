using BuildingBlocks.Domain;

namespace Procurement.Domain.Order;

public class OrderId(Guid value) : TypeIdValueBase(value);