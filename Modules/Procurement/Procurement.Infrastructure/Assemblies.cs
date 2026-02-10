using System.Reflection;
using Procurement.Application.Configuration.Commands;

namespace Procurement.Infrastructure;

internal static class Assemblies
{
    public static readonly Assembly Application    = typeof(IProcurementModule).Assembly;
    public static readonly Assembly Infrastructure = typeof(IProcurementModule).Assembly;
}