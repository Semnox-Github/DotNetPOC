/********************************************************************************************
 * Project Name - CheckInPauseLogBL
 * Description  - Bussiness logic of CheckInPauseLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *1.00        28-Mar-2019   Indhu          Created 
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.Transaction
{
    public class CheckInPauseLogBL
    {
        Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        CheckInPauseLogDTO checkInPauseLogDTO;
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        private CheckInPauseLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            checkInPauseLogDTO = null;
            this.executionContext = executionContext;
            log.LogMethodExit(null);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="checkInPauseLogDTO"></param>
        public CheckInPauseLogBL(ExecutionContext executionContext, CheckInPauseLogDTO checkInPauseLogDTO) : 
            this(executionContext)
        {
            log.LogMethodEntry(checkInPauseLogDTO);
            this.checkInPauseLogDTO = checkInPauseLogDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized Constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="checkInPauseLogId"></param>
        /// <param name="sqlTransaction"></param>
        public CheckInPauseLogBL(ExecutionContext executionContext, int checkInPauseLogId, SqlTransaction sqlTransaction = null) :
            this(executionContext)
        {
            log.LogMethodEntry(executionContext, checkInPauseLogId, sqlTransaction);
            CheckInPauseLogDataHandler checkInPauseLogDataHandler = new CheckInPauseLogDataHandler(sqlTransaction);
            checkInPauseLogDTO = checkInPauseLogDataHandler.GetCheckInPauseLogDTO(checkInPauseLogId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CheckInPauseLog
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CheckInPauseLogDataHandler CheckInPauseLogDataHandler = new CheckInPauseLogDataHandler(sqlTransaction);
            if (checkInPauseLogDTO.CheckInPauseLogId < 0)
            {
                checkInPauseLogDTO = CheckInPauseLogDataHandler.InsertCheckInPauseLogDTO(checkInPauseLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                checkInPauseLogDTO.AcceptChanges();
            }
            else
            {
                if (checkInPauseLogDTO.IsChanged)
                {
                    checkInPauseLogDTO = CheckInPauseLogDataHandler.UpdateCheckInPauseLogDTO(checkInPauseLogDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    checkInPauseLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CheckInPauseLogDTO CheckInPauseLogDTO
        {
            get
            {
                return checkInPauseLogDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CheckInPauseLog
    /// </summary>
    public class CheckInPauseLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public CheckInPauseLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the CheckInPauseLog list
        /// </summary>
        public List<CheckInPauseLogDTO> GetCheckInPauseLogDTOList(List<KeyValuePair<CheckInPauseLogDTO.SearchByCheckInPauseLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CheckInPauseLogDataHandler CheckInPauseLogDataHandler = new CheckInPauseLogDataHandler(sqlTransaction);
            List<CheckInPauseLogDTO> returnValue = CheckInPauseLogDataHandler.GetCheckInPauseLogDTOList(searchParameters, sqlTransaction);
            log.LogMethodExit(returnValue);
            return returnValue;
        }
    }
}
