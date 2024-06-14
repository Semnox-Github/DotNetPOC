/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of ContactDTODefinition      
 *  
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        10-Aug-2019   Girish kundar  Modified : Added Logger Methods and Removed Unused namespace's.
 ********************************************************************************************/
using Semnox.Core.GenericUtilities.Excel;

namespace Semnox.Parafait.Customer
{
    class ContactDTODefinition:ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public ContactDTODefinition(string fieldName):base(fieldName, typeof(ContactDTO))
        {
            log.LogMethodEntry(fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("ContactType", "Contact Type", new EnumValueConverter(typeof(ContactType))));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Attribute1", "Attribute 1", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("Attribute2", "Attribute 2", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("UUID", "UUID", new StringValueConverter()));
            log.LogMethodExit();
        }
    }
}
