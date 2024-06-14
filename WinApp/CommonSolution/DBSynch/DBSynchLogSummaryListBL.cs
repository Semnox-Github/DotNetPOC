/********************************************************************************************
 * Project Name - DBSynchLog BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        29-Sep-2023      Lakshminarayana     Created
 ********************************************************************************************/
using System.Collections.Generic;

namespace Semnox.Parafait.DBSynch
{
    /// <summary>
    /// Manages the list of DBSynchLogSummary
    /// </summary>
    public class DBSynchLogSummaryListBL
    {
        private static  readonly Semnox.Parafait.logging.Logger  log = new  Semnox.Parafait.logging.Logger (System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        public DBSynchLogSummaryListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the DBSynchLog list
        /// </summary>
        public List<DBSynchLogSummaryDTO> GetDBSynchLogSummaryDTOList(List<KeyValuePair<DBSynchLogSummaryDTO.SearchByParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            DBSynchLogSummaryDataHandler dBSynchLogDataHandler = new DBSynchLogSummaryDataHandler();
            List<DBSynchLogSummaryDTO> returnValue = dBSynchLogDataHandler.GetDBSynchLogSummaryDTOList(searchParameters);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
