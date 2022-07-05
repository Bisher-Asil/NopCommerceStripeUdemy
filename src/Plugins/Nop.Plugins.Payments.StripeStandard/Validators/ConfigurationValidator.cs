using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.Payments.StripeStandard.Models;
using Nop.Web.Framework.Validators;

namespace Nop.Plugins.Payments.StripeStandard.Validators
{
    public class ConfigurationValidator : BaseNopValidator<ConfigurationModel>
    {
        public ConfigurationValidator()
        {
            RuleFor(model => model.Title).NotEmpty().WithMessage("Title cannot be empty");

            RuleFor(model => model.TestPublishableKey).NotEmpty().WithMessage("You are using sandbox, this is required")
                .When(model => model.UseSandbox);

            RuleFor(model => model.TestSecretKey).NotEmpty().WithMessage("You are using sandbox, this is required")
                .When(model => model.UseSandbox);

            RuleFor(model => model.LivePublishableKey).NotEmpty().WithMessage("You are not using sandbox, this is required")
                .When(model => !model.UseSandbox);

            RuleFor(model => model.LiveSecretKey).NotEmpty().WithMessage("You are not using sandbox, this is required")
                .When(model => !model.UseSandbox);
        }
    }
}
