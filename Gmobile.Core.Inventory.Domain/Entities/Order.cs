using Gmobile.Core.Inventory.Models.Const;
using ServiceStack;
using ServiceStack.DataAnnotations;
using ServiceStack.Model;

namespace Gmobile.Core.Inventory.Domain.Entities;

[UniqueConstraint(nameof(OrderCode))]
[UniqueConstraint(nameof(SaleTransCode))]
[UniqueConstraint(nameof(ProviderCode), nameof(TransRef))]
public class Order : AuditBase, IHasId<long>
{
    [AutoIncrement] [PrimaryKey] public long Id { get; set; }
    [StringLength(50)] public string OrderCode { get; set; }

    [StringLength(50)] [Index] public string TransRef { get; set; }

    //[StringLength(50)] [Index] public string ProviderTransCode { get; set; }
    [StringLength(50)] [Index] public string SaleTransCode { get; set; }
    [StringLength(50)] [Index] public string ProviderCode { get; set; }
    [StringLength(50)] public string AccountCode { get; set; }
    [StringLength(50)] public string ServiceCode { get; set; }
    [StringLength(50)] public string CategoryCode { get; set; }
    [StringLength(50)] public string ProductCode { get; set; }
    [StringLength(5000)] public string ExtraInfo { get; set; }
    public decimal PaymentAmount { get; set; }
    public decimal Fee { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public int Quantity { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime? PaymentDate { get; set; }
}