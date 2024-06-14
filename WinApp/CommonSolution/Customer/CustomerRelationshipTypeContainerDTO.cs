/********************************************************************************************
* Project Name - Customer
* Description  - CustomerRelationshipTypeContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    31-Aug-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.Customer
{
    public class CustomerRelationshipTypeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int customerRelationshipTypeId;
        private string name;
        private string description;

        public CustomerRelationshipTypeContainerDTO()
        {
            log.LogMethodEntry();

            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public CustomerRelationshipTypeContainerDTO(int customerRelationshipTypeIdPassed, string name, string description) : this()
        {
            log.LogMethodEntry(customerRelationshipTypeIdPassed, name);
            this.customerRelationshipTypeId = customerRelationshipTypeIdPassed;
            this.name = name;
            this.description = description;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int CustomerRelationshipTypeId { get { return customerRelationshipTypeId; } set { customerRelationshipTypeId = value; } }

        /// <summary>
        /// Get/Set method of the Name field
        /// </summary>
        [DisplayName("Name")]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Get/Set method of the Description field
        /// </summary>
        [DisplayName("Description")]
        public string Description { get { return description; } set { description = value; } }
    }
}
