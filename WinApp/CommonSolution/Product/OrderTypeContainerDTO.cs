/********************************************************************************************
* Project Name - Product
* Description  - OrderTypeContainerDTO class 
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0    19-Jul-2021      Mushahid Faizan        Created 
********************************************************************************************/

using System.ComponentModel;

namespace Semnox.Parafait.Product
{
    public class OrderTypeContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int orderTypeId;
        private string name;
        private string description;

        public OrderTypeContainerDTO()
        {
            log.LogMethodEntry();
            
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor with Required fields
        /// </summary>
        ///
        public OrderTypeContainerDTO(int orderTypeIdPassed, string name, string description) : this()
        {
            log.LogMethodEntry(orderTypeIdPassed, name);
            this.orderTypeId = orderTypeIdPassed;
            this.name = name;
            this.description = description;
            log.LogMethodExit();
        }
        /// <summary>
        /// Get/Set method of the Id field
        /// </summary>
        [DisplayName("Id")]
        public int OrderTypeId { get { return orderTypeId; } set { orderTypeId = value; } }

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
