/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of CustomerDTODefinition      s
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer
{
    /// <summary>
    /// Customer DTO definition class
    /// </summary>
    public class CustomerDTODefinition:ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="fieldName"></param>
        public CustomerDTODefinition(ExecutionContext executionContext, string fieldName):base(fieldName, typeof(CustomerDTO))
        {
            log.LogMethodEntry(executionContext,  fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Id", "Customer Id", new ForeignKeyValueConverter()));
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TITLE") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Title", "Title", new StringValueConverter()));
            }
            
            attributeDefinitionList.Add(new SimpleAttributeDefinition("FirstName", "First Name", new StringValueConverter()));
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "MIDDLE_NAME") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("MiddleName", "Middle Name", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "LAST_NAME") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("LastName", "Last Name", new StringValueConverter()));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "BIRTH_DATE") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("DateOfBirth", "Date Of Birth", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"))));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ANNIVERSARY") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Anniversary", "Anniversary", new NullableDateTimeValueConverter(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DATE_FORMAT"))));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "GENDER") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Gender", "Gender", new StringValueConverter()));
            }
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "NOTES") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Notes", "Notes", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "RIGHTHANDED") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("RightHanded", "Right Handed", new BooleanValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TEAMUSER") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("TeamUser", "Team User", new BooleanValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "UNIQUE_ID") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("UniqueIdentifier", "Unique_Id", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TAXCODE") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("TaxCode", "Tax Code", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COMPANY") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Company", "Company", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "DESIGNATION") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Designation", "Designation", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "USERNAME") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("UserName", "UserName", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EXTERNALSYSTEMREF") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("ExternalSystemReference", "External System Reference", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CHANNEL") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("Channel", "Channel", new StringValueConverter()));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CUSTOMERTYPE") != "N")
            {
                attributeDefinitionList.Add(new SimpleAttributeDefinition("CustomerType", "Customer Type", new CustomerTypeConverter()));
            }
            //if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CHANNEL") != "N")
            //{
            //    attributeDefinitionList.Add(new SimpleAttributeDefinition("Verified", "Verified", new BooleanValueConverter()));
            //}
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Verified", "Verified", new BooleanValueConverter()));

            attributeDefinitionList.Add(new CustomDataSetDTODefinition(executionContext, "CustomDataSetDTO", "CUSTOMER"));
            if(ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS_TYPE") != "N"||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS1") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS2") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "ADDRESS3") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CITY") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "STATE") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "COUNTRY") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "PIN") != "N")
            {
                attributeDefinitionList.Add(new ListAttributeDefinition("AddressDTOList", typeof(AddressDTO), new AddressDTODefinition(executionContext, "")));
            }
            if (ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "CONTACT_PHONE") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "EMAIL") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBUSERID") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "FBACCESSTOKEN") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSTOKEN") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "TWACCESSSECRET") != "N" ||
                ParafaitDefaultContainerList.GetParafaitDefault(executionContext, "WECHAT_ACCESS_TOKEN") != "N")
            {
                attributeDefinitionList.Add(new ListAttributeDefinition("ContactDTOList", typeof(ContactDTO), new ContactDTODefinition("")));
            }
            log.LogMethodExit();    
        }
    }
}
