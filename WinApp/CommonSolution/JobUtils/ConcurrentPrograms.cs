
/********************************************************************************************
 * Project Name - Concurrent Program
 * Description  - Bussiness logic of the Concurrent Program class
 * 
 **************
 **Version Log
 **************
 *Version     Date          Modified By      Remarks          
 *********************************************************************************************
 *1.00        18-Feb-2016   Amaresh          Created 
 *2.70.2      24-Jul-2019   Dakshakh raj     Modified : Save() method Insert/Update method returns DTO.
 *2.90        26-May-2020   Mushahid Faizan  Modified : 3 tier changes for Rest API.
 *2.120.1     08-Jun-2021   Deeksha          Modified as part of AWS Concurrent Programs enhancements
 ********************************************************************************************/

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Semnox.Core.Utilities;
using Semnox.Parafait.Languages;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// Concurrent Program 
    /// </summary>

    public class ConcurrentPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ExecutionContext executionContext;
        protected ConcurrentProgramsDTO concurrentProgramsDTO;
        protected string logFileName;
        protected DatabaseConnectorDTO dbConnectorDTO;
        protected Utilities utilities;
        protected ConcurrentRequestsDTO concurrentRequestsDTO;


        /// <summary>
        /// Default constructor of ConcurrentPrograms class
        /// </summary>
        private ConcurrentPrograms(ExecutionContext executionContext)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }


        /// <summary>
        /// Creates concurrentPrograms object using the ConcurrentProgramsDTO
        /// </summary>
        /// <param name="concurrentPrograms">ConcurrentProgramsDTO object</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentPrograms(ExecutionContext executionContext, ConcurrentProgramsDTO concurrentProgramsDTO, ConcurrentRequestsDTO concurrentRequestsDTO,
                                    string logFileName, DatabaseConnectorDTO dbConnectorDTO, Utilities utilities)
            : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO, utilities);
            this.concurrentProgramsDTO = concurrentProgramsDTO;
            this.logFileName = logFileName;
            this.dbConnectorDTO = dbConnectorDTO;
            this.utilities = utilities;
            this.concurrentRequestsDTO = concurrentRequestsDTO;
            log.LogMethodExit();
        }

        /// <summary>
        /// Constructor with the Program id as the parameter
        /// Would fetch the ConcurrentProgramsDTO object from the database based on the program id passed. 
        /// </summary>
        /// <param name="programId">Program id </param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentPrograms(ExecutionContext executionContext, int programId, bool loadChildRecords = false,
                                        bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
            : this(executionContext)
        {
            log.LogMethodEntry(programId, sqlTransaction);
            ConcurrentProgramsDataHandler concurrentProgramsDataHandler = new ConcurrentProgramsDataHandler(sqlTransaction);
            concurrentProgramsDTO = concurrentProgramsDataHandler.GetConcurrentPrograms(programId);
            if (concurrentProgramsDTO == null)
            {
                string message = MessageContainerList.GetMessage(executionContext, 2196, "ConcurrentPrograms", programId);
                log.LogMethodExit(null, "Throwing Exception - " + message);
                throw new EntityNotFoundException(message);
            }
            if (loadChildRecords)
            {
                Build(activeChildRecords,sqlTransaction);
            }
            log.LogMethodExit();
        }

        private void Build(bool activeChildRecords = true,SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(activeChildRecords, sqlTransaction);

            ConcurrentProgramScheduleList concurrentProgramScheduleList = new ConcurrentProgramScheduleList(executionContext);
            List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> psearchParameters = new List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>>();
            psearchParameters.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID, concurrentProgramsDTO.ProgramId.ToString()));
            psearchParameters.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.ACTIVE, "1"));
            }
            concurrentProgramsDTO.ConcurrentProgramSchedulesDTOList = concurrentProgramScheduleList.GetAllConcurrentProgramSchedule(psearchParameters, sqlTransaction);

            // Child  : ConcurrentProgramParameters DTO

            ConcurrentProgramParametersListBL concurrentProgramParametersListBL = new ConcurrentProgramParametersListBL(executionContext);
            List<KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>> paramterSearchParameters = new List<KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>>();
            paramterSearchParameters.Add(new KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>(ConcurrentProgramParametersDTO.SearchByParameters.CONCURRENT_PROGRAM_PARAMETER_ID, concurrentProgramsDTO.ProgramId.ToString()));
            paramterSearchParameters.Add(new KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>(ConcurrentProgramParametersDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            if (activeChildRecords)
            {
                paramterSearchParameters.Add(new KeyValuePair<ConcurrentProgramParametersDTO.SearchByParameters, string>(ConcurrentProgramParametersDTO.SearchByParameters.IS_ACTIVE, "1"));
            }
            
            concurrentProgramsDTO.ConcurrentProgramParametersDTOList = concurrentProgramParametersListBL.GetConcurrentProgramParametersDTOList(paramterSearchParameters,  sqlTransaction);

        }
    

        /// <summary>
        /// Saves the ConcurrentPrograms 
        /// Checks if the programId is not less than or equal to 0
        /// If it is less than or equal to 0, then inserts
        /// else updates
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            if (concurrentProgramsDTO.IsChangedRecursive == false &&
                 concurrentProgramsDTO.ProgramId > -1)
            {
                log.LogMethodExit(null, "Nothing to save.");
                return;
            }
            List<ValidationError> validationErrors = Validate();
            if (validationErrors.Any())
            {
                string message = MessageContainerList.GetMessage(executionContext, "Validation Error");
                log.LogMethodExit(null, "Throwing Exception - " + message + Environment.NewLine + string.Join(Environment.NewLine, validationErrors.Select(x => x.Message)));
                throw new ValidationException(message, validationErrors);
            }

            ConcurrentProgramsDataHandler concurrentProgramsDataHandler = new ConcurrentProgramsDataHandler(sqlTransaction);

            if (concurrentProgramsDTO.ProgramId <= 0)
            {
                concurrentProgramsDTO = concurrentProgramsDataHandler.InsertConcurrentPrograms(concurrentProgramsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentProgramsDTO.AcceptChanges();
            }
            else if (concurrentProgramsDTO.IsChanged)
            {
                concurrentProgramsDTO = concurrentProgramsDataHandler.UpdateConcurrentProgram(concurrentProgramsDTO, executionContext.GetUserId(), executionContext.GetSiteId());
                concurrentProgramsDTO.AcceptChanges();
            }
            SaveConcurrentProgramsChild(sqlTransaction);
            log.LogMethodExit();
        }

        /// <summary>
        /// Saves the child records 
        /// </summary>
        /// <param name="sqlTransaction"></param>
        private void SaveConcurrentProgramsChild(SqlTransaction sqlTransaction)
        {

            // for Child Records : ConcurrentProgramArgumentsDTO
            //if (concurrentProgramsDTO.ConcurrentProgramArgumentsDTOList != null && concurrentProgramsDTO.ConcurrentProgramArgumentsDTOList.Any())
            //{
            //    List<ConcurrentProgramArgumentsDTO> updatedConcurrentProgramArgumentsDTOList = new List<ConcurrentProgramArgumentsDTO>();
            //    foreach (ConcurrentProgramArgumentsDTO concurrentProgramArgumentsDTO in concurrentProgramsDTO.ConcurrentProgramArgumentsDTOList)
            //    {
            //        if (concurrentProgramArgumentsDTO.ProgramId != concurrentProgramsDTO.ProgramId)
            //        {
            //            concurrentProgramArgumentsDTO.ProgramId = concurrentProgramsDTO.ProgramId;
            //        }
            //        if (concurrentProgramArgumentsDTO.IsChanged)
            //        {
            //            updatedConcurrentProgramArgumentsDTOList.Add(concurrentProgramArgumentsDTO);
            //        }
            //    }
            //    if (updatedConcurrentProgramArgumentsDTOList.Any())
            //    {
            //        ConcurrentProgramArgumentList concurrentProgramArgumentList = new ConcurrentProgramArgumentList(executionContext, updatedConcurrentProgramArgumentsDTOList);
            //        concurrentProgramArgumentList.Save(sqlTransaction);
            //    }
            //}

            // for Child Records : ConcurrentProgramSchedulesDTO
            if (concurrentProgramsDTO.ConcurrentProgramSchedulesDTOList != null && concurrentProgramsDTO.ConcurrentProgramSchedulesDTOList.Any())
            {
                List<ConcurrentProgramSchedulesDTO> updatedConcurrentProgramSchedulesDTOList = new List<ConcurrentProgramSchedulesDTO>();
                foreach (ConcurrentProgramSchedulesDTO concurrentProgramSchedulesDTO in concurrentProgramsDTO.ConcurrentProgramSchedulesDTOList)
                {
                    if (concurrentProgramSchedulesDTO.ProgramId != concurrentProgramsDTO.ProgramId)
                    {
                        concurrentProgramSchedulesDTO.ProgramId = concurrentProgramsDTO.ProgramId;
                    }
                    if (concurrentProgramSchedulesDTO.IsChanged)
                    {
                        updatedConcurrentProgramSchedulesDTOList.Add(concurrentProgramSchedulesDTO);
                    }
                }
                if (updatedConcurrentProgramSchedulesDTOList.Any())
                {
                    ConcurrentProgramScheduleList concurrentProgramScheduleList = new ConcurrentProgramScheduleList(executionContext, updatedConcurrentProgramSchedulesDTOList);
                    concurrentProgramScheduleList.Save(sqlTransaction);
                }
            }

            //// for Child Records : ConcurrentRequestDetailsDTO
            //if (concurrentProgramsDTO.ConcurrentRequestDetailsDTOList != null && concurrentProgramsDTO.ConcurrentRequestDetailsDTOList.Any())
            //{
            //    List<ConcurrentRequestDetailsDTO> updatedConcurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();
            //    foreach (ConcurrentRequestDetailsDTO concurrentRequestDetailsDTO in concurrentProgramsDTO.ConcurrentRequestDetailsDTOList)
            //    {
            //        if (concurrentRequestDetailsDTO.ConcurrentProgramId != concurrentProgramsDTO.ProgramId)
            //        {
            //            concurrentRequestDetailsDTO.ConcurrentProgramId = concurrentProgramsDTO.ProgramId;
            //        }
            //        if (concurrentRequestDetailsDTO.IsChanged)
            //        {
            //            updatedConcurrentRequestDetailsDTOList.Add(concurrentRequestDetailsDTO);
            //        }
            //    }
            //    if (updatedConcurrentRequestDetailsDTOList.Any())
            //    {
            //        ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(executionContext, updatedConcurrentRequestDetailsDTOList);
            //        concurrentRequestDetailsListBL.Save(sqlTransaction);
            //    }
            //}

            //// for Child Records : ConcurrentRequestsDTO
            //if (concurrentProgramsDTO.ConcurrentProgramSchedulesDTOList != null && concurrentProgramsDTO.ConcurrentProgramSchedulesDTOList.Any())
            //{
            //    List<ConcurrentRequestsDTO> updatedConcurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();
            //    foreach (ConcurrentRequestsDTO concurrentRequestsDTO in concurrentProgramsDTO.ConcurrentRequestsDTOList)
            //    {
            //        if (concurrentRequestsDTO.ProgramId != concurrentProgramsDTO.ProgramId)
            //        {
            //            concurrentRequestsDTO.ProgramId = concurrentProgramsDTO.ProgramId;
            //        }
            //        if (concurrentRequestsDTO.IsChanged)
            //        {
            //            updatedConcurrentRequestsDTOList.Add(concurrentRequestsDTO);
            //        }
            //    }
            //    if (updatedConcurrentRequestsDTOList.Any())
            //    {
            //        ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList(executionContext, updatedConcurrentRequestsDTOList);
            //        concurrentRequestList.Save(sqlTransaction);
            //    }
            //}
            // for Child Records : ConcurrentProgramParametersDTO
            if (concurrentProgramsDTO.ConcurrentProgramParametersDTOList!= null && concurrentProgramsDTO.ConcurrentProgramParametersDTOList.Any())
            {
                List<ConcurrentProgramParametersDTO> updatedConcurrentProgramParametersDTO = new List<ConcurrentProgramParametersDTO>();
                foreach (ConcurrentProgramParametersDTO concurrentProgramParametersDTO in concurrentProgramsDTO.ConcurrentProgramParametersDTOList)
                {
                    if (concurrentProgramParametersDTO.ProgramId != concurrentProgramsDTO.ProgramId)
                    {
                        concurrentProgramParametersDTO.ProgramId = concurrentProgramsDTO.ProgramId;
                    }
                    if (concurrentProgramParametersDTO.IsChanged)
                    {
                        updatedConcurrentProgramParametersDTO.Add(concurrentProgramParametersDTO);
                    }
                }
                if (updatedConcurrentProgramParametersDTO.Any())
                {
                    ConcurrentProgramParametersListBL concurrentProgramParametersListBL = new ConcurrentProgramParametersListBL(executionContext, updatedConcurrentProgramParametersDTO);
                    concurrentProgramParametersListBL.Save(sqlTransaction);
                }
            }
        }
        public List<ValidationError> Validate(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            List<ValidationError> validationErrorList = new List<ValidationError>();
            string errorMessage = string.Empty;
            if (concurrentProgramsDTO == null)
            {
                //Validation to be implemented.
            }

            //if (concurrentProgramsDTO.ConcurrentProgramArgumentsDTOList != null && concurrentProgramsDTO.ConcurrentProgramArgumentsDTOList.Any())
            //{
            //    foreach (var concurrentProgramArgumentsDTO in concurrentProgramsDTO.ConcurrentProgramArgumentsDTOList)
            //    {
            //        if (concurrentProgramArgumentsDTO.IsChanged)
            //        {
            //            ConcurrentProgramArguments concurrentProgramArguments = new ConcurrentProgramArguments(executionContext, concurrentProgramArgumentsDTO);
            //            validationErrorList.AddRange(concurrentProgramArguments.Validate(sqlTransaction)); //calls child validation method.
            //        }
            //    }
            //}
            log.LogMethodExit(validationErrorList);
            return validationErrorList;
        }
        /// <summary>
        /// Gets the DTO
        /// </summary>
        public ConcurrentProgramsDTO GetconcurrentProgramsDTO { get { return concurrentProgramsDTO; } }

        public virtual void LaunchRequests()
        {
            log.LogMethodEntry();
            log.LogMethodExit();
        }
    }


    /// <summary>
    /// Manages the list of Concurrent Program
    /// </summary>
    public class ConcurrentProgramList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private List<ConcurrentProgramsDTO> concurrentProgramsDTOList = new List<ConcurrentProgramsDTO>();
        private ExecutionContext executionContext;
        private const string DBQUERY = "QUERY";
        private const string STATIC_LIST = "STATIC_LIST";

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ConcurrentProgramList(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="concurrentProgramsDTOList"></param>
        /// <param name="executionContext"></param>
        public ConcurrentProgramList(List<ConcurrentProgramsDTO> concurrentProgramsDTOList, ExecutionContext executionContext) : this(executionContext)
        {
            log.LogMethodEntry(executionContext, concurrentProgramsDTOList);
            this.concurrentProgramsDTOList = concurrentProgramsDTOList;
            log.LogMethodExit();
        }


        /// <summary>
        ///  Returns the Concurrent Program list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentProgramsDTO> GetAllConcurrentPrograms(List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParameters, bool loadChildRecords = false, bool loadActiveRecords = false,
                                          SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ConcurrentProgramsDataHandler concurrentProgramsDataHandler = new ConcurrentProgramsDataHandler(sqlTransaction);
            List<ConcurrentProgramsDTO> concurrentProgramsDTOList = concurrentProgramsDataHandler.GetConcurrentProgramsList(searchParameters);
            if (concurrentProgramsDTOList != null && concurrentProgramsDTOList.Any() && loadChildRecords)
            {
                Build(concurrentProgramsDTOList, loadActiveRecords, sqlTransaction);
            }
            log.LogMethodExit(concurrentProgramsDTOList);
            return concurrentProgramsDTOList;
        }
        private void Build(List<ConcurrentProgramsDTO> concurrentProgramsDTOList, bool activeChildRecords = true, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(concurrentProgramsDTOList, activeChildRecords, sqlTransaction);
            Dictionary<int, ConcurrentProgramsDTO> programIdDictionary = new Dictionary<int, ConcurrentProgramsDTO>();
            StringBuilder sb = new StringBuilder("");
            string programIdList;
            for (int i = 0; i < concurrentProgramsDTOList.Count; i++)
            {
                if (concurrentProgramsDTOList[i].ProgramId == -1 ||
                    programIdDictionary.ContainsKey(concurrentProgramsDTOList[i].ProgramId))
                {
                    continue;
                }
                if (i != 0)
                {
                    sb.Append(",");
                }
                sb.Append(concurrentProgramsDTOList[i].ProgramId.ToString());
                programIdDictionary.Add(concurrentProgramsDTOList[i].ProgramId, concurrentProgramsDTOList[i]);
            }
            programIdList = sb.ToString();

            ConcurrentProgramScheduleList concurrentProgramScheduleList = new ConcurrentProgramScheduleList(executionContext);
            List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>> psearchParameters = new List<KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>>();
            psearchParameters.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.PROGRAM_ID_LIST, programIdList.ToString()));
            psearchParameters.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            if (activeChildRecords)
            {
                psearchParameters.Add(new KeyValuePair<ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters, string>(ConcurrentProgramSchedulesDTO.SearchByProgramSceduleParameters.ACTIVE, "1"));
            }
            List<ConcurrentProgramSchedulesDTO> concurrentProgramSchedulesDTOList = concurrentProgramScheduleList.GetAllConcurrentProgramSchedule(psearchParameters, sqlTransaction);
            if (concurrentProgramSchedulesDTOList != null && concurrentProgramSchedulesDTOList.Any())
            {
                log.LogVariableState("concurrentProgramSchedulesDTOList", concurrentProgramSchedulesDTOList);
                foreach (var concurrentProgramSchedulesDTO in concurrentProgramSchedulesDTOList)
                {
                    if (programIdDictionary.ContainsKey(concurrentProgramSchedulesDTO.ProgramId))
                    {
                        if (programIdDictionary[concurrentProgramSchedulesDTO.ProgramId].ConcurrentProgramSchedulesDTOList == null)
                        {
                            programIdDictionary[concurrentProgramSchedulesDTO.ProgramId].ConcurrentProgramSchedulesDTOList = new List<ConcurrentProgramSchedulesDTO>();
                        }
                        programIdDictionary[concurrentProgramSchedulesDTO.ProgramId].ConcurrentProgramSchedulesDTOList.Add(concurrentProgramSchedulesDTO);
                    }
                }
            }

            // Child 3 : ConcurrentRequestDetailsDTO

            //ConcurrentRequestDetailsListBL concurrentRequestDetailsListBL = new ConcurrentRequestDetailsListBL(executionContext);
            //List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>> rsearchParameters = new List<KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>>();
            //rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.CONCURRENT_PROGRAM_ID_LIST, programIdList.ToString()));
            //rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            //if (activeChildRecords)
            //{
            //    rsearchParameters.Add(new KeyValuePair<ConcurrentRequestDetailsDTO.SearchByParameters, string>(ConcurrentRequestDetailsDTO.SearchByParameters.IS_ACTIVE, "1"));
            //}
            //List<ConcurrentRequestDetailsDTO> concurrentRequestDetailsDTOList = concurrentRequestDetailsListBL.GetConcurrentRequestDetailsDTOList(rsearchParameters, sqlTransaction);
            //if (concurrentRequestDetailsDTOList != null && concurrentRequestDetailsDTOList.Any())
            //{
            //    log.LogVariableState("concurrentRequestDetailsDTOList", concurrentRequestDetailsDTOList);
            //    foreach (var concurrentRequestDetailsDTO in concurrentRequestDetailsDTOList)
            //    {
            //        if (programIdDictionary.ContainsKey(concurrentRequestDetailsDTO.ConcurrentProgramId))
            //        {
            //            if (programIdDictionary[concurrentRequestDetailsDTO.ConcurrentProgramId].ConcurrentRequestDetailsDTOList == null)
            //            {
            //                programIdDictionary[concurrentRequestDetailsDTO.ConcurrentProgramId].ConcurrentRequestDetailsDTOList = new List<ConcurrentRequestDetailsDTO>();
            //            }
            //            programIdDictionary[concurrentRequestDetailsDTO.ConcurrentProgramId].ConcurrentRequestDetailsDTOList.Add(concurrentRequestDetailsDTO);
            //        }
            //    }
            //}

            //// Child 4 : ConcurrentRequestDTO

            //ConcurrentRequestList concurrentRequestList = new ConcurrentRequestList(executionContext);
            //List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>> requestSearchParameters = new List<KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>>();
            //requestSearchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.PROGRAM_ID_LIST, programIdList.ToString()));
            //requestSearchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.SITE_ID, Convert.ToString(executionContext.GetSiteId())));
            //if (activeChildRecords)
            //{
            //    requestSearchParameters.Add(new KeyValuePair<ConcurrentRequestsDTO.SearchByRequestParameters, string>(ConcurrentRequestsDTO.SearchByRequestParameters.IS_ACTIVE, "1"));
            //}
            //List<ConcurrentRequestsDTO> concurrentRequestsDTOList = concurrentRequestList.GetAllConcurrentRequests(requestSearchParameters, sqlTransaction);
            //if (concurrentRequestsDTOList != null && concurrentRequestsDTOList.Any())
            //{
            //    log.LogVariableState("concurrentRequestsDTOList", concurrentRequestsDTOList);
            //    foreach (var concurrentRequestsDTO in concurrentRequestsDTOList)
            //    {
            //        if (programIdDictionary.ContainsKey(concurrentRequestsDTO.ProgramId))
            //        {
            //            if (programIdDictionary[concurrentRequestsDTO.ProgramId].ConcurrentRequestsDTOList == null)
            //            {
            //                programIdDictionary[concurrentRequestsDTO.ProgramId].ConcurrentRequestsDTOList = new List<ConcurrentRequestsDTO>();
            //            }
            //            programIdDictionary[concurrentRequestsDTO.ProgramId].ConcurrentRequestsDTOList.Add(concurrentRequestsDTO);
            //        }
            //    }
            //}

            Dictionary<int, ConcurrentProgramsDTO> concurrentProgramDTOIdMap = new Dictionary<int, ConcurrentProgramsDTO>();
            List<int> concurrentProgramIdList = new List<int>();
            for (int i = 0; i < concurrentProgramsDTOList.Count; i++)
            {
                if (concurrentProgramDTOIdMap.ContainsKey(concurrentProgramsDTOList[i].ProgramId))
                {
                    continue;
                }
                concurrentProgramDTOIdMap.Add(concurrentProgramsDTOList[i].ProgramId, concurrentProgramsDTOList[i]);
                concurrentProgramIdList.Add(concurrentProgramsDTOList[i].ProgramId);
            }
            ConcurrentProgramParametersDataHandler concurrentProgramParametersDataHandler = new ConcurrentProgramParametersDataHandler(sqlTransaction);
            ConcurrentProgramParametersListBL programParameterInListValuesBL = new ConcurrentProgramParametersListBL(executionContext);
            List<ConcurrentProgramParametersDTO> concurrentProgramParametersDTOList = programParameterInListValuesBL.GetProgramsParameterDTOListOfPrograms(concurrentProgramIdList, activeChildRecords, sqlTransaction);
            if (concurrentProgramParametersDTOList != null && concurrentProgramParametersDTOList.Count > 0)
            {
                foreach (ConcurrentProgramParametersDTO programDTO in concurrentProgramParametersDTOList)
                {
                    if (programDTO.DataSourceType == DBQUERY)
                    {
                        programDTO.ProgramValueList = concurrentProgramParametersDataHandler.GetProgramParameterValuesOfDBQuery(programDTO.DataSource);
                    }
                    else if (programDTO.DataSourceType == STATIC_LIST)
                    {
                        List<string> paramNames = programDTO.DataSource.Split(',').ToList();
                        foreach(string paramName in paramNames)
                        {
                            programDTO.ProgramValueList.Add(new KeyValuePair<string, string>(paramName, paramName));
                        }
                    }
                    programDTO.AcceptChanges();
                }
                for (int i = 0; i < concurrentProgramParametersDTOList.Count; i++)
                {
                    if (concurrentProgramDTOIdMap.ContainsKey(concurrentProgramParametersDTOList[i].ProgramId) == false)
                    {
                        continue;
                    }
                    ConcurrentProgramsDTO concurrentProgramsDTO = concurrentProgramDTOIdMap[concurrentProgramParametersDTOList[i].ProgramId];
                    if (concurrentProgramsDTO.ConcurrentProgramParametersDTOList == null)
                    {
                        concurrentProgramsDTO.ConcurrentProgramParametersDTOList = new List<ConcurrentProgramParametersDTO>();
                    }
                    concurrentProgramsDTO.ConcurrentProgramParametersDTOList.Add(concurrentProgramParametersDTOList[i]);
                }
            }

            log.LogMethodExit();
        }

        /// <summary>
        ///  Returns the ConcurrentProgramJobStatusDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentProgramJobStatusDTO> GetAllConcurrentProgramStatusList(List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ConcurrentProgramJobStatusDataHandler concurrentProgramJobStatusDataHandler = new ConcurrentProgramJobStatusDataHandler(sqlTransaction);
            List<ConcurrentProgramJobStatusDTO> concurrentProgramJobStatusDTOList = concurrentProgramJobStatusDataHandler.GetConcurrentProgramJobStatusList(searchParameters);
            log.LogMethodExit(concurrentProgramJobStatusDTOList);
            return concurrentProgramJobStatusDTOList;
        }

        /// <summary>
        /// Save or update the Concurrent Programs
        /// </summary>
        public void Save(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry();
            if (concurrentProgramsDTOList == null ||
              concurrentProgramsDTOList.Any() == false)
            {
                log.LogMethodExit(null, "List is empty");
                return;
            }
            for (int i = 0; i < concurrentProgramsDTOList.Count; i++)
            {
                var concurrentProgramsDTO = concurrentProgramsDTOList[i];
                if (concurrentProgramsDTO.IsChangedRecursive == false)
                {
                    continue;
                }
                try
                {
                    ConcurrentPrograms concurrentPrograms = new ConcurrentPrograms(executionContext, concurrentProgramsDTO,
                        null,null,null,null);
                    concurrentPrograms.Save(sqlTransaction);
                }
                catch (Exception ex)
                {
                    log.Error("Error occurred while saving concurrentProgramsDTO.", ex);
                    log.LogVariableState("Record Index ", i);
                    log.LogVariableState("concurrentProgramsDTO", concurrentProgramsDTO);
                    throw;
                }
            }
        }
    }

    /// <summary>
    /// Manages the list of Concurrent Program
    /// </summary>
    public class ConcurrentProgramJobStatusList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ExecutionContext executionContext;
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="executionContext"></param>
        public ConcurrentProgramJobStatusList(ExecutionContext executionContext = null)
        {
            log.LogMethodEntry(executionContext);
            this.executionContext = executionContext;
            log.LogMethodExit();
        }

        /// <summary>
        /// Returns the ConcurrentProgramJobStatusDTO list
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <param name="sqlTransaction">sqlTransaction</param>
        /// <returns></returns>
        public List<ConcurrentProgramJobStatusDTO> GetAllConcurrentProgramStatusList(List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>> searchParameters, SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(searchParameters, sqlTransaction);
            ConcurrentProgramJobStatusDataHandler concurrentProgramJobStatusDataHandler = new ConcurrentProgramJobStatusDataHandler(sqlTransaction);
            List<ConcurrentProgramJobStatusDTO> concurrentProgramJobStatusDTOList = concurrentProgramJobStatusDataHandler.GetConcurrentProgramJobStatusList(searchParameters);
            log.LogMethodExit(concurrentProgramJobStatusDTOList);
            return concurrentProgramJobStatusDTOList;
        }
    }
}

