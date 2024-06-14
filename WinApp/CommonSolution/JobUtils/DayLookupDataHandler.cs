
/********************************************************************************************
 * Project Name - 
 * Description  - DayLookupDataHandler
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *2.70.2       18-Sep-2019    Dakshakh         Modified : Added logger
 *2.70.2        10-Dec-2019   Jinto Thomas     Removed siteid from update query                                                          
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using Semnox.Core.Utilities;
namespace Semnox.Parafait.JobUtils
{
    public class DayLookupDataHandler
    {
        /// <summary>
        /// Day Lookup Data Handler - Handles insert, update and select of Lookup Value Data objects
        /// </summary>

        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DataAccessHandler dataAccessHandler;
        public DayLookupDataHandler()
        {
            log.LogMethodEntry();
            dataAccessHandler = new DataAccessHandler();
            log.LogMethodExit();
        }

        /// <summary>
        /// Converts the Data row object to DayLookupDTO class type
        /// </summary>
        /// <param name="dayLookupDataRow">DayLookup DataRow</param>
        /// <returns>Returns DayLookup</returns>
        private DayLookupDTO GetDayLookupDTO(DataRow dayLookupDataRow)
        {
            log.LogMethodEntry(dayLookupDataRow);
            DayLookupDTO userDataObject = new DayLookupDTO(Convert.ToInt32(dayLookupDataRow["Day"]),
                                                                (dayLookupDataRow["Display"].ToString())
                                                           );
            log.LogMethodExit(userDataObject);
            return userDataObject;
        }

        /// <summary>
        /// GetDayLookupValuesList
        /// </summary>
        /// <returns></returns>
        public List<DayLookupDTO> GetDayLookupValuesList()
        {
            log.LogMethodEntry();
            string selectDayLookupQuery ="select * from DayLookup";
            DataTable dayLookupData = dataAccessHandler.executeSelectQuery(selectDayLookupQuery, null);
            if (dayLookupData.Rows.Count > 0)
            {
                List<DayLookupDTO> dayLookupList = new List<DayLookupDTO>();
                foreach (DataRow dayLookupDataRow in dayLookupData.Rows)
                {
                    DayLookupDTO dayLookupDataObject = GetDayLookupDTO(dayLookupDataRow);
                    dayLookupList.Add(dayLookupDataObject);
                }
                log.LogMethodExit(dayLookupList);
                return dayLookupList;
            }
            else
            {
                log.LogMethodExit(null);
                return null;
            }
        }

    }
}
