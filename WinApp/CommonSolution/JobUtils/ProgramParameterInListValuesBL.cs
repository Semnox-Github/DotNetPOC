/********************************************************************************************
 * Project Name - ProgramParameterInListValues BL
 * Description  - BL class
 * 
 **************
 **Version Log
 **************
 *Version     Date             Modified By      Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Semnox.Parafait.JobUtils
{
    public class ProgramParameterInListValuesBL
    {
        private ProgramParameterInListValuesDTO programParameterInListValuesDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProgramParameterInListValuesBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates ProgramParameterInListValuesBL object using the ProgramParameterInListValuesDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="ProgramParameterInListValuesDTO">ProgramParameterInListValuesDTO object</param>
        public ProgramParameterInListValuesBL(ExecutionContext executionContext, ProgramParameterInListValuesDTO programParameterInListValuesDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, programParameterInListValuesDTO);
            this.programParameterInListValuesDTO = programParameterInListValuesDTO;
            log.LogMethodExit();
        }
        /// <summary>
        /// Constructor with the ProgramParameterInListValues  id as the parameter
        /// To fetch the ProgramParameterInListValues object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="ProgramParameterInListValuesId">id - ProgramParameterInListValues </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProgramParameterInListValuesBL(ExecutionContext executionContext, int ProgramParameterInListValuesId,
                                           SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, ProgramParameterInListValuesId, sqlTransaction);
            ProgramParameterInListValuesDataHandler programParameterInListValuesDataHandler = new ProgramParameterInListValuesDataHandler(sqlTransaction);
            programParameterInListValuesDTO = programParameterInListValuesDataHandler.GetProgramParameterInListValuesDTO(ProgramParameterInListValuesId);
            if (programParameterInListValuesDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ProgramParameterInListValues", ProgramParameterInListValuesId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the ProgramParameterInListValues
        /// ProgramParameterInListValues will be inserted if ProgramParameterInListValuesId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ProgramParameterInListValuesDataHandler programParameterInListValuesDataHandler = new ProgramParameterInListValuesDataHandler(sqlTransaction);
            if (programParameterInListValuesDTO.IsChanged == false
                && programParameterInListValuesDTO.ProgramParameterInListValueId > -1)
            {
                log.LogMethodExit(null, "ProgramParameterInListValuesDTO is not changed.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Throwing Validation Exception" + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed.", validationErrors);
            }
            if (programParameterInListValuesDTO.ProgramParameterInListValueId < 0)
            {
                programParameterInListValuesDTO = programParameterInListValuesDataHandler.Insert(programParameterInListValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                programParameterInListValuesDTO.AcceptChanges();
            }
            else
            {
                if (programParameterInListValuesDTO.IsChanged)
                {
                    programParameterInListValuesDTO = programParameterInListValuesDataHandler.Update(programParameterInListValuesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    programParameterInListValuesDTO.AcceptChanges();
                }
            }
            log.LogMethodExit();
        }
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);

            ProgramParameterInListValuesDataHandler programParameterInListValuesDataHandler = new ProgramParameterInListValuesDataHandler(sqlTransaction);
            if (programParameterInListValuesDTO.ProgramParameterValueId > 0)
            {
                programParameterInListValuesDataHandler.Delete(programParameterInListValuesDTO);
                programParameterInListValuesDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the ProgramParameterInListValuesDTO 
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
        /// Gets the ProgramParameterInListValuesDTO
        /// </summary>
        public ProgramParameterInListValuesDTO ProgramParameterInListValuesDTO
        {
            get
            {
                return programParameterInListValuesDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProgramParameterInListValues
    /// </summary>
    public class ProgramParameterInListValuesListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = new List<ProgramParameterInListValuesDTO>();

        /// <summary>
        /// Parameterized constructor of ProgramParameterInListValuesListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public ProgramParameterInListValuesListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="ProgramParameterInListValuesDTOList">ProgramParameterInListValues DTO List as parameter </param>
        public ProgramParameterInListValuesListBL(ExecutionContext executionContext,
                                               List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, programParameterInListValuesDTOList);
            this.executionContext = executionContext;
            this.programParameterInListValuesDTOList = programParameterInListValuesDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the  list of ProgramParameterInListValues DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (programParameterInListValuesDTOList == null ||
                programParameterInListValuesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < programParameterInListValuesDTOList.Count; i++)
            {
                ProgramParameterInListValuesDTO programParameterInListValuesDTO = programParameterInListValuesDTOList[i];
                if (programParameterInListValuesDTO.IsChanged || programParameterInListValuesDTO.ProgramParameterInListValueId < 0)
                {
                    try
                    {
                        ProgramParameterInListValuesBL ProgramParameterInListValuesBL = new ProgramParameterInListValuesBL(executionContext, programParameterInListValuesDTO);
                        ProgramParameterInListValuesBL.Save(sqlTransaction);
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
                        log.Error("Error occurred while saving ProgramParameterInListValuesDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("ProgramParameterInListValuesDTO", programParameterInListValuesDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (programParameterInListValuesDTOList == null ||
                programParameterInListValuesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < programParameterInListValuesDTOList.Count; i++)
            {
                ProgramParameterInListValuesDTO programParameterInListValuesDTO = programParameterInListValuesDTOList[i];
                if (programParameterInListValuesDTO.IsChanged)
                {
                    try
                    {
                        ProgramParameterInListValuesBL programParameterInListValuesBL = new ProgramParameterInListValuesBL(executionContext, programParameterInListValuesDTO);
                        programParameterInListValuesBL.Delete(sqlTransaction);
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
                        log.Error("Error occurred while saving ProgramParameterInListValuesDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("ProgramParameterInListValuesDTO", programParameterInListValuesDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Gets the ProgramParameterInListValuesDTO List for programParameterValueIdList 
        /// </summary>
        /// <param name="programParameterValueIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProgramParameterInListValuesDTO</returns>
        public List<ProgramParameterInListValuesDTO> GetProgramParameterInListValuesDTOListOfValues(List<int> programParameterValueIdList,
                                                bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programParameterValueIdList, activeRecords, sqlTransaction);
            ProgramParameterInListValuesDataHandler programParameterInListValuesDataHandler = new ProgramParameterInListValuesDataHandler(sqlTransaction);
            List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = programParameterInListValuesDataHandler.GetProgramParameterInListValuesDTOListOfValues(programParameterValueIdList, activeRecords);
            log.LogMethodExit(programParameterInListValuesDTOList);
            return programParameterInListValuesDTOList;
        }



        /// <summary>
        /// Gets the ProgramParameterInListValuesDTO List for programParameterValueIdList 
        /// </summary>
        /// <param name="programParameterValueIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProgramParameterInListValuesDTO</returns>
        public List<ProgramParameterInListValuesDTO> GetProgramParameterInListValuesDTOList(List<KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string>> searchParameters, 
                                                                                SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProgramParameterInListValuesDataHandler programParameterInListValuesDataHandler = new ProgramParameterInListValuesDataHandler(sqlTransaction);
            List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = programParameterInListValuesDataHandler.GetAllProgramParameterInListValuesDTOList(searchParameters);
            log.LogMethodExit(programParameterInListValuesDTOList);
            return programParameterInListValuesDTOList;
        }
    }
}
