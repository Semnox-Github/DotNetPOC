/********************************************************************************************
 * Project Name - Approval Log BL
 * Description  - BL of the approval log class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        13-Aug-2019   Deeksha             modifications as per 3 tier standards
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Inventory
{
    public class ApprovalLog
    {
        private ApprovalLogDTO approvalLogDTO;
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        ExecutionContext machineUserContext = null;
        /// <summary>
        /// Default constructor of ApprovalLog class
        /// </summary>
        public ApprovalLog()
        {
            log.LogMethodEntry();
            approvalLogDTO = null;
            log.LogMethodExit();
        }
        // <summary>
        /// Default constructor of ApprovalLog class
        /// </summary>
        public ApprovalLog(ExecutionContext machineUserContext)
            :this()
        {
            log.LogMethodEntry();
            this.machineUserContext = machineUserContext;
            log.LogMethodExit();
        }

        /// <summary>
        ///Constructor will fetch the ApprovalLog DTO based on the approvalLog id passed 
        /// </summary>
        /// <param name="approvalLogId">ApprovalLog id</param>
        public ApprovalLog(int approvalLogId, SqlTransaction sqlTransaction=null)
            : this()
        {
            log.LogMethodEntry(approvalLogId, sqlTransaction);
            ApprovalLogDataHandler approvalLogDataHandler = new ApprovalLogDataHandler(sqlTransaction);
            approvalLogDTO = approvalLogDataHandler.GetApprovalLog(approvalLogId);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates approvalLog object using the ApprovalLogDTO
        /// </summary>
        /// <param name="approvalLog">ApprovalLogDTO object</param>
        public ApprovalLog(ApprovalLogDTO approvalLog)
            : this()
        {
            log.LogMethodEntry(approvalLog);
            this.approvalLogDTO = approvalLog;
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the approvalLog details on identifire
        /// </summary>
        /// <param name="identifire">integer type parameter</param>
        /// <returns>Returns ApprovalLogDTO</returns>
        public ApprovalLogDTO GetUserMessage(int identifire, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(identifire, sqlTransaction);
            ApprovalLogDataHandler approvalLogDataHandler = new ApprovalLogDataHandler(sqlTransaction);
            ApprovalLogDTO approvalLogDTO = new ApprovalLogDTO();
            approvalLogDTO = approvalLogDataHandler.GetApprovalLog(identifire);
            log.LogMethodExit(approvalLogDTO);
            return approvalLogDTO;
        }
        /// <summary>
        /// Saves the approval log record
        /// Checks if the message id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (machineUserContext == null)
            {
                machineUserContext = ExecutionContext.GetExecutionContext();
            }
            ApprovalLogDataHandler approvalLogDataHandler = new ApprovalLogDataHandler(sqlTransaction);
            if (approvalLogDTO.ApprovalLogID < 0)
            {
                approvalLogDTO = approvalLogDataHandler.InsertApprovalLog(approvalLogDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                approvalLogDTO.AcceptChanges();
            }
            else
            {
                if (approvalLogDTO.IsChanged)
                {
                    approvalLogDTO = approvalLogDataHandler.UpdateApprovalLog(approvalLogDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    approvalLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public void InsertApprovalLog(int documentTypeId, string objectGuid, int approvalLevel, string status, string remark, SqlTransaction uploadtrxn)
        {
            log.LogMethodEntry(documentTypeId, objectGuid, approvalLevel, status, remark, uploadtrxn);
            approvalLogDTO = new ApprovalLogDTO();
            approvalLogDTO.DocumentTypeID = documentTypeId;
            approvalLogDTO.ObjectGUID = objectGuid;
            approvalLogDTO.ApprovalLevel = approvalLevel;
            approvalLogDTO.Status = status;
            approvalLogDTO.Remarks = remark;
            Save(uploadtrxn);
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ApprovalLogDTO getApprovalLogDTO { get { return approvalLogDTO; } }
    }

    /// <summary>
    /// Manages the list of approvalLogs
    /// </summary>
    public class ApprovalLogsList
    {
        private static readonly logging.Logger log = new logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        /// <summary>
        /// Returns the approvalLog list
        /// </summary>
        public List<ApprovalLogDTO> GetAllApprovalLog(List<KeyValuePair<ApprovalLogDTO.SearchByApprovalLogParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApprovalLogDataHandler approvalLogDataHandler = new ApprovalLogDataHandler(sqlTransaction);
            List<ApprovalLogDTO> approvalLogDTOs = new List<ApprovalLogDTO>();
            approvalLogDTOs = approvalLogDataHandler.GetApprovalLogList(searchParameters);
            log.LogMethodExit(approvalLogDTOs);
            return approvalLogDTOs;
        }
    }
}
