/********************************************************************************************
 * Project Name - Concurrent Programs
 * Description  - Procedure Concurrent Program Class for Concurrent Programs
 * 
 **************
 **Version Log
 **************
 *Version       Date          Modified By         Remarks          
 *********************************************************************************************
 *2.120.1       26-Apr-2021   Deeksha             Created as part of AWS Concurrent Programs enhancements
 *********************************************************************************************/
using Semnox.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace Semnox.Parafait.JobUtils
{
    class ProcConcurrentPrograms : ConcurrentPrograms
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const string PROGRAM_NAME = "Procedure Program";
        private const string DATATYPE_NUMBER = "NUMBER";
        private const string DATATYPE_TEXT = "TEXT";
        private const string DATATYPE_DATETIME = "DATETIME";
        private const string OPERATOR_DEFAULT = "DEFAULT";
        private const string OPERATOR_INLIST = "INLIST";

        private const string RUNNING_PHASE = "Running";
        private const string COMPLETE_PHASE = "Complete";
        private const string PENDING_PHASE = "Pending";

        private const string ABORTED_STATUS = "Aborted";
        private const string ERROR_STATUS = "Error";
        private const string NORMAL_STATUS = "Normal";
        private string fileName = string.Empty;

        public ProcConcurrentPrograms(Utilities utilities, ConcurrentProgramsDTO concurrentProgramsDTO, ConcurrentRequestsDTO concurrentRequestsDTO, 
            string logFileName, DatabaseConnectorDTO dbConnectorDTO)
            :base(utilities.ExecutionContext, concurrentProgramsDTO, concurrentRequestsDTO, logFileName, dbConnectorDTO, utilities)
        {
            log.LogMethodEntry(executionContext);
            log.LogMethodExit();
        }

        public override void LaunchRequests()
        {
            //Debugger.Launch();
            log.LogMethodEntry();
            try
            {
                ConcurrentProgramHelper.WriteToLog("Starts-execStoredProc() method.", fileName);
                ConcurrentProgramHelper.WriteToLog(concurrentProgramsDTO.ExecutableName, fileName);
                fileName = getLogFileName();
                ConcurrentProgramHelper.UpdateConcurrentRequest(utilities.ExecutionContext, concurrentRequestsDTO.RequestId, NORMAL_STATUS, RUNNING_PHASE, null);
                ProgramParameterValueListBL programParameterValueListBL = new ProgramParameterValueListBL(executionContext);
                List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>> searchByParams = new List<KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>>();
                searchByParams.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.PROGRAM_ID, concurrentRequestsDTO.ProgramId.ToString()));
                searchByParams.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.CONCURRENTPROGRAM_SCHEDULE_ID, concurrentRequestsDTO.ProgramScheduleId.ToString()));
                searchByParams.Add(new KeyValuePair<ProgramParameterValueDTO.SearchByParameters, string>(ProgramParameterValueDTO.SearchByParameters.SITE_ID, utilities.ExecutionContext.GetSiteId().ToString()));
                List<ProgramParameterValueDTO> programParameterValueDTOList = programParameterValueListBL.GetAllProgramParameterValueDTOList(searchByParams, true);

                using (SqlConnection con = new SqlConnection(utilities.DBUtilities.sqlConnection.ConnectionString))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand(concurrentProgramsDTO.ExecutableName, con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Clear();
                        if (programParameterValueDTOList != null && programParameterValueDTOList.Any())
                        {
                            foreach (ProgramParameterValueDTO parameterValueDTO in programParameterValueDTOList)
                            {
                                ConcurrentProgramParametersBL concurrentProgramParametersBL = new ConcurrentProgramParametersBL(executionContext, parameterValueDTO.ParameterId);
                                if (concurrentProgramParametersBL.ConcurrentProgramParametersDTO != null)
                                {
                                    SqlDbType sqlDbType = SqlDbType.VarChar;
                                    if (concurrentProgramParametersBL.ConcurrentProgramParametersDTO.DataType == DATATYPE_NUMBER)
                                    {
                                        sqlDbType = SqlDbType.Int;
                                    }
                                    else if (concurrentProgramParametersBL.ConcurrentProgramParametersDTO.DataType == DATATYPE_TEXT)
                                    {
                                        sqlDbType = SqlDbType.NVarChar;
                                    }
                                    else if (concurrentProgramParametersBL.ConcurrentProgramParametersDTO.DataType == DATATYPE_DATETIME)
                                    {
                                        sqlDbType = SqlDbType.DateTime;
                                    }
                                    if (concurrentProgramParametersBL.ConcurrentProgramParametersDTO.Operator.ToUpper() == OPERATOR_DEFAULT
                                        || string.IsNullOrWhiteSpace(concurrentProgramParametersBL.ConcurrentProgramParametersDTO.Operator))
                                    {
                                        cmd.Parameters.Add(concurrentProgramParametersBL.ConcurrentProgramParametersDTO.SQLParameter,
                                                             sqlDbType).Value = parameterValueDTO.ParameterValue;
                                    }
                                    else if (concurrentProgramParametersBL.ConcurrentProgramParametersDTO.Operator.ToUpper() == OPERATOR_INLIST)
                                    {
                                        List<ProgramParameterInListValuesDTO> programParameterInListValuesDTOList = parameterValueDTO.ProgramParameterInListValuesDTOList;
                                        foreach (ProgramParameterInListValuesDTO inListValueDTO in programParameterInListValuesDTOList)
                                        {
                                            cmd.Parameters.Add(new SqlParameter(concurrentProgramParametersBL.ConcurrentProgramParametersDTO.SQLParameter,
                                               sqlDbType)).Value = inListValueDTO.InListValue;
                                        }
                                    }
                                }
                            }
                        }
                        cmd.ExecuteNonQuery();
                        ConcurrentProgramHelper.WriteToLog("Procedure executed successfully", fileName);
                    }
                    con.Close();
                }

                ConcurrentProgramHelper.UpdateConcurrentRequest(utilities.ExecutionContext, concurrentRequestsDTO.RequestId, NORMAL_STATUS, COMPLETE_PHASE, null);
                ConcurrentProgramHelper.SendEmail(utilities.ExecutionContext, concurrentProgramsDTO.ExecutableName + 
                            "Completed successfully :", true, concurrentRequestsDTO);
            }
            catch (Exception ex)
            {

                log.Error(ex.Message);
                string logMessage = ex.Message;
                string emailMsg = "Error while executing the stored procedure " + concurrentProgramsDTO.ExecutableName + ":" + ex.Message + " <br/><br/> Please check";
                ConcurrentProgramHelper.UpdateErrorInformationToLog(executionContext, concurrentRequestsDTO, logMessage, emailMsg, fileName);
            }
            finally
            {
                ConcurrentProgramHelper.WriteToLog("Ends Call request for" + concurrentProgramsDTO.ExecutableName, fileName);
            }
        }

        string getLogFileName()
        {
            log.LogMethodEntry();
            log.LogMethodExit(logFileName + concurrentRequestsDTO.RequestId.ToString() + ".log");
            return logFileName + concurrentRequestsDTO.RequestId.ToString() + ".log";
        }
    }
}
