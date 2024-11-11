﻿using Funq;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class OrderDetailDto
    {
        public long Id { get; set; }
        
        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public string Range { get; set; }

        public decimal CostPrice { get; set; }

        public decimal SalePrice { get; set; }

        public int CategoryId { get; set; }

        public string CategoryCode { get; set; }
    }
}