using System.ComponentModel.DataAnnotations;
using Nop.Web.Framework.Mvc.ModelBinding;
using Nop.Web.Framework.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace Nop.Plugin.Payments.StripeStandard.Models
{
    public record ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.UseSandbox")]
        public bool UseSandbox { get; set; }
        public bool UseSandbox_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.Title")]
        public string Title { get; set; }
        public bool Title_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.TestPublishableKey")]
        public string TestPublishableKey { get; set; }
        public bool TestPublishableKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.TestSecretKey")]
        public string TestSecretKey { get; set; }
        public bool TestSecretKey_OverrideForStore { get; set; }
      
        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.LivePublishableKey")]
        public string LivePublishableKey { get; set; }
        public bool LivePublishableKey_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.LiveSecretKey")]
        public string LiveSecretKey { get; set; }
        public bool LiveSecretKey_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.AdditionalFee")]
        public decimal AdditionalFee { get; set; }
        public bool AdditionalFee_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.AdditionalFeePercentage")]
        public bool AdditionalFeePercentage { get; set; }
        public bool AdditionalFeePercentage_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Payments.StripeStandard.Fields.PaymentTypeId")]
        public int PaymentTypeId { get; set; }
        public bool PaymentTypeId_OverrideForStore { get; set; }
    
        public IList<SelectListItem> PaymentType { get; set; }
    }
}