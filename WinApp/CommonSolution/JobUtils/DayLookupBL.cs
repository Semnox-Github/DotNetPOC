
/********************************************************************************************
 * Project Name - 
 * Description  - DayLookupBL
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2       18-Sep-2019    Dakshakh         Modified : Added logger
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// DaylookupBL values allowes to access the lookup values based on the business logic.
    /// </summary>

    public class DayLookupBL
    {
        private DayLookupDTO dayLookupDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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
            List<DayLookupDTO> dayLookupDTOList = dayLookupDataHandler.GetDayLookupValuesList();
            log.LogMethodExit(dayLookupDTOList);
            return dayLookupDTOList;
        }
    }

}
