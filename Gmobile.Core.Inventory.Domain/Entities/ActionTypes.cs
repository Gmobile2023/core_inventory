using ServiceStack.Model;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.DataAnnotations;

namespace Gmobile.Core.Inventory.Domain.Entities
{
    [Schema("public")]
    [Alias("action_types")]
    public class ActionTypes : IHasId<long>
    {
        [AutoIncrement][PrimaryKey] public long Id { get; set; }

        [StringLength(30)] public string AcitonType { get; set; }

        [StringLength(100)] public string AcitonName { get; set; }

        public int Status { get; set; }
    }
}
