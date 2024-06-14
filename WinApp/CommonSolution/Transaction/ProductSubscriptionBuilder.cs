/********************************************************************************************
 * Project Name - ProductSubscriptionBuilder BL
 * Description  -BL class to generate subscription header and bill cycle DTO using productSubscriptionDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By         Remarks          
 *********************************************************************************************   
 *2.110.0     09-Dec-2020    Fiona             Created for Subscription changes                                                                               
 *2.120.0     18-Mar-2021    Guru S A          For Subscription phase 2 changes
 ********************************************************************************************/
using Semnox.Core.GenericUtilities;
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Customer.Accounts;
using Semnox.Parafait.Languages;
using Semnox.Parafait.Product;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Product Subscription Builder
    /// </summary>
    public class ProductSubscriptionBuilder
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext = null;
        //private ProductSubscriptionDTO productSubscriptionDTO = null;
        private SubscriptionHeaderDTO subscriptionHeaderDTO = null;
        private LookupValuesList serverTimeOjbect = null;
        private bool taxInclusivePrice = false;
        private string selectedPaymentCollectionMode = string.Empty;
        private bool autoRenew = false;
        /// <summary>
        /// ProductSubscriptionBuilder construtor
        /// </summary>
        /// <param name="executionContext"></param> 
        public ProductSubscriptionBuilder(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            serverTimeOjbect = new LookupValuesList(executionContext);
            log.LogMethodExit();
        }
        /// <summary>
        /// BuildSubscriptionDTO from productsDTO
        /// </summary>
        /// <param name="productsDTO"></param>
        /// <returns></returns>
        public SubscriptionHeaderDTO BuildSubscriptionDTO(ProductsDTO productsDTO)
        {
            log.LogMethodEntry(subscriptionHeaderDTO);
            SubscriptionHeaderDTO newSubscriptionHeaderDTO = null;
            if (productsDTO != null && productsDTO.ProductId > -1)
            {
                ProductSubscriptionListBL productSubscriptionListBL = new ProductSubscriptionListBL(executionContext);
                List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.PRODUCTS_ID, productsDTO.ProductId.ToString()));
                searchParameters.Add(new KeyValuePair<ProductSubscriptionDTO.SearchByParameters, string>(ProductSubscriptionDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
                List<ProductSubscriptionDTO> productSubscriptionDTOList =  productSubscriptionListBL.GetProductSubscriptionDTOList(searchParameters);
                if (productSubscriptionDTOList != null && productSubscriptionDTOList.Any())
                {
                    ProductSubscriptionDTO productSubscriptionDTO = productSubscriptionDTOList[0];
                    this.taxInclusivePrice = string.IsNullOrWhiteSpace(productsDTO.TaxInclusivePrice) == false ? productsDTO.TaxInclusivePrice.Equals("Y") : false;
                    this.selectedPaymentCollectionMode = productSubscriptionDTO.PaymentCollectionMode;
                    this.autoRenew = productSubscriptionDTO.AutoRenew;
                    if (productSubscriptionDTO != null)
                    {
                        newSubscriptionHeaderDTO = CreateSubscriptionHeader(productSubscriptionDTO);
                        List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = CreateSubscriptionBillingSchedule(productSubscriptionDTO);
                        newSubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList = subscriptionBillingScheduleDTOList;
                        newSubscriptionHeaderDTO.SubscriptionStartDate = subscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillFromDate);
                        newSubscriptionHeaderDTO.SubscriptionEndDate = subscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillToDate);
                    }
                }
                else
                {
                    string message = MessageContainerList.GetMessage(executionContext, 2196, "ProductSubscription", productsDTO.ProductId);
                    log.LogMethodExit(null, "Throwing Exception - " + message);
                    throw new EntityNotFoundException(message); 
                }
            }
            log.LogMethodExit(newSubscriptionHeaderDTO);
            return newSubscriptionHeaderDTO;
        }

        /// <summary>
        ///  Build Subscription DTO
        /// </summary>
        /// <param name="subscriptionHeaderDTO"></param>
        /// <returns></returns>
        public SubscriptionHeaderDTO BuildSubscriptionDTO(SubscriptionHeaderDTO subscriptionHeaderDTO)
        {
            log.LogMethodEntry(subscriptionHeaderDTO);
            //ProductSubscriptionDTO productSubscriptionDTO
            ProductSubscriptionBL productSubscriptionBL = new ProductSubscriptionBL(executionContext, subscriptionHeaderDTO.ProductSubscriptionId);
            SubscriptionHeaderDTO newSubscriptionHeaderDTO = null;
            ProductSubscriptionDTO productSubscriptionDTO = productSubscriptionBL.ProductSubscriptionDTO;
            this.taxInclusivePrice = subscriptionHeaderDTO.TaxInclusivePrice;
            this.selectedPaymentCollectionMode = subscriptionHeaderDTO.SelectedPaymentCollectionMode;
            this.autoRenew = subscriptionHeaderDTO.AutoRenew;
            if (productSubscriptionDTO != null)
            {
                newSubscriptionHeaderDTO = CreateSubscriptionHeader(productSubscriptionDTO);
                newSubscriptionHeaderDTO.CustomerId = subscriptionHeaderDTO.CustomerId;
                newSubscriptionHeaderDTO.CustomerContactId = subscriptionHeaderDTO.CustomerContactId;
                newSubscriptionHeaderDTO.CustomerCreditCardsId = subscriptionHeaderDTO.CustomerCreditCardsId;
                List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = CreateSubscriptionBillingSchedule(productSubscriptionDTO);
                newSubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList = subscriptionBillingScheduleDTOList;
                newSubscriptionHeaderDTO.SubscriptionStartDate = subscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillFromDate);
                newSubscriptionHeaderDTO.SubscriptionEndDate = subscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillToDate);
            }
            log.LogMethodExit(newSubscriptionHeaderDTO);
            return newSubscriptionHeaderDTO;
        } 

        private SubscriptionHeaderDTO CreateSubscriptionHeader(ProductSubscriptionDTO productSubscriptionDTO)
        {
            log.LogMethodEntry(productSubscriptionDTO);
            SubscriptionHeaderDTO subscriptionHeaderDTO = null;
            if (productSubscriptionDTO != null)
            {
                subscriptionHeaderDTO = new SubscriptionHeaderDTO(-1, -1, -1, -1, -1, -1, productSubscriptionDTO.ProductsId, productSubscriptionDTO.ProductSubscriptionId,
                               productSubscriptionDTO.ProductSubscriptionName, productSubscriptionDTO.ProductSubscriptionDescription, productSubscriptionDTO.SubscriptionPrice, this.taxInclusivePrice,
                               productSubscriptionDTO.SubscriptionCycle, productSubscriptionDTO.UnitOfSubscriptionCycle, productSubscriptionDTO.SubscriptionCycleValidity,
                               //productSubscriptionDTO.SeasonalSubscription, 
                               productSubscriptionDTO.SeasonStartDate, //productSubscriptionDTO.SeasonEndDate, 
                               productSubscriptionDTO.FreeTrialPeriodCycle,
                               productSubscriptionDTO.BillInAdvance, productSubscriptionDTO.PaymentCollectionMode, this.selectedPaymentCollectionMode, this.autoRenew,
                               productSubscriptionDTO.AutoRenewalMarkupPercent, productSubscriptionDTO.RenewalGracePeriodCycle, productSubscriptionDTO.NoOfRenewalReminders,
                               productSubscriptionDTO.ReminderFrequencyInDays, productSubscriptionDTO.SendFirstReminderBeforeXDays, productSubscriptionDTO.AllowPause,
                               SubscriptionStatus.ACTIVE,null,null,null,null,-1,DateTime.MinValue,DateTime.MinValue,null,null,null,null, productSubscriptionDTO.CancellationOption,null,null,
                               null); 
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Product Subscription DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            log.LogMethodExit(subscriptionHeaderDTO);
            return subscriptionHeaderDTO;
        }
        private List<SubscriptionBillingScheduleDTO> CreateSubscriptionBillingSchedule(ProductSubscriptionDTO productSubscriptionDTO)
        {
            log.LogMethodEntry(productSubscriptionDTO);
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = null;
            if (productSubscriptionDTO != null)
            {
                int subScriptionCycle = productSubscriptionDTO.SubscriptionCycle;
                string cycleUOM = productSubscriptionDTO.UnitOfSubscriptionCycle;
                int subscriptionCycleValidity = productSubscriptionDTO.SubscriptionCycleValidity;
                //DateTime? seasonStartDate = (productSubscriptionDTO.SeasonalSubscription ? productSubscriptionDTO.SeasonStartDate : null);
                //DateTime? seasonEndDate = (productSubscriptionDTO.SeasonalSubscription ? productSubscriptionDTO.SeasonEndDate : null);
                int yearValue = -1;
                int monthValue = -1;
                int dayValue = -1;
                if (productSubscriptionDTO.SeasonStartDate != null)
                {
                    yearValue = (serverTimeOjbect.GetServerDateTime().Year);
                    monthValue = ((DateTime)productSubscriptionDTO.SeasonStartDate).Month;
                    dayValue = ((DateTime)productSubscriptionDTO.SeasonStartDate).Day;
                }
                DateTime? seasonBasedBillFromDate = (yearValue == -1) ? (DateTime?)null : new DateTime(yearValue, monthValue, dayValue);
                DateTime billFromDate = (seasonBasedBillFromDate == null ? serverTimeOjbect.GetServerDateTime().Date : ((DateTime)seasonBasedBillFromDate).Date);
                int freeTrialPeriod = productSubscriptionDTO.FreeTrialPeriodCycle == null ? 0 : (int)productSubscriptionDTO.FreeTrialPeriodCycle;
                int renewalGracePeriod = productSubscriptionDTO.RenewalGracePeriodCycle == null ? 0 : (int)productSubscriptionDTO.RenewalGracePeriodCycle; 

                subscriptionBillingScheduleDTOList = GenerateBillingCycleDTO(billFromDate, subScriptionCycle, cycleUOM, subscriptionCycleValidity, productSubscriptionDTO.SubscriptionPrice, productSubscriptionDTO.BillInAdvance, freeTrialPeriod, renewalGracePeriod);
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Product Subscription DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            log.LogMethodExit(subscriptionBillingScheduleDTOList);
            return subscriptionBillingScheduleDTOList;
        }
        

        private List<SubscriptionBillingScheduleDTO> GenerateBillingCycleDTO(DateTime billFromDate, int subScriptionCycle, string cycleUOM, 
            int subscriptionCycleValidity, decimal subscriptionPrice, bool billInAdvance, int freeTrialPeriod, int renewalGracePeriod)
        {
            log.LogMethodEntry(billFromDate, subScriptionCycle, cycleUOM, subscriptionCycleValidity, subscriptionPrice, billInAdvance, freeTrialPeriod, renewalGracePeriod);
            DateTime billToDate;
            DateTime billOnDate;
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = new List<SubscriptionBillingScheduleDTO>();
            while (subscriptionCycleValidity > 0)
            {
                billToDate = (cycleUOM == UnitOfSubscriptionCycle.DAYS ? billFromDate.AddDays(subScriptionCycle)
                               : (cycleUOM == UnitOfSubscriptionCycle.MONTHS ? billFromDate.AddMonths(subScriptionCycle)
                                : billFromDate.AddYears(subScriptionCycle)));
                billOnDate = billInAdvance ? billFromDate : billToDate;
                decimal cycleSubscriptionPrice = (freeTrialPeriod == 0 ? subscriptionPrice : 0);
                log.Info("freeTrialPeriod: " + freeTrialPeriod.ToString());
                log.Info("cycleSubscriptionPrice: " + cycleSubscriptionPrice.ToString());
                if (freeTrialPeriod > 0)
                {
                    freeTrialPeriod--;
                }
                SubscriptionBillingScheduleDTO billCycleDTO = new SubscriptionBillingScheduleDTO(-1, -1, -1, -1, billFromDate,
                                                                      billToDate, billOnDate, cycleSubscriptionPrice, null, string.Empty, string.Empty,
                                                                      string.Empty, 0, SubscriptionStatus.ACTIVE, null,null,SubscriptionLineType.BILLING_LINE);
                subscriptionBillingScheduleDTOList.Add(billCycleDTO);
                billFromDate = billToDate;
                subscriptionCycleValidity--;
            }
            if (renewalGracePeriod > 0)
            {
                while (renewalGracePeriod > 0)
                {
                    billToDate = (cycleUOM == UnitOfSubscriptionCycle.DAYS ? billFromDate.AddDays(subScriptionCycle)
                               : (cycleUOM == UnitOfSubscriptionCycle.MONTHS ? billFromDate.AddMonths(subScriptionCycle)
                                : billFromDate.AddYears(subScriptionCycle)));
                    billOnDate = billInAdvance ? billFromDate : billToDate;
                    decimal cycleSubscriptionPrice = 0; 
                    log.Info("cycleSubscriptionPrice: " + cycleSubscriptionPrice.ToString()); 
                    SubscriptionBillingScheduleDTO billCycleDTO = new SubscriptionBillingScheduleDTO(-1, -1, -1, -1, billFromDate,
                                                                          billToDate, billOnDate, cycleSubscriptionPrice, null, string.Empty, string.Empty,
                                                                          string.Empty, 0, SubscriptionStatus.ACTIVE, null, null, SubscriptionLineType.GRACE_LINE);
                    subscriptionBillingScheduleDTOList.Add(billCycleDTO);
                    billFromDate = billToDate;
                    renewalGracePeriod--;
                }
            }
            log.LogMethodExit(subscriptionBillingScheduleDTOList);
            return subscriptionBillingScheduleDTOList;

        }

        /// <summary>
        /// Build Subscription DTO For Renewal
        /// </summary>
        /// <param name="subscriptionHeaderDTO"></param>
        /// <param name="productsDTO"></param>
        /// <param name="sourceSubscriptionId"></param>
        /// <returns></returns>
        public SubscriptionHeaderDTO BuildSubscriptionDTOForRenewal(SubscriptionHeaderDTO subscriptionHeaderDTO, ProductsDTO productsDTO, int sourceSubscriptionId)
        {
            log.LogMethodEntry(subscriptionHeaderDTO, productsDTO, sourceSubscriptionId);
            SubscriptionHeaderDTO newSubscriptionHeaderDTO = null;
            if (productsDTO != null && productsDTO.ProductSubscriptionDTO != null)
            {
                this.subscriptionHeaderDTO = subscriptionHeaderDTO;
                ProductSubscriptionDTO productSubscriptionDTO = productsDTO.ProductSubscriptionDTO;
                this.taxInclusivePrice = string.IsNullOrWhiteSpace(productsDTO.TaxInclusivePrice) == false ? productsDTO.TaxInclusivePrice.Equals("Y") : false;
                this.selectedPaymentCollectionMode = subscriptionHeaderDTO.SelectedPaymentCollectionMode;
                this.autoRenew = subscriptionHeaderDTO.AutoRenew;
                if (productSubscriptionDTO != null)
                {
                    newSubscriptionHeaderDTO = CreateSubscriptionHeader(productSubscriptionDTO);
                    if (sourceSubscriptionId == -1)
                    {
                        throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2992));
                        // "Source subscription id information is missing. Unable to build subscription details for renewal"
                    }
                    newSubscriptionHeaderDTO.SourceSubscriptionHeaderId = sourceSubscriptionId;
                    newSubscriptionHeaderDTO.AutoRenew = subscriptionHeaderDTO.AutoRenew;
                    newSubscriptionHeaderDTO.AutoRenewalMarkupPercent = subscriptionHeaderDTO.AutoRenewalMarkupPercent;
                    newSubscriptionHeaderDTO.CustomerId = subscriptionHeaderDTO.CustomerId;
                    newSubscriptionHeaderDTO.CustomerContactId = subscriptionHeaderDTO.CustomerContactId;
                    newSubscriptionHeaderDTO.CustomerCreditCardsId = subscriptionHeaderDTO.CustomerCreditCardsId;
                    List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = CreateSubscriptionBillSchedulesForRenewal(productsDTO);
                    newSubscriptionHeaderDTO.SubscriptionBillingScheduleDTOList = subscriptionBillingScheduleDTOList;
                    newSubscriptionHeaderDTO.SubscriptionStartDate = subscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillFromDate);
                    newSubscriptionHeaderDTO.SubscriptionEndDate = subscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Max(sbs => sbs.BillToDate);
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 2907, MessageContainerList.GetMessage(executionContext,"Subscription")));
            }
            log.LogMethodExit(newSubscriptionHeaderDTO);
            return newSubscriptionHeaderDTO;
        } 

        private List<SubscriptionBillingScheduleDTO> CreateSubscriptionBillSchedulesForRenewal(ProductsDTO productsDTO)
        {
            log.LogMethodEntry();
            List<SubscriptionBillingScheduleDTO> subscriptionBillingScheduleDTOList = null; 
           
            int subScriptionCycle = subscriptionHeaderDTO.SubscriptionCycle;
            string cycleUOM = subscriptionHeaderDTO.UnitOfSubscriptionCycle;
            int subscriptionCycleValidity = subscriptionHeaderDTO.SubscriptionCycleValidity;
            //DateTime? seasonStartDate = (subscriptionHeaderDTO.SeasonalSubscription ? subscriptionHeaderDTO.SeasonStartDate : null);
            //DateTime? seasonEndDate = (productSubscriptionDTO.SeasonalSubscription ? productSubscriptionDTO.SeasonEndDate : null);
            int yearValue = -1;
            int monthValue = -1;
            int dayValue = -1;
            if (productsDTO != null && productsDTO.ProductSubscriptionDTO != null && productsDTO.ProductSubscriptionDTO.SeasonStartDate != null)
            {
                yearValue = ((DateTime)this.subscriptionHeaderDTO.SubscriptionEndDate).AddYears(1).Year;
                monthValue = ((DateTime)productsDTO.ProductSubscriptionDTO.SeasonStartDate).Month;
                dayValue = ((DateTime)productsDTO.ProductSubscriptionDTO.SeasonStartDate).Day;
            }
            DateTime? seasonBasedBillFromDate = (yearValue == -1) ? (DateTime?)null : new DateTime(yearValue, monthValue, dayValue);
            DateTime billFromDate = (seasonBasedBillFromDate == null ? this.subscriptionHeaderDTO.SubscriptionEndDate : ((DateTime)seasonBasedBillFromDate).Date);

            //DateTime billFromDate = this.subscriptionHeaderDTO.SubscriptionEndDate.AddDays(1);
            Decimal subscriptionPrice = (subscriptionHeaderDTO.AutoRenewalMarkupPercent == 0 
                                                ? productsDTO.ProductSubscriptionDTO.SubscriptionPrice
                                                : Math.Round((this.subscriptionHeaderDTO.SubscriptionPrice
                                                                      + (this.subscriptionHeaderDTO.SubscriptionPrice * subscriptionHeaderDTO.AutoRenewalMarkupPercent / 100)), 2));
            log.Info("subscriptionPrice :" + subscriptionPrice.ToString());
            int freeTrialPeriod = 0;
            int renewalGracePeriod = 0;
            subscriptionBillingScheduleDTOList = GenerateBillingCycleDTO(billFromDate, subScriptionCycle, cycleUOM, subscriptionCycleValidity, subscriptionPrice, subscriptionHeaderDTO.BillInAdvance, freeTrialPeriod, renewalGracePeriod);
            log.LogMethodExit(subscriptionBillingScheduleDTOList);
            return subscriptionBillingScheduleDTOList;
        }
        /// <summary>
        /// Get Subscription Product Purchase Price
        /// </summary>
        /// <param name="subscriptionHeaderDTO"></param>
        /// <param name="taxPercentage"></param>
        /// <returns></returns>
        public double GetSubscriptionProductPurchasePrice(SubscriptionHeaderDTO subscriptionHeaderDTO, double taxPercentage)
        {
            log.LogMethodEntry();
            double subscriptionProductPrice = 0;
            if (subscriptionHeaderDTO != null)
            {
                SubscriptionPaymentCollectionMode.ValidSelectedPaymentCollectionMode(subscriptionHeaderDTO.SelectedPaymentCollectionMode);
                if (subscriptionHeaderDTO.SelectedPaymentCollectionMode == SubscriptionPaymentCollectionMode.SUBSCRIPTION_CYCLE ) 
                {
                    if (subscriptionHeaderDTO.SourceSubscriptionHeaderId == -1)
                    {
                        if (subscriptionHeaderDTO.FreeTrialPeriodCycle == null || subscriptionHeaderDTO.FreeTrialPeriodCycle == 0)
                        {
                            subscriptionProductPrice = (double)subscriptionHeaderDTO.SubscriptionPrice;
                        }
                        else
                        {
                            //Get price from first cycle when free trial is set
                            DateTime minBillFromDate = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Where(sbs => sbs.LineType == SubscriptionLineType.BILLING_LINE).Min(sbs => sbs.BillFromDate);
                            SubscriptionBillingScheduleDTO subscriptionBillingScheduleDTO = subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Find(sbs => sbs.BillFromDate == minBillFromDate && sbs.LineType == SubscriptionLineType.BILLING_LINE);
                            subscriptionProductPrice = (double)subscriptionBillingScheduleDTO.BillAmount;
                        }
                    }
                    else
                    {
                        ///during renewal do not charge first cycle
                        subscriptionProductPrice = 0;
                    }
                }
                else if (subscriptionHeaderDTO.SelectedPaymentCollectionMode == SubscriptionPaymentCollectionMode.FULL)
                {
                    //for renewal also do full charge upon renewal
                    subscriptionProductPrice = (double)subscriptionHeaderDTO.SubscriptionBillingScheduleDTOList.Sum(sbs => sbs.BillAmount);
                } 
                else
                {
                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1144, MessageContainerList.GetMessage(executionContext, "Selected Payment Collection Mode")));
                }
                if (subscriptionHeaderDTO.TaxInclusivePrice)
                {
                    subscriptionProductPrice = subscriptionProductPrice / (1.0 + taxPercentage / 100.0); 
                }
            }
            else
            {
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, "Subscription Header DTO") + " " + MessageContainerList.GetMessage(executionContext, "is null"));
            }
            log.LogMethodExit(subscriptionProductPrice);
            return subscriptionProductPrice;
        }

        private DateTime? GetNewDate(DateTime oldBillFromDate, DateTime newBillFromDate, DateTime? oldDate)
        {
            log.LogMethodEntry(oldBillFromDate, newBillFromDate, oldDate);
            DateTime? newDate = null;
            if (oldDate != null)
            {
                TimeSpan ts = oldBillFromDate - (DateTime)oldDate;
                newDate = newBillFromDate.Add(ts);
            }
            log.LogMethodExit(newDate);
            return newDate;
        }
    }
}
