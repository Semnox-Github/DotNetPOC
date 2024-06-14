/********************************************************************************************
 * Project Name - TrxUserLogs BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By         Remarks          
 *********************************************************************************************
 *1.00        12-Sep-2018      Mathew Ninan        Created 
 *2.60.2      24-May-2019      Mathew Ninan        Added Execution Context
 *2.80        29-May-2020      Vikas Dwivedi       Modified as per the Standard CheckList
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.Transaction
{
    /// <summary>
    /// Business logic class to managing TrxUserLogs data
    /// </summary>
    public class TrxUserLogsBL
    {
        private TrxUserLogsDTO trxUserLogsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        
        ///// <summary>
        ///// Default constructor of TrxUserLogs class
        ///// </summary>
        //public TrxUserLogsBL()
        //{
        //    log.LogMethodEntry();
        //    trxUserLogsDTO = null;
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Constructor of TrxUserLogs class with Execution Context as parameter
        /// </summary>
        private TrxUserLogsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        ///// <summary>
        ///// Creates TrxUserLogs object using various properties
        ///// </summary>
        ///// <param name="trxUserLogsDTO">TrxUserLogsDTO object</param>
        //public TrxUserLogsBL(int trxId, int lineId, string loginId, DateTime activityDate, int posMachineId, string action, string activity, string createdBy, string lastUpdatedBy, string approverId = null, DateTime? approvalTime = null)
        //    : this()
        //{
        //    if (trxUserLogsDTO == null)
        //    {
        //        trxUserLogsDTO = new TrxUserLogsDTO();
        //    }
        //    log.LogMethodEntry(trxId, lineId, loginId, activityDate, posMachineId, action, activity, createdBy, lastUpdatedBy, executionContext, approverId, approvalTime);
        //    trxUserLogsDTO.TrxId = trxId;
        //    trxUserLogsDTO.LineId = lineId;
        //    trxUserLogsDTO.LoginId = loginId;
        //    trxUserLogsDTO.ActivityDate = activityDate;
        //    trxUserLogsDTO.PosMachineId = posMachineId;
        //    trxUserLogsDTO.Action = action;
        //    trxUserLogsDTO.Activity = activity;
        //    trxUserLogsDTO.CreatedBy = createdBy;
        //    trxUserLogsDTO.LastUpdatedBy = lastUpdatedBy;
        //    trxUserLogsDTO.ApproverId = approverId;
        //    trxUserLogsDTO.ApprovalTime = approvalTime;
        //    log.LogMethodExit();
        //}

        /// <summary>
        /// Creates TrxUserLogs object using various properties including execution context
        /// </summary>
        /// <param name="trxUserLogsDTO">TrxUserLogsDTO object</param>
        public TrxUserLogsBL(int trxId, int lineId, string loginId, DateTime activityDate, int posMachineId, string action, string activity, string createdBy, string lastUpdatedBy, ExecutionContext executionContext, string approverId = null, DateTime? approvalTime = null)
            : this(executionContext)
        {
            if (trxUserLogsDTO == null)
            {
                trxUserLogsDTO = new TrxUserLogsDTO();
            }
            log.LogMethodEntry(trxId, lineId, loginId, activityDate, posMachineId, action, activity, createdBy, lastUpdatedBy, executionContext, approverId, approvalTime);
            trxUserLogsDTO.TrxId = trxId;
            trxUserLogsDTO.LineId = lineId;
            trxUserLogsDTO.LoginId = loginId;
            trxUserLogsDTO.ActivityDate = activityDate;
            trxUserLogsDTO.PosMachineId = posMachineId;
            trxUserLogsDTO.Action = action;
            trxUserLogsDTO.Activity = activity;
            trxUserLogsDTO.CreatedBy = createdBy;
            trxUserLogsDTO.LastUpdatedBy = lastUpdatedBy;
            trxUserLogsDTO.ApproverId = approverId;
            trxUserLogsDTO.ApprovalTime = approvalTime;
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates TrxUserLogs object using the TrxUserLogsDTO
        /// </summary>
        /// <param name="trxUserLogsDTO">TrxUserLogsDTO object</param>
        public TrxUserLogsBL(ExecutionContext executionContext, TrxUserLogsDTO trxUserLogsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, trxUserLogsDTO);
            this.trxUserLogsDTO = trxUserLogsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the TrxUserLogs details to database
        /// </summary>
        public void Save(SqlTransaction sqltrxn)
        {
            log.LogMethodEntry(sqltrxn);
            TrxUserLogsDataHandler trxUserLogsDataHandler = new TrxUserLogsDataHandler(sqltrxn);
            if (trxUserLogsDTO.TrxUserLogId < 0)
            {
                trxUserLogsDTO = trxUserLogsDataHandler.InsertTrxUserLogs(trxUserLogsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                trxUserLogsDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// gets the GetTrxUserLogsDTO
        /// </summary>
        public TrxUserLogsDTO GetTrxUserLogsDTO
        {
            get { return trxUserLogsDTO; }
        }
    }

    /// <summary>
    /// Manages the list of TrxUserLogs
    /// </summary>
    public class TrxUserLogsList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<TrxUserLogsDTO> trxUserLogsList = new List<TrxUserLogsDTO>();
        private readonly ExecutionContext executionContext;

        /// <summary>
        /// Parameterized Constructor with ExecutionContext
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public TrxUserLogsList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the TrxUserLogs list
        /// </summary>
        public List<TrxUserLogsDTO> GetAllTrxUserLogs(List<KeyValuePair<TrxUserLogsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            TrxUserLogsDataHandler trxUserLogsDataHandler = new TrxUserLogsDataHandler(sqlTransaction);
            trxUserLogsList = trxUserLogsDataHandler.GetTrxUserLogsList(searchParameters, sqlTransaction);
            log.LogMethodExit(trxUserLogsList);
            return trxUserLogsList;
        }

        public List<TrxUserLogsDTO> GetUpdatedTrxUserLogs(List<TrxUserLogsDTO> trxUserLogsList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(trxUserLogsList, sqlTransaction);
            TrxUserLogsDataHandler trxUserLogsDataHandler = new TrxUserLogsDataHandler(sqlTransaction);
            trxUserLogsList = trxUserLogsDataHandler.GetUpdatedTrxUserLogsDTO(trxUserLogsList);
            log.LogMethodExit(trxUserLogsList);
            return trxUserLogsList;
        }
    }
}
