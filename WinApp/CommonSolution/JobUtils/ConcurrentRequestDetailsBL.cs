/********************************************************************************************
 * Project Name -  Concurrent Request Details Request
 * Description  - The bussiness logic for   Concurrent Request Details
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By    Remarks          
 *********************************************************************************************
 *2.70.2        24-Jul-2019   Dakshakh raj   Modified : Save() method Insert/Update method returns DTO.
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentRequestDetailsBL
    {
        private ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// Parameterized constructor of ConcurrentRequestDetailsBL class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ConcurrentRequestDetailsBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the concurrentRequestDetails id as the parameter
        /// Would fetch the concurrentRequestDetails object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="id">Id</param>
        /// <param name="sqlTransaction">Optional sql transaction</param>
        public ConcurrentRequestDetailsBL(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ConcurrentRequestDetailsDataHandler concurrentRequestDetailsDataHandler = new ConcurrentRequestDetailsDataHandler(sqlTransaction);
            concurrentRequestDetailsDTO = concurrentRequestDetailsDataHandler.GetConcurrentRequestDetailsDTO(id);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ConcurrentRequestDetailsBL object using the ConcurrentRequestDetailsDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="concurrentRequestDetailsDTO">ConcurrentRequestDetailsDTO object</param>
        public ConcurrentRequestDetailsBL(ExecutionContext executionContext, ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentRequestDetailsDTO);
            this.concurrentRequestDetailsDTO = concurrentRequestDetailsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ConcurrentRequestDetails
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentRequestDetailsDTO.IsChanged == false &&
                  concurrentRequestDetailsDTO.ConcurrentRequestDetailsId > -1)
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
            ConcurrentRequestDetailsDataHandler concurrentRequestDetailsDataHandler = new ConcurrentRequestDetailsDataHandler(sqlTransaction);
            if (concurrentRequestDetailsDTO.ConcurrentRequestDetailsId < 0)
            {
                concurrentRequestDetailsDTO = concurrentRequestDetailsDataHandler.InsertConcurrentRequestDetails(concurrentRequestDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentRequestDetailsDTO.AcceptChanges();
            }
            else
            {
                if (concurrentRequestDetailsDTO.IsChanged)
                {
                    concurrentRequestDetailsDTO = concurrentRequestDetailsDataHandler.UpdateConcurrentRequestDetails(concurrentRequestDetailsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    concurrentRequestDetailsDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the concurrentRequestDetailsDTO
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
        /// Gets the DTO
        /// </summary>
        public ConcurrentRequestDetailsDTO ConcurrentRequestDetailsDTO
        {
            get
            {
                return concurrentRequestDetailsDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ConcurrentRequestDetails
    /// </summary>
    public class ConcurrentRequestDetailsListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ConcurrentRequestDetailsListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">execution context</param>
        public ConcurrentRequestDetailsListBL(ExecutionContext executionContext, List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList) : this (executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentRequestDetailsDTOList);
            this.concurrentRequestDetailsDTOList = concurrentRequestDetailsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ConcurrentRequestDetails list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentRequestDetailsDTO> GetConcurrentRequestDetailsDTOList(List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ConcurrentRequestDetailsDataHandler concurrentRequestDetailsDataHandler = new ConcurrentRequestDetailsDataHandler(sqlTransaction);
            this.concurrentRequestDetailsDTOList = concurrentRequestDetailsDataHandler.GetConcurrentRequestDetailsDTOList(searchParameters);
            log.LogMethodExit(concurrentRequestDetailsDTOList);
            return concurrentRequestDetailsDTOList;
        }

        /// <summary>
        /// Saves the concurrentRequestDetailsDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentRequestDetailsDTOList == null ||
                concurrentRequestDetailsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < concurrentRequestDetailsDTOList.Count; i++)
            {
                var concurrentRequestDetailsDTO = concurrentRequestDetailsDTOList[i];
                if (concurrentRequestDetailsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ConcurrentRequestDetailsBL concurrentRequestDetailsBL = new ConcurrentRequestDetailsBL(executionContext, concurrentRequestDetailsDTO);
                    concurrentRequestDetailsBL.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving concurrentRequestDetailsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("concurrentRequestDetailsDTO", concurrentRequestDetailsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}
