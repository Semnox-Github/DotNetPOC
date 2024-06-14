/********************************************************************************************
 * Project Name - Job Utils
 * Description  - ExSysSynchLogBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.3      16-Nov -2019     Girish Kundar       Created.
 *2.140       14-Sep-2021      Fiona               Modified: In BL Constructor added null check
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Communication;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.JobUtils
{
    public class ExSysSynchLogBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ExecutionContext machineUserContext;
        private ExSysSynchLogDTO exSysSynchLogDTO;
        private Utilities utilities = new Utilities();

        /// <summary>
        /// Default constructor ExSysSynchLogBL class
        /// </summary>
        public ExSysSynchLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            exSysSynchLogDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with one parameter
        /// </summary>
        /// <param name="Id">Id of the ticket station</param>
        public ExSysSynchLogBL(ExecutionContext executionContext, int id,SqlTransaction sqlTransaction=null)
        {
            log.LogMethodEntry();
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            
            exSysSynchLogDTO = exSysSynchLogDataHandler.GetExSysSynchLogDTO(id);
            if (exSysSynchLogDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ExSysSynchLog", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with exSysSynchLogDTO parameter
        /// </summary>
        /// <param name="exSysSynchLogDTO">parameter of type ExSysSynchLogDTO </param>
        public ExSysSynchLogBL(ExecutionContext executionContext, ExSysSynchLogDTO exSysSynchLogDTO)
        {
            log.LogMethodEntry(exSysSynchLogDTO);
            this.exSysSynchLogDTO = exSysSynchLogDTO;
            machineUserContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the Ticket station details to table
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler();
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            if (exSysSynchLogDTO.LogId < 0)
            {
                exSysSynchLogDTO = exSysSynchLogDataHandler.Insert(exSysSynchLogDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                exSysSynchLogDTO.AcceptChanges();
            }
            else
            {
                if (exSysSynchLogDTO.IsChanged)
                {
                    exSysSynchLogDTO = exSysSynchLogDataHandler.Update(exSysSynchLogDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
                    exSysSynchLogDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the  ExSysSynchLogDTO  .
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            return validationErrorList;
        }

        /// <summary>
        /// Gets the ExSysSynchLogDTO
        /// </summary>
        public ExSysSynchLogDTO ExSysSynchLogDTO
        {
            get { return exSysSynchLogDTO; }
        }

    }
    /// <summary>
    /// Class for ExSysSynchLogListBL List
    /// </summary>
    public class ExSysSynchLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ExSysSynchLogDTO> exSysSynchLogDTOList = new List<ExSysSynchLogDTO>();
        private ExecutionContext executionContext;
        public ExSysSynchLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and ExSysSynchLogDTOList as parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ExSysSynchLogDTOList">exSysSynchLogDTOList</param>
        public ExSysSynchLogListBL(ExecutionContext executionContext, List<ExSysSynchLogDTO> exSysSynchLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.exSysSynchLogDTOList = exSysSynchLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns All active the exSysSynchLogDTO records from the table 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of active ExSysSynchLogDTO</returns>
        public List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>(ExSysSynchLogDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<ExSysSynchLogDTO> exSysSynchLogDTODTOList = exSysSynchLogDataHandler.GetExSysSynchLogDTOList(searchParameters);
            log.LogMethodExit(exSysSynchLogDTODTOList);
            return exSysSynchLogDTODTOList;
        }

        public void UpdateExsysSynchLogRequestIDAndStatus(List<int> siteBasedLogIDList, int lastRequestId,string status,
                                                                  SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(siteBasedLogIDList, lastRequestId, sqlTransaction);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            exSysSynchLogDataHandler.UpdateExsysSynchLogRequestIDAndStatus(siteBasedLogIDList, status, lastRequestId);
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns All the ExSysSynchLogDTO records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of ExSysSynchLogDTO</returns>
        public List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters,
                                                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            List<ExSysSynchLogDTO> exSysSynchLogDTOList = exSysSynchLogDataHandler.GetExSysSynchLogDTOList(searchParameters);
            log.LogMethodExit(exSysSynchLogDTOList);
            return exSysSynchLogDTOList;
        }

        public List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(List<int> exsysLogIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(exsysLogIdList);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            List<ExSysSynchLogDTO> exsysSynchLogDTOList = exSysSynchLogDataHandler.GetExSysSynchLogDTOList(exsysLogIdList);
            log.LogMethodExit(exsysSynchLogDTOList);
            return exsysSynchLogDTOList;
        }

        public List<ExSysSynchLogDTO> GetFailedExSysSynchLogDTOList(List<int> siteIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteIdList, sqlTransaction);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            List<ExSysSynchLogDTO> exsysSynchLogDTOList = exSysSynchLogDataHandler.GetFailedExSysSynchLogDTOList(siteIdList);
            log.LogMethodExit(exsysSynchLogDTOList);
            return exsysSynchLogDTOList;
        }

        /// <summary>
        /// Returns the no of Exsys synch Log matching the search criteria
        /// </summary>
        /// <param name="searchCriteria">search criteria</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        /// <returns></returns>
        public int GetExsysSynchLogCount(List<KeyValuePair<ExSysSynchLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            int result = exSysSynchLogDataHandler.GetExsysSynchLogCount(searchParameters);
            log.LogMethodExit(result);
            return result;
        }

        /// <summary>
        /// Save and Update ExSysSynchLogDTO list Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                if (exSysSynchLogDTOList != null)
                {
                    foreach (ExSysSynchLogDTO exSysSynchLogDTO in exSysSynchLogDTOList)
                    {
                        ExSysSynchLogBL exSysSynchLogBL = new ExSysSynchLogBL(executionContext, exSysSynchLogDTO);
                        exSysSynchLogBL.Save(sqlTransaction);
                    }
                }
            }
            catch (Exception ex)
            {
                log.Error(ex);
                log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                throw;
            }
            log.LogMethodExit();
        }

        public List<ExSysSynchLogDTO> GetExSysSynchLogDTOList(string parafaitObjectName, string status, string remarks, List<int> parafaitObjectIdList, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(parafaitObjectName, status, remarks, parafaitObjectIdList, sqlTransaction);
            ExSysSynchLogDataHandler exSysSynchLogDataHandler = new ExSysSynchLogDataHandler(sqlTransaction);
            List<ExSysSynchLogDTO> exsysSynchLogDTOList = exSysSynchLogDataHandler.GetExSysSynchLogDTOList(parafaitObjectName, status, remarks, parafaitObjectIdList);
            log.LogMethodExit(exsysSynchLogDTOList);
            return exsysSynchLogDTOList;
        }
    }
}
