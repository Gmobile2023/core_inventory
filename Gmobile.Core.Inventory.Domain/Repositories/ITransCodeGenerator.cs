using System.Threading.Tasks;

namespace Gmobile.Core.Inventory.Domain.Repositories;

public interface ITransCodeGenerator
{
    Task<string> TransCodeGeneratorAsync(string prefix = "P");    
}