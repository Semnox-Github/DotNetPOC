/********************************************************************************************
 * Project Name - Utilities
 * Description  - Business Logic of ApplicationRequestLogDetail
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
    /// <summary>
    /// Business logic for ApplicationRequestLogDetail class.
    /// </summary>
    public class ApplicationRequestLogDetailBL
    {
        private ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ApplicationRequestLogDetail class
        /// </summary>
        private ApplicationRequestLogDetailBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Application Request Log Detail id as the parameter
        /// Would fetch the Application Request Log Detail object from the database based on the id passed. 
        /// </summary>
        /// <param name="id">Application Request Log Detail id</param>
        public ApplicationRequestLogDetailBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
             : this(executionContext)
        {
            log.LogMethodEntry(id);
            ApplicationRequestLogDetailDataHandler applicationRequestLogDetailDataHandler = new ApplicationRequestLogDetailDataHandler(sqlTransaction);
            applicationRequestLogDetailDTO = applicationRequestLogDetailDataHandler.GetApplicationRequestLogDetailDTO(id);
            if (applicationRequestLogDetailDTO == null)
            {
                string message = " unable to find Application Request Log Detail with id" + id;
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit(applicationRequestLogDetailDTO);
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        /// <param name="applicationRequestLogDetailDTO"></param>
        public ApplicationRequestLogDetailBL(ExecutionContext executionContext, ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(applicationRequestLogDetailDTO);
            if (applicationRequestLogDetailDTO.Id < 0)
            {
                ValidateEntityGuid(applicationRequestLogDetailDTO.EntityGuid);
            }
            this.applicationRequestLogDetailDTO = applicationRequestLogDetailDTO;
            log.LogMethodExit();
        }

        private void ValidateApplicationRequestLogId(int applicationRequestLogId)
        {
            log.LogMethodEntry(applicationRequestLogId);
            if (applicationRequestLogId < 0)
            {
                string errorMessage = "Please enter valid value for ApplicationRequestLogId";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("The ApplicationRequestLogId must be a non-negative integer.", " ApplicationRequestLog", " ApplicationRequestLogId", errorMessage);
            }
            log.LogMethodExit();
        }

        private void ValidateEntityGuid(string entityGuid)
        {
            log.LogMethodEntry(entityGuid);
            if (string.IsNullOrWhiteSpace(entityGuid))
            {
                string errorMessage = "Please enter a entityGuid";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("entityGuid is empty.", "ApplicationRequestLogDetail", "entityGuid", errorMessage);
            }
            if (entityGuid.Length > 100)
            {
                string errorMessage = "Length of entityGuid should not exceed 100 characters";
                log.LogMethodExit(null, "Throwing Exception - " + errorMessage);
                throw new ValidationException("entityGuid greater than 100 characters.", "ApplicationRequestLog", "entityGuid", errorMessage);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the Application Request Log Detail
        /// Checks if the applicationRequestLogDetailDTO id is not less than or equal to 0
        ///     If it is less than or equal to 0, then inserts
        ///     else updates
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (applicationRequestLogDetailDTO.IsChanged == false
                   && applicationRequestLogDetailDTO.Id > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            ApplicationRequestLogDetailDataHandler applicationRequestLogDetailDataHandler = new ApplicationRequestLogDetailDataHandler(sqlTransaction);
          
            if (applicationRequestLogDetailDTO.Id < 0)
            {
                applicationRequestLogDetailDTO = applicationRequestLogDetailDataHandler.Insert(applicationRequestLogDetailDTO, executionContext != null ? executionContext.GetUserId() : string.Empty, executionContext != null ? executionContext.GetSiteId() : -1);
                applicationRequestLogDetailDTO.AcceptChanges();
            }
            else
            {
                if (applicationRequestLogDetailDTO.IsChanged)
                {
                    applicationRequestLogDetailDTO = applicationRequestLogDetailDataHandler.Update(applicationRequestLogDetailDTO, executionContext != null ? executionContext.GetUserId() : string.Empty, executionContext != null ? executionContext.GetSiteId() : -1);
                    applicationRequestLogDetailDTO.AcceptChanges();
                }
            }
        }

       

        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ApplicationRequestLogDetailDTO GetApplicationRequestLogDetailDTO { get { return applicationRequestLogDetailDTO; } }
    }

    // <summary>
    /// Manages the list of ApplicationRequestLogDetail
    /// </summary>
    public class ApplicationRequestLogDetailListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>(); // To be initialized

        /// <summary>
        /// Parameterized constructor of ApplicationRequestLogDetailListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public ApplicationRequestLogDetailListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="applicationRequestLogDetailDTO">ApplicationRequestLogDetail DTO List as parameter </param>
        public ApplicationRequestLogDetailListBL(ExecutionContext executionContext, List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, applicationRequestLogDetailDTOList);
            this.applicationRequestLogDetailDTOList = applicationRequestLogDetailDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the ApplicationRequestLogDetail DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of ApplicationRequestLogDetailDTO </returns>
        public List<ApplicationRequestLogDetailDTO> GetApplicationRequestLogDetailDTOList(List<KeyValuePair<ApplicationRequestLogDetailDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ApplicationRequestLogDetailDataHandler productUserGroupsMappingDataHandler = new ApplicationRequestLogDetailDataHandler(sqlTransaction);
            List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList = productUserGroupsMappingDataHandler.GetApplicationRequestLogDetailDTOList(searchParameters);
            log.LogMethodExit(applicationRequestLogDetailDTOList);
            return applicationRequestLogDetailDTOList;
        }

        /// <summary>
        /// Gets the ProductUserGroupsMappingDTO List for productUserGroupsIdList 
        /// </summary>
        /// <param name="applicationRequestLogIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProductUserGroupsMappingDTO</returns>
        public List<ApplicationRequestLogDetailDTO> GetApplicationRequestLogDTOListOfRequest(List<int> applicationRequestLogIdList, bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(applicationRequestLogIdList, activeRecords, sqlTransaction);
            ApplicationRequestLogDetailDataHandler applicationRequestLogDetailDataHandler = new ApplicationRequestLogDetailDataHandler(sqlTransaction);
            List<ApplicationRequestLogDetailDTO> applicationRequestLogDetailDTOList = applicationRequestLogDetailDataHandler.GetApplicationRequestLogDetailDTOListOfRequest(applicationRequestLogIdList, activeRecords);
            log.LogMethodExit(applicationRequestLogDetailDTOList);
            return applicationRequestLogDetailDTOList;
        }

        // <summary>
        // Saves the  list of applicationRequestLogDetailDTO DTO.
        // </summary>
        // <param name = "sqlTransaction" > sqlTransaction object</param>
        internal List<ApplicationRequestLogDetailDTO> Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ApplicationRequestLogDetailDTO> savedApplicationRequestLogDetailDTOList = new List<ApplicationRequestLogDetailDTO>();
            using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
            {
                try
                {
                    if (applicationRequestLogDetailDTOList != null && applicationRequestLogDetailDTOList.Any())
                    {
                        parafaitDBTrx.BeginTransaction();
                        foreach (ApplicationRequestLogDetailDTO applicationRequestLogDetailDTO in applicationRequestLogDetailDTOList)
                        {
                            ApplicationRequestLogDetailBL applicationRequestLogDetailBL = new ApplicationRequestLogDetailBL(executionContext, applicationRequestLogDetailDTO);
                            applicationRequestLogDetailBL.Save(parafaitDBTrx.SQLTrx);
                            savedApplicationRequestLogDetailDTOList.Add(applicationRequestLogDetailBL.GetApplicationRequestLogDetailDTO);
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
            log.LogMethodExit(savedApplicationRequestLogDetailDTOList);
            return savedApplicationRequestLogDetailDTOList;
        }
    }
}
