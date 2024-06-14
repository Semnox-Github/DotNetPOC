/********************************************************************************************
 * Project Name - ConcurrentProgramParameters BL
 * Description  - BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       24-May-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    public class ConcurrentProgramParametersBL
    {
        private ConcurrentProgramParametersDTO concurrentProgramParametersDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
       
        private ConcurrentProgramParametersBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates ConcurrentProgramParametersBL object using the ConcurrentProgramParametersDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="ConcurrentProgramParametersDTO">ConcurrentProgramParametersDTO object</param>
        public ConcurrentProgramParametersBL(ExecutionContext executionContext, ConcurrentProgramParametersDTO concurrentProgramParametersDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramParametersDTO);
            this.concurrentProgramParametersDTO = concurrentProgramParametersDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the ConcurrentProgramParameters  id as the parameter
        /// To fetch the ConcurrentProgramParameters object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ConcurrentProgramParametersId">id - ConcurrentProgramParameters </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramParametersBL(ExecutionContext executionContext, int ConcurrentProgramParametersId,
                                           SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ConcurrentProgramParametersId, sqlTransaction);
            ConcurrentProgramParametersDataHandler concurrentProgramParametersDataHandler = new ConcurrentProgramParametersDataHandler(sqlTransaction);
            concurrentProgramParametersDTO = concurrentProgramParametersDataHandler.GetConcurrentProgramParametersDTO(ConcurrentProgramParametersId);
            if (concurrentProgramParametersDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ConcurrentProgramParameters", ConcurrentProgramParametersId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ConcurrentProgramParameters
        /// ConcurrentProgramParameters will be inserted if ConcurrentProgramParametersId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ConcurrentProgramParametersDataHandler concurrentProgramParametersDataHandler = new ConcurrentProgramParametersDataHandler(sqlTransaction);
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Throwing Validation Exception" + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed.", validationErrors);
            }
            if (concurrentProgramParametersDTO.ConcurrentProgramParameterId < 0)
            {
                concurrentProgramParametersDTO = concurrentProgramParametersDataHandler.Insert(concurrentProgramParametersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentProgramParametersDTO.AcceptChanges();
            }
            else
            {
                if (concurrentProgramParametersDTO.IsChanged)
                {
                    concurrentProgramParametersDTO = concurrentProgramParametersDataHandler.Update(concurrentProgramParametersDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    concurrentProgramParametersDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Validates the ConcurrentProgramParametersDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            //Need to be added
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the ConcurrentProgramParametersDTO
        /// </summary>
        public ConcurrentProgramParametersDTO ConcurrentProgramParametersDTO
        {
            get
            {
                return concurrentProgramParametersDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ConcurrentProgramParameters
    /// </summary>
    public class ConcurrentProgramParametersListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList = new List<ConcurrentProgramParametersDTO>();

        /// <summary>
        /// Parameterized constructor of ConcurrentProgramParametersListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public ConcurrentProgramParametersListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ConcurrentProgramParametersDTOList">ConcurrentProgramParameters DTO List as parameter </param>
        public ConcurrentProgramParametersListBL(ExecutionContext executionContext,
                                               List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramParametersDTOList);
            this.executionContext = executionContext;
            this.concurrentProgramParametersDTOList = concurrentProgramParametersDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the ConcurrentProgramParameters DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of ConcurrentProgramParametersDTO </returns>
        public List<ConcurrentProgramParametersDTO> GetConcurrentProgramParametersDTOList(List<KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ConcurrentProgramParametersDataHandler concurrentProgramParametersDataHandler = new ConcurrentProgramParametersDataHandler(sqlTransaction);
            List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList = concurrentProgramParametersDataHandler.GetConcurrentProgramParametersDTOList(searchParameters);
            log.LogMethodExit(concurrentProgramParametersDTOList);
            return concurrentProgramParametersDTOList;
        }



        /// <summary>
        /// Gets the ProgramParameterValueDTO List for programIdList 
        /// </summary>
        /// <param name="programIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProgramParameterValueDTO</returns>
        public List<ConcurrentProgramParametersDTO> GetProgramsParameterDTOListOfPrograms(List<int> programIdList,
                                                bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programIdList, activeRecords, sqlTransaction);
            ConcurrentProgramParametersDataHandler programParameterValueDataHandler = new ConcurrentProgramParametersDataHandler(sqlTransaction);
            List<ConcurrentProgramParametersDTO> programParameterValueDTOList = programParameterValueDataHandler.GetConcurrentProgramParametersDTOList(programIdList, activeRecords);
            log.LogMethodExit(programParameterValueDTOList);
            return programParameterValueDTOList;
        }

        /// <summary>
        /// Saves the  list of ConcurrentProgramParameters DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentProgramParametersDTOList == null ||
                concurrentProgramParametersDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < concurrentProgramParametersDTOList.Count; i++)
            {
                ConcurrentProgramParametersDTO concurrentProgramParametersDTO = concurrentProgramParametersDTOList[i];
                if (concurrentProgramParametersDTO.IsChanged || concurrentProgramParametersDTO.ConcurrentProgramParameterId < 0)
                {
                    try
                    {
                        ConcurrentProgramParametersBL ConcurrentProgramParametersBL = new ConcurrentProgramParametersBL(executionContext, concurrentProgramParametersDTO);
                        ConcurrentProgramParametersBL.Save(sqlTransaction);
                    }
                    catch (SqlException ex)
                    {
                        log.Error(ex);
                        if (ex.Number == 2601)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1872));
                        }
                        else if (ex.Number == 547)
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
                        }
                        else
                        {
                            throw new ValidationException(MessageContainerList.GetMessage(executionContext, ex.Message));
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error("Error occurred while saving ConcurrentProgramParametersDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("ConcurrentProgramParametersDTO", concurrentProgramParametersDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
    }

}
