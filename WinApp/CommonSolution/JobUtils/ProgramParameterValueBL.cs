/********************************************************************************************
 * Project Name - ProgramParameterValue BL
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

namespace Semnox.Parafait.JobUtils
{
    public class ProgramParameterValueBL
    {
        private ProgramParameterValueDTO programParameterValueDTO;
        private readonly ExecutionContext executionContext;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ProgramParameterValueBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Creates ProgramParameterValueBL object using the ProgramParameterValueDTO
        /// </summary>
        /// <param name="executionContext">execution context</param>
        /// <param name="ProgramParameterValueDTO">ProgramParameterValueDTO object</param>
        public ProgramParameterValueBL(ExecutionContext executionContext, ProgramParameterValueDTO programParameterValueDTO)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, programParameterValueDTO);
            this.programParameterValueDTO = programParameterValueDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the RecipeManufacturingHeader  id as the parameter
        /// Would fetch the RecipeManufacturingHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -RecipeManufacturingHeader </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ProgramParameterValueBL(ExecutionContext executionContext, int programParameterValueId,
                                         bool loadChildRecords = false, bool activeChildRecords = false,
                                         SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, programParameterValueId, loadChildRecords, activeChildRecords, sqlTransaction);
            ProgramParameterValueDataHandler programParameterValueDataHandler = new ProgramParameterValueDataHandler(sqlTransaction);
            this.programParameterValueDTO = programParameterValueDataHandler.GetProgramParameterValueDTO(programParameterValueId);
            if (loadChildRecords == false ||
                programParameterValueDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            ProgramParameterInListValuesListBL programParameterInListValuesListBL = new ProgramParameterInListValuesListBL(executionContext);
            programParameterValueDTO.ProgramParameterInListValuesDTOList = programParameterInListValuesListBL.GetProgramParameterInListValuesDTOListOfValues(new List<int> { programParameterValueId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }


        /// <summary>
        /// Saves the ProgramParameterValue
        /// ProgramParameterValue will be inserted if ProgramParameterValueId is less than or equal to
        /// zero else updates the records based on primary key
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            ProgramParameterValueDataHandler programParameterValueDataHandler = new ProgramParameterValueDataHandler(sqlTransaction);
            if (programParameterValueDTO.IsChanged == false
               && programParameterValueDTO.ProgramParameterValueId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                log.LogMethodExit(null, "Throwing Validation Exception" + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException("Validation Failed.", validationErrors);
            }
            if (programParameterValueDTO.ProgramParameterValueId < 0)
            {
                programParameterValueDTO = programParameterValueDataHandler.Insert(programParameterValueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                programParameterValueDTO.AcceptChanges();
            }
            else
            {
                if (programParameterValueDTO.IsChanged)
                {
                    programParameterValueDTO = programParameterValueDataHandler.Update(programParameterValueDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    programParameterValueDTO.AcceptChanges();
                }
            }
            if (programParameterValueDTO.ProgramParameterInListValuesDTOList != null &&
                 programParameterValueDTO.ProgramParameterInListValuesDTOList.Count != 0)
            {
                foreach (ProgramParameterInListValuesDTO programParameterInListValuesDTO in programParameterValueDTO.ProgramParameterInListValuesDTOList)
                {
                    if (programParameterInListValuesDTO.IsChanged)
                    {
                        programParameterInListValuesDTO.ProgramParameterValueId = programParameterValueDTO.ProgramParameterValueId;
                    }
                }
                ProgramParameterInListValuesListBL programParameterInListValuesListBL = new ProgramParameterInListValuesListBL(executionContext, programParameterValueDTO.ProgramParameterInListValuesDTOList);
                programParameterInListValuesListBL.Save(sqlTransaction);
            }
            log.LogMethodExit();
        }

        public void Delete(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            ProgramParameterValueDataHandler programParameterValueDataHandler = new ProgramParameterValueDataHandler(sqlTransaction);

            if (programParameterValueDTO.ProgramParameterInListValuesDTOList != null &&
                  programParameterValueDTO.ProgramParameterInListValuesDTOList.Count != 0)
            {
                ProgramParameterInListValuesListBL programParameterInListValuesListBL = new ProgramParameterInListValuesListBL(executionContext, programParameterValueDTO.ProgramParameterInListValuesDTOList);
                programParameterInListValuesListBL.Delete(sqlTransaction);
            }
            if (programParameterValueDTO.ProgramParameterValueId > 0 && programParameterValueDTO.IsChanged)
            {
                programParameterValueDataHandler.Delete(programParameterValueDTO.ProgramParameterValueId);
                programParameterValueDTO.AcceptChanges();
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Validates the ProgramParameterValueDTO 
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>validationErrorList</returns>
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            ProgramParameterInListValuesListBL programParameterInListValues = new ProgramParameterInListValuesListBL(executionContext);
            List<KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string>>();
            searchParameters.Add(new KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string>(ProgramParameterInListValuesDTO.SearchByParameters.SITE_ID, executionContext.GetSiteId().ToString()));
            searchParameters.Add(new KeyValuePair<ProgramParameterInListValuesDTO.SearchByParameters, string>(ProgramParameterInListValuesDTO.SearchByParameters.PROGRAM_PARAMETER_VALUE_ID, programParameterValueDTO.ProgramParameterValueId.ToString()));
            List<ProgramParameterInListValuesDTO> programParameterInListValueDTOList = programParameterInListValues.GetProgramParameterInListValuesDTOList(searchParameters);
            if (programParameterInListValueDTOList != null && programParameterInListValueDTOList.Any())
            {
                if (programParameterInListValueDTOList.Exists(x => x.InListValue == programParameterValueDTO.ProgramParameterInListValuesDTOList[0].InListValue))
                {
                    log.Debug("Duplicate update entries detail");
                    validationErrorList.Add(new ValidationError("MessagingClient", "ClientName", MessageContainerList.GetMessage(executionContext, 2608, MessageContainerList.GetMessage(executionContext, 
                                                    programParameterValueDTO.ProgramParameterInListValuesDTOList[0].InListValue))));
                }
            }
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }

        /// <summary>
        /// Gets the ProgramParameterValueDTO
        /// </summary>
        public ProgramParameterValueDTO ProgramParameterValueDTO
        {
            get
            {
                return programParameterValueDTO;
            }
        }
    }

    /// <summary>
    /// Manages the list of ProgramParameterValue
    /// </summary>
    public class ProgramParameterValueListBL
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ExecutionContext executionContext;
        private List<ProgramParameterValueDTO> programParameterValueDTOList = new List<ProgramParameterValueDTO>();

        /// <summary>
        /// Parameterized constructor of ProgramParameterValueListBL
        /// </summary>
        /// <param name="executionContext">executionContext object</param>
        public ProgramParameterValueListBL(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="programParameterValueDTOList">ProgramParameterValue DTO List as parameter </param>
        public ProgramParameterValueListBL(ExecutionContext executionContext,
                                               List<ProgramParameterValueDTO> programParameterValueDTOList)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, programParameterValueDTOList);
            this.executionContext = executionContext;
            this.programParameterValueDTOList = programParameterValueDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the Get the ProgramParameterValue DTO list based on the search parameter.
        /// </summary>
        /// <param name="searchParameters">search Parameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>The List of ProgramParameterValueDTO </returns>
        public List<ProgramParameterValueDTO> GetProgramParameterValueDTOList(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters,
                                                            SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ProgramParameterValueDataHandler programParameterValueDataHandler = new ProgramParameterValueDataHandler(sqlTransaction);
            List<ProgramParameterValueDTO> programParameterValueDTOList = programParameterValueDataHandler.GetProgramParameterValueDTOList(searchParameters);
            log.LogMethodExit(programParameterValueDTOList);
            return programParameterValueDTOList;
        }

        /// <summary>
        /// Saves the  list of ProgramParameterValue DTO.
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction object</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (programParameterValueDTOList == null ||
                programParameterValueDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < programParameterValueDTOList.Count; i++)
            {
                ProgramParameterValueDTO programParameterValueDTO = programParameterValueDTOList[i];
                if (programParameterValueDTO.IsChanged || programParameterValueDTO.ProgramParameterValueId < 0)
                {
                    try
                    {
                        ProgramParameterValueBL ProgramParameterValueBL = new ProgramParameterValueBL(executionContext, programParameterValueDTO);
                        ProgramParameterValueBL.Save(sqlTransaction);
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
                        log.Error("Error occurred while saving ProgramParameterValueDTO.", ex);
                        log.LogVariableState("Record Index ", i);
                        log.LogVariableState("ProgramParameterValueDTO", programParameterValueDTO);
                        throw;
                    }
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Delete the programParameterValueDTOList  
        /// This method is only used for Web Management Studio.
        /// </summary>
        public void Delete()
        {
            log.LogMethodEntry();
            if (programParameterValueDTOList != null && programParameterValueDTOList.Any())
            {
                foreach (ProgramParameterValueDTO programParameterValueDTO in programParameterValueDTOList)
                {
                    if (programParameterValueDTO.IsChanged || programParameterValueDTO.IsChangedRecursive)
                    {
                        using (ParafaitDBTransaction parafaitDBTrx = new ParafaitDBTransaction())
                        {
                            try
                            {
                                parafaitDBTrx.BeginTransaction();
                                ProgramParameterValueBL programParameterValueBL = new ProgramParameterValueBL(executionContext, programParameterValueDTO);
                                programParameterValueBL.Delete(parafaitDBTrx.SQLTrx);
                                parafaitDBTrx.EndTransaction();
                            }
                            catch (SqlException sqlEx)
                            {
                                log.Error(sqlEx);
                                log.LogMethodExit(null, "Throwing Validation Exception : " + sqlEx.Message);
                                if (sqlEx.Number == 547)
                                {
                                    throw new ValidationException(MessageContainerList.GetMessage(executionContext, 1869));
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
                    }
                }
            }
            log.LogMethodExit();
        }

        /// <summary>
        /// Gets the ProgramParameterValueDTO List for programIdList 
        /// </summary>
        /// <param name="programIdList">integer list parameter</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns>Returns List of ProgramParameterValueDTO</returns>
        public List<ProgramParameterValueDTO> GetProgramsScheduleDTOListOfPrograms(List<int> programScheduleIdList,
                                            bool activeRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programScheduleIdList, activeRecords, sqlTransaction);
            ProgramParameterValueDataHandler programParameterValueDataHandler = new ProgramParameterValueDataHandler(sqlTransaction);
            List<ProgramParameterValueDTO> programParameterValueDTOList = programParameterValueDataHandler.GetProgramsScheduleDTOListOfPrograms(programScheduleIdList, activeRecords);
            log.LogMethodExit(programParameterValueDTOList);
            return programParameterValueDTOList;
        }

        /// <summary>
        /// Returns the ProgramParameterValueDTO list
        /// </summary>
        public List<ProgramParameterValueDTO> GetAllProgramParameterValueDTOList(List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters,
                                       bool loadChildRecords, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, loadChildRecords, activeChildRecords, sqlTransaction);
            ProgramParameterValueDataHandler ProgramParameterValueDataHandler = new ProgramParameterValueDataHandler(sqlTransaction);
            List<ProgramParameterValueDTO> ProgramParameterValueDTOList = ProgramParameterValueDataHandler.GetProgramParameterValueDTOList(searchParameters);
            if (loadChildRecords == false ||
                ProgramParameterValueDTOList == null ||
                ProgramParameterValueDTOList.Count > 0 == false)
            {
                log.LogMethodExit(ProgramParameterValueDTOList, "Child records are not loaded.");
                return ProgramParameterValueDTOList;
            }
            BuildProgramParameterValueDTOListt(ProgramParameterValueDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(ProgramParameterValueDTOList);
            return ProgramParameterValueDTOList;
        }

        private void BuildProgramParameterValueDTOListt(List<ProgramParameterValueDTO> ProgramParameterValueDTOList, bool activeChildRecords,
                                                    SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(ProgramParameterValueDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ProgramParameterValueDTO> ProgramParameterValueDTOIdMap = new Dictionary<int, ProgramParameterValueDTO>();
            List<int> ProgramParameterValueIdList = new List<int>();
            for (int i = 0; i < ProgramParameterValueDTOList.Count; i++)
            {
                if (ProgramParameterValueDTOIdMap.ContainsKey(ProgramParameterValueDTOList[i].ProgramParameterValueId))
                {
                    continue;
                }
                ProgramParameterValueDTOIdMap.Add(ProgramParameterValueDTOList[i].ProgramParameterValueId, ProgramParameterValueDTOList[i]);
                ProgramParameterValueIdList.Add(ProgramParameterValueDTOList[i].ProgramParameterValueId);
            }
            ProgramParameterInListValuesListBL programParameterInListValuesBL = new ProgramParameterInListValuesListBL(executionContext);
            List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = programParameterInListValuesBL.GetProgramParameterInListValuesDTOListOfValues(ProgramParameterValueIdList, activeChildRecords, sqlTransaction);
            if (programParameterInListValuesDTOList != null && programParameterInListValuesDTOList.Count > 0)
            {
                for (int i = 0; i < programParameterInListValuesDTOList.Count; i++)
                {
                    if (ProgramParameterValueDTOIdMap.ContainsKey(programParameterInListValuesDTOList[i].ProgramParameterValueId) == false)
                    {
                        continue;
                    }
                    ProgramParameterValueDTO ProgramParameterValueDTO = ProgramParameterValueDTOIdMap[programParameterInListValuesDTOList[i].ProgramParameterValueId];
                    if (ProgramParameterValueDTO.ProgramParameterInListValuesDTOList == null)
                    {
                        ProgramParameterValueDTO.ProgramParameterInListValuesDTOList = new List<ProgramParameterInListValuesDTO>();
                    }
                    ProgramParameterValueDTO.ProgramParameterInListValuesDTOList.Add(programParameterInListValuesDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
    }
}

