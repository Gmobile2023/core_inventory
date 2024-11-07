using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Gmobile.Core.Inventory.Domain.Entities;

//[Schema("public")]
[UniqueConstraint(nameof(ProviderCode))]
public class Provider : AuditBase, IHasId<int>
{
    public static string CacheKey = "Booking:Provider";
    [AutoIncrement] [PrimaryKey] public int Id { get; set; }
    [StringLength(50)] public string ProviderCode { get; set; }
    [StringLength(250)] public string ProviderName { get; set; }

    [StringLength(50)] public string UserName { get; set; }

    [StringLength(50)] public string Password { get; set; }

    [StringLength(250)] public string ApiUrl { get; set; }
    [StringLength(250)] public string IpnUrl { get; set; }
    [StringLength(100)] public string SecretKey { get; set; }
    [StringLength(5000)] public string WebViewUrl { get; set; }

    public bool IsActive { get; set; }
    public bool IsDefault { get; set; }
}