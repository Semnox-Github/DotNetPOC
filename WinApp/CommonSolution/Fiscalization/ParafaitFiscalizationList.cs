/********************************************************************************************
* Project Name - Fiscalization
* Description  - Class for ParafaitFiscalizationList 
* 
**************
**Version Log
**************
*Version     Date              Modified By        Remarks          
*********************************************************************************************
*2.155.1       13-Aug-2023       Guru S A           Created for Chile fiscaliation
********************************************************************************************/
using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using Semnox.Core.Utilities;
using System.Linq;
using Semnox.Parafait.Languages;
using Semnox.Parafait.JobUtils;
using System.Globalization;
using System.Text;

namespace Semnox.Parafait.Fiscalization
{
    /// <summary>
    /// ParafaitFiscalizationList
    /// </summary>
    public class ParafaitFiscalizationList
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected ExecutionContext executionContext;
        protected Utilities utilities;
        protected const string REPRO_EXECUTABLE_NAME = "InvoiceJsonReprocessingProgram.Exe";
        protected const string REPRO_PROGRAME_NAME = "Invoice Json Reprocessing Program";
        protected const string PARAM_TRX_ID_LIST = "TransactionIdList";
        protected const string PARAM_FISCALIZATION_TYPE = "FiscalizationType";
        /// <summary>
        /// ParafaitFiscalizationList
        /// </summary> 
        public ParafaitFiscalizationList(ExecutionContext executionContext, Utilities utilities)
        {
            log.LogMethodEntry(executionContext, "utilities");
            this.executionContext = executionContext;
            this.utilities = utilities;
            log.LogMethodExit();
        }
        /// <summary>
        /// GetPendingTransactions
        /// </summary>
        public virtual List<FiscalizationPendingTransactionDTO> GetPendingTransactions(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams,
            int pageNumber = 0, int pageSize = 50, SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParams, pageNumber, pageSize, sqlTrx);
            log.LogMethodExit("NotImplementedException");
            throw new NotImplementedException();
        }
        /// <summary>
        /// GetPendingTransactionCount
        /// </summary>
        public virtual int GetPendingTransactionCount(List<KeyValuePair<FiscalizationPendingTransactionDTO.SearchParameters, string>> searchParams,
            SqlTransaction sqlTrx = null)
        {
            log.LogMethodEntry(searchParams, sqlTrx);
            log.LogMethodExit("NotImplementedException");
            throw new NotImplementedException();
        }
        /// <summary>
        /// PostFiscalizationReprocessingRequest
        /// </summary>
        /// <param name="fiscalizationReprocessDTOList"></param>
        public virtual List<FiscalizationReprocessDTO> PostFiscalizationReprocessingRequest(List<FiscalizationReprocessDTO> fiscalizationReprocessDTOList)
        {
            log.LogMethodEntry(fiscalizationReprocessDTOList);

            if (fiscalizationReprocessDTOList != null && fiscalizationReprocessDTOList.Any())
            {
                List<string> fiscalizationNameList = fiscalizationReprocessDTOList.Select(f => f.Fiscalization).Distinct().ToList();
                if (fiscalizationNameList == null || fiscalizationNameList.Any() == false)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 165, "Fiscalization");
                    //&1 is mandatory
                    ValidationException ve = new ValidationException(msg);
                    throw ve;
                }
                if (fiscalizationNameList.Count > 1)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 5181);
                    // "Cannot process data for mulitple fiscalizations"
                    ValidationException ve = new ValidationException(msg);
                    throw ve;
                }
                List<int> trxIdList = fiscalizationReprocessDTOList.Select(f => f.TransactionId).Distinct().ToList();
                if (trxIdList == null || trxIdList.Any() == false)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 165, "Tranaction Id List");
                    //&1 is mandatory
                    ValidationException ve = new ValidationException(msg);
                    throw ve;
                }
                if (trxIdList.Count > 100)
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 5182, 100);
                    //"Cannot process more than &1 records"
                    ValidationException ve = new ValidationException(msg);
                    throw ve;
                }
                ParafaitFiscalizationNames parafaitFiscalizationNameValue = FiscalizationFactory.GetParafaitFiscalizationNames(fiscalizationNameList[0]);
                //Get programm id
                ConcurrentProgramList concurrentProgramList = new ConcurrentProgramList(executionContext);
                List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>> searchParam = new List<KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>>();
                searchParam.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.EXECUTABLE_NAME, REPRO_EXECUTABLE_NAME));
                searchParam.Add(new KeyValuePair<ConcurrentProgramsDTO.SearchByProgramsParameters, string>(ConcurrentProgramsDTO.SearchByProgramsParameters.SITE_ID, executionContext.SiteId.ToString()));
                List<ConcurrentProgramsDTO> programsDTOList = concurrentProgramList.GetAllConcurrentPrograms(searchParam, true);
                int programId = -1;
                int fiscalizationParamterId = -1;
                int trxIdListParameterId = -1;
                if (programsDTOList != null && programsDTOList.Any())
                {
                    if (programsDTOList.Count > 1)
                    {
                        string msg = MessageContainerList.GetMessage(executionContext, 5183, REPRO_PROGRAME_NAME);
                        //'More than one entry found for &1 '
                        ValidationException ve = new ValidationException(msg);
                        throw ve;
                    }
                    else
                    {
                        programId = programsDTOList[0].ProgramId;
                        if (programsDTOList[0].ConcurrentProgramParametersDTOList != null && programsDTOList[0].ConcurrentProgramParametersDTOList.Any())
                        {
                            ConcurrentProgramParametersDTO parameterOneDTO = programsDTOList[0].ConcurrentProgramParametersDTOList.Find(p => p.ParameterName == PARAM_FISCALIZATION_TYPE);
                            if (parameterOneDTO != null)
                            {
                                fiscalizationParamterId = parameterOneDTO.ConcurrentProgramParameterId;
                            }
                            else
                            {
                                string msg = MessageContainerList.GetMessage(executionContext, 3036) + " - " + MessageContainerList.GetMessage(executionContext, PARAM_FISCALIZATION_TYPE);
                                //Entry not found
                                ValidationException ve = new ValidationException(msg);
                                throw ve;
                            }
                            ConcurrentProgramParametersDTO parametersTwoDTO = programsDTOList[0].ConcurrentProgramParametersDTOList.Find(p => p.ParameterName == PARAM_TRX_ID_LIST);
                            if (parametersTwoDTO != null)
                            {
                                trxIdListParameterId = parametersTwoDTO.ConcurrentProgramParameterId;
                            }
                            else
                            {
                                string msg = MessageContainerList.GetMessage(executionContext, 3036) + " - " + MessageContainerList.GetMessage(executionContext, PARAM_TRX_ID_LIST);
                                //Entry not found
                                ValidationException ve = new ValidationException(msg);
                                throw ve;
                            }
                        }
                    }
                }
                else
                {
                    string msg = MessageContainerList.GetMessage(executionContext, 3036) + " - " + MessageContainerList.GetMessage(executionContext, REPRO_PROGRAME_NAME);
                    //Entry not found
                    ValidationException ve = new ValidationException(msg);
                    throw ve;
                }

                CheckForWIP(executionContext, trxIdListParameterId, trxIdList); //check for WIP request entry
                using (ParafaitDBTransaction dBTransaction = new ParafaitDBTransaction())
                {
                    try
                    {
                        dBTransaction.BeginTransaction();
                        //Create schedule entry
                        string runAtValue = GetRunAtValue();
                        ConcurrentProgramSchedulesDTO schedulesDTO = new ConcurrentProgramSchedulesDTO(programScheduleId: -1, programId: programId,
                            startDate: ServerDateTime.Now.Date, runAt: runAtValue, frequency: -1, endDate: ServerDateTime.Now.Date, isActive: true,
                                                  siteId: executionContext.SiteId, guid: null, synchStatus: false, lastUpdatedDate: DateTime.Now,
                                                  lastUpdatedUser: executionContext.UserId, lastExecutedOn: null, masterEntityId: -1,
                                                  createdBy: executionContext.UserId, creationDate: DateTime.Now);

                        ConcurrentProgramSchedules programSchedules = new ConcurrentProgramSchedules(executionContext, schedulesDTO);
                        programSchedules.Save(dBTransaction.SQLTrx);
                        schedulesDTO = programSchedules.ConcurrentProgramsSchedules;

                        //create parameter list
                        //FiscalizationType
                        ProgramParameterValueDTO parameterValueOneDTO = new ProgramParameterValueDTO(programParameterValueId: -1,
                            concurrentProgramScheduleId: schedulesDTO.ProgramScheduleId, programId: programId, parameterId: fiscalizationParamterId,
                            parameterValue: parafaitFiscalizationNameValue.ToString(), isActive: true,
                                          createdBy: executionContext.UserId, creationDate: DateTime.Now,
                                                   lastUpdatedBy: executionContext.UserId, lastUpdatedDate: DateTime.Now, guid: null,
                                                   synchStatus: false, siteId: executionContext.SiteId, masterEntityId: -1);

                        ProgramParameterValueBL valueBL = new ProgramParameterValueBL(executionContext, parameterValueOneDTO);
                        valueBL.Save(dBTransaction.SQLTrx);

                        //TrxIdList
                        string idString = GetIdString(executionContext, trxIdList);
                        ProgramParameterValueDTO parameterValueTwoDTO = new ProgramParameterValueDTO(programParameterValueId: -1,
                            concurrentProgramScheduleId: schedulesDTO.ProgramScheduleId, programId: programId, parameterId: trxIdListParameterId,
                            parameterValue: idString, isActive: true,
                                          createdBy: executionContext.UserId, creationDate: DateTime.Now,
                                                   lastUpdatedBy: executionContext.UserId, lastUpdatedDate: DateTime.Now, guid: null,
                                                   synchStatus: false, siteId: executionContext.SiteId, masterEntityId: -1);

                        valueBL = new ProgramParameterValueBL(executionContext, parameterValueTwoDTO);
                        valueBL.Save(dBTransaction.SQLTrx);

                        //create request entry
                        ConcurrentRequestsDTO requestsDTO = new ConcurrentRequestsDTO(requestId: -1, programId: programId, programScheduleId: schedulesDTO.ProgramScheduleId,
                            requestedTime: ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), requestedBy: executionContext.UserId,
                            startTime: ServerDateTime.Now.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture), actualStartTime: null,
                            endTime: null, phase: "Pending", status: "Normal", relaunchOnExit: false, argument1: null, argument2: null, argument3: null, argument4: null,
                            argument5: null, argument6: null, argument7: null, argument8: null, argument9: null, argument10: null, processId: -1, errorCount: 0, isActive: true);

                        ConcurrentRequests requestBL = new ConcurrentRequests(executionContext, requestsDTO);
                        requestBL.Save(dBTransaction.SQLTrx);
                        requestsDTO = requestBL.GetconcurrentRequests;
                        dBTransaction.EndTransaction();
                        foreach (var item in fiscalizationReprocessDTOList)
                        {
                            item.ConcurrentRequestId = requestsDTO.RequestId;
                        }
                    }
                    catch (Exception ex)
                    {
                        log.Error(ex);
                        try
                        {
                            if (dBTransaction != null)
                            {
                                dBTransaction.RollBack();
                            }
                        }
                        catch { };
                        string msg = ex.Message + (ex.InnerException != null ? ": " + ex.InnerException.Message : "");
                        ValidationException ve = new ValidationException(msg);
                        throw ve;
                    }
                }
            }
            else
            {
                string msg = MessageContainerList.GetMessage(executionContext, 1930);
                //Invalid Empty Parameters
                ValidationException ve = new ValidationException(msg);
                throw ve;
            }
            log.LogMethodExit();
            return fiscalizationReprocessDTOList;
        }

        protected string GetRunAtValue()
        {
            log.LogMethodEntry();
            string runAtValue = string.Empty;
            int hourVal = ServerDateTime.Now.Hour;
            runAtValue = (hourVal < 10 ? "0" : "") + hourVal + ":00";
            log.LogMethodExit(runAtValue);
            return runAtValue;
        }

        private void CheckForWIP(ExecutionContext executionContext, int trxIdListParameterId, List<int> trxIdList)
        {
            log.LogMethodEntry(executionContext, trxIdListParameterId, trxIdList);
            if (trxIdList != null && trxIdList.Any())
            {
                List<string> wipTrxIdList = new List<string>();
                StringBuilder stringBuilder = new StringBuilder();

                List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchParameters = new List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>>();
                searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PROGRAM_EXECUTABLE_NAME, REPRO_EXECUTABLE_NAME));
                searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PARAMETER_NAME, PARAM_TRX_ID_LIST));
                searchParameters.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PROGRAM_REQUEST_IS_IN_WIP, REPRO_EXECUTABLE_NAME));
                ProgramParameterValueListBL parameterList = new ProgramParameterValueListBL(executionContext);
                List<ProgramParameterValueDTO> parameterDTOList = parameterList.GetProgramParameterValueDTOList(searchParameters);
                if (parameterDTOList != null && parameterDTOList.Any())
                {
                    foreach (ProgramParameterValueDTO item in parameterDTOList)
                    {
                        if (string.IsNullOrWhiteSpace(item.ParameterValue) == false && item.ParameterId == trxIdListParameterId)
                        {
                            string[] idList = item.ParameterValue.Split(',');
                            if (idList != null && idList.Count() > 0)
                            {
                                foreach (string idValue in idList)
                                {
                                    if (string.IsNullOrWhiteSpace(idValue) == false)
                                    {
                                        string trxId = idValue.Trim();
                                        if (trxIdList.Exists(id => id.ToString() == trxId))
                                        {
                                            if (wipTrxIdList.Exists(wid => wid == trxId) == false)
                                            {
                                                wipTrxIdList.Add(trxId);
                                                stringBuilder.Append(trxId);
                                                stringBuilder.Append(", ");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (wipTrxIdList != null && wipTrxIdList.Any())
                    {
                        string wipIdListValue = stringBuilder.ToString();
                        wipIdListValue = wipIdListValue.Substring(0, wipIdListValue.Length - 2);
                        string msg = MessageContainerList.GetMessage(executionContext, 5184, wipIdListValue);
                        //"Unable to submit new request, in progress request is found for following transaction ids [ &1 ]"
                        ValidationException ve = new ValidationException(msg);
                        throw ve;
                    }
                }
            }
            log.LogMethodExit();
        }
        private string GetIdString(ExecutionContext executionContext, List<int> trxIdList)
        {
            log.LogMethodEntry();
            string idString = string.Empty;
            if (trxIdList != null && trxIdList.Any())
            {
                foreach (int item in trxIdList)
                {
                    idString = idString + item.ToString() + ", ";
                }
                idString = idString.Substring(0, idString.Length - 2);
            }
            log.LogMethodExit(idString);
            return idString;
        }
    }
}
