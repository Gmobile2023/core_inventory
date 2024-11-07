using Inventory.Shared.Dtos.CommonDto;
using ServiceStack;

namespace Gmobile.Core.Inventory.Component.Services;

public abstract class AppServiceBase : Service
{
    protected CustomUserSession? UserSession => base.SessionAs<CustomUserSession>();
}