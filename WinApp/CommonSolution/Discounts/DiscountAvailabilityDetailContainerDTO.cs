/********************************************************************************************
 * Project Name - Discounts
 * Description  - Data structure of DiscountAvailabilityDetail
 *  
 **************
 **Version Log
 **************
 *Version     Date             Modified By               Remarks          
 *********************************************************************************************
 2.150.0      05-May-2021      Lakshminarayana           Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Discounts
{
    /// <summary>
    /// This is the Discount Availability Detail data object class
    /// </summary>
    public class DiscountAvailabilityDetailContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime startDateTime;
        private DateTime endDateTime;
        private bool available;

        /// <summary>
        /// Default constructor
        /// </summary>
        public DiscountAvailabilityDetailContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public DiscountAvailabilityDetailContainerDTO(DateTime startDateTime, DateTime endDateTime, bool available)
        {
            log.LogMethodEntry(startDateTime, endDateTime, available);
            this.startDateTime = startDateTime;
            this.endDateTime = endDateTime;
            this.available = available;
            log.LogMethodExit();
        }

        /// <summary>
        /// Get/Set method of the startDateTime field
        /// </summary>
        public DateTime StartDateTime
        {
            get { return startDateTime; }
            set { startDateTime = value; }
        }

        /// <summary>
        /// Get/Set method of the endDateTime field
        /// </summary>
        public DateTime EndDateTime
        {
            get { return endDateTime; }
            set { endDateTime = value; }
        }

        /// <summary>
        /// Get/Set method of the available field
        /// </summary>
        public bool Available
        {
            get { return available; }
            set { available = value; }
        }
    }
}
