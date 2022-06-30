using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugins.Payments.StripeStandard.EnumWork;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Nop.Plugins.Payments.StripeStandard
{
    public class StripeStandardPaymentProcessor: BasePlugin
    {
        #region Fields
        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly IWebHelper _webHelper;

        #endregion

        #region Ctor
        public StripeStandardPaymentProcessor(ISettingService settingService,
            ILocalizationService localizationService,
            IWebHelper webHelper
            )
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _webHelper = webHelper;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Install the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task InstallAsync()
        {
            //settings
            await _settingService.SaveSettingAsync(new StripeStandardPaymentSettings
            {
                UseSandbox = true,
                Title = "Credit Card (Stripe)",
                AdditionalFee = 0,
                PaymentType = PaymentType.Authorize
            });

            //locales
            await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
            {                
                ["Plugins.Payments.StripeStandard.Fields.AdditionalFee"] = "Additional fee",
                ["Plugins.Payments.StripeStandard.Fields.AdditionalFee.Hint"] = "Enter additional fee to charge your customers.",
                ["Plugins.Payments.StripeStandard.Fields.AdditionalFeePercentage"] = "Additional fee. Use percentage",
                ["Plugins.Payments.StripeStandard.Fields.AdditionalFeePercentage.Hint"] = "Determines whether to apply a percentage additional fee to the order total. If not enabled, a fixed value is used.",
                ["Plugins.Payments.StripeStandard.Fields.UseSandbox"] = "Use Sandbox",
                ["Plugins.Payments.StripeStandard.Fields.UseSandbox.Hint"] = "Check to enable Sandbox (testing environment).",
                ["Plugins.Payments.StripeStandard.Fields.Title"] = "Title",
                ["Plugins.Payments.StripeStandard.Fields.Title.Hint"] = "Specify your title.",
                ["Plugins.Payments.StripeStandard.Fields.TestPublishableKey"] = "TestPublishableKey",
                ["Plugins.Payments.StripeStandard.Fields.TestPublishableKey.Hint"] = "Specify your TestPublishableKey.",
                ["Plugins.Payments.StripeStandard.Fields.TestSecretKey"] = "TestSecretKey",
                ["Plugins.Payments.StripeStandard.Fields.TestSecretKey.Hint"] = "Specify your TestSecretKey.",
                ["Plugins.Payments.StripeStandard.Fields.LivePublishableKey"] = "LivePublishableKey",
                ["Plugins.Payments.StripeStandard.Fields.LivePublishableKey.Hint"] = "Specify your LivePublishableKey.",
                ["Plugins.Payments.StripeStandard.Fields.LiveSecretKey"] = "LiveSecretKey",
                ["Plugins.Payments.StripeStandard.Fields.LiveSecretKey.Hint"] = "Specify your LiveSecretKey.",
                ["Plugins.Payments.StripeStandard.Fields.PaymentType"] = "Payment Type",
                ["Plugins.Payments.StripeStandard.Fields.PaymentType.Hint"] = "Specify your PaymentType.",
                ["Plugins.Payments.StripeStandard.Fields.PaymentTypeId"] = "Payment Type",
                ["Plugins.Payments.StripeStandard.Fields.PaymentTypeId.Hint"] = "Specify your PaymentType.",


                ["Plugins.Payments.StripeStandard.Instructions"] = @"
                    <p>
	                    <b>Look mom I made a plugin :D</b>
                        <br />
	                    <b>If you're using this gateway ensure that your primary store currency is supported by Stripe.</b>
	                    <br />
	                    <br />To use PDT, you must activate PDT and Auto Return in your Stripe account profile. You must also acquire a PDT identity token, which is used in all PDT communication you send to Stripe. Follow these steps to configure your account for PDT:<br />
	                    <br />1. Log in to your Stripe account (click <a href=""https://www.Stripe.com/us/webapps/mpp/referral/Stripe-business-account2?partner_id=9JJPJNNPQ7PZ8"" target=""_blank"">here</a> to create your account).
	                    <br />2. Click on the Profile button.
	                    <br />3. Click on the <b>Account Settings</b> link.
	                    <br />4. Select the <b>Website payments</b> item on left panel.
	                    <br />5. Find <b>Website Preferences</b> and click on the <b>Update</b> link.
	                    <br />6. Under <b>Auto Return</b> for <b>Website payments preferences</b>, select the <b>On</b> radio button.
	                    <br />7. For the <b>Return URL</b>, enter and save the URL on your site that will receive the transaction ID posted by Stripe after a customer payment (<em>{0}</em>).
                        <br />8. Under <b>Payment Data Transfer</b>, select the <b>On</b> radio button and get your <b>Identity token</b>.
	                    <br />9. Enter <b>Identity token</b> in the field below on the plugin configuration page.
                        <br />10. Click <b>Save</b> button on this page.
	                    <br />
                    </p>",           
            });

            await base.InstallAsync();
        }

        /// <summary>
        /// Get the configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/PaymentStripeStandard/Configure";
        }
        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        /// <returns>A task that represents the asynchronous operation</returns>
        public override async Task UninstallAsync()
        {
            //settings
            await _settingService.DeleteSettingAsync<StripeStandardPaymentSettings>();

            //locales
            await _localizationService.DeleteLocaleResourcesAsync("Plugins.Payments.StripeStandard");

            await base.UninstallAsync();
        }
    }
    #endregion
}
