using Gmobile.Core.Inventory.Models.Const;
using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Models.Dtos
{
    public class CategoryDto
    {
        public long Id { get; set; }

        public long? ParentCategoryId { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public int CategoryType { get; set; }

        public CategoryStatus Status { get; set; }

        public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
