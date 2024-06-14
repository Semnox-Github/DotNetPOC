/********************************************************************************************
 * Project Name - concurrent program Schedule
 * Description  - buisness Logic of Concurrent Program Schedule class
 * 
 **************
 **Version Log
 **************
 *Version     Date           Modified By      Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016    Amaresh          Created 
 *2.70.2      24-Jul-2019    Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90.0      24-Jun-2020    Faizan           Modified : REST API phase -2 changes /private constructor
 *2.120.1     09-Jun-2021    Deeksha          Modified: As part of AWS concurrent program enhancements
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
    /// Concurrent Program Schedules, Schedules the programs to be executed
    /// </summary>
    public class ConcurrentProgramSchedules
    {
        private ConcurrentProgramSchedulesDTO concurrentProgramsSchedulesDTO;
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;

        /// <summary>
        /// Default constructor of ConcurrentProgramSchedules class
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        private ConcurrentProgramSchedules(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
          
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Program id as the parameter
        /// Would fetch the ConcurrentProgramSchedulesDTO object from the database based on the program id. 
        /// </summary>
        /// <param name="programId">Program id </param>
        /// <param name="executionContext">executionContext</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramSchedules(int programId, ExecutionContext executionContext, SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(programId, executionContext, sqlTransaction);
            ConcurrentProgramSchedulesDataHandler concurrentProgramsSchedulesDataHandler = new ConcurrentProgramSchedulesDataHandler(sqlTransaction);
            this.concurrentProgramsSchedulesDTO = concurrentProgramsSchedulesDataHandler.GetConcurrentProgramsSchedules(programId);
            log.LogMethodExit(concurrentProgramsSchedulesDTO);
        }


        /// <summary>
        /// Constructor with the RecipeManufacturingHeader  id as the parameter
        /// Would fetch the RecipeManufacturingHeader object from the database based on the id passed. 
        /// </summary>
        /// <param name="executionContext">ExecutionContext</param>
        /// <param name="id">id -RecipeManufacturingHeader </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramSchedules(ExecutionContext executionContext, int scheduleId,
                                         bool loadChildRecords = false, bool activeChildRecords = false,
                                         SqlTransaction sqlTransaction = null)
           : this(executionContext)
        {
            log.LogMethodEntry(executionContext, scheduleId, loadChildRecords, activeChildRecords, sqlTransaction);
            ConcurrentProgramSchedulesDataHandler concurrentProgramSchedulesDataHandler = new ConcurrentProgramSchedulesDataHandler(sqlTransaction);
            this.concurrentProgramsSchedulesDTO = concurrentProgramSchedulesDataHandler.GetConProgramSchedule(scheduleId);
            if (loadChildRecords == false ||
                concurrentProgramsSchedulesDTO == null)
            {
                log.LogMethodExit();
                return;
            }
            ProgramParameterValueListBL programParameterValueListBL = new ProgramParameterValueListBL(executionContext);
            concurrentProgramsSchedulesDTO.ProgramParameterValueDTOList = programParameterValueListBL.GetProgramsScheduleDTOListOfPrograms(new List<int> { scheduleId }, activeChildRecords, sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Creates concurrentProgramsSchedules object using the ConcurrentProgramSchedulesDTO
        /// </summary>
        /// <param name="concurrentProgramsSchedules">ConcurrentProgramSchedulesDTO object</param>
        /// <param name="executionContext">executionContext</param>
        public ConcurrentProgramSchedules(ExecutionContext executionContext, ConcurrentProgramSchedulesDTO concurrentProgramsSchedules)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramsSchedules);
            this.executionContext = executionContext;
            this.concurrentProgramsSchedulesDTO = concurrentProgramsSchedules;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Concurrent Program Schedule DTO based on the program schedule ID
        /// </summary>
        /// <param name="programScheduleId">programScheduleId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public ConcurrentProgramSchedulesDTO GetConProgramSchedule(int programScheduleId, 
                    SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programScheduleId);
            ConcurrentProgramSchedulesDataHandler conProScdlDataHandler = new ConcurrentProgramSchedulesDataHandler(sqlTransaction);
            log.LogMethodExit();
            return conProScdlDataHandler.GetConProgramSchedule(programScheduleId);
        }

        

        /// <summary>
        /// Saves the ConcurrentProgramSchedules 
        /// Checks if the ProgramScheduleId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null, string connectionString = null)
        {
            log.LogMethodEntry(sqlTransaction, connectionString);
            ConcurrentProgramSchedulesDataHandler concurrentProgramsScheduleDataHandler = new ConcurrentProgramSchedulesDataHandler(connectionString);
            if (concurrentProgramsSchedulesDTO.IsChanged == false &&
            concurrentProgramsSchedulesDTO.ProgramScheduleId > -1)
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
            if (concurrentProgramsSchedulesDTO.ProgramScheduleId <= 0)
            {
                concurrentProgramsSchedulesDTO = concurrentProgramsScheduleDataHandler.InsertConcurrentProgramsSchedule(concurrentProgramsSchedulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentProgramsSchedulesDTO.AcceptChanges();
            }
            else
            {
                if (concurrentProgramsSchedulesDTO.IsChanged)
                {
                    concurrentProgramsSchedulesDTO = concurrentProgramsScheduleDataHandler.UpdateConcurrentProgramsSchedule(concurrentProgramsSchedulesDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                    concurrentProgramsSchedulesDTO.AcceptChanges();
                }
            }
            if (concurrentProgramsSchedulesDTO.ProgramParameterValueDTOList != null &&
                  concurrentProgramsSchedulesDTO.ProgramParameterValueDTOList.Count != 0)
            {
                foreach (ProgramParameterValueDTO programParameterValueDTO in concurrentProgramsSchedulesDTO.ProgramParameterValueDTOList)
                {
                    if (programParameterValueDTO.IsChanged)
                    {
                        programParameterValueDTO.ConcurrentProgramScheduleId = concurrentProgramsSchedulesDTO.ProgramScheduleId;
                    }
                }
                ProgramParameterValueListBL programParameterValueListBL = new ProgramParameterValueListBL(executionContext, concurrentProgramsSchedulesDTO.ProgramParameterValueDTOList);
                programParameterValueListBL.Save(sqlTransaction);
            }
        }
        /// <summary>
        /// Validate the concurrentProgramsSchedulesDTO
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
        public ConcurrentProgramSchedulesDTO ConcurrentProgramsSchedules { get { return concurrentProgramsSchedulesDTO; } }
    }

   /// <summary>
   /// Manages the list of Concurrent Programs Schedule
   /// </summary>
   public class ConcurrentProgramScheduleList
   {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        private List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList;

        /// <summary>
        /// Concurrent Program Schedule List
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        public ConcurrentProgramScheduleList(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Concurrent Program Schedule List
        /// </summary>
        /// <param name="executionContext">executionContext</param>
        /// <param name="concurrentProgramSchedulesDTOList">concurrentProgramSchedulesDTOList</param>
        public ConcurrentProgramScheduleList(ExecutionContext executionContext,
            List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramSchedulesDTOList);
            this.executionContext = executionContext;
            this.concurrentProgramSchedulesDTOList = concurrentProgramSchedulesDTOList;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the Concurrent Program Schedule list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentProgramSchedulesDTO> GetAllConcurrentProgramSchedule(List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> searchParameters,
                                                                                         string connectionString)
        {
            log.LogMethodEntry(searchParameters);
            ConcurrentProgramSchedulesDataHandler concurrentProgramsScheduleDataHandler = new ConcurrentProgramSchedulesDataHandler(connectionString);
            List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList = concurrentProgramsScheduleDataHandler.GetConcurrentProgramsSchedulesList(searchParameters);
            log.LogMethodExit(concurrentProgramSchedulesDTOList);
            return concurrentProgramSchedulesDTOList;
        }

        /// <summary>
        /// Returns the Concurrent Program Schedule list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentProgramSchedulesDTO> GetAllConcurrentProgramSchedule(List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> searchParameters, 
            SqlTransaction sqlTransaction = null, bool loadChildRecords = false, bool activeChildRecords = true)
        {
            log.LogMethodEntry(searchParameters);
            ConcurrentProgramSchedulesDataHandler concurrentProgramsScheduleDataHandler = new ConcurrentProgramSchedulesDataHandler(sqlTransaction);
            List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList = concurrentProgramsScheduleDataHandler.GetConcurrentProgramsSchedulesList(searchParameters);
            if (loadChildRecords == false ||
               concurrentProgramSchedulesDTOList == null ||
               concurrentProgramSchedulesDTOList.Count > 0 == false)
            {
                log.LogMethodExit(concurrentProgramSchedulesDTOList, "Child records are not loaded.");
                return concurrentProgramSchedulesDTOList;
            }
            BuildProgramParametersValueDTOList(concurrentProgramSchedulesDTOList, activeChildRecords, sqlTransaction);
            log.LogMethodExit(concurrentProgramSchedulesDTOList);
            return concurrentProgramSchedulesDTOList;
        }


        private void BuildProgramParametersValueDTOList(List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList, bool activeChildRecords,
                                                    SqlTransaction sqlTransaction)
        {
            log.LogMethodEntry(concurrentProgramSchedulesDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ConcurrentProgramSchedulesDTO> concurrentProgramScheduleDTOIdMap = new Dictionary<int, ConcurrentProgramSchedulesDTO>();
            List<int> ConcurrentProgramScheduleIdList = new List<int>();
            for (int i = 0; i < concurrentProgramSchedulesDTOList.Count; i++)
            {
                if (concurrentProgramScheduleDTOIdMap.ContainsKey(concurrentProgramSchedulesDTOList[i].ProgramScheduleId))
                {
                    continue;
                }
                concurrentProgramScheduleDTOIdMap.Add(concurrentProgramSchedulesDTOList[i].ProgramScheduleId, concurrentProgramSchedulesDTOList[i]);
                ConcurrentProgramScheduleIdList.Add(concurrentProgramSchedulesDTOList[i].ProgramScheduleId);
            }
            ProgramParameterValueListBL programParameterValueListBL = new ProgramParameterValueListBL(executionContext);
            List<ProgramParameterValueDTO> programParameterValueDTOList = programParameterValueListBL.GetProgramsScheduleDTOListOfPrograms(ConcurrentProgramScheduleIdList, activeChildRecords, sqlTransaction);
            if (programParameterValueDTOList != null && programParameterValueDTOList.Count > 0)
            {
                for (int i = 0; i < programParameterValueDTOList.Count; i++)
                {
                    if (concurrentProgramScheduleDTOIdMap.ContainsKey(programParameterValueDTOList[i].ParameterId) == false)
                    {
                        continue;
                    }
                    ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO = concurrentProgramScheduleDTOIdMap[programParameterValueDTOList[i].ConcurrentProgramScheduleId];
                    if (concurrentProgramSchedulesDTO.ProgramParameterValueDTOList == null)
                    {
                        concurrentProgramSchedulesDTO.ProgramParameterValueDTOList = new List<ProgramParameterValueDTO>();
                    }
                    concurrentProgramSchedulesDTO.ProgramParameterValueDTOList.Add(programParameterValueDTOList[i]);
                }
            }
            log.LogMethodExit();
        }
        /// <summary>
        /// Saves the concurrentProgramSchedulesDTO List
        /// Checks if the  id is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">SqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null,string connectionString = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentProgramSchedulesDTOList == null ||
                concurrentProgramSchedulesDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }

            for (int i = 0; i < concurrentProgramSchedulesDTOList.Count; i++)
            {
                var concurrentProgramSchedulesDTO = concurrentProgramSchedulesDTOList[i];
                if (concurrentProgramSchedulesDTO.IsChanged == false)
                {
                    continue;
                }
                try
                {
                    ConcurrentProgramSchedules concurrentProgramSchedules = new ConcurrentProgramSchedules(executionContext, concurrentProgramSchedulesDTO);
                    concurrentProgramSchedules.Save(sqlTransaction, connectionString);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving concurrentProgramSchedulesDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("concurrentProgramSchedulesDTO", concurrentProgramSchedulesDTO);
                    throw;
                }
            }
            log.LogMethodExit();
        }


        /// <summary>
        /// Returns the Concurrent Program Schedule DTO based on the program schedule ID
        /// </summary>
        /// <param name="programScheduleId">programScheduleId</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public ConcurrentProgramSchedulesDTO GetConProgramSchedule(int programScheduleId, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(programScheduleId);
            ConcurrentProgramSchedulesDataHandler conProScdlDataHandler = new ConcurrentProgramSchedulesDataHandler(sqlTransaction);
            log.LogMethodExit();
            return conProScdlDataHandler.GetConProgramSchedule(programScheduleId);
        }
    }
}
