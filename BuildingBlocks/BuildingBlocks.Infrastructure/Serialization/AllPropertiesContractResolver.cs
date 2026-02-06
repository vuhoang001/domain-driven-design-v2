using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace BuildingBlocks.Infrastructure.Serialization;

/// <summary>
/// Đây là một class giúp mở khóa tất cả các properties của một object khi serialize/deserialize bằng Newtonsoft.json.
/// Chi tiết:
///     1. BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance - lấy tất cả properties, bao gồm cả những cái private hoặc protected, không chỉ public
///     2. p.Writable = true - Cho phép ghi dữ liệu vào những properties này khi chuyển JSon thành object
///     3. p.Readable = true - Cho phép đọc dữ liệu từ những properties này khi chuyển object thành JSON
/// </summary>
public class AllPropertiesContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var properties = type.GetProperties(
                BindingFlags.Public    |
                BindingFlags.NonPublic |
                BindingFlags.Instance)
            .Select(p => this.CreateProperty(p, memberSerialization))
            .ToList();

        properties.ForEach(p =>
        {
            p.Writable = true;
            p.Readable = true;
        });

        return properties;
    }
}