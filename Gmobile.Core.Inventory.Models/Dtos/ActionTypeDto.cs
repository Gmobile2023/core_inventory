using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class ActionTypeDto
    {
        public long Id { get; set; }

        public string AcitonType { get; set; }

        public string AcitonName { get; set; }

        public int Status { get; set; }

        public string Description { get; set; }
    }
}
