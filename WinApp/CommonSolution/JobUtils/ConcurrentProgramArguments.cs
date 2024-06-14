
/********************************************************************************************
 * Project Name - Concurrent Program Arguments
 * Description  - Bussiness logic of the Concurrent Program Arguments class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        7-Mar-2016     Amaresh          Created 
 *2.70.2        24-Jul-2019    Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90.0        24-Jun-2020    Faizan           Modified : REST API changes Phase -2.
 *2.140       14-Sep-2021      Fiona          Modified: Issue fixes added Id Constructor
 ********************************************************************************************/
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Program Arguments 
    /// </summary>

    public class ConcurrentProgramArguments
    {
        private ConcurrentProgramArgumentsDTO concurrentProgramArgumentsDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ConcurrentProgramArguments class
        /// </summary>
        private ConcurrentProgramArguments(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
       /// <summary>
       /// 
       /// </summary>
       /// <param name="executionContext"></param>
       /// <param name="id"></param>
       /// <param name="sqlTransaction"></param>
        public ConcurrentProgramArguments(ExecutionContext executionContext, int id, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, id, sqlTransaction);
            ConcurrentProgramArgumentsDataHandler concurrentProgramArgumentsDataHandler = new ConcurrentProgramArgumentsDataHandler(sqlTransaction);
            concurrentProgramArgumentsDTO = concurrentProgramArgumentsDataHandler.GetConcurrentProgramArgumentsDTO(id);
            if (concurrentProgramArgumentsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ConcurrentProgramArguments", id);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates ConcurrentProgramArguments object using the ConcurrentProgramArgumentsDTO
        /// </summary>
        /// <param name="concurrentProgramArguments">ConcurrentProgramArgumentsDTO object</param>
        public ConcurrentProgramArguments(ExecutionContext executionContext, ConcurrentProgramArgumentsDTO concurrentProgramArguments)
            : this(executionContext)
        {
            log.LogMethodEntry(concurrentProgramArguments);
            this.concurrentProgramArgumentsDTO = concurrentProgramArguments;
            log.LogMethodExit();
        }
       
        /// <summary>
        ///  Saves the ConcurrentProgramArguments 
        /// Checks if the Argument is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentProgramArgumentsDTO.IsChanged == false &&
               concurrentProgramArgumentsDTO.ArgumentId > -1)
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

            ConcurrentProgramArgumentsDataHandler concurrentProgramArgumentsDataHandler = new ConcurrentProgramArgumentsDataHandler(sqlTransaction);
            if (concurrentProgramArgumentsDTO.ArgumentId <= 0)
            {
                concurrentProgramArgumentsDTO = concurrentProgramArgumentsDataHandler.InsertConcurrentProgramArguments(concurrentProgramArgumentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentProgramArgumentsDTO.AcceptChanges();
            }
            else if (concurrentProgramArgumentsDTO.IsChanged)
            {
                concurrentProgramArgumentsDTO = concurrentProgramArgumentsDataHandler.UpdateConcurrentProgramArguments(concurrentProgramArgumentsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentProgramArgumentsDTO.AcceptChanges();
            }

            log.LogMethodExit();
        }

        /// <summary>
        /// Validate the ConcurrentProgramArgumentsDTO
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
        public ConcurrentProgramArgumentsDTO GetconcurrentProgramArguments { get { return concurrentProgramArgumentsDTO; } }
    }

    /// <summary>
    /// Manages the list of Concurrent Program Arguments
    /// </summary>
    public class ConcurrentProgramArgumentList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ConcurrentProgramArgumentsDTO> concurrentProgramArgumentsDTOList;
        /// <summary>
        /// Parameterizaed constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ConcurrentProgramArgumentList(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterizaed constructor
        /// </summary>
        /// <param name="concurrentProgramArgumentsDTOList"></param>
        /// <param name="executionContext"></param>
        public ConcurrentProgramArgumentList(ExecutionContext executionContext, List<ConcurrentProgramArgumentsDTO> concurrentProgramArgumentsDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramArgumentsDTOList);
            this.concurrentProgramArgumentsDTOList = concurrentProgramArgumentsDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Concurrent Requests based on the searchParameter
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentProgramArgumentsDTO> GetConcurrentProgramArguments(List<KeyValuePair<ConcurrentProgramArgumentsDTO.SearchByProgramArgumentsParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters);
            ConcurrentProgramArgumentsDataHandler concurrentProgramArgumentsDataHandler = new ConcurrentProgramArgumentsDataHandler(sqlTransaction);
            List<ConcurrentProgramArgumentsDTO> concurrentProgramArgumentsDTOs = concurrentProgramArgumentsDataHandler.GetConcurrentProgramArguments(searchParameters);
            log.LogMethodExit(concurrentProgramArgumentsDTOs);
            return concurrentProgramArgumentsDTOs;
        }

        /// <summary>
        /// Saves the concurrentProgramArgumentsDTOList
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        internal void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentProgramArgumentsDTOList == null ||
                concurrentProgramArgumentsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < concurrentProgramArgumentsDTOList.Count; i++)
            {
                var concurrentProgramArgumentsDTO = concurrentProgramArgumentsDTOList[i];
                if (concurrentProgramArgumentsDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ConcurrentProgramArguments concurrentProgramArguments = new ConcurrentProgramArguments(executionContext, concurrentProgramArgumentsDTO);
                    concurrentProgramArguments.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving concurrentProgramArgumentsDTOList.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("concurrentProgramArgumentsDTOList", concurrentProgramArgumentsDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }
    }
}

