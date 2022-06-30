using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Plugin.Payments.StripeStandard.Models;
using Nop.Plugins.Payments.StripeStandard;
using Nop.Plugins.Payments.StripeStandard.EnumWork;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Payments.StripeStandard.Controllers
{
    public class PaymentStripeStandardController : BasePaymentController
    {
        #region Fields
        private readonly IStoreContext _storeContext;
        private readonly ISettingService _settingService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;

        #endregion

        #region CTOR
        public PaymentStripeStandardController(IPermissionService permissionService, 
            IStoreContext storeContext,
            ISettingService settingService,
            INotificationService notificationService,
            ILocalizationService localizationService
            )
        {
            _permissionService = permissionService;
            _storeContext = storeContext;
            _settingService = settingService;
            _notificationService = notificationService;
            _localizationService = localizationService;
        }
        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure()
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var stripeStandardPaymentSettings = await _settingService.LoadSettingAsync<StripeStandardPaymentSettings>(storeScope);

            var model = new ConfigurationModel
            {
                UseSandbox = stripeStandardPaymentSettings.UseSandbox,
                Title = stripeStandardPaymentSettings.Title,
                TestPublishableKey = stripeStandardPaymentSettings.TestPublishableKey,
                TestSecretKey = stripeStandardPaymentSettings.TestSecretKey,
                LivePublishableKey = stripeStandardPaymentSettings.LivePublishableKey,
                LiveSecretKey = stripeStandardPaymentSettings.LiveSecretKey,
                AdditionalFee = stripeStandardPaymentSettings.AdditionalFee,
                AdditionalFeePercentage = stripeStandardPaymentSettings.AdditionalFeePercentage,
                ActiveStoreScopeConfiguration = storeScope,
                PaymentTypeId = (int)stripeStandardPaymentSettings.PaymentType
            };

            if (storeScope <= 0)
                return View("~/Plugins/Payments.StripeStandard/Views/Configure.cshtml", model);

            model.UseSandbox_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.UseSandbox, storeScope);
            model.Title_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.Title, storeScope);
            model.TestPublishableKey_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.TestPublishableKey, storeScope);
            model.LivePublishableKey_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.LivePublishableKey, storeScope);
            model.TestSecretKey_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.TestSecretKey, storeScope);
            model.LiveSecretKey_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.LiveSecretKey, storeScope);
            model.AdditionalFee_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.AdditionalFee, storeScope);
            model.AdditionalFeePercentage_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.AdditionalFeePercentage, storeScope);
            model.PaymentTypeId_OverrideForStore = await _settingService.SettingExistsAsync(stripeStandardPaymentSettings, x => x.PaymentType, storeScope);
            
            return View("~/Plugins/Payments.StripeStandard/Views/Configure.cshtml", model);
        }

        [HttpPost]
        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public async Task<IActionResult> Configure(ConfigurationModel model)
        {
            if (!await _permissionService.AuthorizeAsync(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            if (!ModelState.IsValid)
                return await Configure();

            //load settings for a chosen store scope
            var storeScope = await _storeContext.GetActiveStoreScopeConfigurationAsync();
            var stripeStandardPaymentSettings = await _settingService.LoadSettingAsync<StripeStandardPaymentSettings>(storeScope);

            //save settings
            stripeStandardPaymentSettings.UseSandbox = model.UseSandbox;
            stripeStandardPaymentSettings.Title = model.Title;
            stripeStandardPaymentSettings.TestPublishableKey = model.TestPublishableKey;
            stripeStandardPaymentSettings.TestSecretKey = model.TestSecretKey;
            stripeStandardPaymentSettings.LivePublishableKey = model.LivePublishableKey;
            stripeStandardPaymentSettings.LiveSecretKey = model.LiveSecretKey;
            stripeStandardPaymentSettings.PaymentType = (PaymentType) model.PaymentTypeId;

            stripeStandardPaymentSettings.AdditionalFee = model.AdditionalFee;
            stripeStandardPaymentSettings.AdditionalFeePercentage = model.AdditionalFeePercentage;

            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.UseSandbox, model.UseSandbox_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.Title, model.Title_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.TestPublishableKey, model.TestPublishableKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.TestSecretKey, model.TestSecretKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.LivePublishableKey, model.LivePublishableKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.LiveSecretKey, model.LiveSecretKey_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.PaymentType, model.PaymentTypeId_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.AdditionalFee, model.AdditionalFee_OverrideForStore, storeScope, false);
            await _settingService.SaveSettingOverridablePerStoreAsync(stripeStandardPaymentSettings, x => x.AdditionalFeePercentage, model.AdditionalFeePercentage_OverrideForStore, storeScope, false);

            //now clear settings cache
            await _settingService.ClearCacheAsync();

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Plugins.Saved"));

            return await Configure();
        }
        #endregion
    }
}
