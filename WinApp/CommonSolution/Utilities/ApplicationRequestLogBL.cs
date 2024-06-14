/********************************************************************************************
 * Project Name - Utilities
 * Description  - Business Logic of ApplicationRequestLog
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By             Remarks          
 *********************************************************************************************
 *2.120.10    06-Jul-2021   Abhishek                Created 
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Core.Utilities
{
    public class ApplicationRequestLogBL
    {
        private ApplicationRequestLogDTO applicationRequestLogDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ApplicationRequestLogBL class
        /// </summary>
        /// <param name="executionContext"></param>
        private ApplicationRequestLogBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the ApplicationRequestLog id as the parameter
        /// Would fetch the ApplicationRequestLog object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext object is passed as parameter</param>
        /// <param name="id">id of ApplicationRequestLogs Object </param>
        /// <param name="loadChildRecords">loadChildRecords holds either true or false.</param>
        /// <param name="activeChildRecords">activeChildRecords holds either true or false.</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ApplicationRequestLogBL(ExecutionContext executionContext, int id, bool loadChildRecords = true,
                                   bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ApplicationRequestLogDataHandler applicationRequestLogDataHandler = new ApplicationRequestLogDataHandler(sqlTransaction);
            applicationRequestLogDTO = applicationRequestLogDataHandler.GetApplicationRequestLogDTO(id);
            if (applicationRequestLogDTO == null)
            {
                string message = " unable to find Application Request Log with id" + id;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords == false)
            {
                log.LogMethodExit();
                return;
            }
            ApplicationRequestLogDetailListBL applicationRequestLogDetailListBL = new ApplicationRequestLogDetailListBL(executionContext);
            applicationRequestLogDTO.ApplicationRequestLogDetailDTOList = applicationRequestLogDetailListBL.GetApplicationRequestLogDTOListOfRequest(new List<int> { id }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        // <summary>
        // Creates ApplicationRequestLogBL object using the ApplicationRequestLogDTO
        // </summary>
        // <param name = "executionContext" > ExecutionContext object is passed as parameter</param>
        // <param name = "parameterApplicationRequestLogDTO" > ApplicationRequestLogDTO object is passed as parameter</param>
        public ApplicationRequestLogBL(ExecutionContext executionContext, ApplicationRequestLogDTO parameterApplicationRequestLogDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, parameterApplicationRequestLogDTO);
            if(parameterApplicationRequestLogDTO.Id >= 0)
            {
                string errorMessage = "Invalid operation. This constructor can't be used to instantiate saved record.";
                log.LogMethodExit(null, "Throwing Error - " + errorMessage);
                throw new Exception(errorMessage);
            }
            ValidateRequestGuid(parameterApplicationRequestLogDTO.RequestGuid);
            ValidateRequestModule(parameterApplicationRequestLogDTO.Module);
            ValidateRequestUsecase(parameterApplicationRequestLogDTO.Usecase);
            ValidateRequestTimestamp(parameterApplicationRequestLogDTO.Timestamp);
            ValidateRequestLoginId(parameterApplicationRequestLogDTO.LoginId);

            this.applicationRequestLogDTO = new ApplicationRequestLogDTO(-1, parameterApplicationRequestLogDTO.RequestGuid, 
                                                                        parameterApplicationRequestLogDTO.Module, parameterApplicationRequestLogDTO.Usecase,
                                                                        parameterApplicationRequestLogDTO.Timestamp, parameterApplicationRequestLogDTO.LoginId,
                                                                        parameterApplicationRequestLogDTO.IsActive);
            if(parameterApplicationRequestLogDTO.ApplicationRequestLogDetailDTOList != null)
            {
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
                foreach (var parameterApplicationRequestLogDetailDTO in parameterApplicationRequestLogDTO.ApplicationRequestLogDetailDTOList)
                {
                    var applicationRequestLogDetailDTO = new ApplicationRequestLogDetailDTO(-1, -1, parameterApplicationRequestLogDetailDTO.EntityGuid,
                                                                                            parameterApplicationRequestLogDetailDTO.IsActive);
                    ApplicationRequestLogDetailBL applicationRequestLogDetailBL = new ApplicationRequestLogDetailBL(executionContext, applicationRequestLogDetailDTO);
                    applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Add(applicationRequestLogDetailBL.GetApplicationRequestLogDetailDTO);
                }
                
            }
            log.LogMethodExit();
        }

        private void ValidateRequestLoginId(string loginId)
        {
            log.LogMethodEntry(loginId);
            if (string.IsNullOrWhiteSpace(loginId))
            {
                log.LogMethodExit(null, "loginId empty");
                return;
            }
            if (loginId.Length > 100)
            {
                string errorMessage = "Length of loginId should not exceed 100 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("loginId greater than 100 characters.", "ApplicationRequestLog", "loginId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateRequestTimestamp(DateTime timestamp)
        {
            log.LogMethodEntry(timestamp);
            if (timestamp == DateTime.MinValue && timestamp == DateTime.MaxValue)
            {
                string errorMessage = "Please enter a valid timestamp";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("timestamp shold not be minimum or maximum.", "ApplicationRequestLog", "timestamp", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateRequestUsecase(string usecase)
        {
            log.LogMethodEntry(usecase);
            if (string.IsNullOrWhiteSpace(usecase))
            {
                string errorMessage = "Please enter a usecase";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("usecase is empty.", "ApplicationRequestLog", "usecase", errorMessage);
            }
            if (usecase.Length > 100)
            {
                string errorMessage = "Length of Usecase should not exceed 100 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("Usecase greater than 100 characters.", "ApplicationRequestLog", "usecase", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateRequestModule(string module)
        {
            log.LogMethodEntry(module);
            if (string.IsNullOrWhiteSpace(module))
            {
                string errorMessage = "Please enter a module";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("module is empty.", "ApplicationRequestLog", "module", errorMessage);
            }
            if (module.Length > 100)
            {
                string errorMessage = "Length of module should not exceed 100 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("module greater than 100 characters.", "ApplicationRequestLog", "module", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateRequestGuid(string requestGuid)
        {
            log.LogMethodEntry(requestGuid);
            if (string.IsNullOrWhiteSpace(requestGuid))
            {
                string errorMessage = "Please enter a requestGuid";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("requestGuid is empty.", "ApplicationRequestLog", "requestGuid", errorMessage);
            }
            if (requestGuid.Length > 100)
            {
                string errorMessage = "Length of requestGuid should not exceed 100 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("requestGuid greater than 100 characters.", "ApplicationRequestLog", "requestGuid", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the AppUIPanel
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (applicationRequestLogDTO.IsChangedRecursive == false &&
                applicationRequestLogDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ApplicationRequestLogDataHandler applicationRequestLogDataHandler = new ApplicationRequestLogDataHandler(sqlTransaction);
            if (applicationRequestLogDTO.Id < 0)
            {
                applicationRequestLogDTO = applicationRequestLogDataHandler.Insert(applicationRequestLogDTO, executionContext != null ? executionContext.GetUserId() : string.Empty, executionContext != null ? executionContext.GetSiteId() : -1);
                applicationRequestLogDTO.AcceptChanges();
            }
            else if (applicationRequestLogDTO.IsChanged)
            {
                applicationRequestLogDTO = applicationRequestLogDataHandler.Update(applicationRequestLogDTO, executionContext != null ? executionContext.GetUserId() : string.Empty, executionContext != null ? executionContext.GetSiteId() : -1);
                applicationRequestLogDTO.AcceptChanges();
            }

            if (applicationRequestLogDTO.ApplicationRequestLogDetailDTOList != null &&
                  applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Count != 0)
            {
                foreach (ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO in applicationRequestLogDTO.ApplicationRequestLogDetailDTOList)
                {
                    applicationRequestLogDetailDTO.ApplicationRequestLogId = applicationRequestLogDTO.Id;
                }
                ApplicationRequestLogDetailListBL applicationRequestLogDetailListBL = new ApplicationRequestLogDetailListBL(executionContext, applicationRequestLogDTO.ApplicationRequestLogDetailDTOList);
                applicationRequestLogDetailListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ApplicationRequestLogDTO GetApplicationRequestLogDTO { get { return new ApplicationRequestLogDTO(applicationRequestLogDTO); } }
    }

    public class ApplicationRequestLogListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ApplicationRequestLogDTO> applicationRequestLogDTOList = new List<ApplicationRequestLogDTO>();

        /// <summary>
        /// Default constructor of ApplicationRequestLogListBL
        /// </summary>
        public ApplicationRequestLogListBL()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor of ApplicationRequestLogListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>                                                                                                                                     
        public ApplicationRequestLogListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="applicationRequestLogDTOList">ApplicationRequestLogDTO List is passed as parameter </param>
        public ApplicationRequestLogListBL(ExecutionContext executionContext,
                                       List<ApplicationRequestLogDTO> applicationRequestLogDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationRequestLogDTOList);
            this.applicationRequestLogDTOList = applicationRequestLogDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ApplicationRequestLogDTO list
        /// </summary>
        public List<ApplicationRequestLogDTO> GetAllApplicationRequestLogDTOList(List<KeyValuePair<ApplicationRequestLogDTO.SearchByParameters, string>> searchParameters,
                                                                         bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            ApplicationRequestLogDataHandler applicationRequestLogDataHandler = new ApplicationRequestLogDataHandler(sqlTransaction);
            List<ApplicationRequestLogDTO> applicationRequestLogDTOList = applicationRequestLogDataHandler.GetApplicationRequestLogList(searchParameters);
            if (loadChildRecords == false ||
                applicationRequestLogDTOList == null ||
                applicationRequestLogDTOList.Count > 0 == false)
            {
                log.LogMethodExit(applicationRequestLogDTOList, "Child records are not loaded.");
                return applicationRequestLogDTOList;
            }
            BuildApplicationRequestLogDTOList(applicationRequestLogDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(applicationRequestLogDTOList);
            return applicationRequestLogDTOList;
        }

        private void BuildApplicationRequestLogDTOList(List<ApplicationRequestLogDTO> applicationRequestLogDTOList, bool activeChildRecords,
                                                       SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(applicationRequestLogDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ApplicationRequestLogDTO> applicationRequestLogDTOIdMap = new Dictionary<int, ApplicationRequestLogDTO>();
            List<int> applicationRequestLogsIdList = new List<int>();
            for (int i = 0; i < applicationRequestLogDTOList.Count; i++)
            {
                if (applicationRequestLogDTOIdMap.ContainsKey(applicationRequestLogDTOList[i].Id))
                {
                    continue;
                }
                applicationRequestLogDTOIdMap.Add(applicationRequestLogDTOList[i].Id, applicationRequestLogDTOList[i]);
                applicationRequestLogsIdList.Add(applicationRequestLogDTOList[i].Id);
            }

            ApplicationRequestLogDetailListBL applicationRequestLogDetailListBL = new ApplicationRequestLogDetailListBL(executionContext);
            List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList = applicationRequestLogDetailListBL.GetApplicationRequestLogDTOListOfRequest(applicationRequestLogsIdList, activeChildRecords, sqlTransaction);
            if (applicationRequestLogDetailDTOList != null && applicationRequestLogDetailDTOList.Count > 0)
            {
                for (int i = 0; i < applicationRequestLogDetailDTOList.Count; i++)
                {
                    if (applicationRequestLogDTOIdMap.ContainsKey(applicationRequestLogDetailDTOList[i].Id) == false)
                    {
                        continue;
                    }
                    ApplicationRequestLogDTO applicationRequestLogDTO = applicationRequestLogDTOIdMap[applicationRequestLogDetailDTOList[i].Id];
                    if (applicationRequestLogDTO.ApplicationRequestLogDetailDTOList == null)
                    {
                        applicationRequestLogDTO.ApplicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
                    }
                    applicationRequestLogDTO.ApplicationRequestLogDetailDTOList.Add(applicationRequestLogDetailDTOList[i]);
                }
            }
            log.LogMethodExit();
        }

        internal ApplicationRequestLogDTO GetApplicationRequestLogDTO(string requestGuid, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requestGuid);
            ApplicationRequestLogDataHandler applicationRequestLogDataHandler = new ApplicationRequestLogDataHandler(sqlTransaction);
            ApplicationRequestLogDTO applicationRequestLogDTO = applicationRequestLogDataHandler.GetApplicationRequestLogsDTOOfGuid(requestGuid);
            if (applicationRequestLogDTO != null)
            {
                ApplicationRequestLogDetailListBL applicationRequestLogDetailListBL = new ApplicationRequestLogDetailListBL(executionContext);
                applicationRequestLogDTO.ApplicationRequestLogDetailDTOList = applicationRequestLogDetailListBL.GetApplicationRequestLogDTOListOfRequest(new List<int> { applicationRequestLogDTO.Id }, true, sqlTransaction);
                log.LogMethodExit(applicationRequestLogDTO); ;
                return applicationRequestLogDTO;
            }
            log.LogMethodExit(applicationRequestLogDTO); ;
            return applicationRequestLogDTO;
        }

        // <summary>
        // Saves the  list of applicationRequestLog DTO.
        // </summary>
        // <param name = "sqlTransaction" > sqlTransaction object</param>
        internal List<ApplicationRequestLogDTO> Save(SqlTransaction sqlTransaction = null)
        {
            //please implement similar to user
            log.LogMethodEntry();
            List<ApplicationRequestLogDTO> savedApplicationRequestLogDTOList = new List<ApplicationRequestLogDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (applicationRequestLogDTOList != null && applicationRequestLogDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (ApplicationRequestLogDTO applicationRequestLogDTO in applicationRequestLogDTOList)
                        {
                            ApplicationRequestLogBL applicationRequestLogBL = new ApplicationRequestLogBL(executionContext, applicationRequestLogDTO);
                            applicationRequestLogBL.Save(parafaitDBTrx.SQLTrx);
                            savedApplicationRequestLogDTOList.Add(applicationRequestLogBL.GetApplicationRequestLogDTO);
                        }
                        parafaitDBTrx.EndTransaction();
                    }
                }
                catch (SqlException sqlEx)
                {
                    log.Error(sqlEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                    if (sqlEx.Number == 547)
                    {
                        throw new ValidationException("Unable to delete this record.Please check the reference record first.");
                    }
                    if (sqlEx.Number == 2601)
                    {
                        throw new ValidationException("You cannot insert the duplicate record");
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (ValidationException valEx)
                {
                    log.Error(valEx);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Validation Exception : " + valEx.Message);
                    throw;
                }
                catch (Exception ex)
                {
                    log.Error(ex.Message);
                    parafaitDBTrx.RollBack();
                    log.LogMethodExit(null, "Throwing Exception : " + ex.Message);
                    throw;
                }

            }
            log.LogMethodExit(savedApplicationRequestLogDTOList);
            return savedApplicationRequestLogDTOList;
        }
    }
}
