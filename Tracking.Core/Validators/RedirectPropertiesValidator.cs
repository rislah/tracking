using FluentValidation;
using Tracking.Core.Models;

namespace Tracking.Core.Validators
{
    public class RedirectPropertiesValidator : AbstractValidator<RedirectProperties>
    {
        public RedirectPropertiesValidator()
        {
            RuleFor(p => p.EventName).NotEmpty();
            RuleFor(p => p.TargetUrl).NotEmpty();
        }
    }
}