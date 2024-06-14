/********************************************************************************************
 * Project Name - Reports
 * Description  - CommunicationLogBL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By        Remarks          
 *********************************************************************************************
 *2.70.3      16-Nov -2019     Girish Kundar       Created.
 ********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Semnox.Parafait.Reports
{
    public class CommunicationLogBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected static ExecutionContext machineUserContext = ExecutionContext.GetExecutionContext();
        private CommunicationLogDTO communicationLogDTO;
        private Utilities utilities = new Utilities();

        /// <summary>
        /// Default constructor CommunicationLogBL class
        /// </summary>
        public CommunicationLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            communicationLogDTO = null;
            log.LogMethodExit();
        }

        /// <summary>
        /// constructor with communicationLogDTO parameter
        /// </summary>
        /// <param name="communicationLogDTO">parameter of type CommunicationLogDTO </param>
        public CommunicationLogBL(ExecutionContext executionContext, CommunicationLogDTO communicationLogDTO)
        {
            log.LogMethodEntry(communicationLogDTO);
            this.communicationLogDTO = communicationLogDTO;
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
            CommunicationLogDataHandler communicationLogDataHandler = new CommunicationLogDataHandler();
            List<ValidationError> validationErrorList = Validate();
            if (validationErrorList.Count > 0)
            {
                throw new ValidationException("Validation Failed", validationErrorList);
            }
            communicationLogDTO = communicationLogDataHandler.Insert(communicationLogDTO, machineUserContext.GetUserId(), machineUserContext.GetSiteId());
            communicationLogDTO.AcceptChanges();
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the  CommunicationLogDTO  .
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
        /// Gets the CommunicationLogDTO
        /// </summary>
        public CommunicationLogDTO CommunicationLogDTO
        {
            get { return communicationLogDTO; }
        }

    }
    /// <summary>
    /// Class for ExSysSynchLogListBL List
    /// </summary>
    public class CommunicationLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<CommunicationLogDTO> communicationLogDTOList = new List<CommunicationLogDTO>();
        private ExecutionContext executionContext;
        public CommunicationLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry();
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor with executionContext and communicationLogDTOList as parameters
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="communicationLogDTOList">communicationLogDTOList</param>
        public CommunicationLogListBL(ExecutionContext executionContext, List<CommunicationLogDTO> communicationLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry();
            this.communicationLogDTOList = communicationLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns All active the communicationLogDTO records from the table 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of  CommunicationLogDTO</returns>
        public List<CommunicationLogDTO> GetAllCommunicationLogDTOList(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            CommunicationLogDataHandler communicationLogDataHandler = new CommunicationLogDataHandler(sqlTransaction);
            List<KeyValuePair<CommunicationLogDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<CommunicationLogDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<CommunicationLogDTO.SearchByParameters, string>(CommunicationLogDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            List<CommunicationLogDTO> communicationLogDTOList = communicationLogDataHandler.GetCommunicationLogDTOList(searchParameters);
            log.LogMethodExit(communicationLogDTOList);
            return communicationLogDTOList;
        }

        /// <summary>
        ///  Returns All the CommunicationLogDTO records from the table 
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>List of CommunicationLogDTO</returns>
        public List<CommunicationLogDTO> GetCommunicationLogDTOList(List<KeyValuePair<CommunicationLogDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            CommunicationLogDataHandler communicationLogDataHandler = new CommunicationLogDataHandler(sqlTransaction);
            List<CommunicationLogDTO> communicationLogDTOList = communicationLogDataHandler.GetCommunicationLogDTOList(searchParameters);
            log.LogMethodExit(communicationLogDTOList);
            return communicationLogDTOList;
        }

        /// <summary>
        /// Save and Update CommunicationLogDTO list Method
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            try
            {
                if (communicationLogDTOList != null)
                {
                    foreach (CommunicationLogDTO communicationLogDTO in communicationLogDTOList)
                    {
                        CommunicationLogBL communicationLogBL = new CommunicationLogBL(executionContext, communicationLogDTO);
                        communicationLogBL.Save(sqlTransaction);
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
    }
}
