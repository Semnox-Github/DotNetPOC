/********************************************************************************************
 * Project Name - Common
 * Description  - API for the sheet objects. This is a generic Controller which can be used for all the sheet object upload and to build the template.
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By       Remarks          
 *********************************************************************************************
 *2.80        16-Jun-2019   Mushahid Faizan   Created 
 *2.110.0     08-Nov-2020   Mushahid Faizan   Web Inventory UI Changes
 *2.150.0     08-Nov-2021   Abhishek          Web Inventory Redesign - Product Activity View and BOM Upload/Download
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Semnox.CommonAPI.Helpers;
using Semnox.Core.GenericUtilities;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Customer;
using Semnox.Parafait.Discounts;
using Semnox.Parafait.Game;
using Semnox.Parafait.Inventory;
using Semnox.Parafait.Inventory.PhysicalCount;
using Semnox.Parafait.Inventory.Recipe;
using Semnox.Parafait.Product;
using Semnox.Parafait.Redemption;
using Semnox.Parafait.Tags;
using Semnox.Parafait.User;
using Semnox.Parafait.Vendor;

namespace Semnox.CommonAPI.Common
{
    public class SheetUploadController : ApiController
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// Get the JSON Sheet object.
        /// </summary>       
        /// <returns>HttpResponseMessage</returns>
        [HttpGet]
        [Authorize]
        [Route("api/Common/Sheets")]
        public HttpResponseMessage Get(string activityType = null, DateTime? fromDate = null, DateTime? toDate = null, string transactions = null, bool credits = false,
                                        bool courtesy = false, bool bonus = false, bool time = false, bool buildChildRecords = false, int physicalCountId = -1, int locationId = -1,
                                        bool buildTemplate = false, int productId = -1, int lotId = -1, int discountId = -1)
        {
            log.LogMethodEntry(activityType, fromDate, toDate, transactions, credits, courtesy, bonus, time, buildChildRecords, physicalCountId, locationId,
                buildTemplate, productId, lotId, discountId);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                Sheet sheet = new Sheet();
                List<Sheet> sheetList = new List<Sheet>();

                LookupValuesList serverTimeObject = new LookupValuesList(executionContext);
                DateTime startDate = serverTimeObject.GetServerDateTime();
                DateTime endDate = startDate.AddDays(1);

                if (fromDate != null)
                {
                    startDate = Convert.ToDateTime(fromDate.ToString());
                    if (startDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }

                if (toDate != null)
                {
                    endDate = Convert.ToDateTime(toDate.ToString());
                    if (endDate == DateTime.MinValue)
                    {
                        string customException = "Invalid date format, expected format is yyyy-mm-dd hh:mm:ss";
                        log.Error(customException);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = customException });
                    }
                }
                else
                {
                    endDate = serverTimeObject.GetServerDateTime();
                }

                if (activityType.ToUpper().ToString() == "PARTNERREVENUE")
                {
                    PartnerRevenueShareList discountServices = new PartnerRevenueShareList(executionContext);
                    List<string> gamePlays = new List<string>();
                    // Below condition is used to add the columns for the query.
                    if (credits)
                    {
                        gamePlays.Add("Credits");
                    }
                    if (courtesy)
                    {
                        gamePlays.Add("Courtesy");
                    }
                    if (bonus)
                    {
                        gamePlays.Add("Bonus");
                    }
                    if (time)
                    {
                        gamePlays.Add("Time");
                    }
                    // Check for transaction methods, Default it should be Card Payment or else 'All' need to be passed 
                    if (string.IsNullOrEmpty(transactions))
                    {
                        transactions = "CardPayment";
                    }
                    sheet = discountServices.GetPartnerRevenueSheet(startDate, endDate, transactions, gamePlays);
                    log.LogMethodExit(sheet);
                }

                if ((activityType.ToUpper().ToString() == "PAYMENTMODECOUPONS"
                     || activityType.ToUpper().ToString() == "PRODUCTDISCOUNTCOUPONS"))
                {
                    DiscountServices discountServices = new DiscountServices(executionContext);
                    sheet = discountServices.BuildTemplete(discountId, activityType, buildTemplate);
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "CUSTOMERS"))
                {
                    CustomerListBL customerListBL = new CustomerListBL(executionContext);
                    sheet = customerListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "UOM"))
                {
                    UOMList uomListBL = new UOMList(executionContext);
                    sheet = uomListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "REQUISITION"))
                {
                    RequisitionList requisitionListBL = new RequisitionList(executionContext);
                    sheetList = requisitionListBL.BuildTemplate(buildChildRecords);
                    log.LogMethodExit(sheetList);
                }
                if ((activityType.ToUpper().ToString() == "PURCHASEORDER"))
                {
                    PurchaseOrderList requisitionListBL = new PurchaseOrderList(executionContext);
                    sheetList = requisitionListBL.BuildTemplate(buildChildRecords);
                    log.LogMethodExit(sheetList);
                }
                if ((activityType.ToUpper().ToString() == "CATEGORY"))
                {
                    CategoryList categoryListBL = new CategoryList(executionContext);
                    sheet = categoryListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "LOCATION"))
                {
                    LocationList locationListBL = new LocationList(executionContext);
                    sheet = locationListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "REDEMPTIONCURRENCY"))
                {
                    RedemptionCurrencyList redemptionCurrencyListBL = new RedemptionCurrencyList(executionContext);
                    sheet = redemptionCurrencyListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "REDEMPTIONCURRENCYRULE"))
                {
                    RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext);
                    sheet = redemptionCurrencyRuleListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "TAXES"))
                {
                    TaxList taxListBL = new TaxList(executionContext);
                    sheet = taxListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "VENDOR"))
                {
                    VendorList vendorListBL = new VendorList(executionContext);
                    sheet = vendorListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "APPROVALRULE"))
                {
                    ApprovalRulesList approvalRulesListBL = new ApprovalRulesList(executionContext);
                    sheet = approvalRulesListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "SEGMENTS"))
                {
                    SegmentDefinitionList segmentDefinitonListBL = new SegmentDefinitionList(executionContext);
                    sheet = segmentDefinitonListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "WASTAGES"))
                {
                    InventoryAdjustmentsList inventoryAdjustmentsListBL = new InventoryAdjustmentsList(executionContext);
                    sheet = inventoryAdjustmentsListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "ADJUSTMENTS"))
                {
                    InventoryAdjustmentsList inventoryAdjustmentsListBL = new InventoryAdjustmentsList(executionContext);
                    sheet = inventoryAdjustmentsListBL.BuildAjustmentTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "PHYSICALCOUNT"))
                {
                    InventoryPhysicalCountList inventoryPhysicalCountListBL = new InventoryPhysicalCountList(executionContext);
                    sheet = inventoryPhysicalCountListBL.BuildTemplate();
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "PHYSICALCOUNTREVIEW"))
                {
                    if (physicalCountId < 0)
                    {
                        throw new Exception("Physical Count is not Valid");
                    }
                    DateTime physicalCountStartDate = Convert.ToDateTime(fromDate);
                    if (fromDate == null)
                    {
                        physicalCountStartDate = ServerDateTime.Now;
                    }
                    PhysicalCountReviewList inventoryPhysicalCountListBL = new PhysicalCountReviewList(executionContext);
                    sheet = inventoryPhysicalCountListBL.BuildTemplate(physicalCountId, locationId, physicalCountStartDate);
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "NOTIFICATIONTAGS"))
                {
                    NotificationTagsListBL notificationTagsListBL = new NotificationTagsListBL(executionContext);
                    sheet = notificationTagsListBL.BuildTemplate(buildTemplate);
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "KITCHENPRODUCTIONS"))
                {
                    RecipeManufacturingDetailsListBL recipeManufacturingListBL = new RecipeManufacturingDetailsListBL(executionContext);
                    sheet = recipeManufacturingListBL.BuildTemplate(fromDate, toDate);
                    log.LogMethodExit(sheet);
                }

                if ((activityType.ToUpper().ToString() == "RECIPEPLANDETAILS"))
                {
                    RecipePlanDetailsListBL recipePlanListBL = new RecipePlanDetailsListBL(executionContext);
                    sheet = recipePlanListBL.BuildTemplate(fromDate, toDate);
                    log.LogMethodExit(sheet);
                }

                if (activityType.ToUpper().ToString() == "MACHINES")
                {
                    MachineServices machineServices = new MachineServices(executionContext);
                    sheet = machineServices.BuildTemplete();
                    log.LogMethodExit(sheet);
                }
                if (activityType.ToUpper().ToString() == "IMPORTDISCOUNTSUSEDCOUPONS")
                {
                    DiscountServices discountServices = new DiscountServices(executionContext);
                    sheet = discountServices.DiscountUsedCouponBuildTemplete(buildTemplate);
                    log.LogMethodExit(sheet);
                }
                if ((activityType.ToUpper().ToString() == "PURCHASEORDER"))
                {
                    PurchaseOrderList requisitionListBL = new PurchaseOrderList(executionContext);
                    sheetList = requisitionListBL.BuildTemplate(buildChildRecords);
                    log.LogMethodExit(sheetList);
                }
                if ((activityType.ToUpper().ToString() == "PRODUCTACTIVITYVIEW"))
                {
                    InventoryAdjustmentsList inventoryAdjustmentsListBL = new InventoryAdjustmentsList(executionContext);
                    sheet = inventoryAdjustmentsListBL.BuildProductActivityViewTemplate(productId, locationId, lotId);
                    log.LogMethodExit(sheetList);
                }
                if (activityType.ToUpper().ToString() == "BOM")
                {
                    BOMList bOMListBL = new BOMList(executionContext);
                    sheet = bOMListBL.BuildTemplate(buildTemplate);
                    log.LogMethodExit(sheet);
                }
                log.LogMethodExit(sheet);
                return Request.CreateResponse(HttpStatusCode.OK, new { data = sheet, SheetList = sheetList });

            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }


        /// <summary>
        /// Post the JSON Sheet object
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Common/Sheets")]
        public HttpResponseMessage Post([FromBody]Sheet sheet, [FromUri]string activityType, [FromUri]int discountId = -1,
                                            [FromUri]int discountHeaderId = -1, [FromUri]int paymentModeId = -1, [FromUri]int physicalCountId = -1)
        {
            log.LogMethodEntry(sheet, activityType, discountId, discountHeaderId, paymentModeId);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);
            try
            {
                CustomerListBL customerListBL = new CustomerListBL(executionContext);
                DiscountServices discountServices = new DiscountServices(executionContext);
                var content = (dynamic)null;
                switch (activityType.ToUpper().ToString())
                {
                    case "CUSTOMERS": // For WMS : Promotions Module
                        if (sheet != null)
                        {
                            content = customerListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "UOM":
                        if (sheet != null)
                        {
                            UOMList uOMListBL = new UOMList(executionContext);
                            content = uOMListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "CATEGORY":
                        if (sheet != null)
                        {
                            CategoryList categoryListBL = new CategoryList(executionContext);
                            content = categoryListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "LOCATION":
                        if (sheet != null)
                        {
                            LocationList locationListBL = new LocationList(executionContext);
                            content = locationListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "REDEMPTIONCURRENCY":
                        if (sheet != null)
                        {
                            RedemptionCurrencyList redemptionCurrencyListBL = new RedemptionCurrencyList(executionContext);
                            content = redemptionCurrencyListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "REDEMPTIONCURRENCYRULE":
                        if (sheet != null)
                        {
                            RedemptionCurrencyRuleListBL redemptionCurrencyRuleListBL = new RedemptionCurrencyRuleListBL(executionContext);
                            content = redemptionCurrencyRuleListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "TAXES":
                        if (sheet != null)
                        {
                            TaxList taxListBL = new TaxList(executionContext);
                            content = taxListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "VENDOR":
                        if (sheet != null)
                        {
                            VendorList vendorListBL = new VendorList(executionContext);
                            content = vendorListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "APPROVALRULE":
                        if (sheet != null)
                        {
                            ApprovalRulesList approvalRulesListBL = new ApprovalRulesList(executionContext);
                            content = approvalRulesListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "SEGMENTS":
                        if (sheet != null)
                        {
                            SegmentDefinitionList segmentDefinitionListBL = new SegmentDefinitionList(executionContext);
                            content = segmentDefinitionListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "PHYSICALCOUNT":
                        if (sheet != null)
                        {
                            InventoryPhysicalCountList inventoryPhysicalCountListBL = new InventoryPhysicalCountList(executionContext);
                            content = inventoryPhysicalCountListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "PHYSICALCOUNTREVIEW":
                        if (sheet != null)
                        {
                            PhysicalCountReviewList inventoryPhysicalCountListBL = new PhysicalCountReviewList(executionContext);
                            content = inventoryPhysicalCountListBL.BulkUpload(sheet, physicalCountId);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "NOTIFICATIONTAGS":
                        if (sheet != null)
                        {
                            NotificationTagsListBL notificationTagsListBL = new NotificationTagsListBL(executionContext);
                            content = notificationTagsListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "ADJUSTMENTS":
                        if (sheet != null)
                        {
                            InventoryAdjustmentsList inventoryAdjustmentsListBL = new InventoryAdjustmentsList(executionContext);
                            content = inventoryAdjustmentsListBL.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "MACHINES":
                        if (sheet != null)
                        {
                            MachineServices machineServices = new MachineServices(executionContext);
                            content = machineServices.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "IMPORTDISCOUNTSUSEDCOUPONS":
                        if (sheet != null)
                        {
                            content = discountServices.BulkUploadUsedCoupons(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "PRODUCTDISCOUNTCOUPONS":
                    case "PAYMENTMODECOUPONS":
                        if (sheet != null)
                        {
                            /// In sitesetup module => Paymentmode entity, the paymentModeId > 0
                            /// In Products module => Discounts Entity, the paymentModeId = -1
                            //If paymentModeId > 0 then DTODefination is for Payment coupon else DTODefination is for Discount Coupon
                            content = discountServices.BulkUpload(sheet, discountId, discountHeaderId, paymentModeId);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "BOM":
                        if (sheet != null)
                        {
                            BOMList bOMList = new BOMList(executionContext);
                            content = bOMList.BulkUpload(sheet);
                            log.LogMethodExit(content);
                        }
                        break;
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = valEx.ValidationErrorList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }

        /// <summary>
        /// Post the JSON Sheet object
        /// </summary>
        /// <param name="sheetList"></param>
        /// <returns>HttpResponseMessage</returns>
        [HttpPost]
        [Authorize]
        [Route("api/Common/SheetLists")]
        public HttpResponseMessage PostSheetList([FromBody]List<Sheet> sheetList, string activityType)
        {
            log.LogMethodEntry(sheetList);
            ExecutionContext executionContext = ExecutionContextBuilder.GetExecutionContext(Request);

            try
            {
                var content = (dynamic)null;
                switch (activityType.ToUpper().ToString())
                {
                    case "REQUISITION":
                        if (sheetList != null && sheetList.Any())
                        {
                            RequisitionList requisitionListBL = new RequisitionList(executionContext);
                            content = requisitionListBL.BulkUpload(sheetList);
                            log.LogMethodExit(content);
                        }
                        break;
                    case "PURCHASEORDER":
                        if (sheetList != null && sheetList.Any())
                        {
                            PurchaseOrderList purchaseOrderListBL = new PurchaseOrderList(executionContext);
                            content = purchaseOrderListBL.BulkUpload(sheetList);
                            log.LogMethodExit(content);
                        }
                        break;
                }
                log.LogMethodExit();
                return Request.CreateResponse(HttpStatusCode.OK, new { data = content });
            }
            catch (ValidationException valEx)
            {
                log.Error(valEx);
                return Request.CreateResponse(HttpStatusCode.BadRequest, new { data = valEx.ValidationErrorList });
            }
            catch (Exception ex)
            {
                string customException = GenericExceptionMessage.GetValidCustomExeptionMessage(ex, executionContext);
                log.Error(customException);
                return Request.CreateResponse(HttpStatusCode.InternalServerError, new { data = customException });
            }
        }
    }
}
