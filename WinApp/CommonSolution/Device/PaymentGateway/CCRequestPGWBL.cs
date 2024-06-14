/********************************************************************************************
 * Project Name - CCRequestPGW BL
 * Description  - Business logic
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By    Remarks          
 *********************************************************************************************
 *1.00        22-Jun-2017       Lakshminarayana     Created 
 *2.70.2        09-Jul-2019       Girish Kundar       Modified : Save() method : Insert/update methods returns DTO instead of Id.
 *                                                             LogMethodEntry() and LogMethodExit(). 
 *2.70.2        04-Feb-2020      Nitin Pai           Guest App phase 2 changes                                                             
 *2.110.0       30-Dec-2020      Girish Kundar       Modified : Added delete method = Payment link changes                                                     
 *2.150.2       31-Jan-2023      Nitin Pai           Modified - Added a new method to change the status of the CC Request. This checks the current status.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.Device.PaymentGateway
{
    /// <summary>
    /// Business logic for CCRequestPGW class.
    /// </summary>
    public class CCRequestPGWBL
    {
        private CCRequestPGWDTO cCRequestPGWDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of CCRequestPGWBL class
        /// </summary>
        private CCRequestPGWBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the cCRequestPGW id as the parameter
        /// Would fetch the cCRequestPGW object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Id</param>
        public CCRequestPGWBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(id);
            CCRequestPGWDataHandler cCRequestPGWDataHandler = new CCRequestPGWDataHandler(sqlTransaction);
            cCRequestPGWDTO = cCRequestPGWDataHandler.GetCCRequestPGWDTO(id);
            log.LogMethodExit();
        }

        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            CCTransactionsPGWListBL cCTransactionsPGWListBL = new CCTransactionsPGWListBL();
            List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>> ccTransactionsSearchParams = new List<KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>>();
            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.INVOICE_NUMBER, cCRequestPGWDTO.RequestID.ToString()));
            ccTransactionsSearchParams.Add(new KeyValuePair<CCTransactionsPGWDTO.SearchByParameters, string>(CCTransactionsPGWDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CCTransactionsPGWDTO> cCTransactionsPGWDTOList = cCTransactionsPGWListBL.GetCCTransactionsPGWDTOList(ccTransactionsSearchParams);
            if (cCTransactionsPGWDTOList == null) // No cc transaction exists , delete the request
            {
                CCRequestPGWDataHandler cCRequestPGWDataHandler = new CCRequestPGWDataHandler(sqlTransaction);
                cCRequestPGWDataHandler.Delete(cCRequestPGWDTO.RequestID);
            }
            else
            {
                log.Error("Unable to delete this record.Please check the reference record first.");
                throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates CCRequestPGWBL object using the CCRequestPGWDTO
        /// </summary>
        /// <param name="cCRequestPGWDTO">CCRequestPGWDTO object</param>
        public CCRequestPGWBL( ExecutionContext executionContext, CCRequestPGWDTO cCRequestPGWDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.cCRequestPGWDTO = cCRequestPGWDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the CCRequestPGW
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = executionContext == null ? ExecutionContext.GetExecutionContext() : executionContext;
            CCRequestPGWDataHandler cCRequestPGWDataHandler = new CCRequestPGWDataHandler(sqlTransaction);
            if (cCRequestPGWDTO.RequestID < 0)
            {
                cCRequestPGWDTO = cCRequestPGWDataHandler.InsertCCRequestPGW(cCRequestPGWDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                cCRequestPGWDTO.AcceptChanges();
            }
            else
            {
                if (cCRequestPGWDTO.IsChanged)
                {
                    cCRequestPGWDTO = cCRequestPGWDataHandler.UpdateCCRequestPGW(cCRequestPGWDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    cCRequestPGWDTO.AcceptChanges();
                }
            }

            log.LogMethodExit();
        }

        public int ChangePaymentProcessingStatus(String currentStatus, String newStatus, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ExecutionContext machineUserContext = executionContext == null ? ExecutionContext.GetExecutionContext() : executionContext;
            cCRequestPGWDTO.PaymentProcessStatus = newStatus;
            CCRequestPGWDataHandler cCRequestPGWDataHandler = new CCRequestPGWDataHandler(sqlTransaction);
            int rowsUpdated = cCRequestPGWDataHandler.ChangePaymentProcessingStatus(cCRequestPGWDTO, currentStatus, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
            cCRequestPGWDTO.AcceptChanges();
            log.LogMethodExit(rowsUpdated);
            return rowsUpdated;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public CCRequestPGWDTO CCRequestPGWDTO
        {
            get
            {
                return cCRequestPGWDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of CCRequestPGW
    /// </summary>
    public class CCRequestPGWListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Returns the CCRequestPGW list
        /// </summary>
        public List<CCRequestPGWDTO> GetCCRequestPGWDTOList(List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CCRequestPGWDataHandler cCRequestPGWDataHandler = new CCRequestPGWDataHandler(sqlTransaction);
            List<CCRequestPGWDTO> cCRequestPGWDTOList = cCRequestPGWDataHandler.GetCCRequestPGWDTOList(searchParameters);
            log.LogMethodExit(cCRequestPGWDTOList);
            return cCRequestPGWDTOList;
        }

        /// <summary>
        /// Returns the Latest CCRequestPGW 
        /// </summary>
        public CCRequestPGWDTO GetLastestCCRequestPGWDTO(List<KeyValuePair<CCRequestPGWDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            CCRequestPGWDataHandler cCRequestPGWDataHandler = new CCRequestPGWDataHandler(sqlTransaction);
            CCRequestPGWDTO cCRequestPGWDTO = cCRequestPGWDataHandler.GetLatestCCRequestPGWDTO(searchParameters);
            log.LogMethodExit(cCRequestPGWDTO);
            return cCRequestPGWDTO;
        }

    }
}
