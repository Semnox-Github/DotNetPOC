
/********************************************************************************************
 * Project Name - Concurrent Requests
 * Description  - Bussiness logic of the Concurrent Requests class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        24-Feb-2016    Jeevan           Created 
 *2.70.2      24-Jul-2019    Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.70.2      25-Nov-2019    Akshay G         Added - GetTopConcurrentRequestsCompleted()
  *2.90       26-May-2020   Mushahid Faizan     Modified : 3 tier changes for Rest API.
 *2.100       09-May-2020    Nitin Pai        Fix, included site id check
 *2.140.6     29-May-2023    Deeksha          Modified as part of Aloha BSP Enhancements
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Requests defines the various 
    /// </summary>
    public class ConcurrentRequests
   {
       private ConcurrentRequestsDTO concurrentRequests;
       private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Default constructor of ConcurrentRequestsBL class
        /// </summary> 
        public ConcurrentRequests(ExecutionContext executionContext) // Used in the ParafaitExSysServer.cs
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Request id as the parameter
        /// Would fetch the ConcurrentRequestsDTO object from the database based on the id passed. 
        /// </summary>
        /// <param name="requestId">Request id </param>
        /// <param name="sqltransaction">sqltransaction </param>
        public ConcurrentRequests(ExecutionContext executionContext, int requestId, SqlTransaction sqltransaction = null, string connectionString = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, requestId, sqltransaction);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(connectionString);
            concurrentRequests = concurrentRequestsDataHandler.GetConcurrentRequests(requestId);
            log.LogMethodExit(requestId);
        }

        /// <summary>
        /// Creates concurrentRequests object using the ConcurrentExecutableDTO
        /// </summary>
        /// <param name="concurrentRequests">ConcurrentRequestsDTO object</param>
       public ConcurrentRequests(ExecutionContext executionContext, ConcurrentRequestsDTO concurrentRequests)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentRequests); 
            this.concurrentRequests = concurrentRequests;
            log.LogMethodEntry();
        }

        /// <summary>
        /// Saves the ConcurrentRequests 
        /// Checks if the RequestId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null,string connectionstring = null)
        {
            log.LogMethodEntry(sqlTransaction);

            if (concurrentRequests.IsChanged == false && concurrentRequests.RequestId > -1)
            {
                log.LogMethodExit(null, "No Changes to save");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(connectionstring, sqlTransaction);
           if (concurrentRequests.RequestId <= 0)
           {
               concurrentRequests = concurrentRequestsDataHandler.InsertConcurrentRequests(concurrentRequests, executionContext.GetUserId(), executionContext.GetSiteId());
               concurrentRequests.AcceptChanges();
           }
           else
           {
               if (concurrentRequests.IsChanged)
               {
                   concurrentRequests = concurrentRequestsDataHandler.UpdateConcurrentRequests(concurrentRequests, executionContext.GetUserId(), executionContext.GetSiteId());
                   concurrentRequests.AcceptChanges();
               }
           }
            log.LogMethodExit();
       }

        /// <summary>
        /// Validate the concurrentRequests
        /// </summary>
        /// <returns></returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            // Validation Logic here.
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the top ConcurrentRequests record where phase - Complete and Status - Normal
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public ConcurrentRequestsDTO GetTopCompletedConcurrentRequest(int programId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programId);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDataHandler.GetTopCompletedConcurrentRequest(programId, executionContext.GetSiteId());
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        } 


        /// Gets the top ConcurrentRequests record where phase - Complete and Status - Normal
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public ConcurrentRequestsDTO GetTopCompletedConcurrentRequest(int programId, int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programId);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDataHandler.GetTopCompletedConcurrentRequest(programId, siteId);
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        }

        /// Gets the top ConcurrentRequests record where phase - Complete and Status - Normal
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public ConcurrentRequestsDTO GetTopRunningRequestsForBSP(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDataHandler.GetTopCompletedRunningRequestsForBSP(siteId);
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        }

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ConcurrentRequestsDTO GetconcurrentRequests { get { return concurrentRequests; } }
    }

    /// <summary>
    /// Manages the list of Concurrent Requests
    /// </summary>
    public class ConcurrentRequestList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ConcurrentRequestsDTO> concurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();

        public ConcurrentRequestList(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        public ConcurrentRequestList(ExecutionContext executionContext, List<ConcurrentRequestsDTO> concurrentRequestsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentRequestsDTOList);
            this.concurrentRequestsDTOList = concurrentRequestsDTOList;
            log.LogMethodExit();
        }
        /// <summary>
        /// Returns the Concurrent Requests based on the request ID
        /// </summary>
        /// <param name="requestId">requestId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public ConcurrentRequestsDTO GetConcurrentRequest(int requestId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(requestId, sqlTransaction);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDataHandler.GetConcurrentRequests(requestId);
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        }

        /// <summary>
        /// Returns the Concurrent Requests list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> GetAllConcurrentRequests(List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestsDataHandler.GetConcurrentRequestsList(searchParameters);
            log.LogMethodEntry();
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Returns the Concurrent Requests list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> GetAllConcurrentRequests(List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> searchParameters, 
                                                                        string connectionString)
        {
            log.LogMethodEntry(searchParameters, connectionString);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(connectionString);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestsDataHandler.GetConcurrentRequestsList(searchParameters);
            log.LogMethodEntry(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Gets the Concurrent Requests which are running  
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns Datatable</returns>
        public DataTable GetConcurrentRequestsRunning(int siteId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteId, sqlTransaction);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            DataTable dataTable = concurrentRequestsDataHandler.GetConcurrentRequestsRunning(siteId);
            log.LogMethodExit(dataTable);
            return dataTable;
        }

        /// <summary>
        /// Gets the Concurrent Requests which are scheduled to run  
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns Datatable</returns>
        public List<ConcurrentRequestsDTO> GetConcurrentRequestsScheduledToRun(int siteId, string connectionString = null)
        {
            log.LogMethodEntry(siteId, connectionString);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(connectionString);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestsDataHandler.GetConcurrentRequestsScheduledToRun(siteId);
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Method to clean up the running requests
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns ConcurrentRequestsDTO </returns>
        public List<ConcurrentRequestsDTO> GetCleanupRequests(string connectionString = null)
        {
            log.LogMethodEntry(connectionString);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(connectionString);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestsDataHandler.GetCleanupRequests();
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Method to checks schedule and Get programs schedule which are due for the current day
        /// </summary>
        /// <param name="IncludeSystemPrograms">IncludeSystemPrograms</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> BuildNewRequestDTOForProgramsDueForSchedules(bool IncludeSystemPrograms,int siteId , string connectionString)
        {
            log.LogMethodEntry(IncludeSystemPrograms);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(connectionString);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = null;
            if (IncludeSystemPrograms)
            {
                concurrentRequestsDTOList = concurrentRequestsDataHandler.BuildNewRequestDTOForProgramsDueForSystemPrograms(siteId);
            }
            else
            {
                concurrentRequestsDTOList = concurrentRequestsDataHandler.BuildNewRequestDTOForProgramsDueForSchedules(siteId);
            }
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

        /// <summary>
        /// Method to checks schedule and Get programs schedule which are due for the current day
        /// </summary>
        /// <param name="IncludeSystemPrograms">IncludeSystemPrograms</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public DataTable GetConcurrentProgramScheduleDue(bool IncludeSystemPrograms, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(IncludeSystemPrograms, sqlTransaction);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            DataTable dataTable = concurrentRequestsDataHandler.GetConcurrentProgramScheduleDue(IncludeSystemPrograms, sqlTransaction);
            log.LogMethodExit(dataTable);
            return dataTable;
        }


        /// <summary>
        /// Saves the concurrentRequestsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
         public void Save(SqlTransaction sqlTransaction = null,string connectionString = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentRequestsDTOList == null ||
                concurrentRequestsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < concurrentRequestsDTOList.Count; i++)
            {
                var concurrentRequestsDTO = concurrentRequestsDTOList[i];
                if (concurrentRequestsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ConcurrentRequests concurrentRequests = new ConcurrentRequests(executionContext, concurrentRequestsDTO);
                    concurrentRequests.Save(sqlTransaction, connectionString);
                    log.Debug("Request added " + concurrentRequestsDTO.RequestId);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving concurrentRequestsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("concurrentRequestsDTO", concurrentRequestsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
        /// Gets the top ConcurrentRequests record where phase - Complete and Status - Normal
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public ConcurrentRequestsDTO GetTopCompletedConcurrentRequest(int programId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programId);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            ConcurrentRequestsDTO concurrentRequestsDTO = concurrentRequestsDataHandler.GetTopCompletedConcurrentRequest(programId, executionContext.GetSiteId());
            log.LogMethodExit(concurrentRequestsDTO);
            return concurrentRequestsDTO;
        }

        /// <summary>
        /// Gets the DTO List
        /// </summary>
        public List<ConcurrentRequestsDTO> GetconcurrentRequestsDTOList { get { return concurrentRequestsDTOList; } }


        /// <summary>
        /// Gets the top ConcurrentRequests record where phase - Complete and Status - Normal
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="sqlTransaction"></param>
        /// <returns></returns>
        public List<ConcurrentRequestsDTO> GetTopCompletedConcurrentRequestList(List<int> siteIdList,
                                                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(siteIdList);
            ConcurrentRequestsDataHandler concurrentRequestsDataHandler = new ConcurrentRequestsDataHandler(sqlTransaction);
            List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestsDataHandler.GetTopCompletedConcurrentRequest(siteIdList);
            log.LogMethodExit(concurrentRequestsDTOList);
            return concurrentRequestsDTOList;
        }

    }
}
