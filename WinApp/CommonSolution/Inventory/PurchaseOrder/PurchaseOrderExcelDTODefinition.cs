/********************************************************************************************
 * Project Name - Inventory
 * Description  - Created Data object for excel sheet
 *  
 **************
 **Version Log
 **************
 *Version    Date          Created By               Remarks          
 ***************************************************************************************************
 *2.110.1    18-Feb-2020  Mushahid Faizan         Created.
 ***************************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System.Globalization;

namespace Semnox.Parafait.Inventory
{
    class PurchaseOrderExcelDTODefinition : ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public PurchaseOrderExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(PurchaseOrderDTO))
        {
            log.LogMethodEntry(executionContext, fieldName);

            attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseOrderId", "PurchaseOrder Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OrderStatus", "Order Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OrderNumber", "Order Number", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OrderDate", "Order Date", new NullableDateTimeValueConverter("yyyy-MM-dd HH:mm:ss")));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorId", "Vendor Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ContactName", "Contact Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Phone", "Phone", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorAddress2", "VendorAddress 2", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorAddress1", "VendorAddress 1", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorCity", "Vendor City", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorState", "Vendor State", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorCountry", "Vendor Country", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("VendorPostalCode", "VendorPostal Code", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToAddress1", "ShipToAddress 1", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToAddress2", "ShipToAddress 2", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToCity", "Ship To City", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToState", "Ship To State", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToCountry", "Ship To Country", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToPostalCode", "Ship To PostalCode", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShipToAddressRemarks", "Ship To AddressRemarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestShipDate", "Request ShipDate", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OrderTotal", "Order Total", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastModUserId", "LastModUser Id", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastModDttm", "LastMod Dttm", new NullableDateTimeValueConverter("yyyy-MM-dd HH:mm:ss")));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReceivedDate", "Received Date", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReceiveRemarks", "Receive Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("CancelledDate", "cancelled Date", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsCreditPO", "isCredit PO", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DocumentTypeID", "documentType ID", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Fromdate", "fromdate", new NullableDateTimeValueConverter("yyyy-MM-dd HH:mm:ss")));
            //attributeDefinitionList.Add(new SimpleAttributeDefinition("Fromdate", "fromdate", new NullableDateTimeValueConverter(ParafaitDefaultContainer.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ToDate", "toDate", new NullableDateTimeValueConverter("yyyy-MM-dd HH:mm:ss")));
            //attributeDefinitionList.Add(new SimpleAttributeDefinition("ToDate", "toDate", new NullableDateTimeValueConverter(ParafaitDefaultContainer.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OrderRemarks", "orderRemarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ReprintCount", "reprintCount", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AmendmentNumber", "amendment Number", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DocumentStatus", "document Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FromSiteId", "fromSite Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ToSiteId", "toSite Id", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("OriginalReferenceGUID", "originalReference GUID", new StringValueConverter()));
            log.LogMethodExit();                                                                                                    
        }                                                              
    }                                                                  
}                                                                      
                                                                       
                                                                       
                                                                       
                                                                       
                                                                       
                                                                       