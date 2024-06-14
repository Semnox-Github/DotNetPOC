/********************************************************************************************
 * Project Name - DayLookupBL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.70.2        25-Jul-2019      Dakshakh raj     Modified : Logs
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Core.GenericUtilities
{
    /// <summary>
    /// DaylookupBL values allowes to access the lookup values based on the business logic.
    /// </summary>

    public class DayLookupBL
    {
        private DayLookupDTO dayLookupDTO;
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Default constructor
        /// </summary>
        public DayLookupBL()
        {
            log.LogMethodEntry();
            dayLookupDTO = null;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the DTO parameter
        /// </summary>
        /// <param name="dayLookupDTO">DayLookupDTO values DTO Object</param>
        public DayLookupBL(DayLookupDTO dayLookupDTO)
        {
            log.LogMethodEntry(dayLookupDTO);
            this.dayLookupDTO = dayLookupDTO;
            log.LogMethodExit();
        }
    }

    /// <summary>
    /// Manages the list of dayLookupValues
    /// </summary>
    public class DayLookupList
    {
        /// <summary>
        /// Returns the lookup values list
        /// </summary>
        public List<DayLookupDTO> GetAllDayLookup()
        {
            Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            log.LogMethodEntry();
            DayLookupDataHandler dayLookupDataHandler = new DayLookupDataHandler();
            log.LogMethodExit(dayLookupDataHandler.GetDayLookupValuesList());
            return dayLookupDataHandler.GetDayLookupValuesList();
        }
    }

}
