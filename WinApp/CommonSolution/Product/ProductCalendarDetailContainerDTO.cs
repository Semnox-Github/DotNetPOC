/********************************************************************************************
* Project Name - Product
* Description  - Data structure of ProductCalendar Container
*  
**************
**Version Log
**************
*Version     Date             Modified By               Remarks          
*********************************************************************************************
2.130.0      04-Aug-2021      Lakshminarayana           Created 
********************************************************************************************/
using System;

namespace Semnox.Parafait.Product
{
    /// <summary>
    /// This is the Product Calendar Detail data object class
    /// </summary>
    public class ProductCalendarDetailContainerDTO
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private DateTime startDateTime;
        private DateTime endDateTime;
        private bool available;

        /// <summary>
        /// Default constructor
        /// </summary>
        public ProductCalendarDetailContainerDTO()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with all the data fields
        /// </summary>
        public ProductCalendarDetailContainerDTO(DateTime startDateTime, DateTime endDateTime, bool available)
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