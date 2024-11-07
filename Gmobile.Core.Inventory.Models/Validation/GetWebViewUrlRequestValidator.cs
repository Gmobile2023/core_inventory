using Gmobile.Core.Inventory.Models.Routes.App;
using Inventory.Shared.Const;
using ServiceStack.FluentValidation;

namespace Gmobile.Core.Inventory.Models.Validation;

public class GetWebViewUrlRequestValidator : AbstractValidator<GetWebViewUrlRequest>
{
    public GetWebViewUrlRequestValidator()
    {
        RuleFor(x => x.ServiceCode).NotEmpty().WithMessage("ServiceCode not found")
            .WithErrorCode(ResponseCodeConst.Error);
        RuleFor(x => x.ProviderCode).NotEmpty().WithMessage("ProviderCode not found")
            .WithErrorCode(ResponseCodeConst.Error);
    }
}