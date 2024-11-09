using ServiceStack.Model;
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
    [Alias("category")]
    public class Category : AuditBase, IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        public long? ParentCategoryId { get; set; }

        [StringLength(30)] public string CategoryCode { get; set; }

        [StringLength(150)] public string CategoryName { get; set; }

        public int CategoryType { get; set; }

        public CategoryStatus Status { get; set; }

        [StringLength(30)] public string UserCreated { get; set; }

        public DateTime CreatedDate { get; set; }
        
    }
}
