using ServiceStack.Data;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

namespace Gmobile.Core.Inventory.Domain;

public interface IIventoryConnectionFactory : IDbConnectionFactory
{
}

public class IventoryConnectionFactory : OrmLiteConnectionFactory, IIventoryConnectionFactory
{
    public IventoryConnectionFactory(string s) : base(s)
    {
    }


    public IventoryConnectionFactory(string s, PostgreSqlDialectProvider provider) : base(s, provider)
    {
    }
}