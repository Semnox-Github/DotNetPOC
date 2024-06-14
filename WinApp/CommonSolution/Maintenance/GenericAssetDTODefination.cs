/********************************************************************************************
 * Project Name -  GenericAsset                                                                       
 * Description  - Bulk Upload Mapper GenericAssetDTODefination Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.60       18-Sept-2019   Rakesh Kumar  Created    
 *2.80        10-May-2020   Girish Kundar  Modified: REST API Changes merge from WMS  
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Maintenance
{
    public class GenericAssetDTODefination: ComplexAttributeDefinition
    {
        public GenericAssetDTODefination(ExecutionContext executionContext, string fieldName, bool isAssetLimited = false): base(fieldName, typeof(GenericAssetDTO))
        {
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AssetId", "Asset Id", new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Name", "Asset Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Description", "Description", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Machineid", "Machine Id", new NullableIntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AssetTypeId", "Asset Type", new AssetTypeValueConverter(executionContext)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Location", "Location", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AssetStatus", "Asset Status", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("URN", "URN", new StringValueConverter()));
            if (isAssetLimited)
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseDate", "Purchase Date", new StringValueConverter()));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("SaleDate", "Sale Date", new StringValueConverter()));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("ScrapDate", "Scrap Date", new StringValueConverter()));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("AssetTaxTypeId", "Tax Type", new AssetTaxValueConverter(executionContext)));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("PurchaseValue", "Purchase Value", new NullableDoubleValueConverter()));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("SaleValue", "Sale Value", new NullableDoubleValueConverter()));
                attributeDefinitionList.Add(new SimpleAttributeDefinition("ScrapValue", "Scrap Value", new NullableDoubleValueConverter()));
            }
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "Active?", new AssetTrueValueConverter()));
        }
    }
}
