using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Configuration;
using Nop.Plugins.Payments.StripeStandard.EnumWork;

namespace Nop.Plugins.Payments.StripeStandard
{
    /// <summary>
    /// Represents settings of the Stripe Standard payment plugin
    /// </summary>
    public class StripeStandardPaymentSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use sandbox (testing environment)
        /// </summary>
        public bool UseSandbox { get; set; }

        /// <summary>
        /// Gets or sets a title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets TestPublishableKey
        /// </summary>
        public string TestPublishableKey { get; set; }


        /// <summary>
        /// Gets or sets TestSecretKey
        /// </summary>
        public string TestSecretKey { get; set; }


        /// <summary>
        /// Gets or sets LivePublishableKey
        /// </summary>
        public string LivePublishableKey { get; set; }


        /// <summary>
        /// Gets or sets LiveSecretKey
        /// </summary>
        public string LiveSecretKey { get; set; }
        /// <summary>
        /// Gets or sets an additional fee
        /// </summary>
        public decimal AdditionalFee { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to "additional fee" is specified as percentage. true - percentage, false - fixed value.
        /// </summary>
        public bool AdditionalFeePercentage { get; set; }

        /// <summary>
        /// Gets or sets the payment type, uses enum found in plugin folder
        /// </summary>
        public PaymentType PaymentType { get; set; }

   
        public string GetSecretKey() // if we are using sandbox, gives test key not live
        {

            return UseSandbox ? TestSecretKey : LiveSecretKey;
        }
        public string GetPublishableKey() // if we are using sandbox, gives test key not live
        {
            return UseSandbox ? TestPublishableKey : LivePublishableKey;
        }
    }
}
