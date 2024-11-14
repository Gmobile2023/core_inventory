using Gmobile.Core.Inventory.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.BusinessServices
{
    public interface IFileService
    {
        Task<List<SettingItem>> ReadFileXls(Stream stream);
    }
}
