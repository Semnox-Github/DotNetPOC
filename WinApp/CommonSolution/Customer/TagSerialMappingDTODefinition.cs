/********************************************************************************************
 * Project Name - Customer
 * Description  - Class for  of TagSerialMappingDTODefinition      s
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
    /// <summary>
    /// TagSerialMappingDTO Definition class
    /// </summary>
    public class TagSerialMappingDTODefinition:ComplexAttributeDefinition
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="fieldName"></param>
        public TagSerialMappingDTODefinition(string fieldName):base(fieldName, typeof(TagSerialMappingDTO))
        {
            log.LogMethodEntry(fieldName);
            attributeDefinitionList.Add(new SimpleAttributeDefinition("SerialNumber", "SerialNumber", new StringValueConverter()));
            attributeDefinitionList.Add(new SimpleAttributeDefinition("TagNumber", "CardNumber", new StringValueConverter()));
            log.LogMethodExit();
        }
    }
}
