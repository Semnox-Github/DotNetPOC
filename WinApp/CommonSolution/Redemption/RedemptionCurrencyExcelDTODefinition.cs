/********************************************************************************************
 * Project Name - Redemption 
 * Description  - VendorExcRedemptionCurrencyExcelDTODefinitionel  object of currency Excel information
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By             Remarks          
 *********************************************************************************************
 *2.100.0    14-Oct-2020        Girish Kundar          Created 
 *2.110.0    03-Dec-2020        Mushahid Faizan        Web Inventory Enhancement.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using Semnox.Parafait.Product;

namespace Semnox.Parafait.Redemption
{
    public class RedemptionCurrencyExcelDTODefinition : ComplexAttributeDefinition
    {

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public RedemptionCurrencyExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(RedemptionCurrencyDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("CurrencyId", "Currency Id", new IntValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("CurrencyName", "Currency Name", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ProductId", "Inventory Product", new ProductValueConverter(executionContext)));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ValueInTickets", "Value In Tickets", new NullableDoubleValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("BarCode", "BarCode", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "IsActive", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShowQtyPrompt", "Quantity Prompt", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ManagerApproval", "Manager Approval", new BooleanValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("ShortCutKeys", "ShortCut Keys", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastModifiedBy", "LastModifiedBy", new StringValueConverter()));

            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedDate", "LastUpdatedDate", new NullableDateTimeValueConverter()));

        }
    }

    class ProductValueConverter : ValueConverter
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        List<KeyValuePair<int, ProductDTO>> productIdCategoryKeyDTOValuePair;
        List<KeyValuePair<string, ProductDTO>> productNameProductDTOKeyValuePair;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ProductValueConverter(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            productNameProductDTOKeyValuePair = new List<KeyValuePair<string, ProductDTO>>();
            productIdCategoryKeyDTOValuePair = new List<KeyValuePair<int, ProductDTO>>();
            List<ProductDTO> productList = new List<ProductDTO>();

            ProductList productDTOList = new ProductList(executionContext);
            List<KeyValuePair<ProductDTO.SearchByProductParameters, string>> searchParams = new List<KeyValuePair<ProductDTO.SearchByProductParameters, string>>();
            searchParams.Add(new KeyValuePair<ProductDTO.SearchByProductParameters, string>(ProductDTO.SearchByProductParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            productList = productDTOList.GetAllProducts(searchParams);
            if (productList != null && productList.Count > 0)
            {
                foreach (ProductDTO productDTO in productList)
                {
                    productIdCategoryKeyDTOValuePair.Add(new KeyValuePair<int, ProductDTO>(productDTO.ProductId, productDTO));
                    productNameProductDTOKeyValuePair.Add(new KeyValuePair<string, ProductDTO>(productDTO.ProductName, productDTO));
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts productname to productid
        /// </summary>
        /// <param name="stringValue"></param>
        /// <returns></returns>
        public override object FromString(string stringValue)
        {
            log.LogMethodEntry(stringValue);
            int productId = -1;
            for (int i = 0; i < productNameProductDTOKeyValuePair.Count; i++)
            {
                if (productNameProductDTOKeyValuePair[i].Key == stringValue)
                {
                    productNameProductDTOKeyValuePair[i] = new KeyValuePair<string, ProductDTO>(productNameProductDTOKeyValuePair[i].Key, productNameProductDTOKeyValuePair[i].Value);
                    productId = productNameProductDTOKeyValuePair[i].Value.ProductId;
                }
            }
            log.LogMethodExit(productId);
            return productId;
        }
        /// <summary>
        /// Converts productid to productname
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>

        public override string ToString(object value)
        {
            log.LogMethodEntry(value);
            string productName = string.Empty;

            for (int i = 0; i < productIdCategoryKeyDTOValuePair.Count; i++)
            {
                if (productIdCategoryKeyDTOValuePair[i].Key == Convert.ToInt32(value))
                {
                    productIdCategoryKeyDTOValuePair[i] = new KeyValuePair<int, ProductDTO>(productIdCategoryKeyDTOValuePair[i].Key, productIdCategoryKeyDTOValuePair[i].Value);

                    productName = productIdCategoryKeyDTOValuePair[i].Value.ProductName;
                }
            }
            log.LogMethodExit(productName);
            return productName;
        }
    }
}
