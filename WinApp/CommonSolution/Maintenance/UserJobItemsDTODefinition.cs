/********************************************************************************************
 * Project Name -  GenericAsset                                                                       
 * Description  - Bulk Upload Mapper GenericAssetDTODefination Class 
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.80       22-Nov-2019    Jagan Mohana   Created    
 *2.80       10-May-2020    Girish Kundar  Modified: REST API Changes merge from WMS  
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
    public class UserJobItemsDTODefinition : ComplexAttributeDefinition
    {
        public UserJobItemsDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(UserJobItemsDTO))
        {
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MaintChklstdetId", "Job ID", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AssetId", "Asset Name", new GenericAssetValueConverter(executionContext)));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MaintJobName", "Request Title", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestDate", "Request Date", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ChklstScheduleTime", "Schedule Date", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestDetail", "Request Detail", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Resolution", "Resolution", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Comments", "Comments", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Status", "Status", new LookupValueConverter(executionContext, "MAINT_JOB_STATUS")));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ImageName", "Image Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestedBy", "Requested By", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("AssignedTo", "Assigned To", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ContactPhone", "Contact Phone", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ContactEmailId", "Contact Email Id", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ChklistRemarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RequestType", "Request Type", new LookupValueConverter(executionContext, "MAINT_REQUEST_TYPE")));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("RepairCost", "Repair Cost", new NullableDoubleValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DocFileName", "Doc File Name", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ChecklistCloseDate", "Close Date", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsActive", "Active?", new AssetTrueValueConverter()));
        }
    }
}
