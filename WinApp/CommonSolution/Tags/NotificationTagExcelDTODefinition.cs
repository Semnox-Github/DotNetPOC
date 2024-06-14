/********************************************************************************************
 * Project Name - Tags
 * Description  - Data object of NotificationTagExcelDTO
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.110.2    26-Mar-2021   Mushahid Faizan Created 
 *2.130.0    16-Jul-2021   Mushahid Faizan Removed LastUpdatedBy and LastUpdatedDate column .
 ********************************************************************************************/

using Semnox.Core.GenericUtilities.Excel;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Tags
{
    public class NotificationTagExcelDTODefinition : ComplexAttributeDefinition
    {
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public NotificationTagExcelDTODefinition(ExecutionContext executionContext, string fieldName) : base(fieldName, typeof(NotificationTagsDTO))
        {

            attributeDefinitionList.Add(new SimpleAttributeDefinition("NotificationTagId", "NotificationTagId", new IntValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TagNumber", "TagNumber", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TagNotificationStatus", "TagNotificationStatus", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("MarkedForStorage", "MarkedForStorage", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("IsInStorage", "IsInStorage", new NullableBooleanValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Remarks", "Remarks", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("DefaultChannel", "DefaultChannel", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("LastStorageMarkedDate", "LastStorageMarkedDate", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATETIME_FORMAT"))));
            //attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdatedBy", "LastUpdatedBy", new StringValueConverter()));
            //attributeDefinitionList.Add(new SimpleAttributeDefinition("LastUpdateDate", "LastUpdateDate", new NullableDateTimeValueConverter()));

        }
    }
}

