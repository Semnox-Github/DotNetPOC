/********************************************************************************************
 * Project Name - Inventory
 * Description  - InventoryUseCaseFactory class 
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 *2.100.0    09-Nov-2020       Mushahid Faizan         Created : Web Inventory UI Redesign with REST API
 ********************************************************************************************/
using System.Configuration;
using Semnox.Core.Utilities;
using Semnox.Parafait.Category;
using Semnox.Parafait.Inventory.Location;
using Semnox.Parafait.Vendor;
using Semnox.Parafait.Product;
using Semnox.Parafait.Inventory.Requisition;
using Semnox.Parafait.Inventory.PhysicalCount;
using Semnox.Parafait.Reports;

namespace Semnox.Parafait.Inventory
{
    /// <summary>
    /// 
    /// </summary>
    public class InventoryUseCaseFactory
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ILocationUseCases GetLocationUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILocationUseCases locationUseCases;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                locationUseCases = new RemoteLocationUseCases(executionContext);
            }
            else
            {
                locationUseCases = new LocalLocationUseCases(executionContext);
            }

            log.LogMethodExit(locationUseCases);
            return locationUseCases;
        }

        public static IApprovalRuleUseCases GetApprovalRuleUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IApprovalRuleUseCases approvalRuleUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                approvalRuleUseCases = new RemoteApprovalRuleUseCases(executionContext);
            }
            else
            {
                approvalRuleUseCases = new LocalApprovalRuleUseCases(executionContext);
            }

            log.LogMethodExit(approvalRuleUseCases);
            return approvalRuleUseCases;
        }

        public static IProductsUseCases GetProductUseCases(ExecutionContext executionContext, string requestGuid = null)
        {
            log.LogMethodEntry(executionContext);
            IProductsUseCases productUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                productUseCases = new RemoteProductsUseCases(executionContext, requestGuid);
            }
            else
            {
                productUseCases = new LocalProductsUseCases(executionContext, requestGuid);
            }

            log.LogMethodExit(productUseCases);
            return productUseCases;
        }

        public static ICategoryUseCases GetCategoryUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ICategoryUseCases categoryUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                categoryUseCases = new RemoteCategoryUseCases(executionContext);
            }
            else
            {
                categoryUseCases = new LocalCategoryUseCases(executionContext);
            }

            log.LogMethodExit(categoryUseCases);
            return categoryUseCases;
        }
        public static Semnox.Parafait.Product.ITaxUseCases GetTaxUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ITaxUseCases taxUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                taxUseCases = new RemoteTaxUseCases(executionContext);
            }
            else
            {
                taxUseCases = new LocalTaxUseCases(executionContext);
            }

            log.LogMethodExit(taxUseCases);
            return taxUseCases;
        }
        public static IPurchaseOrderUseCases GetPurchaseOrderUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPurchaseOrderUseCases purchaseOrderUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
               purchaseOrderUseCases = new RemotePurchaseOrderUseCases(executionContext);
            }
            else
            {
                purchaseOrderUseCases = new LocalPurchaseOrderUseCases(executionContext);
            }

            log.LogMethodExit(purchaseOrderUseCases);
            return purchaseOrderUseCases;
        }
        public static IInventoryNotesUseCases GetInventoryNotesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryNotesUseCases inventoryNotesUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                  inventoryNotesUseCases = new RemoteInventoryNotesUseCases(executionContext);
            }
            else
            {
                inventoryNotesUseCases = new LocalInventoryNotesUseCases(executionContext);
            }

            log.LogMethodExit(inventoryNotesUseCases);
            return inventoryNotesUseCases;
        }
        public static IVendorUseCases GetVendorUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IVendorUseCases vendorUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                vendorUseCases = new RemoteVendorUseCases(executionContext);
            }
            else
            {
                vendorUseCases = new LocalVendorUseCases(executionContext);
            }

            log.LogMethodExit(vendorUseCases);
            return vendorUseCases;
        }

        public static IUOMUseCases GetUOMUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IUOMUseCases uomUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                uomUseCases = new RemoteUOMUseCases(executionContext);
            }
            else
            {
                uomUseCases = new LocalUOMUseCases(executionContext);
            }

            log.LogMethodExit(uomUseCases);
            return uomUseCases;
        }
        public static ISegmentDefinitionUseCases GetSegmentDefinitionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ISegmentDefinitionUseCases segmentDefinitionUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                segmentDefinitionUseCases = new RemoteSegmentDefinitionUseCases(executionContext);
            }
            else
            {
                segmentDefinitionUseCases = new LocalSegmentDefinitionUseCases(executionContext);
            }

            log.LogMethodExit(segmentDefinitionUseCases);
            return segmentDefinitionUseCases;
        }
        public static ILocationTypeUseCases GetLocationTypeUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            ILocationTypeUseCases locationTypeUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                locationTypeUseCases = new RemoteLocationTypeUseCases(executionContext);
            }
            else
            {
                locationTypeUseCases = new LocalLocationTypeUseCases(executionContext);
            }

            log.LogMethodExit(locationTypeUseCases);
            return locationTypeUseCases;
        }

        public static IInventoryStockUseCases GetInventoryStockUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryStockUseCases inventoryStockUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                inventoryStockUseCases = new RemoteInventoryStockUseCases(executionContext);
            }
            else
            {
                inventoryStockUseCases = new LocalInventoryStockUseCases(executionContext);
            }

            log.LogMethodExit(inventoryStockUseCases);
            return inventoryStockUseCases;
        }

        public static IRequisitionUseCases GetRequisitionUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRequisitionUseCases requisitionUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                requisitionUseCases = new RemoteRequisitionUseCases(executionContext);
            }
            else
            {
                requisitionUseCases = new LocalRequisitionUseCases(executionContext);
            }

            log.LogMethodExit(requisitionUseCases);
            return requisitionUseCases;
        }

        public static IRequisitionTemplatesUseCases GetRequisitionTemplatesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IRequisitionTemplatesUseCases requisitionTemplatesUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                requisitionTemplatesUseCases = new RemoteRequisitionTemplatesUseCases(executionContext);
            }
            else
            {
                requisitionTemplatesUseCases = new LocalRequisitionTemplatesUseCases(executionContext);
            }

            log.LogMethodExit(requisitionTemplatesUseCases);
            return requisitionTemplatesUseCases;
        }

        public static IReceiptUseCases GetReceiptsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IReceiptUseCases receiptUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                receiptUseCases = new RemoteReceiptUseCases(executionContext);
            }
            else
            {
                receiptUseCases = new LocalReceiptUseCases(executionContext);
            }

            log.LogMethodExit(receiptUseCases);
            return receiptUseCases;
        }

        public static IInventoryWastageUseCases GetInventoryWastagesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryWastageUseCases inventoryWastageUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                inventoryWastageUseCases = new RemoteInventoryWastageUseCases(executionContext);
            }
            else
            {
                inventoryWastageUseCases = new LocalInventoryWastageUseCases(executionContext);
            }
            log.LogMethodExit(inventoryWastageUseCases);
            return inventoryWastageUseCases;
        }

        public static IInventoryReceiveLinesUseCases GetInventoryReceiveLinesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryReceiveLinesUseCases InventoryReceiveLinesUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                InventoryReceiveLinesUseCases = new RemoteInventoryReceiveLinesUseCases(executionContext);
            }
            else
            {
                InventoryReceiveLinesUseCases = new LocalInventoryReceiveLinesUseCases(executionContext);
            }

            log.LogMethodExit(InventoryReceiveLinesUseCases);
            return InventoryReceiveLinesUseCases;
        }
        public static IInventoryIssueHeaderUseCases GetInventoryIssueHeadersUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryIssueHeaderUseCases inventoryIssueHeadersUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                inventoryIssueHeadersUseCases = new RemoteInventoryIssueHeaderUseCases(executionContext);
            }
            else
            {
                inventoryIssueHeadersUseCases = new LocalInventoryIssueHeaderUseCases(executionContext);
            }

            log.LogMethodExit(inventoryIssueHeadersUseCases);
            return inventoryIssueHeadersUseCases;
        }
        public static IEmailUseCases SendInventoryEmail(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IEmailUseCases iEmailUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                iEmailUseCases = new RemoteEmailUseCases(executionContext);
            }
            else
            {
                iEmailUseCases = new LocalEmailUseCases(executionContext);
            }

            log.LogMethodExit(iEmailUseCases);
            return iEmailUseCases;
        }

        public static IInventoryAdjustmentsUseCases GetInventoryAdjustmentsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryAdjustmentsUseCases inventoryAdjustmentsUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                inventoryAdjustmentsUseCases = new RemoteInventoryAdjustmentsUseCases(executionContext);
            }
            else
            {
                inventoryAdjustmentsUseCases = new LocalInventoryAdjustmentsUseCases(executionContext);
            }
            log.LogMethodExit(inventoryAdjustmentsUseCases);
            return inventoryAdjustmentsUseCases;
        }

        public static IProductActivityUseCases GetProductActivityUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IProductActivityUseCases productActivityUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                productActivityUseCases = new RemoteProductActivityUseCases(executionContext);
            }
            else
            {
                productActivityUseCases = new LocalProductActivityUseCases(executionContext);
            }
            log.LogMethodExit(productActivityUseCases);
            return productActivityUseCases;
        }

        public static PhysicalCount.IInventoryPhysicalCountUseCases GetInventoryPhysicalCountsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryPhysicalCountUseCases inventoryAdjustmentsUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                inventoryAdjustmentsUseCases = new RemoteInventoryPhysicalCountUseCases(executionContext);
            }
            else
            {
                inventoryAdjustmentsUseCases = new LocalInventoryPhysicalCountUseCases(executionContext);
            }
            log.LogMethodExit(inventoryAdjustmentsUseCases);
            return inventoryAdjustmentsUseCases;
        }

        public static IInventoryActivityLogUseCases GetInventoryActivityLogUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryActivityLogUseCases invenoryActivityLogUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                invenoryActivityLogUseCases = new RemoteInventoryActivityLogUseCases(executionContext);
            }
            else
            {
                invenoryActivityLogUseCases = new LocalInventoryActivityLogUseCases(executionContext);
            }
            log.LogMethodExit(invenoryActivityLogUseCases);
            return invenoryActivityLogUseCases;
        }

        public static IReceiptReportsUseCase PrintReports(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IReceiptReportsUseCase iReceiptReportsUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                iReceiptReportsUseCases = new RemoteReceiptReportsUseCases(executionContext);
            }
            else
            {
                iReceiptReportsUseCases = new LocalReceiptReportsUseCases(executionContext);
            }
            log.LogMethodExit(iReceiptReportsUseCases);
            return iReceiptReportsUseCases;
        }

        public static IPhysicalCountInventoryUseCases GetPhysicalCountReviewsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPhysicalCountInventoryUseCases physicalCountInventoryUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                physicalCountInventoryUseCases = new RemotePhysicalCountInventoryUseCases(executionContext);
            }
            else
            {
                physicalCountInventoryUseCases = new LocalPhysicalCountInventoryUseCases(executionContext);
            }
            log.LogMethodExit(physicalCountInventoryUseCases);
            return physicalCountInventoryUseCases;
        }

        public static IUserMessagesUseCases GetUserMessagesUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IUserMessagesUseCases userMessagesUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                userMessagesUseCases = new RemoteUserMessagesUseCases(executionContext);
            }
            else
            {
                userMessagesUseCases = new LocalUserMessagesUseCases(executionContext);
            }
            log.LogMethodExit(userMessagesUseCases);
            return userMessagesUseCases;
        }

        public static IPOTaxViewUseCases GetPOTaxViewsUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IPOTaxViewUseCases poTaxViewUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                poTaxViewUseCases = new RemotePOTaxViewUseCases(executionContext);
            }
            else
            {
                poTaxViewUseCases = new LocalPOTaxViewUseCases(executionContext);
            }
            log.LogMethodExit(poTaxViewUseCases);
            return poTaxViewUseCases;
        }

        public static IInventoryLotUseCases GetInventoryLotUseCases(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            IInventoryLotUseCases inventoryLotUseCases = null;
            if (ConfigurationManager.AppSettings["EXECUTION_MODE"] == "Remote")
            {
                inventoryLotUseCases = new RemoteInventoryLotUseCases(executionContext);
            }
            else
            {
                inventoryLotUseCases = new LocalInventoryLotUseCases(executionContext);
            }

            log.LogMethodExit(inventoryLotUseCases);
            return inventoryLotUseCases;
        }


    }
}
