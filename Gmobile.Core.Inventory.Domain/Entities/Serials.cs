﻿using ServiceStack.Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;
using Gmobile.Core.Inventory.Models.Const;

namespace Gmobile.Core.Inventory.Domain.Entities
{
    [Schema("public")]
    [Alias("serial")]
    public class Serials : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string StockCode { get; set; }

        [StringLength(30)] public string StockCurrentCode { get; set; }

        [References(typeof(Inventory))]
        public int StockId { get; set; }

        [References(typeof(Category))]
        public int CategorId { get; set; }        

        [StringLength(30)] public string CategoryCode { get; set; }

        [StringLength(50)] public string Serial { get; set; }

        public SerialStatus Status { get; set; }

        [StringLength(50)] public string SouceTransCode { get; set; }

        [StringLength(50)] public string TransCode { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }
        [StringLength(30)] public string UserCreated { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ConfirmDate { get; set; }

        [StringLength(30)] public string UserSale { get; set; }

        public DateTime? SaleDate { get; set; }

        [StringLength(150)] public string TreePath { get; set; }
    }
}
