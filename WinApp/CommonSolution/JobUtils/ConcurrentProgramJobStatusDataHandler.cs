
/********************************************************************************************
 * Project Name - ConcurrentProgramJobStatusDataHandler
 * Description  - Data handler of the ConcurrentProgramJobStatusDataHandler class
 * 
 **************
 **Version Log
 **************
 *Version     Date              Modified By    Remarks          
 *********************************************************************************************
 *1.00        15-March-2017    Amaresh          Created 
 *2.70.2      24-Jul-2019      Dakshakh raj     Modified : SQL injection Issue Fix
 *2.90        27-May-2020      Mushahid Faizan Modified: Added Site_ID Search Parameter
 *2.120.1     09-Jun-2021      Deeksha         Modified: As part of AWS concurrent program enhancements
********************************************************************************************/

using System;
using System.Collections.Generic;
//using Semnox.Core.Utilities;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Text;
using Semnox.Core.Utilities;

namespace Semnox.Parafait.JobUtils
{
    /// <summary>
    /// class of ConcurrentProgramJobStatusDataHandler
    /// </summary>
    public class ConcurrentProgramJobStatusDataHandler
    {
        private static readonly Semnox.Parafait.logging.Logger log = new Semnox.Parafait.logging.Logger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly DataAccessHandler dataAccessHandler;
        private readonly SqlTransaction sqlTransaction;
        private static readonly Dictionary<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string> DBSearchParameters = new Dictionary<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>
        {
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PROGRAM_ID, "cr.ProgramId"},
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PHASE, "cr.Phase"},
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.STATUS, "cr.Status"},
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.START_TIME , "cr.StartTime"},
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.END_TIME , "cr.EndTime"},
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.SITE_ID , "cr.Site_Id"},
              {ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PROGRAM_NAME , "cp.ProgramName"}
        };

        /// <summary>
        /// Default constructor of ConcurrentProgramJobStatusDataHandler class
        /// </summary>
        /// <param name="sqlTransaction">sqlTransaction</param>
        public ConcurrentProgramJobStatusDataHandler(SqlTransaction sqlTransaction = null)
        {
            log.LogMethodEntry(sqlTransaction);
            dataAccessHandler = new DataAccessHandler();
            this.sqlTransaction = sqlTransaction;
            log.LogMethodExit();
        }


        /// <summary>
        /// Method to Convert datarow to ConcurrentProgramJobStatusDTO
        /// </summary>
        /// <param name="programJobStatusDataRow">programJobStatusDataRow</param>
        /// <returns></returns>
        private ConcurrentProgramJobStatusDTO GetConcurrentProgramJobStatusDTO(DataRow programJobStatusDataRow)
        {
            log.LogMethodEntry(programJobStatusDataRow);
            ConcurrentProgramJobStatusDTO programStatusDataObject = new ConcurrentProgramJobStatusDTO(Convert.ToInt32(programJobStatusDataRow["ProgramId"]),
                                                    programJobStatusDataRow["RequestId"] == DBNull.Value ? -1 : Convert.ToInt32(programJobStatusDataRow["RequestId"]),
                                                    programJobStatusDataRow["ProgramName"].ToString(),
                                                    programJobStatusDataRow["ExecutableName"].ToString(),
                                                    programJobStatusDataRow["StartTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(programJobStatusDataRow["StartTime"]),
                                                    programJobStatusDataRow["EndTime"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(programJobStatusDataRow["EndTime"]),
                                                    programJobStatusDataRow["Phase"].ToString(),
                                                    programJobStatusDataRow["Status"].ToString(),
                                                    programJobStatusDataRow["ErrorNotificationMailId"].ToString(),
                                                    programJobStatusDataRow["Arguments"].ToString(),
                                                    programJobStatusDataRow["SuccessNotificationMailId"].ToString(),
                                                    programJobStatusDataRow["ErrorCount"].ToString()
                                                    );
            log.LogMethodExit(programStatusDataObject);
            return programStatusDataObject;
        }

        /// <summary>
        /// Returns list of ConcurrentProgramJobDTO
        /// </summary>
        /// <param name="searchParameters">searchParameters</param>
        /// <returns></returns>
        public List<ConcurrentProgramJobStatusDTO> GetConcurrentProgramJobStatusList(List<KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string>> searchParameters)
        {
            log.LogMethodEntry(searchParameters);
            int count = 0;
            List<ConcurrentProgramJobStatusDTO> programStatusList = null;
            List<SqlParameter> parameters = new List<SqlParameter>();
            string selectProgramJobStatusQuery = @"select cp.ProgramId, cr.RequestId, cp.ProgramName, cp.ExecutableName, cr.StartTime, cr.EndTime, 
                                                    cr.Phase, cr.Status, cp.ErrorNotificationMailId, cp.SuccessNotificationMailId,
                                                    (select case WHEN (Argument1 is null or Argument1 = '') then '' else (Argument1) end + 
						                                    case WHEN (Argument2 is null or Argument2 = '') then '' else (', '+ Argument2) end +
						                                    case WHEN (Argument3 is null or Argument3 = '') then '' else (', '+ Argument3) end +
						                                    case WHEN (Argument4 is null or Argument4 = '') then '' else (', '+ Argument4 ) end +
						                                    case WHEN (Argument5 is null or Argument5 = '') then '' else (','+ Argument5 ) end +
						                                    case WHEN (Argument6 is null or Argument6 = '') then '' else (', '+ Argument6 ) end +
						                                    case WHEN (Argument7 is null or Argument7 = '') then '' else (','+ Argument7 ) end +
						                                    case WHEN (Argument8 is null or Argument8 = '') then '' else (', '+ Argument8 ) end +
						                                    case WHEN (Argument9 is null or Argument9 = '') then '' else (', '+ Argument9 ) end +
						                                    case WHEN (Argument10 is null or Argument10 = '') then '' else (', '+ Argument10 ) end 
						                                    from ConcurrentRequests Where cr.RequestId = RequestId) AS Arguments, cr.ErrorCount
                                                    from ConcurrentPrograms cp
                                                    inner join ConcurrentRequests cr 
                                                    on cp.ProgramId = cr.ProgramId and cp.Active = 1";

            if (searchParameters != null)
            {
                StringBuilder query = new StringBuilder(" where ");

                foreach (KeyValuePair<ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters, string> searchParameter in searchParameters)
                {
                    if (DBSearchParameters.ContainsKey(searchParameter.Key))
                    {
                        string joinOperator = (count == 0 ? string.Empty : " and ");

                        if (searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PROGRAM_ID)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.START_TIME)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + ">=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            //query.Append(joinOperator + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) >= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.END_TIME)
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "<=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            //query.Append(joinOperator + "ISNULL(" + DBSearchParameters[searchParameter.Key] + ",GETDATE()) <= " + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), DateTime.ParseExact(searchParameter.Value, "yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.SITE_ID)
                        {
                            query.Append(joinOperator + "(" + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key) + " or " + dataAccessHandler.GetParameterName(searchParameter.Key) + "=-1)");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), Convert.ToInt32(searchParameter.Value)));
                        }
                        else if (searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PHASE
                                 || searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.STATUS
                                 || searchParameter.Key == ConcurrentProgramJobStatusDTO.SearchByConcurrentProgramJobStatusParameters.PROGRAM_NAME) 
                        {
                            query.Append(joinOperator + DBSearchParameters[searchParameter.Key] + "=" + dataAccessHandler.GetParameterName(searchParameter.Key));
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }
                        else
                        {
                            query.Append(joinOperator + "Isnull(" + DBSearchParameters[searchParameter.Key] + ",'') like " + "N'%'+" + dataAccessHandler.GetParameterName(searchParameter.Key) + "+'%'");
                            parameters.Add(new SqlParameter(dataAccessHandler.GetParameterName(searchParameter.Key), searchParameter.Value));
                        }

                        count++;
                    }
                    else
                    {
                        string message = "The query parameter does not exist " + searchParameter.Key;
                        log.LogVariableState("searchParameter.Key", searchParameter.Key);
                        log.LogMethodExit(null, "Throwing exception -" + message);
                        throw new Exception(message);
                    }
                }

                if (searchParameters.Count > 0)
                    selectProgramJobStatusQuery = selectProgramJobStatusQuery + query;

                selectProgramJobStatusQuery = selectProgramJobStatusQuery + " Order by StartTime desc";
            }

            DataTable programStatusData = dataAccessHandler.executeSelectQuery(selectProgramJobStatusQuery, parameters.ToArray(), sqlTransaction);
            if (programStatusData.Rows.Count > 0)
            {
                programStatusList = new List<ConcurrentProgramJobStatusDTO>();
                foreach (DataRow programsStatusDataRow in programStatusData.Rows)
                {
                    ConcurrentProgramJobStatusDTO programStatusDataObject = GetConcurrentProgramJobStatusDTO(programsStatusDataRow);
                    programStatusList.Add(programStatusDataObject);
                }
              
            }
            log.LogMethodExit(programStatusList);
            return programStatusList;
        }


    }
}
