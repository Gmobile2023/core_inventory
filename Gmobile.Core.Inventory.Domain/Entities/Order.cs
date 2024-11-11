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
    [AutoIncrement][PrimaryKey] public long Id { get; set; }
    [StringLength(50)] public string OrderCode { get; set; }

    [StringLength(20)] public string OrderType { get; set; }

    [StringLength(200)] public string OrderTitle { get; set; }

    /// <summary>
    /// 1: Đơn cho số
    /// 2: Đơn cho sim
    /// </summary>
    public int SimType { get; set; }

    [StringLength(30)] public string SrcStockCode { get; set; }

    [StringLength(30)] public string DesStockCode { get; set; }


    [References(typeof(Inventory))]
    public int? DesStockId { get; set; }

    [References(typeof(Inventory))]
    public int? SrcStockId { get; set; }

    [StringLength(50)][Index] public string TransRef { get; set; }

    [StringLength(50)][Index] public string SaleTransCode { get; set; }
    [StringLength(50)][Index] public string ProviderCode { get; set; }
    [StringLength(30)] public string UserCreated { get; set; }

    [StringLength(30)] public string UserConfirm { get; set; }

    [StringLength(30)] public string UserApprove { get; set; }

    public DateTime CreatedDate { get; set; }
    public DateTime? ConfirmDate { get; set; }
    public DateTime? ApproveDate { get; set; }
    [StringLength(50)] public string CategoryCode { get; set; }

    public decimal Fee { get; set; }
    public decimal Discount { get; set; }
    public decimal Tax { get; set; }
    public int Quantity { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public OrderStatus Status { get; set; }
    public decimal PaymentAmount { get; set; }
    public DateTime? PaymentDate { get; set; }
    [StringLength(5000)] public string Description { get; set; }
    [StringLength(5000)] public string Document { get; set; }
}