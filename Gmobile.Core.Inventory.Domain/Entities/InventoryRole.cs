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
    [Alias("inventory_roles")]
    public class InventoryRoles : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public int StockId { get; set; }

        [StringLength(30)] public string AccountCode { get; set; }

        public RoleType RoleType { get; set; }
    }
}